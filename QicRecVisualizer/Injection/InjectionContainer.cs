using System;
using System.Windows;
using SimpleInjector;
// ReSharper disable UnusedMember.Global

namespace QicRecVisualizer.Injection
{
    internal interface IInjectionContainer : IDisposable
    {
        TInterface Resolve<TInterface>() where TInterface : class;
        T Resolve<T>(Type type) where T : class;
        void RegisterInstance<TInterface>(TInterface instance) where TInterface : class;

        void Register<TInterface, TClass>(Lifestyle lifestyle)
            where TInterface : class
            where TClass : class, TInterface;

        void RegisterType<TClass>(Lifestyle lifestyle) where TClass : class;
    }

    internal sealed class InjectionContainer : IInjectionContainer
    {
        private readonly Container _internalContainer;
        private bool _disposed;

        public InjectionContainer(Container internalContainer)
        {
            _internalContainer = internalContainer;
            _internalContainer.Options.EnableAutoVerification = false;
            _internalContainer.ResolveUnregisteredType += ResolveUnregisteredType;
            _internalContainer.RegisterInstance<IInjectionContainer>(this);
        }

        private static void ResolveUnregisteredType(object sender, UnregisteredTypeEventArgs unregisteredTypeEventArgs)
        {
            MessageBox.Show($"ResolveUnregisteredType: {unregisteredTypeEventArgs.UnregisteredServiceType.FullName}");
        }
        
        public TInterface Resolve<TInterface>() where TInterface : class
        {
            return _internalContainer.GetInstance<TInterface>();
        }

        public T Resolve<T>(Type type) where T : class
        {
            return (T)_internalContainer.GetInstance(type);
        }
        
        public void RegisterInstance<TInterface>(TInterface instance) where TInterface : class
        {
            _internalContainer.RegisterInstance(instance);
        }

        public void RegisterType<TClass>(Lifestyle lifestyle) where TClass : class
        {
            _internalContainer.Register<TClass>(lifestyle);
        }

        public void Register<TInterface, TClass>(Lifestyle lifestyle)
            where TInterface : class
            where TClass : class, TInterface
        {
            _internalContainer.Register<TInterface, TClass>(lifestyle);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;
            _internalContainer.Dispose();
        }
    }
}