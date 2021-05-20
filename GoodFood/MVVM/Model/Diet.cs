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
    public class Diet
    {
        SqlCommand cmd;
        public SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public string Name { get; set; } = " ";

        public string Text { get; set; } = " ";

        public int ID_Diet { get; set; } = 0;

        public static bool IsDiet { get; set; }

        public Diet()
        {
            connection.Open();
            cmd = new SqlCommand("SELECT ID_diet FROM Диета WHERE ID_goal=@goal and ID_Диагноза=@diagnoz", connection);
            cmd.Parameters.AddWithValue("@goal", PersonalCab.CurrentUser.id_goal);
            cmd.Parameters.AddWithValue("@diagnoz", PersonalCab.CurrentUser.id_diagnoz);
            string res = Convert.ToString(cmd.ExecuteScalar());
            if (res == "")
            {
                MessageBox.Show("Для вас диеты нет");
                IsDiet = false;
            }        
            else
            {
                ID_Diet = int.Parse(res);
                cmd = new SqlCommand("SELECT [Название диеты] FROM Диета WHERE ID_diet=@id", connection);
                cmd.Parameters.AddWithValue("@id", ID_Diet);
                res = cmd.ExecuteScalar().ToString();
                Name = "Ваша наиболее подходящая диета: " + res;
                cmd = new SqlCommand("SELECT Диета FROM Диета WHERE ID_diet=@id", connection);
                cmd.Parameters.AddWithValue("@id", ID_Diet);
                Text = cmd.ExecuteScalar().ToString();

                cmd = new SqlCommand("UPDATE users SET ID_diet=@id WHERE ID_user=@id2", connection);
                cmd.Parameters.AddWithValue("@id", ID_Diet);
                cmd.Parameters.AddWithValue("@id2", PersonalCab.CurrentUser.user_id);
                cmd.ExecuteNonQuery();
                IsDiet = true;
            }
            connection.Close();
        }


        public Diet(String name, String text, int id)
        {
            Name = name;
            Text = text;
            ID_Diet = id;
        }
    }
}
