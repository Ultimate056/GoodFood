using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;


namespace GoodFood
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {

        SqlCommand cmd;
        public SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public Registration()
        {
            InitializeComponent();
        }

        bool CheckNullBox(TextBox t)
        {
            if (t.Text == "")
                return true;
            else
                return false;
        }
        bool CheckNullBox(PasswordBox p)
        {
            if (p.Password == "")
                return true;
            else
                return false;
        }

        private void ButtonClickRegist(object sender, RoutedEventArgs e)
        {
            bool check = true;

            // Проверка логина, пароля

            if (CheckNullBox(Login))
            {
                check = false;
                LoginError.Content = "Пустое поле";
            }
            if (CheckNullBox(Password))
            {
                check = false;
                PasswordError.Content = "Пустой поле";
            }
            if (CheckNullBox(PasswordAgain))
            {
                check = false;
                PasswordAgainError.Content = "Пустой поле";
            }
            if (MainWindow.CheckRusLetter(Login.Text) || MainWindow.CheckRusLetter(Password.Password) || MainWindow.CheckRusLetter(PasswordAgain.Password))
            {
                check = false;
                MainError.Content = "Русские буквы недопустимы в логине или пароле";
            }
            if (Password.Password != PasswordAgain.Password)
            {
                check = false;
                MainError.Content = "Пароли не совпадают";
            }
            // Проверка остальных полей
            if (Name.Text == "")
            {
                check = false;
                NameError.Content = "Незаполненные поля в ФИО";
            }
            if (SexMale.IsChecked == false && SexFemale.IsChecked == false)
            {
                check = false;
                SexError.Content = "Не выбран пол";
            }
            if (!DateBirthday.SelectedDate.HasValue)
            {
                check = false;
                DateError.Content = "Не выбрана дата рождения";
            }
            else
            {
                if (DateTime.Now.Year - DateBirthday.SelectedDate.Value.Year < 17)
                {
                    check = false;
                    DateError.Content = "Вам не должно быть меньше 17 лет";
                }
            }
            // Проверка роста и веса
            double height = 0, weight = 0;
            if (!Double.TryParse(Height.Text, out height) || !Double.TryParse(Weight.Text, out weight))
            {
                check = false;
                HWError.Content = "Некорректно введены значения";
            }
            else
            {
                if (height < 150 || height > 200 || weight < 45 || weight > 117)
                {
                    check = false;
                    HWError.Content = "Некорректно введены значения";
                }
            }

            if (check)
            {
                //БД
                string sex;
                if (SexMale.IsChecked == true) 
                    sex = "мужской";
                else
                    sex = "женский";
                try
                {
                    bool isLogin = false;
                    SqlCommand cmdCheckLogin = new SqlCommand($"SELECT Логин FROM users WHERE Логин=@loginn",connection);
                    connection.Open();
                    cmdCheckLogin.Parameters.AddWithValue("@loginn", Login.Text);
                    try
                    {
                        string em = Convert.ToString(cmdCheckLogin.ExecuteScalar());
                        if (em != "")
                            isLogin = true;
                        connection.Close();
                    }
                    catch (Exception es)
                    {
                        MessageBox.Show("Ошибка проверки логина" + es.Message);
                        connection.Close();
                    }

                    if(!isLogin)
                    {
                        cmd = new SqlCommand("insert into users (Имя, Логин ,sex, [Дата Рождения], Пароль, Рост, Вес,ID_diagnoses, ID_goal, ID_activity, ID_diet) values (@name, @email, @sex, @date, @pas, @rost, @wes,4,4,4,4)", connection);
                        connection.Open();
                        cmd.Parameters.AddWithValue("@name", Name.Text);
                        cmd.Parameters.AddWithValue("@email", Login.Text);
                        cmd.Parameters.AddWithValue("@sex", sex);
                        cmd.Parameters.AddWithValue("@date", DateBirthday.SelectedDate);
                        cmd.Parameters.AddWithValue("@pas", Password.Password);
                        cmd.Parameters.AddWithValue("@rost", Height.Text);
                        cmd.Parameters.AddWithValue("@wes", Weight.Text);
                        
                        cmd.ExecuteNonQuery();
                        connection.Close();

                        MessageBox.Show("Регистрация прошла успешно");
                        this.Close();
                    }
                    else
                        MessageBox.Show("Аккаунт с таким именем уже существует");
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка регистрации" + ex.Message);
                }
            }
        }
        private void Login_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!CheckNullBox(Login))
                LoginError.Content = "";
            if (!(MainWindow.CheckRusLetter(Login.Text) || MainWindow.CheckRusLetter(Password.Password) || MainWindow.CheckRusLetter(PasswordAgain.Password)))
                MainError.Content = "";
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!CheckNullBox(Password))
                PasswordError.Content = "";
            if (!(MainWindow.CheckRusLetter(Login.Text) || MainWindow.CheckRusLetter(Password.Password) || MainWindow.CheckRusLetter(PasswordAgain.Password)))
                MainError.Content = "";
            if (Password.Password == PasswordAgain.Password)
                MainError.Content = "";
        }

        private void PasswordAgain_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!CheckNullBox(PasswordAgain))
                PasswordAgainError.Content = "";
            if (!(MainWindow.CheckRusLetter(Login.Text) || MainWindow.CheckRusLetter(Password.Password) || MainWindow.CheckRusLetter(PasswordAgain.Password)))
                MainError.Content = "";
            if (Password.Password == PasswordAgain.Password)
                MainError.Content = "";
        }

        private void SecondName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Name.Text != "")
                NameError.Content = "";
        }


        private void DateBirthday_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DateBirthday.SelectedDate.HasValue)
                DateError.Content = "";
            if (DateTime.Now.Year - DateBirthday.SelectedDate.Value.Year >= 17)
                DateError.Content = "";
        }

        private void HeightHuman_TextChanged(object sender, TextChangedEventArgs e)
        {
            double height = 0, weight = 0;
            if (Double.TryParse(Height.Text, out height) || Double.TryParse(Weight.Text, out weight))
            {
                if (height < 150 || height > 200 || weight < 45 || weight > 117)
                    HWError.Content = "";
            }
        }

        private void SexMale_Checked(object sender, RoutedEventArgs e)
        {
            if (SexMale.IsChecked == true || SexFemale.IsChecked == true)
                SexError.Content = "";
        }

    }
}
