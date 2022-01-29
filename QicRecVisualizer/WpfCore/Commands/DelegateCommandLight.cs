using System;
using System.Threading.Tasks;
using System.Windows.Input;
using QicRecVisualizer.WpfCore.UiThreadHelpers;
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global

namespace QicRecVisualizer.WpfCore.Commands
{
    /// <summary>
    /// define the most basic command that have a manual RaiseCanExecuteChanged
    /// </summary>
    public interface IDelegateCommandLightBase : ICommand
    {
        /// <summary>
        /// Request an evaluation of the CanExecute of the given command
        /// </summary>
        Task RaiseCanExecuteChanged();
    }

    /// <summary>
    /// basic commands without parameters
    /// </summary>
    public interface IDelegateCommandLight : IDelegateCommandLightBase
    {
        /// <summary>
        /// CanExecute
        /// </summary>
        bool CanExecute();

        /// <summary>
        /// Execute
        /// </summary>
        void Execute();
    }

    /// <summary>
    /// basic commands with parameter
    /// </summary>
    public interface IDelegateCommandLight<in T> : IDelegateCommandLightBase
    {
        /// <summary>
        /// CanExecute with the correct expected type
        /// </summary>
        bool CanExecute(T parameter);

        /// <summary>
        /// Execute with the correct expected type
        /// </summary>
        void Execute(T parameter);
    }

    /// <summary>
    /// Async command with parameters
    /// </summary>
    public interface IDelegateCommandLightAsync<in T> : IDelegateCommandLightBase
    {
        /// <summary>
        /// CanExecute with the correct expected type
        /// </summary>
        bool CanExecute(T parameter);

        /// <summary>
        /// Execute with the correct expected type
        /// </summary>
        Task ExecuteAsync(T parameter);
    }

    /// <summary>
    /// Async command without parameters
    /// </summary>
    public interface IDelegateCommandLightAsync : IDelegateCommandLightBase
    {
        /// <summary>
        /// CanExecute
        /// </summary>
        bool CanExecute();

        /// <summary>
        /// Execute async
        /// </summary>
        Task ExecuteAsync();
    }

    /// <inheritdoc />
    public abstract class DelegateCommandLightBase : IDelegateCommandLightBase
    {
        /// <inheritdoc />
        public async Task RaiseCanExecuteChanged()
        {
            await UiThreadDispatcher.ExecuteOnUIAsync(() => CanExecuteChanged?.Invoke(this, EventArgs.Empty)).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public abstract bool CanExecute(object parameter);

        /// <inheritdoc />
        public abstract void Execute(object parameter);

        /// <inheritdoc />
        public event EventHandler CanExecuteChanged;
    }

    public sealed class DelegateCommandLight : DelegateCommandLightBase, IDelegateCommandLight
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        /// <inheritdoc />
        public DelegateCommandLight(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <inheritdoc />
        public bool CanExecute() => _canExecute == null || _canExecute();

        /// <inheritdoc />
        public void Execute()
        {
            _execute();
        } 

        /// <inheritdoc />
        public override bool CanExecute(object parameter) => CanExecute();

        /// <inheritdoc />
        public override void Execute(object parameter) 
        {
            _execute();
        } 
    }

    public sealed class DelegateCommandLight<T> : DelegateCommandLightBase, IDelegateCommandLight<T>
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        /// <inheritdoc />
        public DelegateCommandLight(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        /// <inheritdoc />
        public bool CanExecute(T parameter) => _canExecute == null || _canExecute(parameter);

        /// <inheritdoc />
        public void Execute(T parameter) 
        {
            _execute(parameter);
        } 

        /// <inheritdoc />
        public override bool CanExecute(object parameter) => CanExecute(parameter.CheckCastParameter<T>());

        /// <inheritdoc />
        public override void Execute(object parameter)
        {
            _execute(parameter.CheckCastParameter<T>());
        }
    }
}