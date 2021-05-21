using GoodFood.MVVM.Model;
using GoodFood.MVVM.ViewModel;
using System.Collections.Generic;
using System.Windows;


namespace GoodFood
{
    public partial class PersonalCab : Window
    {
        public static User CurrentUser = null;
        public static PersonalCabViewModel currentViewModel;
        public static List<Diet> Diets { get; set; } = null;
        public PersonalCab(string USER_ID)
        {
            CurrentUser = new User(int.Parse(USER_ID));
            InitializeComponent();
            Application.Current.MainWindow = this;
            Name.Text = CurrentUser.Login;
            currentViewModel = new PersonalCabViewModel();
            DataContext =  currentViewModel;
        }
        private void Choise_Diet_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(!AcceptDiet.IsEnabled)
                AcceptDiet.IsEnabled = true;
            if (Diets != null)
            {
                for (int i = 0; i < Diets.Count; i++)
                {
                    if (Diets[i].Name == Choise_Diet.SelectedItem.ToString())
                    {
                        PersonalCab.CurrentUser.currentDiet = new Diet(Diets[i].Name, Diets[i].Text, Diets[i].ID_Diet);
                        break;
                    }
                }
                PersonalCab.currentViewModel.UpdateDiet();
            }
        }

        private void AcceptDiet_Click(object sender, RoutedEventArgs e)
        {
            Diet.AcceptChanges(PersonalCab.CurrentUser.currentDiet.ID_Diet);
            MessageBox.Show("Вы успешно выбрали диету");
            AcceptDiet.IsEnabled = false;
        }
    }
}
