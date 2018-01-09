using System;
using System.Collections.Generic;
using System.Windows;
//для работы с App.config/Web.config
using System.Configuration;

using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Documents;

namespace AdoModule01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _conectionString = "";
        private string _conectionStringOleDb = "";
        public MainWindow()
        {
            InitializeComponent();

            _conectionString =
                ConfigurationManager
                    .ConnectionStrings["DefaultConnection"]
                    .ConnectionString;

            _conectionStringOleDb =
               ConfigurationManager
                   .ConnectionStrings["OleDbConnection"]
                   .ConnectionString;
        }


        private void ConnectToServerButton_Click(object sender, RoutedEventArgs e)
        {
            #region exml 1
            SqlConnection connection =
                new SqlConnection(_conectionString);

            try
            {
                connection.Open();
                ConnectMessage.Text += "Подключение открыто ... \n";
            }
            catch (Exception ex)
            {
                ConnectMessage.Text += ex.Message + " \n";
            }
            finally
            {
                connection.Close();
                ConnectMessage.Text += "Подключение закрыто ... \n";
            }
            #endregion

            #region exml 2

            ConnectMessage.Text = "";
            using (SqlConnection conn =
                new SqlConnection(_conectionString))
            {
                conn.Open();
                ConnectMessage.Text += "Подключение открыто ... \n\n";
                //Получим информацию о подключении
                ConnectMessage.Text += "Свойство подключения: \n";

                ConnectMessage.Text +=
                    "\t --> строка подключения: "
                    + conn.ConnectionString + "\n";

                ConnectMessage.Text +=
                    "\t --> база данных: "
                    + conn.Database + "\n";

                ConnectMessage.Text +=
                    "\t --> сервер: "
                    + conn.ServerVersion + "\n";

                ConnectMessage.Text +=
                    "\t --> состояние: "
                    + conn.State + "\n";

                ConnectMessage.Text +=
                    "\t --> workstationId: "
                    + conn.WorkstationId + "\n";
            }

            ConnectMessage.Text += "\n Подключение закрыто ... \n";
            #endregion

            #region exmpl 3
            //ConnectMessage.Text = "";
            //using (OleDbConnection conn =
            //    new OleDbConnection(_conectionStringOleDb))
            //{
            //    conn.Open();
            //    ConnectMessage.Text += "Подключение открыто ... \n\n";

            //    //Получим информацию о подключении
            //    ConnectMessage.Text += "Свойство подключения: \n";

            //    ConnectMessage.Text +=
            //        "\t --> строка подключения: "
            //        + conn.ConnectionString + "\n";

            //    ConnectMessage.Text +=
            //        "\t --> база данных: "
            //        + conn.Database + "\n";

            //    ConnectMessage.Text +=
            //        "\t --> сервер: "
            //        + conn.ServerVersion + "\n";

            //    ConnectMessage.Text +=
            //        "\t --> состояние: "
            //        + conn.State + "\n";

            //}
            #endregion
        }

        private void GetServerDataButton_Click(object sender, RoutedEventArgs e)
        {
            #region Connect to server
            List<Equiment> equiments = new List<Equiment>();
            ConnectMessage.Text = "";

            Stopwatch sw = new Stopwatch();
            sw.Start();

            using (SqlConnection conn =
                new SqlConnection("Data Source=192.168.111.154;Initial Catalog=MCS;User ID=sa;Password=Mc123456"))
            {
                conn.Open();
                SqlCommand cmd =
                    new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select * from newEquipment";

                SqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    Equiment eq =new Equiment();
                    eq.EquipmentId = Int32.Parse(reader[0].ToString());
                    eq.GarageRoom = reader[1].ToString();

                    equiments.Add(eq);
                }
            }
            sw.Stop();
            
            ListViewEquipment.ItemsSource = equiments;

            MessageBox.Show(sw.ElapsedMilliseconds.ToString());

            #endregion
        }
    }

    public class Equiment
    {
        public int EquipmentId { get; set; }
        public string GarageRoom { get; set; }
    }
}
