using QicRecVisualizer.Services.ImagesCache;
using QicRecVisualizer.WpfCore;

namespace QicRecVisualizer.Views.RecValidation.Adapters
{
    internal sealed class ImageInCacheAdapter : ViewModelBase
    {
        private readonly IImageInCache _imageInCache;
        public string ImageName { get; }
        public string ImageFullName { get; }
        private bool _isSelectedAsSecondImage;
        private bool _isSelectedAsFirstImage;

        public ImageInCacheAdapter(IImageInCache imageInCache)
        {
            _imageInCache = imageInCache;
            ImageFullName = imageInCache.File.FullName;
            ImageName = imageInCache.FileName;
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
        public IImageInCache GetImageModel() => _imageInCache;
    }
}