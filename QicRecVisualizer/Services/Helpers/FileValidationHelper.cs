using System.IO;

namespace QicRecVisualizer.Services.Helpers
{
    public class FileValidationHelper
    {
        public static bool TryValidateFileInput(string file, out FileInfo validFile)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(file) && File.Exists(file))
                {
                    // try to create the file
                    validFile = new FileInfo(file);
                    return true;
                }
            }
            catch
            {
                // ignore and return false
            }
            validFile = null;
            return false;
        }
    }
}