using System;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
using QicRecVisualizer.Views.RecValidation.Adapters;
using QicRecVisualizer.WpfCore;
using QicRecVisualizer.WpfCore.Browsers;
using QicRecVisualizer.WpfCore.Commands;
using QuadrantsImageComparerLib.Core;
using QuadrantsImageComparerLib.Dto;
using QuadrantsImageComparerLib.Models;

namespace QicRecVisualizer.Views.QuadrantsControls
{
    internal interface IQuadrantsControlsViewModel
    {
    }

    internal sealed class QuadrantsControlsViewModel : ViewModelBase, IQuadrantsControlsViewModel
    {
        public IDelegateCommandLight LoadDiffFileCommand { get; }

        private string _aoiResume;

        public string AoiResume
        {
            get => _aoiResume;
            set => SetProperty(ref _aoiResume, value);
        }

        private MatrixAdapter[] _rgbMatricesForControlAdapterList = Array.Empty<MatrixAdapter>();

        public QuadrantsControlsViewModel()
        {
            LoadDiffFileCommand = new DelegateCommandLight(ExecuteLoadDiffFileCommand);
        }

        public MatrixAdapter[] RgbMatricesForControlAdapterList
        {
            get => _rgbMatricesForControlAdapterList;
            private set => SetProperty(ref _rgbMatricesForControlAdapterList, value);
        }

        private void ExecuteLoadDiffFileCommand()
        {
            AsyncWrapper.Wrap(() =>
            {
                if (BrowserDialogManager.TryOpenFileBrowser("json files (*.json)|*.json", out var selectedFile))
                {
                    var json = selectedFile.ReadAllText();
                    var diff = JsonConvert.DeserializeObject<QuadrantDiffDto>(json);

                    if (diff == null)
                    {
                        MessageBox.Show("unable to retrieve data");
                        return;
                    }

                    Threshold = diff.Threshold;
                    
                    AoiResume = $"AOI [COLUMS,ROWS] = [{diff.AoiInfo.QuadrantColumns},{diff.AoiInfo.QuadrantRows}] - L: {diff.AoiInfo.AoiLeftPercentage}% | T: {diff.AoiInfo.AoiTopPercentage}% | R: {diff.AoiInfo.AoiRightPercentage}% | B: {diff.AoiInfo.AoiBottomPercentage}%";
                    
                    // display matrix:
                    RgbMatricesForControlAdapterList = new[]
                    {
                        new MatrixAdapter("Red", new Array2D(diff.Red), diff.Threshold),
                        new MatrixAdapter("Green", new Array2D(diff.Green), diff.Threshold),
                        new MatrixAdapter("Blue", new Array2D(diff.Blue), diff.Threshold),
                    };
                }
            });
        }

        private int _threshold;


        public int Threshold
        {
            get => _threshold;
            private set => SetProperty(ref _threshold, value);
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
}