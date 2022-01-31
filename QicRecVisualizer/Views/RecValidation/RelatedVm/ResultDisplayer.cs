using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using QicRecVisualizer.Services;
using QicRecVisualizer.Views.RecValidation.Adapters;
using QicRecVisualizer.Views.RecValidation.Adapters.PanelTabs;
using QicRecVisualizer.WpfCore;
using QicRecVisualizer.WpfCore.CustomCollections;
using QuadrantsImageComparerLib.Models;

namespace QicRecVisualizer.Views.RecValidation.RelatedVm
{
    internal interface IResultDisplayer
    {
        IDisplayPanelAdapter SelectedResultDisplay { get; }
        ICollectionView TabsAvailableToDisplay { get; }
        void AddNewResult(FileInfo image1, FileInfo image2, ImageAoi imageAoi);
        void SelectImageAoiTab();
        void DeleteResultPanelTab(ResultPanelAdapter resultPanel);
    } 

    internal sealed class ResultDisplayer : ViewModelBase, IResultDisplayer
    {
        private readonly Dictionary<TabHeaderAdapter, IDisplayPanelAdapter> _resultTabs = new Dictionary<TabHeaderAdapter, IDisplayPanelAdapter>();
        private readonly ObservableCollectionRanged<TabHeaderAdapter> _tabsAvailableToDisplay;
        public ICollectionView TabsAvailableToDisplay { get; }
        private readonly TabHeaderAdapter _imageTab;
        private int _resCount = 1;

        public ResultDisplayer(IImageDisplayer imageDisplayer)
        {
            _imageTab = new TabHeaderAdapter("AOI selection", OnSelectedTabChanged);
            
            _resultTabs.Add(_imageTab, new ImageAoiPanelAdapter(imageDisplayer));
            
            TabsAvailableToDisplay = ObservableCollectionSource.GetDefaultView(new[] { _imageTab }, out _tabsAvailableToDisplay);
            SelectImageAoiTab();
        }

        /// <inheritdoc />
        public void SelectImageAoiTab()
        {
            _imageTab.IsSelected = true;
        }

        /// <inheritdoc />
        public void DeleteResultPanelTab(ResultPanelAdapter resultPanel)
        {
            // select the aoi image
            SelectImageAoiTab();
            
            var matching = _resultTabs.FirstOrDefault(o => ReferenceEquals(o.Value, resultPanel));
            Debug.Assert(matching.Value != null, "matchingHeader is null");
            _tabsAvailableToDisplay.Remove(matching.Key);
            _resultTabs.Remove(matching.Key);
            // dispose the result:
            resultPanel.Dispose();
        }

        private void OnSelectedTabChanged(TabHeaderAdapter tabSelected)
        {   
            AsyncWrapper.Wrap(() =>
            {
                if (tabSelected == null || !_resultTabs.TryGetValue(tabSelected, out var tabToDisplay))
                {
                    Debug.Fail("should never happen");
                    return;
                }
                SelectedResultDisplay = tabToDisplay;
            });
        }

        private IDisplayPanelAdapter _selectedResultDisplay;

        public IDisplayPanelAdapter SelectedResultDisplay
        {
            get => _selectedResultDisplay;
            private set => SetProperty(ref _selectedResultDisplay, value);
        }


        /// <inheritdoc />
        public void AddNewResult(FileInfo image1, FileInfo image2, ImageAoi imageAoi)
        {
            var result = new ResultPanelAdapter(image1, image2, imageAoi);
            result.ComputeWithParameters(QicRecConstants.DEFAULT_QUADRANT_ROWS, QicRecConstants.DEFAULT_QUADRANT_COLUMNS);
            var tabHeaderResult = new TabHeaderAdapter($"Result {_resCount++}", OnSelectedTabChanged);
            _resultTabs.Add(tabHeaderResult, result);
            _tabsAvailableToDisplay.Add(tabHeaderResult);
        }
    }
}