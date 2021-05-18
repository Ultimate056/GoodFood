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

        public Diet()
        {
            connection.Open();
            cmd = new SqlCommand("SELECT ID_diet FROM Диета WHERE ID_goal=@goal and ID_Диагноза=@diagnoz", connection);
            cmd.Parameters.AddWithValue("@goal", PersonalCab.CurrentUser.id_goal);
            cmd.Parameters.AddWithValue("@diagnoz", PersonalCab.CurrentUser.id_diagnoz);
            ID_Diet = int.Parse(Convert.ToString(cmd.ExecuteScalar()));

            cmd = new SqlCommand("SELECT [Название диеты] FROM Диета WHERE ID_diet=@id", connection);
            cmd.Parameters.AddWithValue("@id", ID_Diet);
            Name = cmd.ExecuteScalar().ToString();

            cmd = new SqlCommand("SELECT Диета FROM Диета WHERE ID_diet=@id", connection);
            cmd.Parameters.AddWithValue("@id", ID_Diet);
            Text = cmd.ExecuteScalar().ToString();

            connection.Close();
        }
    }
}
