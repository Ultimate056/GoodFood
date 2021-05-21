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
        private  Diet _d ;
        public Diet d { get=>_d; set { _d = value; OnPropertyChanged(); } }

        public RelayCommand ChangeDietCommand { get; set; }

        public DietsViewModel()
        {
            if (PersonalCab.CurrentUser.IsDiet)
                d = new Diet(PersonalCab.CurrentUser.currentDiet.Name, PersonalCab.CurrentUser.currentDiet.Text, PersonalCab.CurrentUser.currentDiet.ID_Diet);
            else
                d = new Diet(" ", " ", 0);
            ChangeDietCommand = new RelayCommand(o =>
            {
                d = PersonalCab.CurrentUser.currentDiet;
            });
        }
    }
}
