using GoodFood.Core;
using GoodFood.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GoodFood.MVVM.ViewModel
{
    public class DietsViewModel : ObservableObject
    {
        private Diet _CurrentDiet;
        public Diet Diet { get => _CurrentDiet; set => Set(ref _CurrentDiet, value);}

        public ICommand DietChangeCommand { get; set; }

        public void OnDietChangeCommandExecuted(object p)
        {
            _CurrentDiet = new Diet();
        }
        public bool CanDietChangeCommandExecute(object p) => true;
  

        public DietsViewModel()
        {
            DietChangeCommand = new RelayCommand(OnDietChangeCommandExecuted, CanDietChangeCommandExecute);
        }
       
    }
}
