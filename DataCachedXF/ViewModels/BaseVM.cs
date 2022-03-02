using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataCachedXF.ViewModels
{
    public class BaseVM : INotifyPropertyChanged
    {
        // TODO: aggiungere proprietà per tracciare gli errori di parsing delle risposte HTTP
        // (per esempio lo status code, il body, etc...)

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        //public static T Clone<T>(T instance)
        //{
        //    var json = JsonConvert.SerializeObject(instance);
        //    return JsonConvert.DeserializeObject<T>(json);
        //}

    }
}
