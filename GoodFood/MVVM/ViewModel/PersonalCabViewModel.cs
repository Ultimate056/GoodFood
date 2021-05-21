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
        #region Команды по изменению контента (выборе вьюшки)
        public RelayCommand DietsViewCommand { get; set; }

        public RelayCommand DescriptionViewCommand { get; set; }

        public RelayCommand FAQViewCommand { get; set; }

        public RelayCommand DevSViewCommand { get; set; }
        #endregion

        #region Диеты
        
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


            #region Свойства для обработки включения компонентов в форме "Личный кабинет" 
        private bool _OnElements;
        public bool OnElements { get { return _OnElements; } set { _OnElements = value; OnPropertyChanged(); } }

        private System.Windows.Visibility vis;
        public System.Windows.Visibility OnVisible { get { return vis; } set { vis = value; OnPropertyChanged(); } }
            #endregion

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

        }


        // Метод, который инициализирует массив объектов имён для инициализации его в комбобоксе
        public static void SetDiets(List<Diet> coll)
        {
            string[] temp = new string[coll.Count];
            for (int i = 0; i < coll.Count; i++)
                temp[i] = coll[i].Name;
            PersonalCab.currentViewModel.ArrayNamesDiets = temp;
        }

        // Метод, включающий компоненты для выбора диет
        public static void OnElementss(bool value, System.Windows.Visibility vss)
        {
            PersonalCab.currentViewModel.OnElements = value;
            PersonalCab.currentViewModel.OnVisible = vss;
        }

        // Устанавливает список диет как объектов
        public static void SetValueDiets(List<Diet> coll)
        {
            PersonalCab.currentViewModel.ArrayDiets = coll;
        }

        // Обновляет диету пользователя
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
