﻿using System.ComponentModel;
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

        public ImagesListHolder(IImageCacheService imageCacheService, IQicRecConfigProvider config)
        {
            _imageCacheService = imageCacheService;
            _config = config;
            ImagesAvailableInCache = ObservableCollectionSource.GetDefaultView(
                    imageCacheService.GetAllImagesInCache().Select(o => new ImageInCacheAdapter(o)), 
                    out _imagesInCacheList);
            
            DeleteImageInCacheCommand = new DelegateCommandLight<ImageInCacheAdapter>(ExecuteDeleteImageInCacheCommand);
            OpenCacheFolderCommand = new DelegateCommandLight(ExecuteOpenCacheFolderCommand);
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

                if (MessageBox.Show("confirm deletion ?", "warning", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    if (_imageCacheService.DeleteImage(imageInCacheAdapter.GetImageModel()))
                    {
                        _imagesInCacheList.Remove(imageInCacheAdapter);
                    }
                }
            });
        }
    }
}