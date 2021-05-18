using GoodFood.Core;
using GoodFood.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodFood.MVVM.ViewModel
{
    public class PersonalCabViewModel : ObservableObject
    {
        public RelayCommand DietsViewCommand { get; set; }

        public RelayCommand DescriptionViewCommand { get; set; }

        public RelayCommand FAQViewCommand { get; set; }

        public RelayCommand DevSViewCommand { get; set; }


        private object _currentView;

        public DietsViewModel DietsVM { get; set; }

        public DescriptionViewModel DescriptionVM { get; set; }

        public FAQViewModel FAQVM { get; set; }

        public DevSViewModel DevSVM { get; set; }

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
    }
}
