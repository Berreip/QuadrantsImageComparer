using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using QicRecVisualizer.Services;
using QicRecVisualizer.Services.Helpers;
using QicRecVisualizer.Views.RecValidation.RelatedVm;
using QicRecVisualizer.WpfCore;
using QicRecVisualizer.WpfCore.Browsers;
using QicRecVisualizer.WpfCore.Commands;
using QuadrantsImageComparerLib;
using QuadrantsImageComparerLib.Models;

namespace QicRecVisualizer.Views.RecValidation
{
    internal interface IImportFilesViewModel
    {
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class RecValidationViewModel : ViewModelBase, IImportFilesViewModel
    {
        public IImagesListHolder ImagesListHolder { get; }
        public IImageDisplayer ImageDisplayer { get; }
        private string _selectedFilePath;
        public IDelegateCommandLight SelectFileCommand { get; }
        public IDelegateCommandLight AddSelectedFileCommand { get; }
        public IDelegateCommandLight AddClipBoardToFileCommand { get; }
        public IDelegateCommandLight ProcesSelectedImageCommand { get; }

        private FileInfo _validFile;

        public RecValidationViewModel(IImagesListHolder imagesListHolder, IImageDisplayer imageDisplayer)
        {
            ImagesListHolder = imagesListHolder;
            ImageDisplayer = imageDisplayer;
            SelectFileCommand = new DelegateCommandLight(ExecuteSelectFileCommand);
            AddSelectedFileCommand = new DelegateCommandLight(ExecuteAddSelectedFileCommand);
            AddClipBoardToFileCommand = new DelegateCommandLight(ExecuteAddClipBoardToFileCommand);
            ProcesSelectedImageCommand = new DelegateCommandLight(ExecuteProcesSelectedImageCommand);
        }

        private void ExecuteProcesSelectedImageCommand()
        {
            AsyncWrapper.Wrap(() =>
            {
                if (!ImagesListHolder.TryGetSelectedImageCouple(out var images))
                {
                    MessageBox.Show(@"you have to select an image 1 and image 2 in order to compare them");
                    return;
                }

                using (var image1 = new Bitmap(images.Image1.FullName))
                using (var image2 = new Bitmap(images.Image2.FullName))
                {
                    var result = QuadrantComparer.ComputeDelta(
                        image1,
                        image2,
                        new QuadrantConfig(QicRecConstants.DEFAULT_QUADRANT_ROWS, QicRecConstants.DEFAULT_QUADRANT_COLUMNS)
                        {
                            Aoi = ImageDisplayer.AoiAdapter.GetAoi()
                        });
                }
            });
        }

        public string SelectedFilePath
        {
            get => _selectedFilePath;
            set
            {
                if (SetProperty(ref _selectedFilePath, value))
                {
                    IsSelectedFileValid = FileValidationHelper.TryValidateFileInput(value, out _validFile);
                }
            }
        }

        private bool _isSelectedFileValid;

        public bool IsSelectedFileValid
        {
            get => _isSelectedFileValid;
            private set => SetProperty(ref _isSelectedFileValid, value);
        }

        private void ExecuteAddClipBoardToFileCommand()
        {  
            AsyncWrapper.Wrap(() =>
            {
                if (Clipboard.ContainsImage())
                {
                    if (Clipboard.GetDataObject()?.GetData(DataFormats.Bitmap) is Bitmap image)
                    {
                        ImagesListHolder.AddImageBitmap(image);
                        return;
                    }
                }
                MessageBox.Show(@"the clipboard does not contains an image");
            });
        }
        
        private void ExecuteAddSelectedFileCommand()
        {
            AsyncWrapper.Wrap(() =>
            {
                if (_validFile == null) return;
                ImagesListHolder.AddImageFile(_validFile);
            });
        }

        private void ExecuteSelectFileCommand()
        {
            if (BrowserDialogManager.TryOpenFileBrowser("png files (*.png)|*.png", out var file))
            {
                SelectedFilePath = file.FullName;
            }
        }
    }
}