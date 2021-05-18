using GoodFood.MVVM.Model;
using GoodFood.MVVM.ViewModel;
using System.Windows;


namespace GoodFood
{
    public partial class PersonalCab : Window
    {
        public static User CurrentUser = null;
        public PersonalCab(string USER_ID)
        {
            CurrentUser = new User(int.Parse(USER_ID));
            InitializeComponent();
            Application.Current.MainWindow = this;
            Name.Text = CurrentUser.Login;
            DataContext = new PersonalCabViewModel();
        }
    }
}
