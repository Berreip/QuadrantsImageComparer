using System.IO;

namespace QicRecVisualizer.Services
{
    internal static class QicRecConstants
    {
        public const int DEFAULT_QUADRANT_ROWS = 40;
        public const int DEFAULT_QUADRANT_COLUMNS = 40;
        public const string QIC_REC_FOLDER = "QicRecVisualizer";
        public const string CACHE_FOLDER = "Cache";
        public const string IMAGE_CACHE_FOLDER = "Images";
        public const string IMAGE_CLIPBOARD_NAME = "clipImg";
        public const string DIFF_EXTENSIONS = ".QicRecDiff";
        public const string DIFF_EXTENSIONS_V1 = ".json";

        /// <summary>
        /// Returns true if the file extensions could match with the current or previously used QicRecDiffFormat
        /// </summary>
        public static bool IsQuicRecDiffExtension(this FileInfo file)
        {
            return file.Extension == DIFF_EXTENSIONS || file.Extension == DIFF_EXTENSIONS_V1;
        }
    }
}