using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class AddDriver : Form
    {
        private bool ch1;
        private bool ch;
#pragma warning disable CS0618 // Type or member is obsolete
        static string connetionString = String.Format("server={0};database={1};uid={2};pwd={3};", ConfigurationSettings.AppSettings["Server"], ConfigurationSettings.AppSettings["Database"], ConfigurationSettings.AppSettings["User"], ConfigurationSettings.AppSettings["Pass"]);
#pragma warning restore CS0618 // Type or member is obsolete
                              // static string connetionString = "server=188.226.193.8;database=ESOL;uid=userss;pwd=Chrvasilis95;";
                              //static string connetionString = "server=localhost;database=esol;uid=root;pwd=Chrvasilis95;";
        MySqlConnection cnn = new MySqlConnection(connetionString);
        private DataSet ds = new DataSet();

        public AddDriver()
        {
            InitializeComponent();

            cnn.Open();

            refresh1();

            DataGridViewColumn column0 = dataGridView1.Columns[0];
            column0.Width = 20;
            DataGridViewColumn column1 = dataGridView1.Columns[1];
            column1.Width = 100;
            DataGridViewColumn column2 = dataGridView1.Columns[2];
            column2.Width = 100;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 15)
            {
                textBox1.BackColor = Color.Red;
                ch = false;
                button2.Hide();
            }
            else
            {
                textBox1.BackColor = Color.White;
                ch = true;
                button2.Show();
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals("FirstName"))
            {
                textBox1.ForeColor = Color.Black;
                textBox1.Text = "";
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 15)
            {
                textBox1.BackColor = Color.Red;
                ch = false;
                button2.Hide();
            }
            else
            {
                textBox1.BackColor = Color.White;
                ch = true;
                button2.Show();
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text.Equals("LastName"))
            {
                textBox2.ForeColor = Color.Black;
                textBox2.Text = "";
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (!int.TryParse(textBox3.Text, out n) && !textBox3.Text.Equals("ID") && !textBox3.Text.Equals(""))
            {
                textBox3.BackColor = Color.Red;
                ch1 = false;
                button3.Hide();
            }
            else
            {
                textBox2.BackColor = Color.White;
                ch1 = true;
                button3.Show();
            }
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (textBox3.Text.Equals("ID"))
            {
                textBox3.ForeColor = Color.Black;
                textBox3.Text = "";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DeleteMeth();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Addmeth();
            textBox1.Text = "FirstName";
            textBox2.Text = "LastName";
            textBox1.ForeColor = Color.Gray;
            textBox2.ForeColor = Color.Gray;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cnn.Close();
            this.Close();
        }

        private void DeleteMeth()
        {
            if (ch1 && !textBox3.Text.Equals("ID"))
            {
                try
                {
                    MySqlCommand cmd = cnn.CreateCommand();
                    cmd.CommandText = "update Book set Driver=NULL where Driver=@i";
                    cmd.Parameters.AddWithValue("@i", textBox3.Text);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "Delete from Drivers where ID=@id ;";
                    cmd.Parameters.AddWithValue("@id", textBox3.Text);
                    cmd.ExecuteNonQuery();
                    textBox3.Text = "ID";
                    textBox3.ForeColor = Color.Gray;
                    refresh1();
                }
                catch { MessageBox.Show("ERRor"); }
            }
        }

        private void refresh1()
        {
            try
            {
                ds.Clear();
                MySqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select * from Drivers ;";
                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                adap.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
            }
            catch { }
        }

        private void Addmeth()
        {
            if (ch && !textBox1.Text.Equals("FirstName") && !textBox2.Text.Equals("LastName"))
            {

                try
                {
                    MySqlCommand cmd = cnn.CreateCommand();
                    cmd.CommandText = "insert into Drivers(FirstName,LastName) values(@fn,@ln) ;";
                    cmd.Parameters.AddWithValue("@fn", textBox1.Text);
                    cmd.Parameters.AddWithValue("@ln", textBox2.Text);
                    cmd.ExecuteNonQuery();
                    textBox1.Text = "FirstName";
                    textBox1.ForeColor = Color.Gray;
                    textBox2.Text = "LastName";
                    textBox2.ForeColor = Color.Gray;
                    refresh1();
                }
                catch { }
            }
        }

    }
}
