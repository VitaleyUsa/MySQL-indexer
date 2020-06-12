using MySql.Data.MySqlClient;
using System;
using System.Data;

//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

//using System.Drawing;
//using System.Linq;
//using System.Text;
using System.Windows.Forms;

namespace MySQL_indexer
{
    public partial class Form1 : Form
    {
        private string Year = "2020";
        private int _savebutton;

        public int SaveButton
        {
            get { return _savebutton; }
            set
            {
                _savebutton = value;
                if (_savebutton == 0)
                {
                    button2.Enabled = false;
                }
                else
                {
                    button2.Enabled = true;
                }
            }
        }

        public Form1()
        {
            InitializeComponent();

            for (int i = 0; i < Properties.Settings.Default.PassNumber; i++)
                Properties.Settings.Default.Password = Properties.Settings.Default.Password.DecryptString();

            Properties.Settings.Default.PassNumber = 0;

            InputName.Text = Properties.Settings.Default.Username;
            InputPassword.Text = Properties.Settings.Default.Password;
            InputDB.Text = Properties.Settings.Default.Database;
            InputServer.Text = Properties.Settings.Default.Server;
        }

        // Сохраняем настройки
        private void SaveSettings()
        {
            Properties.Settings.Default.PassNumber += 1;
            Properties.Settings.Default.Password = Properties.Settings.Default.Password.EncryptString();
            Properties.Settings.Default.Save();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            Properties.Settings.Default.Username = InputName.Text;
            Properties.Settings.Default.Password = InputPassword.Text;
            Properties.Settings.Default.Database = InputDB.Text;
            Properties.Settings.Default.Server = InputServer.Text;

            string Database = InputDB.Text;
            if (Database == "") Database = "enotdb";
            string Source = InputServer.Text;
            if (Source == "") Source = "localhost";
            string User = InputName.Text;
            if (User == "") User = "enot";

            string Password = InputPassword.Text;
            string con = "Server=" + Source + ";Database=" + Database + ";Uid=" + User + ";Pwd=" + Password + ";SslMode=Preferred";

            try
            {
                string query = "select Distinct DocDate, RegNumPrefix, RegNumIndex from NotarialActs where YEAR(docDate)='" + Year + "' Order by RegNumPrefix, RegNumIndex";

                MySqlConnection connection = new MySqlConnection(con);
                MySqlCommand cmd = new MySqlCommand(query, connection);
                connection.Open();

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                string[] prefixes = new string[dt.Rows.Count];

                int counter = 0;
                int counter_prefix = 0;
                int counter_1time = 0;
                string prev_prefix;

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int index = Convert.ToInt32(reader["RegNumIndex"]);
                        string prefix = reader["RegNumPrefix"].ToString();

                        prefixes[counter_prefix] = Convert.ToString(reader["RegNumPrefix"]);

                        if (counter_1time == 0)
                        {
                            counter_1time++;
                            prev_prefix = prefixes[counter_prefix];
                        }
                        else
                        {
                            prev_prefix = prefixes[counter_prefix - 1];
                        }

                        if ((prefixes[counter_prefix] != prev_prefix) || (index < counter))
                            counter = 0;

                        if (index == counter)
                            counter = index - 1;

                        counter_prefix++;

                        //Console.WriteLine(counter.ToString() + " | " + index.ToString());

                        if (counter + 1 < index) 
                        {
                            while (counter + 1 < index)
                            {
                                counter++;
                                //Console.WriteLine("count = " + counter.ToString() );
                                listView1.Items.Add(prefix + counter);
                            }
                        }
                        counter++;
                    }
                    reader.Close();
                }
                connection.Close();

