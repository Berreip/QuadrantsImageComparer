using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using NUnit.Framework;
using QuadrantsImageComparerLib.Core;
// ReSharper disable InconsistentNaming

namespace UnitTests.RessourcesFiles
{
    [SuppressMessage("Interoperability", "CA1416:Valider la compatibilité de la plateforme")]
    internal static class ImgFileGetter
    {
        private static readonly DirectoryInfo _ressourceFilesDirectory = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.TestDirectory, @"RessourcesFiles"));
        
        public static FileInfo GetImageFile(ImgKey imgName)
        {
            if (!_ressourceFilesDirectory.TryGetFile($"{imgName.ToString()}.png", out var file))
            {
                throw new ArgumentException($"the file {imgName}.png could not be found in [{_ressourceFilesDirectory.FullName}]");
            }
            return file;
        }   
        
        public static FileInfo GetQuadrantInfoFile(QuadrantInfo infoJson)
        {
            if (!_ressourceFilesDirectory.TryGetFile($"{infoJson.ToString()}.json", out var file))
            {
                throw new ArgumentException($"the file {infoJson}.json could not be found in [{_ressourceFilesDirectory.FullName}]");
            }
            return file;
        }   

        public static Bitmap GetImage(ImgKey imgName)
        {
            if (!_ressourceFilesDirectory.TryGetFile($"{imgName.ToString()}.png", out var file))
            {
                throw new ArgumentException($"the file {imgName}.png could not be found in [{_ressourceFilesDirectory.FullName}]");
            }
            return new Bitmap(file.FullName);
        }
    }

    internal enum QuadrantInfo
    {
        blue_orange_invalid_aoi,
        blue_orange_valid_aoi
    }

    internal enum ImgKey
    {
        img100x100_1,
        img100x100_blue_border,
        img100x100_orange_border,
        img150x60_violet,
        img10x10_black,
        img10x10_blue,
        img10x10_green,
        img10x10_red,
        
    }
}