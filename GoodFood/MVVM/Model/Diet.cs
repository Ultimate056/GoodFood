using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
        SqlDataAdapter adapter;
        
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public string Name { get; set; } = " ";

        public string Text { get; set; } = " ";

        public int ID_Diet { get; set; } = 0;

        public static bool IsDiet { get; set; }

        public List<int> ID_Diets;

        public static bool IsManyDiets;
        public Diet()
        {
            connection.Open();
            adapter = new SqlDataAdapter($"SELECT ID_diet FROM Диета WHERE ID_goal={PersonalCab.CurrentUser.id_goal} and ID_Диагноза={PersonalCab.CurrentUser.id_diagnoz}", connection);
            DataSet TableID_Diets = new DataSet();
            adapter.Fill(TableID_Diets);
            foreach(DataTable dt in TableID_Diets.Tables) // Здесь одна таблица
            {
                if (dt.Rows.Count > 1) // Если найденных айдишников диет много с таким условием, то добавляем их в массив
                {
                    IsManyDiets = true;
                    ID_Diets = new List<int>();
                    DataRowCollection tempRow = dt.Rows;
                    for (int i = 0; i < tempRow.Count; i++)
                    {
                        object R = tempRow[i].ItemArray.FirstOrDefault();
                        ID_Diets.Add(int.Parse(R.ToString()));
                    }      

                }
                else
                {
                    IsManyDiets = false;
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
                        AcceptChanges(ID_Diet);
                        IsDiet = true;
                    }
                }
                    
            }
            connection.Close();
        }

        public static void AcceptChanges(int id_diet)
        {
            if(connection.State != ConnectionState.Open)
                connection.Open();
            SqlCommand cmd = new SqlCommand("UPDATE users SET ID_diet=@id WHERE ID_user=@id2", connection);
            cmd.Parameters.AddWithValue("@id", id_diet);
            cmd.Parameters.AddWithValue("@id2", PersonalCab.CurrentUser.user_id);
            cmd.ExecuteNonQuery();
            connection.Close();
        }


        public static List<Diet> GetCollectionDiets(List<int> ID_Diets)
        {
            List<Diet> diets = new List<Diet>();
            connection.Open();
            int i = 1;
            foreach (int id in ID_Diets)
            {
                SqlCommand cmd;
                cmd = new SqlCommand("SELECT [Название диеты] FROM Диета WHERE ID_diet=@id", connection);
                cmd.Parameters.AddWithValue("@id", id);
                string Name = i + ". " + cmd.ExecuteScalar().ToString();
                cmd = new SqlCommand("SELECT Диета FROM Диета WHERE ID_diet=@id", connection);
                cmd.Parameters.AddWithValue("@id", id);
                string Text = cmd.ExecuteScalar().ToString();
                diets.Add(new Diet(Name, Text, id));
                i++;
            }
            connection.Close();
            return diets;
        }


        public Diet(String name, String text, int id)
        {
            Name = name;
            Text = text;
            ID_Diet = id;
        }
    }
}
