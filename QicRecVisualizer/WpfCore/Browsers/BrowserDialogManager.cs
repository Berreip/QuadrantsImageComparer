using System.IO;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;


namespace QicRecVisualizer.WpfCore.Browsers
{
    public static class BrowserDialogManager
    {
        public static bool TryOpenFileBrowser(string filter, out FileInfo selectedFile, string title = "Choose File", string initialDirectory = null)
        {
            try
            {
                var ofd = initialDirectory != null
                    ? new OpenFileDialog
                    {
                        Filter = filter,
                        Title = title,
                        InitialDirectory = initialDirectory,
                        Multiselect = false
                    }
                    : new OpenFileDialog
                    {
                        Filter = filter,
                        Title = title,
                        Multiselect = false
                    };
                if (ofd.ShowDialog() == true && File.Exists(ofd.FileName))
                {
                    selectedFile = new FileInfo(ofd.FileName);
                    return true;
                }

                selectedFile = null;
                return false;
            }
            catch
            {
                selectedFile = null;
                return false;
            }
        }
    }
}