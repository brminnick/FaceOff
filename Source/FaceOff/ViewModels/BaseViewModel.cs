﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AsyncAwaitBestPractices;

namespace FaceOff
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		readonly WeakEventManager _notifyProprtyChangedEventManager = new WeakEventManager();

		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add => _notifyProprtyChangedEventManager.AddEventHandler(value);
			remove => _notifyProprtyChangedEventManager.RemoveEventHandler(value);
		}

		protected void SetProperty<T>(ref T backingStore, in T value, in Action? onChanged = null, [CallerMemberName] in string propertyname = "")
		{
			if (EqualityComparer<T>.Default.Equals(backingStore, value))
				return;

			backingStore = value;

			onChanged?.Invoke();

			OnPropertyChanged(propertyname);
		}

		protected void OnPropertyChanged([CallerMemberName] in string name = "") =>
			_notifyProprtyChangedEventManager.RaiseEvent(this, new PropertyChangedEventArgs(name), nameof(INotifyPropertyChanged.PropertyChanged));
	}
}