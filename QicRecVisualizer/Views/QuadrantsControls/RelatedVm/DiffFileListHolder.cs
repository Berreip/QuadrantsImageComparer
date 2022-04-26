using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using QicRecVisualizer.Views.QuadrantsControls.Adapters;
using QicRecVisualizer.WpfCore;
using QicRecVisualizer.WpfCore.Commands;
using QicRecVisualizer.WpfCore.CustomCollections;
using QuadrantsImageComparerLib.Dto;

namespace QicRecVisualizer.Views.QuadrantsControls.RelatedVm
{
    /// <summary>
    /// Keep all diff file loaded during the current session
    /// </summary>
    internal interface IDiffFileListHolder
    {
        void TryAddLoadedFile(FileInfo file);
        ICollectionView AllLoadedDiffFiles { get; }
        ICollectionView TabsAvailableToDisplay { get; }
        IDelegateCommandLight<LoadedDiffFileAdapter> RemoveDiffCommand { get; }

        /// <summary>
        /// The currently selected diff for visualization
        /// </summary>
        LoadedDiffFileAdapter SelectedDiffVisualization { get; }

        DiffDeltaDisplayAdapter DiffDeltaDisplay { get; }
        string SearchFilter { get; set; }
    }

    internal sealed class DiffFileListHolder : ViewModelBase, IDiffFileListHolder
    {
        private readonly ObservableCollectionRanged<LoadedDiffFileAdapter> _allLoadedDiffFiles;
        private readonly ObservableCollectionRanged<TabHeaderDiffAdapter> _tabsAvailableToDisplay;
        public IDelegateCommandLight<LoadedDiffFileAdapter> RemoveDiffCommand { get; }
        private LoadedDiffFileAdapter _selectedDiffVisualization;
        private DiffDeltaDisplayAdapter _diffDeltaDisplay;
        
        private readonly Dictionary<DiffKind, TabHeaderDiffAdapter> _headersByDiffPosition = new Dictionary<DiffKind, TabHeaderDiffAdapter>();

        /// <inheritdoc />
        public ICollectionView AllLoadedDiffFiles { get; }

        public ICollectionView TabsAvailableToDisplay { get; }

        public DiffFileListHolder()
        {
            AllLoadedDiffFiles = ObservableCollectionSource.GetDefaultView(out _allLoadedDiffFiles);
            AllLoadedDiffFiles.SortDescriptions.Add(new SortDescription(nameof(LoadedDiffFileAdapter.DiffName), ListSortDirection.Ascending));
            TabsAvailableToDisplay = ObservableCollectionSource.GetDefaultView(out _tabsAvailableToDisplay);
            TabsAvailableToDisplay.SortDescriptions.Add(new SortDescription(nameof(TabHeaderDiffAdapter.Kind), ListSortDirection.Ascending));
            RemoveDiffCommand = new DelegateCommandLight<LoadedDiffFileAdapter>(ExecuteRemoveDiffCommand);
        }

        private void ExecuteRemoveDiffCommand(LoadedDiffFileAdapter diff)
        {
            if (diff != null)
            {
                if (_allLoadedDiffFiles.Remove(diff))
                {
                    DiffDeltaDisplay = null;
                    // remove from diff if it was displayed
                    foreach (var data in _headersByDiffPosition.ToArray())
                    {
                        if (ReferenceEquals(data.Value.DiffAdapter, diff))
                        {
                            _headersByDiffPosition.Remove(data.Key);
                            _tabsAvailableToDisplay.Remove(data.Value);
                        }

                        if (ReferenceEquals(data.Value.DiffAdapter, SelectedDiffVisualization))
                        {
                            SelectedDiffVisualization = _headersByDiffPosition.Values.FirstOrDefault()?.DiffAdapter;
                        }
                    }
                }
            }
        }
        
        private string _searchFilter;

