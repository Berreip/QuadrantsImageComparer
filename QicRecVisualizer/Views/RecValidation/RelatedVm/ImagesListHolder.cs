using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using QicRecVisualizer.Services.Configuration;
using QicRecVisualizer.Services.ImagesCache;
using QicRecVisualizer.Views.RecValidation.Adapters;
using QicRecVisualizer.WpfCore;
using QicRecVisualizer.WpfCore.Browsers;
using QicRecVisualizer.WpfCore.Commands;
using QicRecVisualizer.WpfCore.CustomCollections;

namespace QicRecVisualizer.Views.RecValidation.RelatedVm
{
    internal interface IImagesListHolder
    {
        ICollectionView ImagesAvailableInCache { get; }
        IDelegateCommandLight<ImageInCacheAdapter> DeleteImageInCacheCommand { get; }    
        IDelegateCommandLight OpenCacheFolderCommand { get; }
        void AddImageFile(FileInfo validFile);
        void AddImageBitmap(Bitmap image);
        bool TryGetSelectedImageCouple(out (FileInfo Image1, FileInfo Image2) images);
    }

    internal sealed class ImagesListHolder : IImagesListHolder
    {
        private readonly IImageCacheService _imageCacheService;
        private readonly IQicRecConfigProvider _config;
        private readonly ObservableCollectionRanged<ImageInCacheAdapter> _imagesInCacheList;
        public ICollectionView ImagesAvailableInCache { get; }
        public IDelegateCommandLight<ImageInCacheAdapter> DeleteImageInCacheCommand { get; }

        /// <inheritdoc />
        public IDelegateCommandLight OpenCacheFolderCommand { get; }

        public ImagesListHolder(IImageCacheService imageCacheService, IQicRecConfigProvider config, IImageDisplayer imageDisplayer)
        {
            _imageCacheService = imageCacheService;
            _config = config;
            ImagesAvailableInCache = ObservableCollectionSource.GetDefaultView(
                    imageCacheService.GetAllImagesInCache().Select(o => new ImageInCacheAdapter(o)), 
                    out _imagesInCacheList);
            
            DeleteImageInCacheCommand = new DelegateCommandLight<ImageInCacheAdapter>(ExecuteDeleteImageInCacheCommand);
            OpenCacheFolderCommand = new DelegateCommandLight(ExecuteOpenCacheFolderCommand);
            
            if (_imagesInCacheList.Count != 0)
            {
                imageDisplayer.SelectImage(_imagesInCacheList[0]);
            }
        }
        
        /// <inheritdoc />
        public void AddImageFile(FileInfo validFile)
        {
            _imagesInCacheList.Add(new ImageInCacheAdapter(_imageCacheService.AddImage(validFile)));
        }

        /// <inheritdoc />
        public void AddImageBitmap(Bitmap image)
        {
            _imagesInCacheList.Add(new ImageInCacheAdapter(_imageCacheService.AddImageBitmap(image)));
        }

        /// <inheritdoc />
        public bool TryGetSelectedImageCouple(out (FileInfo Image1, FileInfo Image2) images)
        {
            FileInfo first = null;
            FileInfo second = null;
            foreach (var img in _imagesInCacheList)
            {
                if (img.IsSelectedAsFirstImage)
                {
                    first = img.ImageFile;
                }
                if (img.IsSelectedAsSecondImage)
                {
                    second = img.ImageFile;
                }
            }

            if (first == null || second == null)
            {
                images = (null, null);
                return false;
            }

            images = (first, second);
            return true;
        }


        private void ExecuteOpenCacheFolderCommand()
        {
            AsyncWrapper.Wrap(() =>
            {
                _config.GetCacheImageFolder().OpenFolderInExplorer();
            });
        }

        private void ExecuteDeleteImageInCacheCommand(ImageInCacheAdapter imageInCacheAdapter)
        {
            AsyncWrapper.Wrap(() =>
            {
                if (imageInCacheAdapter == null)
                {
                    return;
                }

                // if (MessageBox.Show("confirm deletion ?", "warning", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                if (_imageCacheService.DeleteImage(imageInCacheAdapter.GetImageModel()))
                {
                    _imagesInCacheList.Remove(imageInCacheAdapter);
                }
            });
        }
    }
}