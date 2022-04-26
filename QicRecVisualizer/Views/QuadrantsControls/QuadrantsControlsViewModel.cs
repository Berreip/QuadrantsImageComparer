using System.Collections.Generic;
using System.IO;
using QicRecVisualizer.Views.QuadrantsControls.RelatedVm;
using QicRecVisualizer.WpfCore;
using QicRecVisualizer.WpfCore.Browsers;
using QicRecVisualizer.WpfCore.Commands;

namespace QicRecVisualizer.Views.QuadrantsControls
{
    internal interface IQuadrantsControlsViewModel
    {
        /// <summary>
        /// Load a set of files to add in the diff comparisons
        /// </summary>
        void LoadDroppedFiles(IReadOnlyCollection<FileInfo> matchingFiles);
    }

    internal sealed class QuadrantsControlsViewModel : ViewModelBase, IQuadrantsControlsViewModel
    {
        public IDelegateCommandLight LoadDiffFileCommand { get; }
        public IDiffFileListHolder DiffFilesListHolder { get; }
        
        public QuadrantsControlsViewModel(IDiffFileListHolder diffFilesListHolder)
        {
            DiffFilesListHolder = diffFilesListHolder;
            LoadDiffFileCommand = new DelegateCommandLight(ExecuteLoadDiffFileCommand);
        }
        
        private void ExecuteLoadDiffFileCommand()
        {
            AsyncWrapper.Wrap(() =>
            {
                if (BrowserDialogManager.TryOpenFileBrowser("json files (*.json)|*.json", out var selectedFile))
                {
                    DiffFilesListHolder.TryAddLoadedFile(selectedFile);
                }
            });
        }

        /// <inheritdoc />
        public void LoadDroppedFiles(IReadOnlyCollection<FileInfo> matchingFiles)
        {
            foreach (var file in matchingFiles)
            {
                DiffFilesListHolder.TryAddLoadedFile(file);
            }
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
}