using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using QuadrantsImageComparerLib.Core;
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
        
        public static DirectoryInfo OpenDirectoryBrowser(string description, string originalPath = "")
        {
            using (var fbd = new FolderBrowserDialog {SelectedPath = originalPath, Description = description})
            {
                return fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath) && Directory.Exists(fbd.SelectedPath)
                    ? new DirectoryInfo(fbd.SelectedPath)
                    : null;
            }
        }

        public static void OpenFolderInExplorer(this DirectoryInfo dir)
        {
            if (dir == null || !dir.ExistsExplicit()) return;
            using (var _ = Process.Start(new ProcessStartInfo("explorer.exe", dir.FullName))){}
        }

    }
}