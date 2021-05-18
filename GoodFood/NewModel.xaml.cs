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



        public NewModel()
        {
            InitializeComponent();
            Height.Text = Convert.ToString(PersonalCab.CurrentUser.Height);
            Weight.Text = Convert.ToString(PersonalCab.CurrentUser.Weight);
            goals.ItemsSource = goalss;
            TypeActivity.ItemsSource = activites;
            Diagnoses.ItemsSource = diagnoses;

        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (!Height.IsEnabled)
                Height.IsEnabled = true;
            else
                Height.IsEnabled = false;
        }

        private void Hyperlink_Click2(object sender, RoutedEventArgs e)
        {
            if (!Weight.IsEnabled)
                Weight.IsEnabled = true;
            else
                Weight.IsEnabled = false;
        }
        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            double IMT = Math.Round((PersonalCab.CurrentUser.Weight) / (Math.Pow((Convert.ToDouble(PersonalCab.CurrentUser.Height) / 100), 2)), 1);
            int id_goal = 0;
            int id_activity = 0;
            int id_diagnos = 0;
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
                MessageBox.Show("Ошибка в расчетах!!!", "Что-то пошло не так:(");
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

                PersonalCab.CurrentUser.id_goal = id_goal;
                PersonalCab.CurrentUser.id_diagnoz = id_diagnos;
                PersonalCab.CurrentUser.id_typeactivity = id_activity;
                PersonalCab.CurrentUser.currentDiet = new Diet();

                MessageBox.Show("Диета подобрана");
                this.Close();
            }


        }



    }
}
