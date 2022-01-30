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

        public ImageInCacheAdapter(IImageInCache imageInCache)
        {
            _imageInCache = imageInCache;
            ImageFile = imageInCache.File;
        }

        public bool IsSelectedAsFirstImage
        {
            get => _isSelectedAsFirstImage;
            set => SetProperty(ref _isSelectedAsFirstImage, value);
        }
        
        public bool IsSelectedAsSecondImage
        {
            get => _isSelectedAsSecondImage;
            set => SetProperty(ref _isSelectedAsSecondImage, value);
        }

        public FileInfo ImageFile { get; }

        public IImageInCache GetImageModel() => _imageInCache;
    }
}