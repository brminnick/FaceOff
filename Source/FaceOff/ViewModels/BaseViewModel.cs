using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using AsyncAwaitBestPractices;

namespace FaceOff
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Constant Fields
        readonly WeakEventManager _notifyProprtyChangedEventManager = new WeakEventManager();
        #endregion

        #region Events
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add => _notifyProprtyChangedEventManager.AddEventHandler(value);
            remove => _notifyProprtyChangedEventManager.RemoveEventHandler(value);
        }
        #endregion

        #region Methods
        protected void SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyname = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return;

            backingStore = value;

            onChanged?.Invoke();

            OnPropertyChanged(propertyname);
        }

        void OnPropertyChanged([CallerMemberName]string name = "") =>
            _notifyProprtyChangedEventManager.HandleEvent(this, new PropertyChangedEventArgs(name), nameof(INotifyPropertyChanged.PropertyChanged));
        #endregion
    }
}
