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

    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class QuadrantsControlsViewModel : ViewModelBase, IQuadrantsControlsViewModel
    {
        public IDelegateCommandLight LoadDiffFileCommand { get; }

        private MatrixAdapter[] _rgbMatricesForControlAdapterList = Array.Empty<MatrixAdapter>();
       
        public MatrixAdapter[] RgbMatricesForControlAdapterList
        {
            get => _rgbMatricesForControlAdapterList;
            private set => SetProperty(ref _rgbMatricesForControlAdapterList, value);
        }

        public QuadrantsControlsViewModel()
        {
            LoadDiffFileCommand = new DelegateCommandLight(ExecuteLoadDiffFileCommand);
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
}