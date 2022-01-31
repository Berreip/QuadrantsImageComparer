using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using QicRecVisualizer.Views.RecValidation.Adapters;
using QicRecVisualizer.WpfCore.Images;

// ReSharper disable UnusedMethodReturnValue.Global

namespace QicRecVisualizer.WpfCore
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;
       
        /// <summary>
        /// Notifie un changement de valeur d'une propriété notifiable
        /// </summary>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            InvokeProperty(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Si la valeur a changé, met à jour l'ancienne valeur et notifie le changement de valeur de la propriété
        /// </summary>
        protected bool SetProperty<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (oldValue == null && newValue == null)
            {
                return false;
            }
            if (oldValue != null && oldValue.Equals(newValue))
            {
                return false;
            }
            oldValue = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }

        private void InvokeProperty(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            try
            {
                handler?.Invoke(this, e);
            }
            catch
            {
                //uniquement pour prévenir les rares cas de plantage (bug du framework) en cas de notification d'une propriété mise à jour de puis un thread de background 
                //et surveillée par le liveshapping d'une collectionview 
                try
                {
                    //du coup on re-essaye:
                    handler?.Invoke(this, e);
                }
                catch
                {
                    //et on avale si ca replante une 2e fois
                }
            }
        }
    }
}