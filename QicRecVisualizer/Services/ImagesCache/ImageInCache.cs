using System.IO;

namespace QicRecVisualizer.Services.ImagesCache
{
    internal interface IImageInCache
    {
        string FileName { get; }
        FileInfo File { get; }
    }

    internal sealed class ImageInCache : IImageInCache
    {
        public ImageInCache(FileInfo pngFile)
        {
            File = pngFile;
            FileName = pngFile.Name;
        }

        /// <inheritdoc />
        public string FileName { get; }

        /// <inheritdoc />
        public FileInfo File { get; }
    }
}