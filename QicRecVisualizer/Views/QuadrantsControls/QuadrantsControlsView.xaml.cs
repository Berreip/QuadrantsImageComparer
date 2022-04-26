using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using QicRecVisualizer.Services;
using QicRecVisualizer.Services.Navigation;

namespace QicRecVisualizer.Views.QuadrantsControls
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed partial class QuadrantsControlsView : INavigeablePanel
    {
        private readonly IQuadrantsControlsViewModel _vm;

        public QuadrantsControlsView(IQuadrantsControlsViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            _vm = vm;
        }

        public void OnNavigateTo()
        {
            // do nothing
        }

        public void OnNavigateExit()
        {
            // do nothing
        }

        private void DropDiffFile(object sender, DragEventArgs e)
        {
            try
            {

                var files = e.Data.GetData(DataFormats.FileDrop, false) as string[];
                if (files == null || files.Length == 0)
                {
                    MessageBox.Show(@"no diff file to drop");
                    return;
                }
               
                var matchingFiles = files.Select(o => new FileInfo(o)).Where(f => f.Exists && f.IsQuicRecDiffExtension()).ToArray();
                
                if (matchingFiles.Length == 0)
                {
                    MessageBox.Show(@"no diff file found among dropped files");
                    return;
                }

                _vm.LoadDroppedFiles(matchingFiles);
            }
            catch (Exception exception)
            {
                Debug.Fail(exception.ToString());
            }
        }

        private void DragDiffEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop) || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
            else
            {
                e.Effects = DragDropEffects.Copy;
            }
        }
    }
}