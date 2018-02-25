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
    public partial class AddUser : Form
    {
#pragma warning disable CS0618 // Type or member is obsolete
        static string connetionString = String.Format("server={0};database={1};uid={2};pwd={3};", ConfigurationSettings.AppSettings["Server"], ConfigurationSettings.AppSettings["Database"], ConfigurationSettings.AppSettings["User"], ConfigurationSettings.AppSettings["Pass"]);
#pragma warning restore CS0618 // Type or member is obsolete
                              // static string connetionString = "server=188.226.193.8;database=ESOL;uid=userss;pwd=Chrvasilis95;";
                              //static string connetionString = "server=localhost;database=esol;uid=root;pwd=Chrvasilis95;";
        public AddUser()
        {
            InitializeComponent();
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
            refresh();
        }

        private void refresh() {
            MySqlConnection cnn = new MySqlConnection(connetionString);
            cnn.Open();
            MySqlCommand cmd = cnn.CreateCommand();
            cmd.CommandText = "select UserName,ID from Users;";
            cmd.ExecuteNonQuery();
            cnn.Close();
            MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adap.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0].DefaultView;
            DataGridViewColumn column0 = dataGridView1.Columns[0];
            column0.Width = 60;
            DataGridViewColumn column1 = dataGridView1.Columns[1];
            column1.Width = 52;
            
        }
        private void AddUser_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!(textBox4.Text.Equals("") || textBox1.Text.Equals("")))
            {
                MySqlConnection cnn = new MySqlConnection(connetionString);
                cnn.Open();
                MySqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "INSERT INTO Users (UserName,Password,ID) VALUES (@username,@password, @id)";
                cmd.Parameters.AddWithValue("@username", textBox1.Text);
                cmd.Parameters.AddWithValue("@password", "12345");
                cmd.Parameters.AddWithValue("@id", textBox4.Text);
                cmd.ExecuteNonQuery();
                cnn.Close();
                refresh();
                textBox4.Text = "";
                textBox1.Text = "";
            }

            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!textBox4.Text.Equals("")){
                MySqlConnection cnn = new MySqlConnection(connetionString);
                cnn.Open();
                MySqlCommand cmd = cnn.CreateCommand();

                cmd.CommandText = "Delete from Book where ID=@id;";
                cmd.Parameters.AddWithValue("@id", textBox4.Text);
                cmd.ExecuteNonQuery();
                //*/
                cmd.CommandText = "Delete from Users where ID=@id2;";
                cmd.Parameters.AddWithValue("@id2", textBox4.Text);
                cmd.ExecuteNonQuery();
                cnn.Close();
                //*/
                refresh();
                textBox4.Text = "";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MySqlConnection cnn = new MySqlConnection(connetionString);
            cnn.Open();
            MySqlCommand cmd = cnn.CreateCommand();
            cmd.CommandText = "update Users set Password = 12345 where binary ID=@id;";
            cmd.Parameters.AddWithValue("@id",textBox4.Text);
            cmd.ExecuteNonQuery();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
