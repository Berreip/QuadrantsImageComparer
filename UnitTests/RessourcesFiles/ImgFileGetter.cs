using System;
using System.IO;
using NUnit.Framework;
using QuadrantsImageComparerLib.Core;

namespace UnitTests.RessourcesFiles
{
    internal static class ImgFileGetter
    {
        private static readonly DirectoryInfo _ressourceFilesDirectory = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.TestDirectory, @"RessourcesFiles"));
        
        public static FileInfo GetImage(string imgName)
        {
            if (!_ressourceFilesDirectory.TryGetFile(imgName, out var file))
            {
                throw new ArgumentException($"the file {imgName} could not be found in [{_ressourceFilesDirectory.FullName}]");
            }
            return file;
        }
    }
}