using GoodFood.MVVM.Model;
using GoodFood.MVVM.ViewModel;
using System.Collections.Generic;
using System.Windows;


namespace GoodFood
{
    public partial class PersonalCab : Window
    {
        public static User CurrentUser = null; // Текущий пользователь, который вошёл в программу
        public static PersonalCabViewModel currentViewModel; // Текущая вьюмодель для данного окна
        public static List<Diet> Diets { get; set; } = null; // Список диет на случай, если на выбор диет будет много
        public PersonalCab(string USER_ID)
        {
            CurrentUser = new User(int.Parse(USER_ID));
            InitializeComponent();
            Application.Current.MainWindow = this;
            Name.Text = CurrentUser.Login; 
            currentViewModel = new PersonalCabViewModel();
            DataContext =  currentViewModel; // Привязка окна к вьюмодели
        }

        // Если пользователь выбирает между диетами
        private void Choise_Diet_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(!AcceptDiet.IsEnabled)
                AcceptDiet.IsEnabled = true;
            if (Diets != null)
            {
                for (int i = 0; i < Diets.Count; i++)
                {
                    // Проверяем имя выбранной диеты с именем диеты в массиве
                    if (Diets[i].Name == Choise_Diet.SelectedItem.ToString())
                    {
                        // Если да, то в текущую диету пользователя инициализируем диету
                        PersonalCab.CurrentUser.currentDiet = new Diet(Diets[i].Name, Diets[i].Text, Diets[i].ID_Diet);
                        break;
                    }
                }
                // Данный метод предназначен для обновления диеты на вью модели DietsViewModel
                PersonalCab.currentViewModel.UpdateDiet();
            }
        }

        // Событие при нажатии на кнопку "выбрать диету"
        private void AcceptDiet_Click(object sender, RoutedEventArgs e)
        {
            // Запускаем метод по инициализации айдишника выбранной диеты в бд
            if (Choise_Diet.Text != null && Choise_Diet.Text != "")
            {
                Diet.AcceptChanges(PersonalCab.CurrentUser.currentDiet.ID_Diet);
                MessageBox.Show("Вы успешно выбрали диету");
                AcceptDiet.IsEnabled = false;
            }
            else
                MessageBox.Show("Выберите диету!");
               
        }
    }
}
