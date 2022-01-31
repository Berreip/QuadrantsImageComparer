using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Media.Imaging;
using QicRecVisualizer.Views.RecValidation.Adapters;
using QicRecVisualizer.WpfCore;
using QicRecVisualizer.WpfCore.Images;

namespace QicRecVisualizer.Views.RecValidation.RelatedVm
{
    internal interface IImageDisplayer
    {
        BitmapImage CurrentImage { get; }
        bool HasCurrentImage { get; }
        IAoiRectangleAdapter AoiAdapter { get; }
        string ImageHeightFormated { get; }
        string ImageWidthFormated { get; }
        void SelectImage(ImageInCacheAdapter imageInCache);
    }

    internal sealed class ImageDisplayer : ViewModelBase, IImageDisplayer
    {
        public IAoiRectangleAdapter AoiAdapter { get; }
        
        public ImageDisplayer()
        {
            AoiAdapter = new AoiRectangleAdapter(10, 10, 10, 10);
            RefrehshImageDependentsValues(null);
        }


        public void SelectImage(ImageInCacheAdapter imageInCache)
        {
            if (imageInCache == null)
            {
                Debug.Fail("imageInCache is null");
                return;
            }
            using (var img = new Bitmap(imageInCache.ImageFullName))
            {
                CurrentImage = img.GetBitmapImage();
            }
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
    }
}