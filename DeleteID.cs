using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class DeleteID : Form
    {
        string t;
        static string connetionString = "server=188.226.193.8;database=ESOL;uid=userss;pwd=Chrvasilis95;";
        DataSet ds = new DataSet();

        //        static string connetionString = "server=localhost;database=esol;uid=root;pwd=Chrvasilis95;";
        MySqlConnection cnn = new MySqlConnection(connetionString);
        public DeleteID(string text)
        {
            t = text;
            InitializeComponent();
            label1.Text = "Do you want to Delete Registration:" + text+"?";
               // label1.ForeColor = Color.Red;
               // label2.Text=" \n Press OK to Delete"+"\n else Press Cancel";
        }

        private void DeleteID_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            cnn.Open();
            MySqlCommand cmd = cnn.CreateCommand();
            cmd.CommandText = "Delete from Book where IDBook=@id;";
            cmd.Parameters.AddWithValue("@id", t);
            cmd.ExecuteNonQuery();
            cnn.Close();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
