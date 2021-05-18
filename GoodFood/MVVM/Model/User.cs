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

        string sex;
        public User(int id_user)
        {
            user_id = id_user;
            name = ExtractValueFromBD("ФИО");
            Height = float.Parse(ExtractValueFromBD("Рост"));
            Weight = float.Parse(ExtractValueFromBD("Вес"));
            sex = ExtractValueFromBD("sex");
            Login = ExtractValueFromBD("email");
            Password = ExtractValueFromBD("Пароль");
        }

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
