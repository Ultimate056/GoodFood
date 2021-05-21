using GoodFood.Core;
using GoodFood.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GoodFood.MVVM.ViewModel
{
    public class PersonalCabViewModel : ObservableObject
    {
        public RelayCommand DietsViewCommand { get; set; }

        public RelayCommand DescriptionViewCommand { get; set; }

        public RelayCommand FAQViewCommand { get; set; }

        public RelayCommand DevSViewCommand { get; set; }


        #region Диеты
        public RelayCommand SelectDiet { get; set; }
        private string[] arrayNamesDiets;
        public string[] ArrayNamesDiets
        {
            get { return arrayNamesDiets; }
            set { arrayNamesDiets = value; OnPropertyChanged(); }
        }

        private List<Diet> arrayDiets;

        public List<Diet> ArrayDiets
        { 
            get { return arrayDiets; } set { arrayDiets = value; OnPropertyChanged(); }
        }



        private bool _OnElements;
        public bool OnElements { get { return _OnElements; } set { _OnElements = value; OnPropertyChanged(); } }

        private System.Windows.Visibility vis;
        public System.Windows.Visibility OnVisible { get { return vis; } set { vis = value; OnPropertyChanged(); } }


        #endregion Конец диет

        private object _currentView;

        public static DietsViewModel DietsVM { get; set; }

        public static DescriptionViewModel DescriptionVM { get; set; }

        public static FAQViewModel FAQVM { get; set; }

        public static DevSViewModel DevSVM { get; set; }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }

        }

        public PersonalCabViewModel()
        {
            arrayDiets = null;
            arrayNamesDiets = null;
            OnElements = false;
            OnVisible = System.Windows.Visibility.Hidden;
            DietsVM = new DietsViewModel();
            DescriptionVM = new DescriptionViewModel();
            FAQVM = new FAQViewModel();
            DevSVM = new DevSViewModel();
            CurrentView = DietsVM;

            DietsViewCommand = new RelayCommand(o =>
            {
                CurrentView = DietsVM;
            });
            DescriptionViewCommand = new RelayCommand(o =>
            {
                CurrentView = DescriptionVM;
            });
            FAQViewCommand = new RelayCommand(o =>
            {
                CurrentView = FAQVM;
            });
            DevSViewCommand = new RelayCommand(o =>
            {
                CurrentView = DevSVM;
            });

            SelectDiet = new RelayCommand(o =>
            {
                
            });
        }

        public static void SetDiets(List<Diet> coll)
        {
            string[] temp = new string[coll.Count];
            for (int i = 0; i < coll.Count; i++)
                temp[i] = coll[i].Name;
            PersonalCab.currentViewModel.ArrayNamesDiets = temp;
        }
        public static void OnElementss(bool value, System.Windows.Visibility vss)
        {
            PersonalCab.currentViewModel.OnElements = value;
            PersonalCab.currentViewModel.OnVisible = vss;
        }

        public static void SetValueDiets(List<Diet> coll)
        {
            PersonalCab.currentViewModel.ArrayDiets = coll;
        }

        public void UpdateDiet()
        {
            string DefName = PersonalCab.CurrentUser.currentDiet.Name;
            string name = DefName.Substring(2);
            name.Trim();
            PersonalCab.CurrentUser.currentDiet.Name = name;
            DietsVM.d = PersonalCab.CurrentUser.currentDiet;
        }

    }
}
