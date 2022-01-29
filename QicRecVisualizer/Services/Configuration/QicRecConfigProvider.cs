using System;
using System.IO;

namespace QicRecVisualizer.Services.Configuration
{
    internal interface IQicRecConfigProvider
    {
        DirectoryInfo GetCacheImageFolder();
    }

    internal sealed class QicRecConfigProvider : IQicRecConfigProvider
    {
        /// <inheritdoc />
        public DirectoryInfo GetCacheImageFolder()
        {
            return new DirectoryInfo(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    QicRecConstants.QIC_REC_FOLDER,
                    QicRecConstants.CACHE_FOLDER,
                    QicRecConstants.IMAGE_CACHE_FOLDER));
        }
    }
}