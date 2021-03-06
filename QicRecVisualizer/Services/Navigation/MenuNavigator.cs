using System.Linq;
using QicRecVisualizer.Services.Injection;
using QicRecVisualizer.Views.QuadrantsControls;
using QicRecVisualizer.Views.RecValidation;
using QicRecVisualizer.WpfCore;

namespace QicRecVisualizer.Services.Navigation
{
    internal interface IMenuNavigator
    {
        INavigationCommand[] AvailableMenuCommands { get; }
        INavigeablePanel MainPanel { get; }
        bool ShouldDisplayMenu { get; set; }
        void NavigateToFirstView();
    }

    internal interface INavigeablePanel
    {
        /// <summary>
        /// Calls when arriving to the view
        /// </summary>
        void OnNavigateTo();
        
        /// <summary>
        /// Call when exiting from the view
        /// </summary>
        void OnNavigateExit();
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class MenuNavigator : ViewModelBase, IMenuNavigator
    {
        private INavigeablePanel _mainPanel;
        private bool _shouldDisplayMenu;

        public MenuNavigator(IInjectionContainer container)
        {
            AvailableMenuCommands = new INavigationCommand[]
            {
                new NavigationCommand("Record & Validation", () => MainPanel = container.Resolve<RecValidationView>()),
                new NavigationCommand("Extract Diff", () => MainPanel = container.Resolve<QuadrantsControlsView>())
            };
        }

        public INavigationCommand[] AvailableMenuCommands { get; }

        public bool ShouldDisplayMenu
        {
            get => _shouldDisplayMenu;
            set => SetProperty(ref _shouldDisplayMenu, value);
        }
        
        public INavigeablePanel MainPanel
        {
            get => _mainPanel;
            private set
            {
                if(_mainPanel == value) return;
                // call the exiting method
                _mainPanel?.OnNavigateExit();
                // call the entering method on the new one before displaying
                value?.OnNavigateTo();
                // hide selection menu
                ShouldDisplayMenu = false;
                // and assign
                _mainPanel = value;
                RaisePropertyChanged();
            }
        }

        public void NavigateToFirstView()
        {
            var firstNavigablePanel = AvailableMenuCommands.FirstOrDefault();
            if (firstNavigablePanel == null)
            {
                return;
            }

            firstNavigablePanel.IsSelected = true;
            firstNavigablePanel.Command.Execute();

        }
    }
}