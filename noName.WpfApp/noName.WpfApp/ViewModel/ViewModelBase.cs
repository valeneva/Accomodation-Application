// ----------------------------------------------------------------------
// <copyright file="NotifyPRopertyChanged.cs" company="dataPartner">
//     Copyright (c) dataPartner s. r. o.  All rights reserved.
// </copyright>
// <author>
//     marek.banci
// </author>
// ----------------------------------------------------------------------

namespace noName.WpfApp.ViewModel
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;



    // Podle WiX (src\Setup\WixBA\PropertyNotifyBase.cs),
    // coz je pro zmenu podle https://msdn.microsoft.com/en-us/magazine/dd419663.aspx

    /// <summary>
    /// Poskytuje základní podporu pro notifikace o změně vlastností.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected ViewModelBase()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged(string? propertyName)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool SetPropertyValue<T>(ref T propertyBackingField, T value, [CallerMemberName] string? propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(propertyBackingField, value))
            {
                propertyBackingField = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }
    }

}

