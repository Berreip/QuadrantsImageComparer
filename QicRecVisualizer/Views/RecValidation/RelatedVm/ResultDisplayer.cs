using System.ComponentModel;
using System.IO;
using System.Linq;
using QicRecVisualizer.Services;
using QicRecVisualizer.Views.RecValidation.Adapters;
using QicRecVisualizer.WpfCore;
using QicRecVisualizer.WpfCore.Commands;
using QicRecVisualizer.WpfCore.CustomCollections;
using QuadrantsImageComparerLib.Models;

namespace QicRecVisualizer.Views.RecValidation.RelatedVm
{
    internal interface IResultDisplayer
    {
        IDelegateCommandLight ShowMainPanelImageCommand { get; }
        IDelegateCommandLight<ResultPanelAdapter> ShowResultPanelCommand { get; }
        bool IsShowingResult { get; set; }
        ICollectionView ResultsTabs { get; }
        ResultPanelAdapter SelectedResultDisplay { get; }
        int[] AvailableRowsColumnsValues { get; }
        void AddNewResult(FileInfo image1, FileInfo image2, ImageAoi imageAoi);
    } 

    internal sealed class ResultDisplayer : ViewModelBase, IResultDisplayer
    {
        /// <inheritdoc />
        public IDelegateCommandLight ShowMainPanelImageCommand { get; }
        public IDelegateCommandLight<ResultPanelAdapter> ShowResultPanelCommand { get; }

        /// <inheritdoc />
        public ICollectionView ResultsTabs { get; }
        private readonly ObservableCollectionRanged<ResultPanelAdapter> _resultTabs;
        public int[] AvailableRowsColumnsValues { get; }

        public ResultDisplayer()
        {
            AvailableRowsColumnsValues = Enumerable.Range(1, 200).ToArray();
            ShowMainPanelImageCommand = new DelegateCommandLight(ExecuteShowMainPanelImageCommand);
            ShowResultPanelCommand = new DelegateCommandLight<ResultPanelAdapter>(ExecuteShowResultPanelCommand);
            ResultsTabs = ObservableCollectionSource.GetDefaultView(out _resultTabs);
        }

        private void ExecuteShowResultPanelCommand(ResultPanelAdapter res)
        {
            AsyncWrapper.Wrap(() =>
            {
                if (res == null) return;
                SelectedResultDisplay = res;
            });
        }

        private ResultPanelAdapter _selectedResultDisplay;

        public ResultPanelAdapter SelectedResultDisplay
        {
            get => _selectedResultDisplay;
            private set
            {
                if(SetProperty(ref _selectedResultDisplay, value))
                {
                    IsShowingResult = true;
                }
            }
        }


        /// <inheritdoc />
        public void AddNewResult(FileInfo image1, FileInfo image2, ImageAoi imageAoi)
        {
            var result = new ResultPanelAdapter(image1, image2, _resultTabs.Count, imageAoi);

            result.ComputeWithParameters(QicRecConstants.DEFAULT_QUADRANT_ROWS, QicRecConstants.DEFAULT_QUADRANT_COLUMNS);
            _resultTabs.Add(result);
        }

        private void ExecuteShowMainPanelImageCommand()
        {
            IsShowingResult = false;
        }

        private bool _isShowingResult;

        public bool IsShowingResult
        {
            get => _isShowingResult;
            set => SetProperty(ref _isShowingResult, value);
        }
    }
}