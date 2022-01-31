using QicRecVisualizer.Services.Navigation;

namespace QicRecVisualizer.Views.QuadrantsControls
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed partial class QuadrantsControlsView : INavigeablePanel
    {
        public QuadrantsControlsView(IQuadrantsControlsViewModel vm)
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