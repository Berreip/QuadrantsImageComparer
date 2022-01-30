using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using QicRecVisualizer.WpfCore;
using QicRecVisualizer.WpfCore.Images;
using QuadrantsImageComparerLib;
using QuadrantsImageComparerLib.Extractors;
using QuadrantsImageComparerLib.Helpers;
using QuadrantsImageComparerLib.Models;

namespace QicRecVisualizer.Views.RecValidation.Adapters
{
    public sealed class ResultPanelAdapter : ViewModelBase, IDisposable
    {
        private readonly ImageAoi _imageAoi;
        private IQuadrantDelta _quadrantDelta;
        private readonly Bitmap _image1Original;
        private readonly Bitmap _image2Original;
        public FileInfo Image1File { get; }
        public FileInfo Image2File { get; }
        public string DisplayHeader { get; }

        public BitmapImage Image1 { get; }
        public BitmapImage Image2 { get; }

        public ResultPanelAdapter(FileInfo image1File, FileInfo image2File, int resultTabsCount, ImageAoi imageAoi)
        {
            _imageAoi = imageAoi;
            Image1File = image1File;
            Image2File = image2File;
            DisplayHeader = $"Result {resultTabsCount}";
            
            _image1Original = new Bitmap(image1File.FullName);
            _image2Original = new Bitmap(image2File.FullName);
            
            Image1 = ComputedAoiAndExtractBitmapImage(_image1Original, imageAoi);
            Image2 = ComputedAoiAndExtractBitmapImage(_image2Original, imageAoi);
        }

        private static BitmapImage ComputedAoiAndExtractBitmapImage(Bitmap imageOriginal, ImageAoi imageAoi)
        {
            var computedAoi1 = imageAoi.ComputeAoi(imageOriginal.Size);
            using (var imgSpitted = SplitHelpers.CropAndResizeImage(imageOriginal, computedAoi1, imageOriginal.Size))
            {
               return imgSpitted.GetBitmapImage();
            }
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