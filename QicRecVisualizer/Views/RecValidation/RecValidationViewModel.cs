using QicRecVisualizer.WpfCore;
using QicRecVisualizer.WpfCore.Browsers;
using QicRecVisualizer.WpfCore.Commands;

namespace QicRecVisualizer.Views.RecValidation
{
    internal interface IImportFilesViewModel
    {
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class RecValidationViewModel : ViewModelBase, IImportFilesViewModel
    {
        private string _selectedFilePath;
        public IDelegateCommandLight SelectFileCommand { get; }

        public RecValidationViewModel()
        {
            SelectFileCommand = new DelegateCommandLight(ExecuteSelectFileCommand);
        }

        public string SelectedFilePath
        {
            get => _selectedFilePath;
            set => SetProperty(ref _selectedFilePath, value);
        }

        private void ExecuteSelectFileCommand()
        {
            if (BrowserDialogManager.TryOpenFileBrowser("All files (*.*)|*.*", out var file))
            {
                SelectedFilePath = file.FullName;
            }
            
        }
    }
}