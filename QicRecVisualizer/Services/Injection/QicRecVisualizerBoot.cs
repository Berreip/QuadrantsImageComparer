using System.Threading.Tasks;
using System.Windows;
using QicRecVisualizer.Main;
using QicRecVisualizer.Services.Configuration;
using QicRecVisualizer.Services.ImagesCache;
using QicRecVisualizer.Services.Navigation;
using QicRecVisualizer.Views.QuadrantsControls;
using QicRecVisualizer.Views.QuadrantsControls.RelatedVm;
using QicRecVisualizer.Views.RecValidation;
using QicRecVisualizer.Views.RecValidation.RelatedVm;
using SimpleInjector;

namespace QicRecVisualizer.Services.Injection
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
            
            _internalContainer.RegisterType<QuadrantsControlsView>(Lifestyle.Singleton);
            _internalContainer.Register<IQuadrantsControlsViewModel, QuadrantsControlsViewModel>(Lifestyle.Singleton);
            
            // services:
            _internalContainer.Register<IImageCacheService, ImageCacheService>(Lifestyle.Singleton);
            _internalContainer.Register<IDiffFileListHolder, DiffFileListHolder>(Lifestyle.Singleton);
            
            // VM related services
            _internalContainer.Register<IImagesListHolder, ImagesListHolder>(Lifestyle.Singleton);
            _internalContainer.Register<IQicRecConfigProvider, QicRecConfigProvider>(Lifestyle.Singleton);
            _internalContainer.Register<IImageDisplayer, ImageDisplayer>(Lifestyle.Singleton);
            _internalContainer.Register<IResultDisplayer, ResultDisplayer>(Lifestyle.Singleton);
            
        }
        
        private void Initialize()
        {
            _internalContainer.Resolve<IImageCacheService>().LoadAllImagesInCache();
            _internalContainer.Resolve<IMenuNavigator>().NavigateToFirstView();

        }

        public void OnExit(object sender, ExitEventArgs e)
        {
            _internalContainer.Dispose();
        }
    }
}
