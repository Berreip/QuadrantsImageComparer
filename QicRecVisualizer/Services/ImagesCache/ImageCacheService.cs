using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using QicRecVisualizer.Services.Configuration;
using QicRecVisualizer.Views.RecValidation.Adapters;
using QuadrantsImageComparerLib.Core;

namespace QicRecVisualizer.Services.ImagesCache
{
    internal interface IImageCacheService
    {
        IReadOnlyCollection<IImageInCache> GetAllImagesInCache();
        void LoadAllImagesInCache();
        bool DeleteImage(IImageInCache imageToDelete);
        IImageInCache AddImage(FileInfo validFile);
        IImageInCache AddImageBitmap(Bitmap image);
    }

    internal sealed class ImageCacheService : IImageCacheService
    {
        private readonly HashSet<ImageInCache> _imagesInCache = new HashSet<ImageInCache>();
        private readonly DirectoryInfo _cacheImageFolder;
        private readonly object _key = new object();

        public ImageCacheService(IQicRecConfigProvider config)
        {
            _cacheImageFolder = config.GetCacheImageFolder();
            _cacheImageFolder.CreateIfNotExist();

        }
        
        /// <inheritdoc />
        public IReadOnlyCollection<IImageInCache> GetAllImagesInCache()
        {
            lock (_key)
            {
                return _imagesInCache.ToArray();
            }
        }

        /// <inheritdoc />
        public void LoadAllImagesInCache()
        {
            lock (_key)
            {
                foreach (var pngFile in _cacheImageFolder.GetFiles("*.png"))
                {
                    _imagesInCache.Add(new ImageInCache(pngFile));
                }
            }
        }

        /// <inheritdoc />
        public bool DeleteImage(IImageInCache imageToDelete)
        {
            lock (_key)
            {
                if (imageToDelete != null && _imagesInCache.Contains(imageToDelete))
                {
                    imageToDelete.File.Delete();
                    return true;
                }
                return false;
            }
        }

        /// <inheritdoc />
        public IImageInCache AddImage(FileInfo validFile)
        {
            var copiedFile = validFile.CopyFileTo(Path.Combine(_cacheImageFolder.FullName, validFile.Name));
            lock (_key)
            {
                var imageInCache = new ImageInCache(copiedFile);
                _imagesInCache.Add(imageInCache);
                return imageInCache;
            }
        }

        /// <inheritdoc />
        public IImageInCache AddImageBitmap(Bitmap image)
        {
            var availableFileName = FilesAndDirectoryInfoExtension.AutoRenameFileToAvoidDuplicate(Path.Combine(_cacheImageFolder.FullName, $"{QicRecConstants.IMAGE_CLIPBOARD_NAME}_{DateTime.Now:mm_ss_ffff}.png"));
            image.Save(availableFileName, System.Drawing.Imaging.ImageFormat.Png);
            lock (_key)
            {
                var imageInCache = new ImageInCache(new FileInfo(availableFileName));
                _imagesInCache.Add(imageInCache);
                return imageInCache;
            }
        }
    }
}