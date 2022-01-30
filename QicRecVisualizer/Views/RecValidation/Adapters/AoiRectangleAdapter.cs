using System;
using QicRecVisualizer.Services.Helpers;
using QicRecVisualizer.WpfCore;
using QuadrantsImageComparerLib.Models;

namespace QicRecVisualizer.Views.RecValidation.Adapters
{
    internal interface IAoiRectangleAdapter
    {
        double BottomSliderValue { get; set; }
        double TopSliderValue { get; set; }
        double RightSliderValue { get; set; }
        double LeftSliderValue { get; set; }
        double RectangleTopPosition { get; }
        double RectangleLeftPosition { get; }
        int RectangleWidth { get; }
        int RectangleHeight { get; }
        string AoiResume { get; }
        int ImageActualWidth { get; set; }
        int ImageActualHeight { get; set; }
        ImageAoi GetAoi();
    }

    internal sealed class AoiRectangleAdapter : ViewModelBase, IAoiRectangleAdapter
    {
        private double _bottomSliderValue;
        private double _topSliderValue;
        private double _rightSliderValue;
        private double _leftSliderValue;
        private int _rectangleWidth;
        private double _rectangleLeftPosition;
        private double _rectangleTopPosition;
        private string _aoiResume;
        private int _rectangleHeight;
        private int _imageCurrentWidth;
        
        public AoiRectangleAdapter(int bottomSliderValue, int topSliderValue, int leftSliderValue, int rightSliderValue)
        {
            _bottomSliderValue = bottomSliderValue;
            _topSliderValue = topSliderValue;
            _leftSliderValue = leftSliderValue;
            _rightSliderValue = rightSliderValue;
            RefreshAoiResume();
        }
        
        /// <inheritdoc />
        public string AoiResume
        {
            get => _aoiResume;
            private set => SetProperty(ref _aoiResume, value);
        }
        
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

        /// <inheritdoc />
        public ImageAoi GetAoi()
        {
            return new ImageAoi
            {
                AoiBottomPercentage = (int)Math.Round(_bottomSliderValue),
                AoiLeftPercentage = (int)Math.Round(LeftSliderValue),
                AoiTopPercentage = (int)Math.Round(TopSliderValue),
                AoiRightPercentage = (int)Math.Round(RightSliderValue),
            };
        }

        /// <inheritdoc />
        public int RectangleWidth
        {
            get => _rectangleWidth;
            private set => SetProperty(ref _rectangleWidth, value);
        }

        /// <inheritdoc />
        public int RectangleHeight
        {
            get => _rectangleHeight;
            private set => SetProperty(ref _rectangleHeight, value);
        }  
        
        /// <inheritdoc />
        public double RectangleTopPosition
        {
            get => _rectangleTopPosition;
            private set => SetProperty(ref _rectangleTopPosition, value);
        }

        /// <inheritdoc />
        public double RectangleLeftPosition
        {
            get => _rectangleLeftPosition;
            private set => SetProperty(ref _rectangleLeftPosition, value);
        }
        
        /// <inheritdoc />
        public double BottomSliderValue
        {
            get => _bottomSliderValue;
            set
            {
                if (SetProperty(ref _bottomSliderValue, value))
                {
                    UpdateRectangleVerticalRelated(value, _topSliderValue, _imageCurrentHeight);
                    RefreshAoiResume();
                }
            }
        }

        /// <inheritdoc />
        public double TopSliderValue
        {
            get => _topSliderValue;
            set 
            {
                if (SetProperty(ref _topSliderValue, value))
                {
                    UpdateRectangleVerticalRelated(_bottomSliderValue, value, _imageCurrentHeight);
                    RefreshAoiResume();
                }
            }
        }

        /// <inheritdoc />
        public double RightSliderValue
        {
            get => _rightSliderValue;
            set 
            {
                if (SetProperty(ref _rightSliderValue, value))
                {
                    UpdateRectangleHorizontalRelated(_leftSliderValue, value, _imageCurrentWidth);
                    RefreshAoiResume();
                }
            }
        }

        /// <inheritdoc />
        public double LeftSliderValue
        {
            get => _leftSliderValue;
            set 
            {
                if (SetProperty(ref _leftSliderValue, value))
                {
                    UpdateRectangleHorizontalRelated(value, _rightSliderValue, _imageCurrentWidth);
                    RefreshAoiResume();
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

        private void RefreshAoiResume()
        {
            AoiResume =  $"AOI = L: {_leftSliderValue}% | T: {_topSliderValue}% | R: {_rightSliderValue}% | B: {_bottomSliderValue}%";
        }
    }
}