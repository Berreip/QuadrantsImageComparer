using System;
using System.Drawing;
using System.Windows.Media.Imaging;
using QicRecVisualizer.Services.Helpers;
using QicRecVisualizer.Views.RecValidation.Adapters;
using QicRecVisualizer.WpfCore;
using QicRecVisualizer.WpfCore.Commands;
using QicRecVisualizer.WpfCore.Images;

namespace QicRecVisualizer.Views.RecValidation.RelatedVm
{
    internal interface IImageDisplayer
    {
        IDelegateCommandLight<ImageInCacheAdapter> DisplayImageCommand { get; }

        BitmapImage CurrentImage { get; }
        bool HasCurrentImage { get; }
        double BottomSliderValue { get; set; }
        double TopSliderValue { get; set; }
        double RightSliderValue { get; set; }
        double LeftSliderValue { get; set; }
        int RectangleWidth { get; }
        int RectangleHeight { get; }
        int ImageActualWidth { get; set; }
        int ImageActualHeight { get; set; }
        double RectangleTopPosition { get; }
        double RectangleLeftPosition { get; }
        string ImageHeightFormated { get; }
        string ImageWidthFormated { get; }
    }

    internal sealed class ImageDisplayer : ViewModelBase, IImageDisplayer
    {
        public IDelegateCommandLight<ImageInCacheAdapter> DisplayImageCommand { get; }

        public ImageDisplayer()
        {
            DisplayImageCommand = new DelegateCommandLight<ImageInCacheAdapter>(ExecuteDisplayImageCommand);
            _bottomSliderValue = 10;
            _topSliderValue = 10;
            _leftSliderValue = 10;
            _rightSliderValue = 10;
            RefrehshImageDependentsValues(null);
        }

        private void ExecuteDisplayImageCommand(ImageInCacheAdapter imageInCache)
        {
            AsyncWrapper.Wrap(() =>
            {
                using (var img = new Bitmap(imageInCache.ImageFullName))
                {
                    CurrentImage = img.GetBitmapImage();
                }
            });
        }

        private BitmapImage _currentImage;

        /// <inheritdoc />
        public BitmapImage CurrentImage
        {
            get => _currentImage;
            private set
            {
                if (SetProperty(ref _currentImage, value))
                {
                    RefrehshImageDependentsValues(value);
                }
            }
        }

        private void RefrehshImageDependentsValues(BitmapImage value)
        {
            if (value == null)
            {
                HasCurrentImage = false;
                ImageHeightFormated = @"/";
                ImageWidthFormated = @"/";
            }
            else
            {
                HasCurrentImage = true;
                ImageHeightFormated = $@"H: {(int)Math.Round(value.Height)} px";
                ImageWidthFormated = $@"W: {(int)Math.Round(value.Width)} px";
            }
        }

        private bool _hasCurrentImage;

        /// <inheritdoc />
        public bool HasCurrentImage
        {
            get => _hasCurrentImage;
            private set => SetProperty(ref _hasCurrentImage, value);
        }

        private double _bottomSliderValue;

        /// <inheritdoc />
        public double BottomSliderValue
        {
            get => _bottomSliderValue;
            set
            {
                if (SetProperty(ref _bottomSliderValue, value))
                {
                    UpdateRectangleVerticalRelated(value, _topSliderValue, _imageCurrentHeight);
                }
            }
        }

        private double _topSliderValue;

        /// <inheritdoc />
        public double TopSliderValue
        {
            get => _topSliderValue;
            set 
            {
                if (SetProperty(ref _topSliderValue, value))
                {
                    UpdateRectangleVerticalRelated(_bottomSliderValue, value, _imageCurrentHeight);
                }
            }
        }

        private double _rightSliderValue;

        /// <inheritdoc />
        public double RightSliderValue
        {
            get => _rightSliderValue;
            set 
            {
                if (SetProperty(ref _rightSliderValue, value))
                {
                    UpdateRectangleHorizontalRelated(_leftSliderValue, value, _imageCurrentWidth);
                }
            }
        }

        private double _leftSliderValue;

        /// <inheritdoc />
        public double LeftSliderValue
        {
            get => _leftSliderValue;
            set 
            {
                if (SetProperty(ref _leftSliderValue, value))
                {
                    UpdateRectangleHorizontalRelated(value, _rightSliderValue, _imageCurrentWidth);
                }
            }
        }

        private int _rectangleWidth = 50;
      

        private double _rectangleTopPosition;

        /// <inheritdoc />
        public double RectangleTopPosition
        {
            get => _rectangleTopPosition;
            private set => SetProperty(ref _rectangleTopPosition, value);
        }

        private double _rectangleLeftPosition;

        /// <inheritdoc />
        public double RectangleLeftPosition
        {
            get => _rectangleLeftPosition;
            private set => SetProperty(ref _rectangleLeftPosition, value);
        }

        private string _imageHeightFormated;

        /// <inheritdoc />
        public string ImageHeightFormated
        {
            get => _imageHeightFormated;
            private set => SetProperty(ref _imageHeightFormated, value);
        }

        private string _imageWidthFormated;

        /// <inheritdoc />
        public string ImageWidthFormated
        {
            get => _imageWidthFormated;
            private set => SetProperty(ref _imageWidthFormated, value);
        }

        /// <inheritdoc />
        public int RectangleWidth
        {
            get => _rectangleWidth;
            private set => SetProperty(ref _rectangleWidth, value);
        }

        private int _rectangleHeight;

        /// <inheritdoc />
        public int RectangleHeight
        {
            get => _rectangleHeight;
            private set => SetProperty(ref _rectangleHeight, value);
        }  
        
        private int _imageCurrentWidth;

        /// <inheritdoc />
        public int ImageActualWidth
        {
            get => _imageCurrentWidth;
            set 
            {
                if (SetProperty(ref _imageCurrentWidth, value))
                {
                    UpdateRectangleHorizontalRelated(_leftSliderValue, _rightSliderValue, value);
                }
            }
        }

        private int _imageCurrentHeight;

        /// <inheritdoc />
        public int ImageActualHeight
        {
            get => _imageCurrentHeight;
            set 
            {
                if (SetProperty(ref _imageCurrentHeight, value))
                {
                    UpdateRectangleVerticalRelated(_bottomSliderValue, _topSliderValue, value);
                }
            }
        }

        private void UpdateRectangleVerticalRelated(double bottomSliderValue, double topSliderValue, int imageCurrentHeight)
        {
            var (topPosition, rectangleHeight) = RectangleCropCalculator.GetNewRectangleHeightAndTopPosition(bottomSliderValue, topSliderValue, imageCurrentHeight);
            RectangleHeight = rectangleHeight;
            RectangleTopPosition = topPosition;
        }

        private void UpdateRectangleHorizontalRelated(double rightSliderValue, double leftSliderValue, int imageCurrentWidth)
        {
            var (leftPosition, rectangleWidth) = RectangleCropCalculator.GetNewRectangleWidthAndLeftPosition(rightSliderValue, leftSliderValue, imageCurrentWidth);
            RectangleWidth = rectangleWidth;
            RectangleLeftPosition = leftPosition;
        }


    }
}