        public string SearchFilter
        {
            get => _searchFilter;
            set
            {
                if (SetProperty(ref _searchFilter, value))
                {
                    AllLoadedDiffFiles.Filter = o => SearchFilters.FilterParts(o, value);
                }
            }
        }

        /// <inheritdoc />
        public LoadedDiffFileAdapter SelectedDiffVisualization
        {
            get => _selectedDiffVisualization;
            private set => SetProperty(ref _selectedDiffVisualization, value);
        }


        /// <inheritdoc />
        public DiffDeltaDisplayAdapter DiffDeltaDisplay
        {
            get => _diffDeltaDisplay;
            private set => SetProperty(ref _diffDeltaDisplay, value);
        }

        /// <inheritdoc />
        public void TryAddLoadedFile(FileInfo file)
        {
            try
            {
                // first check if the file if already loaded:
                if (_allLoadedDiffFiles.Any(o => o.IsSameFile(file)))
                {
                    return;
                }

                var jsonContent = File.ReadAllText(file.FullName);
                var diffDto = JsonConvert.DeserializeObject<QuadrantDiffDto>(jsonContent);
                if (diffDto != null)
                {
                    _allLoadedDiffFiles.Add(new LoadedDiffFileAdapter(file, diffDto, OnDiffSelected));
                }
            }
            catch (Exception e)
            {
                Debug.Fail($"unable to load file : {file.Name}{Environment.NewLine}{e}");
            }
        }

        private void OnDiffSelected(DiffKind diffKind, LoadedDiffFileAdapter diffToDisplay)
        {
            Debug.Assert(diffToDisplay != null, "diffToDisplay should never be null");

            if (_headersByDiffPosition.TryGetValue(diffKind, out var previous))
            {
                // if header is not the same, replace it
                if(previous.IsNotTheSame(diffToDisplay))
                {
                    _headersByDiffPosition.Remove(diffKind);
                    _tabsAvailableToDisplay.Remove(previous);
                    AddNewTabHeaderAndSelectIt(diffKind, diffToDisplay);
                }
            }
            else
            {
                // if not present, just add it
                AddNewTabHeaderAndSelectIt(diffKind, diffToDisplay);
            }
        }

        private void AddNewTabHeaderAndSelectIt(DiffKind diffKind, LoadedDiffFileAdapter diffToDisplay)
        {
            var header = new TabHeaderDiffAdapter(diffKind, diffToDisplay, OnTabSelected);
            _tabsAvailableToDisplay.Add(header);
            _headersByDiffPosition.Add(diffKind, header);
            header.IsSelected = true;
            
            // recompute the delta:
            if (_headersByDiffPosition.Count == 2)
            {
                DiffDeltaDisplay = new DiffDeltaDisplayAdapter(
                    _headersByDiffPosition[DiffKind.ReferenceDiff].DiffAdapter,
                    _headersByDiffPosition[DiffKind.ComparedDiff].DiffAdapter);
            }
            else
            {
                DiffDeltaDisplay = null;
            }
        }

        private void OnTabSelected(TabHeaderDiffAdapter tabSelected)
        {
            if (tabSelected != null)
            {
                foreach (var data in _headersByDiffPosition.Values)
                {
                    if (ReferenceEquals(data, tabSelected))
                    {
                        SelectedDiffVisualization = data.DiffAdapter;
                    }
                    else
                    {
                        data.IsSelected = false;
                    }
                }
            }
        }
    }

    public enum DiffKind
    {
        ReferenceDiff,
        ComparedDiff,
    }

    public static class DiffSelectedPositionExtension
    {
        public static string ToDisplayString(this DiffKind diffKind)
        {
            switch (diffKind)
            {
                case DiffKind.ReferenceDiff:
                    return "REF DIFF - ";
                case DiffKind.ComparedDiff:
                    return string.Empty;
                default:
                    throw new ArgumentOutOfRangeException(nameof(diffKind), diffKind, null);
            }
        }
    }
}