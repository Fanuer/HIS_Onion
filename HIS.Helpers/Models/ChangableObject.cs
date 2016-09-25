using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HIS.Helpers.Models
{
    public class ChangableObject
    {
        #region EVENTS
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        #endregion

        #region METHODS
        /// <summary>
        /// Excecutes an Event after a property is changed
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Set a property value and triggers a propertyChanged event. 
        /// Usage: public string RefreshToken { get{return refreshToken;} internal set { Set(ref this.refreshToken, value);} }
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="field">referenz of the private field</param>
        /// <param name="newValue">New Value</param>
        /// <param name="propertyName">Name of the Property</param>
        /// <returns></returns>
        protected bool Set<T>(ref T field, T newValue = default(T), [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
                return false;
            field = newValue;
            if (propertyName != null) this.OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

        #region PROPERTIES
        #endregion

    }
}
