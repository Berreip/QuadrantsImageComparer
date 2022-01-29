using QicRecVisualizer.Services.Navigation;

namespace QicRecVisualizer.Views.RecValidation
{
    internal sealed partial class RecValidationView : INavigeablePanel
    {
        public RecValidationView(IImportFilesViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        public void OnNavigateTo()
        {
            // do nothing
        }

        public void OnNavigateExit()
        {
            // do nothing
        }
    }
}