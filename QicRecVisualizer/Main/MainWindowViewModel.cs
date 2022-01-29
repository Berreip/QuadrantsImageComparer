
using QicRecVisualizer.Services.Navigation;

namespace QicRecVisualizer.Main
{
    internal interface IMainWindowViewModel
    {
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class MainWindowViewModel : IMainWindowViewModel
    {
        public IMenuNavigator MenuNavigator { get; }

        public MainWindowViewModel(IMenuNavigator menuNavigator)
        {
            MenuNavigator = menuNavigator;
        }
    }
}