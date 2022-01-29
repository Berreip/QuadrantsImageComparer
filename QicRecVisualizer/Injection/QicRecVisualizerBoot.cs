using System.Windows;
using QicRecVisualizer.Main;
using QicRecVisualizer.Navigation;
using QicRecVisualizer.Views.Configuration;
using QicRecVisualizer.Views.RecValidation;
using SimpleInjector;

namespace QicRecVisualizer.Injection
{
    internal sealed class QicRecVisualizerBoot
    {
        private readonly IInjectionContainer _internalContainer;

        public QicRecVisualizerBoot()
        {
            _internalContainer = new InjectionContainer(new Container());
        }

        public TMainWindow Run<TMainWindow>() where TMainWindow : class
        {
            Register();
            
            Initialize();
            return _internalContainer.Resolve<TMainWindow>();
        }
        
        private void Register()
        {
            _internalContainer.RegisterType<MainWindowView>(Lifestyle.Singleton);
            _internalContainer.Register<IMainWindowViewModel, MainWindowViewModel>(Lifestyle.Singleton);
            
            _internalContainer.Register<IMenuNavigator, MenuNavigator>(Lifestyle.Singleton);
            
            // views:
            _internalContainer.RegisterType<RecValidationView>(Lifestyle.Singleton);
            _internalContainer.Register<IImportFilesViewModel, RecValidationViewModel>(Lifestyle.Singleton);
            
            _internalContainer.RegisterType<ConfigurationView>(Lifestyle.Singleton);
            _internalContainer.Register<IExportFilesViewModel, ConfigurationViewModel>(Lifestyle.Singleton);
            
        }
        
        private void Initialize()
        {
            _internalContainer.Resolve<IMenuNavigator>().NavigateToFirstView();

        }

        public void OnExit(object sender, ExitEventArgs e)
        {
            _internalContainer.Dispose();
        }
    }
}
