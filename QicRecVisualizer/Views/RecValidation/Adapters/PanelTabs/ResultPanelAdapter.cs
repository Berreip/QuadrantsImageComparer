using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using QicRecVisualizer.Services.Helpers;
using QicRecVisualizer.WpfCore;
using QicRecVisualizer.WpfCore.Browsers;
using QicRecVisualizer.WpfCore.Commands;
using QicRecVisualizer.WpfCore.Images;
using QuadrantsImageComparerLib;
using QuadrantsImageComparerLib.Core;
using QuadrantsImageComparerLib.Dto;
using QuadrantsImageComparerLib.Extractors;
using QuadrantsImageComparerLib.Helpers;
using QuadrantsImageComparerLib.Models;

namespace QicRecVisualizer.Views.RecValidation.Adapters.PanelTabs
{
    internal sealed class ResultPanelAdapter : ViewModelBase, IDisposable, IDisplayPanelAdapter
    {
        private readonly ImageAoi _imageAoi;
        private IQuadrantDelta _quadrantDelta;
        private readonly Bitmap _image1Original;
        private readonly Bitmap _image2Original;
        public FileInfo Image1File { get; }
        public FileInfo Image2File { get; }

        public BitmapImage Image1 { get; }
        public BitmapImage Image2 { get; }

        public int[] AvailableRowsColumnsValues { get; }
        private MatrixAdapter[] _rgbMatricesAdapterList = Array.Empty<MatrixAdapter>();
        public IDelegateCommandLight ExtractDiffCommand { get; }

        public ResultPanelAdapter(FileInfo image1File, FileInfo image2File, ImageAoi imageAoi)
        {
            _imageAoi = imageAoi;
            Image1File = image1File;
            Image2File = image2File;
            AvailableRowsColumnsValues = Enumerable.Range(1, 300).ToArray();
            
            _image1Original = new Bitmap(image1File.FullName);
            _image2Original = new Bitmap(image2File.FullName);
            
            Image1 = ComputedAoiAndExtractBitmapImage(_image1Original, imageAoi);
            Image2 = ComputedAoiAndExtractBitmapImage(_image2Original, imageAoi);
            ExtractDiffCommand = new DelegateCommandLight(ExecuteExtractDiffCommand);
        }

        private void ExecuteExtractDiffCommand()
        {
            AsyncWrapper.Wrap(() =>
            {
                var selectedFolder = BrowserDialogManager.OpenDirectoryBrowser("choose a output directory");
                if (selectedFolder != null)
                {
                    var diffDto = new QuadrantDiffDto
                    {
                        Red = _rgbMatricesAdapterList[0].GetMatrix(),
                        Green = _rgbMatricesAdapterList[1].GetMatrix(),
                        Blue = _rgbMatricesAdapterList[2].GetMatrix(),
                        Threshold = _thresholdValueInt,
                        AoiInfo = new AoiInfoDto
                        {
                            QuadrantRows = _selectedRowValue,
                            QuadrantColumns =_selectedColumnValue,
                            AoiLeftPercentage = _imageAoi.AoiLeftPercentage,
                            AoiTopPercentage = _imageAoi.AoiTopPercentage,
                            AoiRightPercentage = _imageAoi.AoiRightPercentage,
                            AoiBottomPercentage = _imageAoi.AoiBottomPercentage,
                        }
                        
                    };
                    var json = JsonConvert.SerializeObject(diffDto);
                    File.WriteAllText(FilesAndDirectoryInfoExtension.AutoRenameFileToAvoidDuplicate(selectedFolder.FullName, "QuadrantsRecImageDiff.json"), json);
                    
                    // then open the folder
                    selectedFolder.OpenFolderInExplorer();
                }
            });
        }

        private static BitmapImage ComputedAoiAndExtractBitmapImage(Bitmap imageOriginal, ImageAoi imageAoi)
        {
            var computedAoi1 = imageAoi.ComputeAoi(imageOriginal.Size);
            using (var imgSpitted = SplitHelpers.CropAndResizeImage(imageOriginal, computedAoi1, imageOriginal.Size))
            {
               return imgSpitted.GetBitmapImage();
            }
        }

        public MatrixAdapter[] RgbMatricesAdapterList
        {
            get => _rgbMatricesAdapterList;
            private set => SetProperty(ref _rgbMatricesAdapterList, value);
        }
        
        private BitmapImage _imageQuadrant1;

        public BitmapImage ImageQuadrant1
        {
            get => _imageQuadrant1;
            private set => SetProperty(ref _imageQuadrant1, value); 
        }
        
        private BitmapImage _imageQuadrant2;

        public BitmapImage ImageQuadrant2
        {
            get => _imageQuadrant2;
            private set => SetProperty(ref _imageQuadrant2, value); 
        }

        private int _selectedRowValue;

        public int SelectedRowValue
        {
            get => _selectedRowValue;
            set
            {
                if (SetProperty(ref _selectedRowValue, value))
                {
                    ComputeWithParameters(_selectedRowValue, _selectedColumnValue);
                }
            }
        }

        private int _selectedColumnValue;

        public int SelectedColumnValue
        {
            get => _selectedColumnValue;
            set 
            {
                if (SetProperty(ref _selectedColumnValue, value))
                {
                    ComputeWithParameters(_selectedRowValue, _selectedColumnValue);
                }
            }
        }

        private double _thresholdValue;
        private int _thresholdValueInt = 0;

        public double ThresholdValue
        {
            get => _thresholdValue;
            set
            {
                if (SetProperty(ref _thresholdValue, value))
                {
                    _thresholdValueInt = (int)_thresholdValue;
                    foreach (var matrixAdapter in _rgbMatricesAdapterList)
                    {
                        matrixAdapter.RefreshCells(_thresholdValueInt);
                    }
                }
            }
        }
        
        public void ComputeWithParameters(int quadrantRows, int quadrantColumns)
        {
            _selectedRowValue = quadrantRows;
            _selectedColumnValue = quadrantColumns;
            RaisePropertyChanged(nameof(SelectedColumnValue));
            RaisePropertyChanged(nameof(SelectedRowValue));
            
            // dispose the previous if needed
            _quadrantDelta?.Dispose();
            
            // recompute
            _quadrantDelta = QuadrantComparer.ComputeDelta(_image1Original, _image2Original, new QuadrantConfig(quadrantRows, quadrantColumns) { Aoi = _imageAoi });

            // and display the result
            ImageQuadrant1 = _quadrantDelta.QuadrantImg1.GetBitmapImage();
            ImageQuadrant2 = _quadrantDelta.QuadrantImg2.GetBitmapImage();
            
            // display matrix:
            RgbMatricesAdapterList = new[]
            {
                new MatrixAdapter("Red", _quadrantDelta.Red, _thresholdValueInt),
                new MatrixAdapter("Green", _quadrantDelta.Green, _thresholdValueInt),
                new MatrixAdapter("Blue", _quadrantDelta.Blue, _thresholdValueInt),
            };
        }
        
        /// <inheritdoc />
        public void Dispose()
        {
            _quadrantDelta?.Dispose();
            _image1Original.Dispose();
            _image2Original.Dispose();
        }
    }
}