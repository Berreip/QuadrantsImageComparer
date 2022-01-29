using QicRecVisualizer.Services.Navigation;

namespace QicRecVisualizer.Views.Configuration
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed partial class ConfigurationView : INavigeablePanel
    {
        public ConfigurationView()
        {
            InitializeComponent();
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