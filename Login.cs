using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace WindowsFormsApplication3
{
    public partial class Login : Form
    {
#pragma warning disable CS0618 // Type or member is obsolete
        static string connetionString = String.Format("server={0};database={1};uid={2};pwd={3};", ConfigurationSettings.AppSettings["Server"], ConfigurationSettings.AppSettings["Database"], ConfigurationSettings.AppSettings["User"], ConfigurationSettings.AppSettings["Pass"]);
#pragma warning restore CS0618 // Type or member is obsolete
                              // static string connetionString = "server=188.226.193.8;database=ESOL;uid=userss;pwd=Chrvasilis95;";
                              //static string connetionString = "server=localhost;database=esol;uid=root;pwd=Chrvasilis95;";
        MySqlConnection cnn = new MySqlConnection(connetionString);
        Boolean check = true;
        public Login()
        { 
            InitializeComponent();
          //  MessageBox.Show(ConfigurationSettings.AppSettings["Server"]);
            // cnn.Open();
            // InternetGetConnectedState(out description, 0); 
            checkNet();
           
            if (Properties.Settings.Default.Username != string.Empty)
            {
                textBox1.Text = Properties.Settings.Default.Username;
                textBox2.Text = Properties.Settings.Default.Password;
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoginMeth();
           
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                LoginMeth();
            }
        }
        private void LoginMeth() {
            if (check == true)
            {
                bool validA = textBox1.Text.All(c => Char.IsLetterOrDigit(c) || c.Equals('_'));
                bool validB = textBox2.Text.All(c => Char.IsLetterOrDigit(c) || c.Equals('_'));
                if (validA == true && validB == true)
                {

                    MySqlCommand cmd = cnn.CreateCommand();
                    string sg =string.Format("Select UserName,ID from Users where binary UserName='{0}' and binary Password='{1}'",textBox1.Text, textBox2.Text);
                    cmd.CommandText = sg;
                    //cmd.Parameters.AddWithValue("@user", textBox1.Text);
                    //cmd.Parameters.AddWithValue("@pass", textBox2.Text);
                    MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adap.Fill(table);
                    cnn.Close();

                    if (table.Rows.Count > 0)
                    {
                        string name = table.Rows[0].Field<string>(0);
                        string id = table.Rows[0].Field<string>(1);
                        //   label3.ForeColor = Color.Green;
                        if (Remember1.Checked)
                        {
                            Properties.Settings.Default.Username = textBox1.Text;
                            Properties.Settings.Default.Password = textBox2.Text;
                            Properties.Settings.Default.Save();
                        }
                        //  Close();
                        this.Hide();
                        Form2 l = new Form2(id,name);
                        l.Size = new Size(1386,635);
                      //  l.Size = new Size(1386,492);
                        l.Show();
                    }


                    else
                    {
                        label3.Text = "Incorrect login";
                        label3.ForeColor = Color.Red;
                        // MessageBox.Show("Incorrect login");
                    }
                }
                else
                {
                    label3.ForeColor = Color.Red;
                }
            }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(1);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            checkNet();
        }

        private void checkNet()
        {
            try
            {
                System.Net.WebRequest myRequest = System.Net.WebRequest.Create("http://www.google.com");
                System.Net.WebResponse myResponse = myRequest.GetResponse();
                try
                {
                    cnn.Open();
                    MySqlCommand cmd = cnn.CreateCommand();
                    cmd.CommandText = "Select * from Book;";
                    cmd.ExecuteNonQuery();
                    label3.Text = "Connected!";
                    label3.ForeColor = Color.Green;
                    check = true;

                }
                catch
                {
                    label3.Text = "Lost Connection!";
                    label3.ForeColor = Color.Purple;
                    check = false;
                }

            }
            catch
            {
                label3.Text = "No internet connection!";
                label3.ForeColor = Color.Red;
                check = false;


            }
        }
    }
}
