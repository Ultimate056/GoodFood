using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GoodFood.Core
{
    public class ObservableObject : INotifyPropertyChanged
    {
        // Событие, которое уведомляет об изменении
        public event PropertyChangedEventHandler PropertyChanged;

        // Метод, который обновляет данные
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Установка новых данных (для обновления)
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName=null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged();
            return true;
        }
    }
}