                SaveSettings();
            }
            catch
            {
                MessageBox.Show("Возникла ошибка. Проверьте правильность введеных данных. \n \n");
            }
            if (listView1.Items.Count > 0) { SaveButton = 1; }
        }

        private void Year_2020_Click(object sender, EventArgs e)
        {
            Year = "2020";
            listView1.Clear();
            year_2016.Font = new Font(year_2016.Font, FontStyle.Regular);
            year_2017.Font = new Font(year_2017.Font, FontStyle.Regular);
            year_2018.Font = new Font(year_2018.Font, FontStyle.Regular);
            year_2019.Font = new Font(year_2019.Font, FontStyle.Regular);
            year_2020.Font = new Font(year_2020.Font, FontStyle.Bold);
            SaveButton = 0;
        }

        private void Year_2019_Click(object sender, EventArgs e)
        {
            Year = "2019";
            listView1.Clear();
            year_2016.Font = new Font(year_2016.Font, FontStyle.Regular);
            year_2017.Font = new Font(year_2017.Font, FontStyle.Regular);
            year_2018.Font = new Font(year_2018.Font, FontStyle.Regular);
            year_2019.Font = new Font(year_2019.Font, FontStyle.Bold);
            year_2020.Font = new Font(year_2020.Font, FontStyle.Regular);
            SaveButton = 0;
        }

        private void Year_2018_Click(object sender, EventArgs e)
        {
            Year = "2018";
            listView1.Clear();
            year_2016.Font = new Font(year_2016.Font, FontStyle.Regular);
            year_2017.Font = new Font(year_2017.Font, FontStyle.Regular);
            year_2018.Font = new Font(year_2018.Font, FontStyle.Bold);
            year_2019.Font = new Font(year_2019.Font, FontStyle.Regular);
            year_2020.Font = new Font(year_2020.Font, FontStyle.Regular);
            SaveButton = 0;
        }

        private void Year_2017_Click(object sender, EventArgs e)
        {
            Year = "2017";
            listView1.Clear();
            year_2016.Font = new Font(year_2016.Font, FontStyle.Regular);
            year_2017.Font = new Font(year_2017.Font, FontStyle.Bold);
            year_2018.Font = new Font(year_2018.Font, FontStyle.Regular);
            year_2019.Font = new Font(year_2019.Font, FontStyle.Regular);
            year_2020.Font = new Font(year_2020.Font, FontStyle.Regular);
            SaveButton = 0;
        }

        private void Year_2016_Click(object sender, EventArgs e)
        {
            Year = "2016";
            listView1.Clear();
            year_2016.Font = new Font(year_2016.Font, FontStyle.Bold);
            year_2017.Font = new Font(year_2017.Font, FontStyle.Regular);
            year_2018.Font = new Font(year_2018.Font, FontStyle.Regular);
            year_2019.Font = new Font(year_2019.Font, FontStyle.Regular);
            year_2020.Font = new Font(year_2020.Font, FontStyle.Regular);
            SaveButton = 0;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count > 0)
            {
                string txt = "Пропущенные номера_" + Year + ".txt";
                StreamWriter sw = new StreamWriter(txt, true);
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    sw.WriteLine(listView1.Items[i].Text);
                }
                sw.Close();
                Process.Start(txt);
            }
        }
    }

    #region Соленье / приправы

    public static class StringSecurityHelper
    {
        private static readonly byte[] entropy = Encoding.Unicode.GetBytes("1AQ'&mc %;lq@31*@!)@asd@.,,z2389");

        public static string DecryptString(this string encryptedData)
        {
            if (encryptedData == null)
            {
                return null;
            }

            try
            {
                byte[] decryptedData = ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), entropy, DataProtectionScope.CurrentUser);

                return Encoding.Unicode.GetString(decryptedData);
            }
            catch
            {
                return null;
            }
        }

        public static string EncryptString(this string input)
        {
            if (input == null)
            {
                return null;
            }

            byte[] encryptedData = ProtectedData.Protect(Encoding.Unicode.GetBytes(input), entropy, DataProtectionScope.CurrentUser);

            return Convert.ToBase64String(encryptedData);
        }
    }

    #endregion Соленье / приправы
}