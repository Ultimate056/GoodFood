using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace GoodFood
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        SqlCommand cmd;
        public SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        string userID = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        // -* Methods

        public static bool CheckRusLetter(string Text)
        {
            if (Regex.IsMatch(Text, "^[А-Яа-я]+$"))
                return true;
            else
                return false;
        }



        // -* Events

        // Вход в лк
        private void PersonalCab_Button(object sender, RoutedEventArgs e)
        {
            bool check = true;
            if (Login.Text == "" || Password.Password == "")
            {
                MessageBox.Show("Пустые поля. Заполните логин и пароль. Ошибка входа");
                check = false;
            }
            if (CheckRusLetter(Login.Text) || CheckRusLetter(Password.Password))
            {
                MessageBox.Show("Русская кириллица недопустима в логине или пароле! Ошибка входа");
                check = false;
            }

            //БД
            userID = ID(Login.Text, Password.Password);
            if (userID == "") // Проверка на вход
            {
                MessageBox.Show("Неправильный логин или пароль");
                check = false;
            }

            if (check) // Если все норм, то загрузить окно с личным кабинетом + должны подключиться все привязанные данные
            {
                PersonalCab cab = new PersonalCab(userID);
                cab.Show();
                this.Close();
            }
        }

        // Кнопка регистрации
        private void ClickRegistr(object sender, RoutedEventArgs e)
        {
            Registration reg = new Registration();
            reg.Show();
        }


        public string Min()
        {
            connection.Open();
            cmd = new SqlCommand("select min(ID_user) from[users]", connection);
            string MinUserID = Convert.ToString(cmd.ExecuteScalar());
            connection.Close();
            return MinUserID;
        }//БД

        public string Max()
        {
            connection.Open();
            cmd = new SqlCommand("select max(ID_user) from[users]", connection);
            string MaxUserID = Convert.ToString(cmd.ExecuteScalar());
            connection.Close();
            return MaxUserID;
        }        //БД
        


        // Проверка на вход
        public string ID(string login, string password)
        {
            string usersProdID = "";
            connection.Open();
            cmd = new SqlCommand($"SELECT ID_User FROM users WHERE email=@login and Пароль=@password", connection);
            cmd.Parameters.AddWithValue("@login", login);
            cmd.Parameters.AddWithValue("@password", password);
            try
            {
                usersProdID = Convert.ToString(cmd.ExecuteScalar());
                connection.Close();
                return usersProdID;
            }
            catch(Exception es)
            {
                MessageBox.Show(es.Message);
                connection.Close();
                return usersProdID;
            }
        }

    }
}
