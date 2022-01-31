using System.IO;
using QicRecVisualizer.Services.ImagesCache;
using QicRecVisualizer.WpfCore;

namespace QicRecVisualizer.Views.RecValidation.Adapters
{
    internal sealed class ImageInCacheAdapter : ViewModelBase
    {
        private readonly IImageInCache _imageInCache;
        public string ImageName => _imageInCache.File.Name;
        public string ImageFullName => _imageInCache.File.FullName;
        private bool _isSelectedAsSecondImage;
        private bool _isSelectedAsFirstImage;
        private int _firstBorderThickness;
        private int _secondBorderThickness;

        public ImageInCacheAdapter(IImageInCache imageInCache)
        {
            _imageInCache = imageInCache;
            ImageFile = imageInCache.File;
        }

        public bool IsSelectedAsFirstImage
        {
            get => _isSelectedAsFirstImage;
            set
            {
                if (SetProperty(ref _isSelectedAsFirstImage, value))
                {
                    FirstBorderThickness = value ? 2 : 0;
                }
            }
        }

        public bool IsSelectedAsSecondImage
        {
            get => _isSelectedAsSecondImage;
            set 
            {
                if (SetProperty(ref _isSelectedAsSecondImage, value))
                {
                    SecondBorderThickness = value ? 2 : 0;
                }
            }
        }

        public FileInfo ImageFile { get; }

        public int FirstBorderThickness
        {
            get => _firstBorderThickness;
            set => SetProperty(ref _firstBorderThickness, value);
        }

        public int SecondBorderThickness
        {
            get => _secondBorderThickness;
            set => SetProperty(ref _secondBorderThickness, value);
        }


        public IImageInCache GetImageModel() => _imageInCache;
    }
}