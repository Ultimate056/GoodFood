using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GoodFood.MVVM.Model
{
    public class User
    {
        SqlCommand cmd;
        public SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public int user_id { get; set; }
        
        public string name = "";
        public float Height { get; set; }
        public float Weight { get; set; }
        public Diet currentDiet { get; set; }
        public string Login { get; set; }

        public int id_goal { get; set; } = 0;

        public int id_diagnoz { get; set; } = 0;

        public int id_typeactivity { get; set; } = 0;

        private string Password;

        public bool IsDiet; // Для проверки, если диета не будет найдена

        string sex;
        public User(int id_user)
        {
            // Инициализируем объект класса из БД
            user_id = id_user;
            name = ExtractValueFromBD("Имя");
            Height = float.Parse(ExtractValueFromBD("Рост"));
            Weight = float.Parse(ExtractValueFromBD("Вес"));
            sex = ExtractValueFromBD("sex");
            Login = ExtractValueFromBD("Логин");
            Password = ExtractValueFromBD("Пароль");

            // Выполняем метод который возвращает булевое значение, если диета уже была в пользователе
            IsDiet = ContentDiet();
        }


        // Проверка, есть ли у пользователя после входа в программу уже выбранная диета
        public bool ContentDiet()
        {
            connection.Open();

            cmd = new SqlCommand("SELECT ID_diet FROM users WHERE ID_user=@id", connection);
            cmd.Parameters.AddWithValue("@id", user_id);
            string Result = cmd.ExecuteScalar().ToString();
            if(Result != "" || Result !="4")
            {
                cmd = new SqlCommand("SELECT Диета FROM Диета WHERE ID_diet=@id", connection);
                cmd.Parameters.AddWithValue("@id", int.Parse(Result));
                string TextDiet = cmd.ExecuteScalar().ToString();

                cmd = new SqlCommand("SELECT [Название диеты] FROM Диета WHERE ID_diet=@id", connection);
                cmd.Parameters.AddWithValue("@id", int.Parse(Result));
                string NameDiet = cmd.ExecuteScalar().ToString();
                currentDiet = new Diet(NameDiet, TextDiet, int.Parse(Result));
                connection.Close();
                return true;
            }
            connection.Close();
            return false;
        }


        // Извлечь значение по какому-либо столбцу  у пользователя
        public string ExtractValueFromBD(string NameFieldColumn)
        {
            string res = null;
            connection.Open();
            cmd = new SqlCommand($"SELECT {NameFieldColumn} FROM users WHERE ID_user=@user_id", connection);
            cmd.Parameters.AddWithValue("@user_id", user_id);
            try
            {
                res = Convert.ToString(cmd.ExecuteScalar());
                connection.Close();
                return res;
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
                connection.Close();
                return res;
            }
        }

        public string GetName()
        {
            return name;
        }

    }
}
