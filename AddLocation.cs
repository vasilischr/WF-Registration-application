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
    public partial class AddLocation : Form
    {
        Boolean ch = false;
        Boolean ch1 = false;
        string table;
#pragma warning disable CS0618 // Type or member is obsolete
        static string connetionString = String.Format("server={0};database={1};uid={2};pwd={3};", ConfigurationSettings.AppSettings["Server"], ConfigurationSettings.AppSettings["Database"], ConfigurationSettings.AppSettings["User"], ConfigurationSettings.AppSettings["Pass"]);
#pragma warning restore CS0618 // Type or member is obsolete
                              // static string connetionString = "server=188.226.193.8;database=ESOL;uid=userss;pwd=Chrvasilis95;";
                              //static string connetionString = "server=localhost;database=esol;uid=root;pwd=Chrvasilis95;";
        MySqlConnection cnn = new MySqlConnection(connetionString);
        private DataSet ds=new DataSet();

        public AddLocation(string tb)
        {
            InitializeComponent();

            table = tb;

            textBox1.Text = table;
            textBox3.Text = "Van Price";
            textBox4.Text = "Taxi Price";

            textBox1.ForeColor = Color.Gray;
            textBox2.ForeColor = Color.Gray;
            textBox3.ForeColor = Color.Gray;
            textBox4.ForeColor = Color.Gray;
            if (!tb.Equals("DAddress"))
            {
                textBox3.Hide();
                textBox4.Hide();
                dataGridView1.Width = 168;
                this.Size = new Size(291, 261);
            }

            cnn.Open();

            refresh1();

            DataGridViewColumn column0 = dataGridView1.Columns[0];
            column0.Width = 23;
            DataGridViewColumn column1 = dataGridView1.Columns[1];
            column1.Width = 100;
            if (tb.Equals("DAddress")) {
                DataGridViewColumn column2 = dataGridView1.Columns[2];
                column2.Width = 35;
                DataGridViewColumn column3 = dataGridView1.Columns[3];
                column3.Width = 35;
            }
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
            if (textBox1.Text.Length > 15 || textBox1.Text.Equals(""))
            {
                textBox1.BackColor = Color.Red;
                ch = false;
                button1.Hide();
            }
            else {
                textBox1.BackColor = Color.White;
                ch = true;
                button1.Show();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (!int.TryParse(textBox2.Text, out n) && !textBox2.Text.Equals("ID") && !textBox2.Text.Equals(""))
            {
                textBox2.BackColor = Color.Red;
                ch1 = false;
                button2.Hide();
            }
            else
            {
                textBox2.BackColor = Color.White;
                ch1 = true;
                button2.Show();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                DeleteMeth();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                Addmeth();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cnn.Close();
            this.Close();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.ForeColor = Color.Black;
            textBox1.Text = "";
            textBox2.ForeColor = Color.Gray;
            textBox2.Text = "ID";
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBox2.ForeColor = Color.Black;
            textBox2.Text = "";
            textBox1.ForeColor = Color.Gray;
            textBox1.Text = table;
            textBox3.ForeColor = Color.Gray;
            textBox3.Text = "Van Price";
            textBox4.ForeColor = Color.Gray;
            textBox4.Text = "Taxi Price";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Addmeth();     
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DeleteMeth();
        }

        private void refresh1() {
            ds.Clear();
            MySqlCommand cmd = cnn.CreateCommand();
            if (table.Equals("DAddress"))
            {
                cmd.CommandText = "select ID,location,VPrice as VAN,TPrice as TAXI from " + table + " ;";
            }
            else {
                cmd.CommandText = "select * from " + table + " ;";
            }
            MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
            adap.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0].DefaultView;
        }

        private void DeleteMeth() {
            if (ch1 && !textBox2.Text.Equals("ID"))
            {
                try
                {
                    MySqlCommand cmd = cnn.CreateCommand();
                    cmd.CommandText = "Delete from " + table + " where ID=@id ;";
                    cmd.Parameters.AddWithValue("@id", textBox2.Text);
                    cmd.ExecuteNonQuery();
                    textBox2.Text = "ID";
                    textBox2.ForeColor = Color.Gray;
                    refresh1();
                }
                catch { }
            }
        }

        private void Addmeth() {
            MessageBox.Show(ch.ToString());
            if (ch && !textBox1.Text.Equals(table))
            {
                try
                {
                    MySqlCommand cmd = cnn.CreateCommand();
                    if (!table.Equals("DAddress"))
                    {
                        cmd.CommandText = "insert into " + table + " (location) values( @txt ) ;";
                        cmd.Parameters.AddWithValue("@txt", textBox1.Text);
                        cmd.ExecuteNonQuery();
                        textBox1.Text = table;
                        textBox1.ForeColor = Color.Gray;
                       
                    } else {
                        string s = String.Format("insert into " + table + " (location,VPrice,TPrice) values('{0}','{1}','{2}') ;", textBox1.Text, textBox3.Text, textBox4.Text);
                        cmd.CommandText = s;
                        cmd.ExecuteNonQuery();
                        textBox1.Text = table;
                        textBox1.ForeColor = Color.Gray;
                        textBox3.Text = "Van Price";
                        textBox3.ForeColor = Color.Gray;
                        textBox4.Text = "Taxi Price";
                        textBox4.ForeColor = Color.Gray;
                    }
                    refresh1();
                }
                catch { }
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (!int.TryParse(textBox3.Text, out n) && !textBox3.Text.Equals("Van Price") && !textBox3.Text.Equals(""))
            {
                textBox3.BackColor = Color.Red;
                ch1 = false;
                button1.Hide();
            }
            else
            {
                textBox3.BackColor = Color.White;
                ch1 = true;
                button1.Show();
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (!int.TryParse(textBox4.Text, out n) && !textBox4.Text.Equals("Taxi Price") && !textBox4.Text.Equals(""))
            {
                textBox4.BackColor = Color.Red;
                ch1 = false;
                button1.Hide();
            }
            else
            {
                textBox4.BackColor = Color.White;
                ch1 = true;
                button1.Show();
            }
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            int n;
            if (!int.TryParse(textBox4.Text, out n) )
            {
                textBox3.ForeColor = Color.Black;
                textBox3.Text = "";
            }
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            int n;
            if (!int.TryParse(textBox4.Text, out n))
            {
                textBox4.ForeColor = Color.Black;
                textBox4.Text = "";
            }
        }
    }
}
