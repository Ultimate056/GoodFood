using GoodFood.MVVM.Model;
using GoodFood.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GoodFood
{
    /// <summary>
    /// Логика взаимодействия для NewModel.xaml
    /// </summary>
    public partial class NewModel : Window
    {

        SqlCommand cmd;
        public SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        string[] goalss = { "Похудеть", "Поддержать вес", "Набрать вес" };

        string[] activites = { "Низкий", "Средний", "Высокий" };

        string[] diagnoses = { "Язва", "Аннорексия", "Отсутствует" };

        List<Diet> ListDiets = new List<Diet>();

        public NewModel()
        {
            InitializeComponent();
            Height.Text = Convert.ToString(PersonalCab.CurrentUser.Height);
            Weight.Text = Convert.ToString(PersonalCab.CurrentUser.Weight);
            goals.ItemsSource = goalss;
            TypeActivity.ItemsSource = activites;
            Diagnoses.ItemsSource = diagnoses;
            DataContext = PersonalCabViewModel.DietsVM;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (!Height.IsEnabled)
            {
                Height.IsEnabled = true;
                b1.IsEnabled = true;
                b1.Visibility = Visibility.Visible;
            }
        }

        private void Hyperlink_Click2(object sender, RoutedEventArgs e)
        {
            if (!Weight.IsEnabled)
            {
                Weight.IsEnabled = true;
                b1.IsEnabled = true;
                b1.Visibility = Visibility.Visible;
            }
        }
        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            bool check_on_change = true;
            if(b1.IsEnabled)
            {
                MessageBox.Show("Сначала примите изменения!");
                check_on_change = false;
            }

            if(check_on_change)
            {
                double IMT = Math.Round((PersonalCab.CurrentUser.Weight) / (Math.Pow((Convert.ToDouble(PersonalCab.CurrentUser.Height) / 100), 2)), 1);
                int id_goal = 0;
                int id_activity = 0;
                int id_diagnos = 0;
                bool input_check = true;
                if (goals.SelectedItem == null)
                {
                    input_check = false;
                    MessageBox.Show("Цель не выбрана!");
                }
                if (TypeActivity.SelectedItem == null)
                {
                    input_check = false;
                    MessageBox.Show("Тип активности не выбран!");
                }
                if (Diagnoses.SelectedItem == null)
                {
                    input_check = false;
                    MessageBox.Show("Диагнозы не выбран!");
                }

                if (input_check)
                {
                    bool check = true;
                    if (IMT >= 11.3 && IMT <= 18.4) // дефицит массы тела
                    {
                        if (goals.Text == "Похудеть")
                        {
                            MessageBox.Show("Вам нельзя худеть согласно ИМТ=" + IMT);
                            check = false;
                        }

                    }
                    if (IMT >= 18.5 && IMT <= 25.0) // норма
                    {

                    }
                    if (IMT >= 25.1 && IMT <= 30.0) // предожирение
                    {
                        if (goals.Text == "Набрать вес")
                        {
                            MessageBox.Show("Вам нельзя набирать вес согласно ИМТ=" + IMT);
                            check = false;
                        }

                    }
                    if (IMT >= 30.1 && IMT <= 34.9) // Ожирение 1-й степени
                    {
                        if (goals.Text == "Набрать вес")
                        {
                            MessageBox.Show("Вам нельзя набирать вес согласно ИМТ=" + IMT);
                            check = false;
                        }

                    }
                    if (IMT >= 35.0 && IMT <= 40.0) // Ожирение 2-й степени
                    {
                        if (goals.Text == "Набрать вес")
                        {
                            MessageBox.Show("Вам нельзя набирать вес согласно ИМТ=" + IMT);
                            check = false;
                        }

                    }
                    if (IMT >= 40.1 && IMT <= 52.0) // Ожирение 3-й степени
                    {
                        if (goals.Text == "Набрать вес")
                        {
                            MessageBox.Show("Вам нельзя набирать вес согласно ИМТ=" + IMT);
                            check = false;
                        }

                    }
                    if (IMT >= 52.1 || IMT <= 11.2)
                    {
                        MessageBox.Show("Ошибка в расчетах!!!", "Что-то пошло не так:( " + IMT);
                        check = false;
                    }
                    if (check)
                    {
                        // Меняем айдишник у пользователя в бд ЦЕЛЬ
                        connection.Open();
                        cmd = new SqlCommand("SELECT ID_goal FROM Цель WHERE Цель=@cel", connection);
                        cmd.Parameters.AddWithValue("@cel", goals.Text);
                        id_goal = int.Parse(Convert.ToString(cmd.ExecuteScalar()));

                        cmd = new SqlCommand("UPDATE users SET ID_goal=@id WHERE ID_user=@id_user", connection);
                        cmd.Parameters.AddWithValue("@id", id_goal);
                        cmd.Parameters.AddWithValue("@id_user", PersonalCab.CurrentUser.user_id);
                        cmd.ExecuteNonQuery();

                        // Активность

                        cmd = new SqlCommand("SELECT ID_activity FROM Активность WHERE [Тип активности]=@cel", connection);
                        cmd.Parameters.AddWithValue("@cel", TypeActivity.Text);
                        id_activity = int.Parse(Convert.ToString(cmd.ExecuteScalar()));

                        cmd = new SqlCommand("UPDATE users SET ID_activity=@id WHERE ID_user=@id_user", connection);
                        cmd.Parameters.AddWithValue("@id", id_activity);
                        cmd.Parameters.AddWithValue("@id_user", PersonalCab.CurrentUser.user_id);
                        cmd.ExecuteNonQuery();

                        // Диагноз

                        cmd = new SqlCommand("SELECT ID_diagnoses FROM Диагнозы WHERE [Название диагноза]=@cel", connection);
                        cmd.Parameters.AddWithValue("@cel", Diagnoses.Text);
                        id_diagnos = int.Parse(Convert.ToString(cmd.ExecuteScalar()));

                        cmd = new SqlCommand("UPDATE users SET ID_diagnoses=@id WHERE ID_user=@id_user", connection);
                        cmd.Parameters.AddWithValue("@id", id_diagnos);
                        cmd.Parameters.AddWithValue("@id_user", PersonalCab.CurrentUser.user_id);
                        cmd.ExecuteNonQuery();

                        connection.Close();

                        // Инициализируем айдишники в текущий класс пользователя
                        PersonalCab.CurrentUser.id_goal = id_goal;
                        PersonalCab.CurrentUser.id_diagnoz = id_diagnos;
                        PersonalCab.CurrentUser.id_typeactivity = id_activity;


                        // Создаем для пользователя новую диету
                        Diet save = PersonalCab.CurrentUser.currentDiet;
                        PersonalCab.CurrentUser.currentDiet = new Diet();


                        if(Diet.IsManyDiets)
                        {
                            ListDiets = Diet.GetCollectionDiets(PersonalCab.CurrentUser.currentDiet.ID_Diets);
                            
                            PersonalCabViewModel.SetDiets(ListDiets); // установка значения в комбобокс 
                            PersonalCabViewModel.SetValueDiets(ListDiets);
                            PersonalCabViewModel.OnElementss(true, System.Windows.Visibility.Visible);
                            PersonalCab.Diets = ListDiets;
                            MessageBox.Show("Диеты подобраны. Выберите диету");
                            this.Close();
                        }
                        else
                        {
                            if (Diet.IsDiet)
                            {
                                MessageBox.Show("Диета подобрана");
                                this.Close();
                            }
                            else
                                PersonalCab.CurrentUser.currentDiet = save;
                        }       
                    }
                }

            }
        }

        // Кнопка принятия изменения значений роста или веса
        private void b1_Click(object sender, RoutedEventArgs e)
        {
            bool check = true;
            double height = 0;
            double weight = 0;
            if (!Double.TryParse(Weight.Text, out weight) || !Double.TryParse(Height.Text, out height))
            {
                MessageBox.Show("Некорректно введены значения");
                check = false;
            }
            else
            {
                if (height < 150 || height > 200 || weight < 45 || weight > 117)
                {
                    check = false;
                    MessageBox.Show("Некорректно введены значения");
                }
            }
            if (check)
            {
                connection.Open();
                cmd = new SqlCommand("UPDATE users SET Рост=@rost WHERE ID_user=@id", connection);
                cmd.Parameters.AddWithValue("@rost", Height.Text);
                cmd.Parameters.AddWithValue("@id", PersonalCab.CurrentUser.user_id);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("UPDATE users SET Вес=@ves WHERE ID_user=@id", connection);
                cmd.Parameters.AddWithValue("@ves", Weight.Text);
                cmd.Parameters.AddWithValue("@id", PersonalCab.CurrentUser.user_id);

                PersonalCab.CurrentUser.Height = float.Parse(Height.Text);
                PersonalCab.CurrentUser.Weight = float.Parse(Weight.Text);

                connection.Close();
               
                b1.IsEnabled = false;
                b1.Visibility = Visibility.Hidden;
                Height.IsEnabled = false;
                Weight.IsEnabled = false;
            }
        }
    }
}
