﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CommandPattern.Common
{
	public abstract class BindableBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged ([CallerMemberName] string propertyName = null)
		{
			var eventHandler = PropertyChanged;
			if (eventHandler != null) {
				eventHandler (this, new PropertyChangedEventArgs (propertyName));
			}
		}
		protected bool SetProperty<T> (ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (object.Equals (storage, value)) {
				return false;
			}
			storage = value;
			OnPropertyChanged (propertyName);
			return true;
		}
	}
}
