using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Drawing.Printing;
using OfficeOpenXml;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Configuration;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication3
{
    public partial class Form2 : Form
    {
        /**********************************************************/
        /**********************************************************/
#pragma warning disable CS0618 // Type or member is obsolete
        static string connetionString = String.Format("server={0};database={1};uid={2};pwd={3};", ConfigurationSettings.AppSettings["Server"], ConfigurationSettings.AppSettings["Database"], ConfigurationSettings.AppSettings["User"], ConfigurationSettings.AppSettings["Pass"]);
#pragma warning restore CS0618 // Type or member is obsolete
        //static string connetionString = "server=188.226.193.8;database=ESOL;uid=userss;pwd=Chrvasilis95;";
        //static string connetionString = "server=localhost;database=esol;uid=root;pwd=Chrvasilis95;";
        MySqlConnection cnn = new MySqlConnection(connetionString);
        DataSet ds = new DataSet();
        string ID;
        //SAVE FIRST CHECK VALUE
        bool tx1 = true;
        bool tx2 = true;
        bool tx4 = true;
        bool tx5 = true;
        bool tx6 = true;
        bool rtx1 = true;

        bool cb1 = true;
        bool cb6 = true;
        // Boolean ch = true;
        // Boolean chs = true;
        List<string> ids = new List<string>();
        List<string> Drivers = new List<string>();
        List<string> DriversIDS = new List<string>();
        List<string> DAddress = new List<string>();
        List<string> Rowsids = new List<string>();
        DataTable dt = new DataTable();
        DataSet dt12 = new DataSet();
        private string SelectedId = "0";
        Image target_image;
        int width;
        int height;
        string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string file = "Voucher.xlsx";
        //string BackupFIle= Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+@"\ESOL_DB_Backup.sql";
        string CurPath = Directory.GetCurrentDirectory();
        string BackupFIle = "ESOL_DB_Backup.sql";
        private bool ch2;
        string dfpr = Properties.Settings.Default.Printer;
        string DefPrinter = "";
        // string file ="1234.xlsx";
        /**********************************************************/
        /**********************************************************/
        public Form2(string id, String name)
        {

            cnn.Open();

            ID = id;

            InitializeComponent();
            // MessageBox.Show(tx1.ToString());
            dataGridView1.MultiSelect = true;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;


            // checkBox2.Checked = Properties.Settings.Default.Printer;


            /**********************************************************/

            //BUTTON SET

            button8.Hide();

            //CREATE A TABlE IF NEEDED

            /**********************************************************/
            dt.Columns.Add(new DataColumn("ID", typeof(string)));
            dt.Columns.Add(new DataColumn("Car", typeof(string)));
            dt.Columns.Add(new DataColumn("SYNTAGMA", typeof(string)));
            dt.Columns.Add(new DataColumn("KIFISIA", typeof(string)));
            dt.Columns.Add(new DataColumn("AIRPORT", typeof(string)));
            dt.Columns.Add(new DataColumn("GLYFADA", typeof(string)));
            /**********************************************************/

            MySqlCommand cmd = cnn.CreateCommand();
            //READ ALL USERS AND SAVE THEM TO AN ARRAY
            cmd.CommandText = "select ID from Users;";
            MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
            DataSet dss = new DataSet();
            adap.Fill(dss);

            comboBox4.Items.Add("");
            comboBox11.Items.Add("");
            foreach (DataRow row in dss.Tables[0].Rows)
            {
                comboBox4.Items.Add(row["ID"].ToString());
                ids.Add(row["ID"].ToString());
                comboBox11.Items.Add(row["ID"].ToString());
            }
            comboBox4.SelectedIndex = 0;
            /***********************************************************************************************/

            /**********************************************************/


            //SET ALL NEEDED VARIABLE AS SHOULD BE DEFAULT

            /**********************************************************/
            //RETURN
            label29.Hide();
            label28.Hide();
            label30.Hide();
            label31.Hide();
            dateTimePicker7.Hide();
            comboBox8.Hide();
            comboBox9.Hide();
            comboBox10.Hide();
            //CHECKBOX SET

            checkBox1.Checked = false;
            PrinterSettings prname = new PrinterSettings();
            checkBox2.Text = prname.PrinterName;
            DefPrinter = prname.PrinterName;
            PrinterOnline();

            //TEXTBOX SET
            /*
            textBox1.CharacterCasing = CharacterCasing.Upper;
            textBox2.CharacterCasing = CharacterCasing.Upper;
            textBox6.CharacterCasing = CharacterCasing.Upper;
            textBox10.CharacterCasing = CharacterCasing.Upper;
            //*/
            //COMBOBOX SET

            for (int i = 0; i <= 8; i++)
            {
                comboBox1.Items.Add(i);
            }

            comboBox1.SelectedIndex = 0;
            comboBox6.Items.Add("");
            comboBox6.Items.Add("VAN");
            comboBox6.Items.Add("TAXI");
            comboBox6.SelectedValue = "";
            comboBox11.Hide();

            cmd = cnn.CreateCommand();
            cmd.CommandText = "Select location from DAddress;";
            DataTable dt1 = new DataTable();
            adap = new MySqlDataAdapter(cmd);
            adap.Fill(dt1);
            comboBox5.Items.Add("");
            comboBox8.Items.Add("");
            comboBox9.Items.Add("");
            foreach (DataRow dr in dt1.Rows)
            {
                DAddress.Add(dr["location"].ToString());
                comboBox5.Items.Add(dr["location"].ToString());
                comboBox8.Items.Add(dr["location"].ToString());
                comboBox9.Items.Add(dr["location"].ToString());
            }
            dt1.Clear();
            //dt.Dispose();
            comboBox5.SelectedIndex = 0;
            comboBox8.SelectedIndex = 0;
            comboBox9.SelectedIndex = 0;
            string a = "";
            for (int i = 0; i <= 24; i++)
            {

                for (int q = 0; q <= 45; q += 15)
                {

                    if (q == 0)
                        a = "00";
                    else
                        a = q.ToString();

                    if (i < 10)
                    {
                        comboBox2.Items.Add("0" + i + ":" + a);
                        comboBox10.Items.Add("0" + i + ":" + a);
                    }
                    else
                    {
                        comboBox2.Items.Add(i + ":" + a);
                        comboBox10.Items.Add(i + ":" + a);

                        if ((q == 0) && (i == 12))
                        {
                            comboBox2.Items.Add("12:01");
                            comboBox10.Items.Add("12:01");
                        }
                    }
                }

            }

            comboBox2.SelectedIndex = 12 * 4 + 1;
            comboBox10.SelectedIndex = 12 * 4 + 1;

            cmd = cnn.CreateCommand();
            cmd.CommandText = "Select location from MOArrival;";
            DataTable dt2 = new DataTable();
            adap = new MySqlDataAdapter(cmd);
            adap.Fill(dt2);
            comboBox3.Items.Add("");
            foreach (DataRow dr in dt2.Rows)
            {
                comboBox3.Items.Add(dr["location"].ToString());
            }
            dt1.Clear();
            //dt.Dispose();
            comboBox3.SelectedIndex = 0;

            //DATETIMER SET

            dateTimePicker1.Value = DateTime.Today.AddDays(3);
            dateTimePicker2.Value = DateTime.Today.AddDays(1);
            dateTimePicker3.Value = DateTime.Today.AddDays(1);
            dateTimePicker5.Value = DateTime.Today.AddDays(7);
            dateTimePicker6.Value = DateTime.Today.AddDays(1);
            dateTimePicker7.Value = dateTimePicker1.Value;
            dateTimePicker4.Hide();

            //LABEL SET
            label4.Text = name;
            label15.Text = "";
            label19.Text = "";
            label15.ForeColor = Color.Red;
            label36.Hide();

            //ADMIN CHANGES

            if (!ID.Equals("Admin"))
            {
                pictureBox1.Hide();
                pictureBox3.Hide();
                pictureBox6.Hide();
                pictureBox7.Hide();
                label34.Hide();
                label35.Hide();
                tabControl1.TabPages.Remove(tabPage1);
                dataGridView2.Hide();
                button11.Hide();
                button12.Hide();
            }
            else
            {
                timer1.Start();
                Refresh2();
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    row.DefaultCellStyle.BackColor = Color.Red;
                }
            }


            cmd.CommandText = "select ID,LastName from Drivers;";
            adap = new MySqlDataAdapter(cmd);
            DataSet dss1 = new DataSet();
            dss1.Clear();
            adap.Fill(dss1);
            foreach (DataRow row in dss1.Tables[0].Rows)
            {
                DriversIDS.Add(row["ID"].ToString());
                Drivers.Add(row["LastName"].ToString());
            }

            //PICTURE CHANGES

            pictureBox4.Hide();
            try
            {
                target_image = Image.FromFile("Main_Photo.png");
                //DESKTOP
                //target_image = Image.FromFile(@"C:\Users\christod\Documents\SharedProjects\WindowsFormsApplication3\26755492_10215066935045486_846694004_n.png");

                // target_image = Image.FromFile(@"C:\Users\christod\Documents\SharedProjects\WindowsFormsApplication3\3.png");
                //target_image = Image.FromFile(@"C:\Users\christod\Documents\SharedProjects\WindowsFormsApplication3\2.png");
                //target_image = Image.FromFile(@"C:\Users\christod\Documents\SharedProjects\WindowsFormsApplication3\27044637_10215066931405395_649956090_n.png");
                //target_image = Image.FromFile("3.png");

                //LAPTOP
                // target_image = Image.FromFile(@"C:\Users\vasilis\Documents\MEGA\SharedProject\WindowsFormsApplication3\26755492_10215066935045486_846694004_n.png");

                width = target_image.Width;
                height = target_image.Height;
                pictureBox5.Image = target_image;
                pictureBox5.Image = ResizeNow(90, 47);
                pictureBox5.BringToFront();
            }
            catch { }
            // Color c = Color.FromArgb(6, 42, 190);
            Color c = Color.FromArgb(51, 0, 103);
            pictureBox5.BackColor = c;
            panel2.BackColor = c;
            comboBox12.Items.Add("");
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                comboBox12.Items.Add(printer);
            }
            checkBox5.Checked = Properties.Settings.Default.checkBox5;
            comboBox12.Text = Properties.Settings.Default.Printer;



        }

        /****************************************************************************************/
        /*                          FORM CHANGES                                               */

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            cnn.Close();
            timer1.Stop();
            Printer.SetDefaultPrinter(DefPrinter);
            Environment.Exit(1);

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        /****************************************************************************************/
        /*                          MENUITMESCLICK CLICK                                         */

        private void menuItemClicked(Object sender, ToolStripItemClickedEventArgs e) {
            if (!SelectedId.Equals("0"))
            {
                switch (e.ClickedItem.Name.ToString())
                {
                    case "Delete":
                        if (AcceptEvent(SelectedId) == 1)
                        {
                            if (MessageBox.Show("Do you want to Delete Registration: " + SelectedId + " ?", "Delete Registration ", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes)
                            {/*YES*/
                                MySqlCommand cmd = cnn.CreateCommand();
                                cmd.CommandText = "INSERT INTO DeleteBook(ADate,DateAdded,DAddress,Car,Driver,ID,IDBook ) SELECT ADate,DateAdded,DAddress,Car,Driver,ID,IDBook FROM Book WHERE IDBook=@id11 ;";
                                cmd.Parameters.AddWithValue("@id11", SelectedId);
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "Delete from Book where IDBook=@id;";
                                cmd.Parameters.AddWithValue("@id", SelectedId);
                                cmd.ExecuteNonQuery();
                            }
                            else {/*NO*/}
                            SelectedId = "0";
                        }
                        else {
                            MessageBox.Show("You can't Delete Registration which date is tomorrow or later.");
                        }
                        break;
                    case "Update":
                        if (AcceptEvent(SelectedId) == 1)
                        {
                            tabControl1.SelectedTab = Add;
                            button2.Hide();
                            button8.Show();
                            UpdateSetValues();
                        }
                        else
                        {
                            MessageBox.Show("You can't Update Registration which date is tomorrow or later.");
                        }
                        break;
                    case "Print_Voucher":
                        if (Rowsids.Count > 0)
                        {
                            for (int i = 0; i < Rowsids.Count; i++)
                            {
                                PrintVoucherOnline(Int32.Parse(Rowsids[i]));
                                Thread.Sleep(2000);
                            }
                            Rowsids.Clear();
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                row.DefaultCellStyle.BackColor = Color.White;
                            }
                        }
                        else {
                            PrintVoucherOnline(Int32.Parse(SelectedId));
                        }
                        break;
                }
            }


        }

        private void SubItem_OnClick(object sender, EventArgs e)
        {
            int i = Drivers.IndexOf(((ToolStripMenuItem)sender).ToString());
            MySqlCommand cmd = cnn.CreateCommand();
            string c = string.Format("update Book set Driver={0} where IDBook={1} ; ", DriversIDS[i], SelectedId);
            cmd.CommandText = c;
            cmd.ExecuteNonQuery();
        }


        /****************************************************************************************/
        /*                          PICTUREBOX CLICK                                          */

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            AddUser la = new AddUser();
            if (!la.Visible)
            {
                la.Show();
            }
            else
            {
                la.BringToFront();
            }
            // la.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = null;

            Refresh1();
            FixCollumn();

            // demoThread.Start();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            AddDriver la = new AddDriver();
            // MessageBox.Show(la.WindowState.ToString());
            // MessageBox.Show(la.Visible.ToString());
            if (!la.Visible)
            {
                la.Show();
            }
            else
            {
                la.BringToFront();
            }
            // la.Show();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            AddLocation ad = new AddLocation("DAddress");
            if (!ad.Visible)
            {
                ad.Show();
            }
            else
            {
                ad.BringToFront();
            }
            //  ad.Show();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            AddLocation ad = new AddLocation("MOArrival");
            if (!ad.Visible)
            {
                ad.Show();
            }
            else
            {
                ad.BringToFront();
            }
            // ad.Show();
        }

        /************************************************************************************/
        /*                        BUTTON CLICK                                          */

        //LOG OUT
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            cnn.Close();
            Printer.SetDefaultPrinter(DefPrinter);
            Login log = new Login();
            log.Show();
            this.Hide();
        }

        //SAVE
        private void button2_Click(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                if (CheckingValues() && tx1 && tx2 && tx4 && tx5 && tx6 && rtx1 && ch2)
                {
                    try
                    {
                        MySqlCommand cmd1 = cnn.CreateCommand();
                        if (comboBox6.Text.Equals("VAN"))
                        {
                            cmd1.CommandText = "Select VPrice from DAddress where location=@daddress1 ; ";
                        }
                        else
                        {
                            cmd1.CommandText = " Select TPrice from DAddress where location = @daddress1 ;";
                        }
                        cmd1.Parameters.AddWithValue("@daddress1", comboBox5.Text);
                        int value = (int)cmd1.ExecuteScalar();

                        if (MessageBox.Show("ΕΙΣΠΡΑΞΗ: " + value.ToString() + "€", "ΕΙΣΠΡΑΞΗ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {/*YES*/
                            int iw = 0;
                            label15.Text = "";
                            label15.ForeColor = Color.White;
                            MySqlCommand cmd = cnn.CreateCommand();
                            //CREATE COMMAND
                            cmd.CommandText = "insert into Book(DateAdded,FirstName,LastName,NumPeople,Car,phone,Email,DAddress,DAInfo,Remarks,MOArrival,ADate,ATime,ID) Values(@TN,@firstname,@lastname,@numpeople,@car,@phone,@email,@daddress,@Dainfo,@Remarks,@moarrival,@adate,@atime,@id2); select last_insert_id(); ";
                            // cmd.Parameters.AddWithValue("@TN", DateTime.Now);
                            cmd.Parameters.AddWithValue("@TN", dateTimePicker8.Value.ToString("yyyy/MM/dd"));
                            cmd.Parameters.AddWithValue("@firstname", textBox1.Text);
                            cmd.Parameters.AddWithValue("@lastname", textBox2.Text);
                            cmd.Parameters.AddWithValue("@numpeople", comboBox1.Text);
                            cmd.Parameters.AddWithValue("@car", comboBox6.Text);
                            cmd.Parameters.AddWithValue("@phone", textBox4.Text);
                            cmd.Parameters.AddWithValue("@email", textBox5.Text);
                            cmd.Parameters.AddWithValue("@daddress", comboBox5.Text);
                            // cmd.Parameters.AddWithValue("@daddress2", comboBox7.Text);
                            cmd.Parameters.AddWithValue("@Dainfo", textBox6.Text);
                            //cmd.Parameters.AddWithValue("@Dainfo2", textBox12.Text);
                            cmd.Parameters.AddWithValue("@moarrival", comboBox3.Text);
                            cmd.Parameters.AddWithValue("@adate", dateTimePicker1.Value.ToString("yyyy/MM/dd"));
                            cmd.Parameters.AddWithValue("@atime", comboBox2.Text);
                            cmd.Parameters.AddWithValue("@id2", ID);
                            cmd.Parameters.AddWithValue("@Remarks", richTextBox1.Text);
                            //EXECUTE AND KEEP ID
                            iw = Convert.ToInt32(cmd.ExecuteScalar());
                            // MessageBox.Show(iw.ToString());
                            try
                            {
                                if (!checkBox3.Checked)
                                    EmptyBoxes();
                                PrintVoucherOnline(iw);
                            }
                            catch
                            {
                                label15.Text = "Can't Print Voucher.";
                                label15.ForeColor = Color.Red;
                            }
                        }
                        else {/*NO*/}


                        //EMPTYVALUES
                    }
                    catch
                    {
                        label15.Text = "Can't inset values into DB.";
                        label15.ForeColor = Color.Red;
                    }
                }
            }


            if (checkBox3.Checked)
            {
                if (CheckingValuesReturn() && tx1 && tx2 && tx4 && tx5 && ch2)
                {
                    try
                    {
                        int iw = 0;
                        label15.Text = "";
                        label15.ForeColor = Color.White;
                        MySqlCommand cmd1 = cnn.CreateCommand();
                        if (comboBox6.Text.Equals("VAN"))
                        {
                            cmd1.CommandText = "Select VPrice from DAddress where location=@daddress1 ; ";
                        }
                        else
                        {
                            cmd1.CommandText = " Select TPrice from DAddress where location = @daddress1 ;";
                        }
                        cmd1.Parameters.AddWithValue("@daddress1", comboBox5.Text);
                        int value = (int)cmd1.ExecuteScalar();

                        if (MessageBox.Show("ΕΙΣΠΡΑΞΗ: " + value.ToString() + "€", "ΕΙΣΠΡΑΞΗ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {/*YES*/
                            MySqlCommand cmd = cnn.CreateCommand();
                            //CREATE COMMAND
                            cmd.CommandText = "insert into Book(DateAdded,FirstName,LastName,NumPeople,Car,phone,Email,DAddress,MOArrival,ADate,ATime,ID) Values(@TN,@firstname,@lastname,@numpeople,@car,@phone,@email,@daddress,@moarrival,@adate,@atime,@id2); select last_insert_id(); ";
                            cmd.Parameters.AddWithValue("@TN", DateTime.Now);
                            cmd.Parameters.AddWithValue("@firstname", textBox1.Text);
                            cmd.Parameters.AddWithValue("@lastname", textBox2.Text);
                            cmd.Parameters.AddWithValue("@numpeople", comboBox1.Text);
                            cmd.Parameters.AddWithValue("@car", comboBox6.Text);
                            cmd.Parameters.AddWithValue("@phone", textBox4.Text);
                            cmd.Parameters.AddWithValue("@email", textBox5.Text);
                            cmd.Parameters.AddWithValue("@daddress", comboBox8.Text);
                            cmd.Parameters.AddWithValue("@moarrival", comboBox9.Text);
                            cmd.Parameters.AddWithValue("@adate", dateTimePicker7.Value.ToString("yyyy/MM/dd"));
                            cmd.Parameters.AddWithValue("@atime", comboBox10.Text);
                            cmd.Parameters.AddWithValue("@id2", ID);
                            iw = Convert.ToInt32(cmd.ExecuteScalar());
                            //MessageBox.Show(iw.ToString());
                            try
                            {
                                PrintVoucherOnline(iw);

                            }
                            catch
                            {
                                label15.Text = "Can't Print Voucher.";
                                label15.ForeColor = Color.Red;
                            }
                        }
                        else {/*NO*/}
                    }
                    catch {
                        label15.Text = "Can't inset values into DB.";
                        label15.ForeColor = Color.Red;
                    }
                    EmptyBoxes();
                }
            }
            Thread piThread = new Thread(new ThreadStart(Pic));
            piThread.Start();
        }

        //Cancel
        private void button3_Click(object sender, EventArgs e)
        {
            EmptyBoxes();
            button2.Show();
            button8.Hide();
            Rowsids.Clear();
            label15.Text = "";
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.White;
            }
        }

        //Delete Registry
        private void button4_Click(object sender, EventArgs e)
        {


            DeleteID n = new DeleteID(textBox8.Text);
            n.Show();
            textBox8.Text = "";

        }

        //Button User and Pass change
        private void button5_Click(object sender, EventArgs e)
        {

            MySqlCommand cmd = cnn.CreateCommand();
            if (textBox11.Text != "")
            {
                string s = string.Format("update Users set UserName = '{0}' where ID='{1}';", textBox11.Text, ID);
                cmd.CommandText = s;
                // cmd.Parameters.AddWithValue("@id3", ID);
                // cmd.Parameters.AddWithValue("@user", textBox11.Text);
                cmd.ExecuteNonQuery();
                pictureBox4.Show();
                label4.Text = textBox11.Text;
                textBox11.Text = "";
            }
            else
            {
                try
                {
                    cmd.CommandText = "Select UserName from Users where ID=@user and Password=@pass";
                    cmd.Parameters.AddWithValue("@user", ID);
                    cmd.Parameters.AddWithValue("@pass", textBox3.Text);
                    MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adap.Fill(table);


                    if (table.Rows.Count > 0)
                    {

                        if (textBox7.Text.Equals(textBox9.Text))
                        {


                            cmd.CommandText = "update Users set Password = @pas where ID=@id3;";
                            cmd.Parameters.AddWithValue("@id3", ID);
                            cmd.Parameters.AddWithValue("@pas", textBox7.Text);
                            cmd.ExecuteNonQuery();
                            pictureBox4.Show();
                        }
                        else { label19.Text = "New Password and Re-type doesn't Match"; label19.ForeColor = Color.Red; }
                    }
                    else { label19.Text = "Wrong Old Password"; label19.ForeColor = Color.Red; }
                    textBox7.Text = "";
                    textBox3.Text = "";
                    textBox9.Text = "";
                }
                catch
                {
                    label19.Text = "Can't Connect to DB";
                    label19.ForeColor = Color.Purple;

                }
            }
            Thread piThread = new Thread(new ThreadStart(Pic));
            piThread.Start();
        }

        //Search button
        private void button6_Click(object sender, EventArgs e)
        {
            SearchBoxes();
        }

        //PDF CREATE button
        private void button7_Click(object sender, EventArgs e)
        {
            int i = dataGridView1.Columns.Count;
            int ii = dataGridView1.Rows.Count;

            if (i > 0 && ii > 1)
            {
                if (File.Exists(pathDesktop + @"\E_SOL_PDF.pdf"))
                {
                    File.Delete(pathDesktop + @"\E_SOL_PDF.pdf");
                }
                FileStream fs = new FileStream(pathDesktop + @"\E_SOL_PDF.pdf", FileMode.Create);
                iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 25, 25, 30, 30);
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, fs);
                doc.AddAuthor("VasilisChr");

                doc.AddCreator("VasilisChr");

                doc.AddKeywords("");

                doc.AddSubject("E-Sol Data");

                doc.AddTitle("E-Sol Data");

                doc.Open();
                // Font ffont = new Font(Font.FontFamily.UNDEFINED, 5, Font.ITALIC);
                iTextSharp.text.pdf.BaseFont bf = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.TIMES_ROMAN, iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);

                iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 16, iTextSharp.text.Font.NORMAL);

                iTextSharp.text.Paragraph para = new iTextSharp.text.Paragraph("E-Sol \n Data \n", font);

                para.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                // header.
                doc.Add(para);
                para = new iTextSharp.text.Paragraph("\n", font);
                para.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                doc.Add(para);//*/
                              // doc.Add(new iTextSharp.text.Paragraph("E-Sol"));

                iTextSharp.text.pdf.PdfPTable table;
                iTextSharp.text.Font font1 = new iTextSharp.text.Font(bf, 8, iTextSharp.text.Font.NORMAL);
                if (i > DAddress.Count + 2)
                {
                    table = new iTextSharp.text.pdf.PdfPTable(i - 7);
                    table.WidthPercentage = 100;
                    for (int k = 0; k < i; k++)
                    {
                        if (!dataGridView1.Columns[k].HeaderText.Equals("HaveSeen"))
                            if (!dataGridView1.Columns[k].HeaderText.Equals("FirstName"))
                                if (!dataGridView1.Columns[k].HeaderText.Equals("ATime"))
                                    if (!dataGridView1.Columns[k].HeaderText.Equals("phone"))
                                        if (!dataGridView1.Columns[k].HeaderText.Equals("Email"))
                                            if (!dataGridView1.Columns[k].HeaderText.Equals("DAInfo"))
                                                if (!dataGridView1.Columns[k].HeaderText.Equals("DAInfo2"))
                                                    if (!dataGridView1.Columns[k].HeaderText.Equals("Remarks"))
                                                        table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(dataGridView1.Columns[k].HeaderText, font1)));
                    }


                }
                else
                {
                    table = new iTextSharp.text.pdf.PdfPTable(DAddress.Count + 2);
                    table.WidthPercentage = 100;

                    for (int k = 0; k < i; k++)
                    {
                        table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(dataGridView1.Columns[k].HeaderText, font1)));

                    }
                }
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    for (int k = 0; k < i; k++)
                    {

                        if (ii > 0)
                        {

                            if (!dataGridView1.Columns[k].HeaderText.Equals("HaveSeen"))
                                if (!dataGridView1.Columns[k].HeaderText.Equals("FirstName"))
                                    if (!dataGridView1.Columns[k].HeaderText.Equals("ATime"))
                                        if (!dataGridView1.Columns[k].HeaderText.Equals("phone"))
                                            if (!dataGridView1.Columns[k].HeaderText.Equals("Email"))
                                                if (!dataGridView1.Columns[k].HeaderText.Equals("DAInfo"))
                                                    if (!dataGridView1.Columns[k].HeaderText.Equals("DAInfo2"))
                                                        if (!dataGridView1.Columns[k].HeaderText.Equals("Remarks"))
                                                            table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(row.Cells[dataGridView1.Columns[k].HeaderText].Value.ToString(), font1)));

                        }

                    }
                    ii--;

                }

                doc.Add(table);
                doc.Close();
                writer.Close();
                MessageBox.Show("PDF CREATED");
            }

        }

        //UPDATE button
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update Book set FirstName = @firstname  , LastName=@lastname , NumPeople=@numpeople , Car=@car , phone=@phone , Email=@email , DAddress=@daddress , DAInfo=@dainfo,Remarks=@Remarks , MOArrival=@moarrival , ADate=@adate , ATime=@atime   where IDBook=@id3;";
                cmd.Parameters.AddWithValue("@firstname", textBox1.Text.ToString());
                cmd.Parameters.AddWithValue("@lastname", textBox2.Text.ToString());
                cmd.Parameters.AddWithValue("@numpeople", comboBox1.Text.ToString());
                cmd.Parameters.AddWithValue("@car", comboBox6.Text.ToString());
                cmd.Parameters.AddWithValue("@phone", textBox4.Text.ToString());
                cmd.Parameters.AddWithValue("@email", textBox5.Text.ToString());
                cmd.Parameters.AddWithValue("@daddress", comboBox5.Text.ToString());
                //  cmd.Parameters.AddWithValue("@daddress2", comboBox7.Text.ToString());
                //cmd.Parameters.AddWithValue("@Dainfo2", textBox12.Text.ToString());
                cmd.Parameters.AddWithValue("@Dainfo", textBox6.Text.ToString());
                cmd.Parameters.AddWithValue("@Remarks", richTextBox1.Text.ToString());
                cmd.Parameters.AddWithValue("@moarrival", comboBox3.Text.ToString());
                cmd.Parameters.AddWithValue("@adate", dateTimePicker1.Value.ToString("yyyy/MM/dd"));
                cmd.Parameters.AddWithValue("@atime", comboBox2.Text.ToString());
                cmd.Parameters.AddWithValue("@id3", SelectedId);
                cmd.ExecuteNonQuery();
                Refresh1();
                EmptyBoxes();
            }
            catch
            {
                label15.Text = "Can't update Database";
            }



        }

        //Admin search
        private void button10_Click(object sender, EventArgs e)
        {
            ds.Clear();
            try
            {
                MySqlCommand cmd = cnn.CreateCommand();
                string selects;
                // string  selects = string.Format("SELECT HaveSeen, IDBook, FirstName, LastName, NumPeople, Car, phone, Email, DAddress, DAInfo, MOArrival, DATE_FORMAT(ADate,'%d/%m/%Y') as Arrival, ATime,DATE_FORMAT(DateAdded,'%d/%m/%Y') as Added, ID from Book where ADate >='{0}' and ADate <= '{1}' and HaveSeen=0 ", (dateTimePicker6.Value.Date.ToString("yyyy/MM/dd")), (dateTimePicker5.Value.Date.ToString("yyyy/MM/dd")));

                if (!(comboBox4.SelectedIndex == 0))
                {
                    //    MessageBox.Show(comboBox4.SelectedText);
                    selects = string.Format("SELECT  IDBook, Book.FirstName, Book.LastName, NumPeople, Car, phone, Email, DAddress, DAInfo,Remarks, MOArrival, DATE_FORMAT(ADate,'%d/%m/%Y') as Arrival, ATime,DATE_FORMAT(DateAdded,'%d/%m/%Y') as Added, Book.ID,Drivers.LastName as Driver FROM Book left join Drivers on Book.Driver=Drivers.ID  where ADate >='{0}' and ADate <= '{1}'  and Book.ID='{2}' order by ID ", (dateTimePicker6.Value.Date.ToString("yyyy/MM/dd")), (dateTimePicker5.Value.Date.ToString("yyyy/MM/dd")), comboBox4.Text);

                }
                else
                {
                    selects = string.Format("SELECT  IDBook, Book.FirstName, Book.LastName, NumPeople, Car, phone, Email, DAddress, DAInfo,Remarks, MOArrival, DATE_FORMAT(ADate,'%d/%m/%Y') as Arrival, ATime,DATE_FORMAT(DateAdded,'%d/%m/%Y') as Added, Book.ID,Drivers.LastName as Driver FROM Book left join Drivers on Book.Driver=Drivers.ID  where ADate >='{0}' and ADate <= '{1}'  ", (dateTimePicker6.Value.Date.ToString("yyyy/MM/dd")), (dateTimePicker5.Value.Date.ToString("yyyy/MM/dd")));
                }
                // selects = selects + " ;";
                // MessageBox.Show(selects);
                cmd.CommandText = selects;
                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                //DataSet das = new DataSet();
                adap.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
                dateTimePicker5.Value = DateTime.Today.AddDays(7);
                dateTimePicker6.Value = DateTime.Today.AddDays(1);
                FixCollumn();
            }
            catch { }


        }

        /********************************************************************************************/
        /*                           TEXTBOX CHANGES                                              */
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 20)
            {
                textBox1.BackColor = Color.Red;
                label15.Text = "characters<=20";
                label15.ForeColor = Color.Red;
                tx1 = false;
            }
            else
            {
                textBox1.BackColor = Color.White;
                label15.Text = "";
                tx1 = true;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 20)
            {
                textBox2.BackColor = Color.Red;
                label15.Text = "characters<=20";
                label15.ForeColor = Color.Red;
                tx2 = false;
                //  ch2 = false;
            }
            else
            {
                textBox2.BackColor = Color.White;
                label15.Text = "";
                tx2 = true;
                // ch2 = true;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text.Length > 10)
            {
                textBox4.BackColor = Color.Red;
                label15.Text = "characters<=10";
                label15.ForeColor = Color.Red;
                tx4 = false;
            }
            else
            {
                textBox4.BackColor = Color.White;
                label15.Text = "";
                tx4 = true;
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text.Length > 50)
            {
                textBox6.BackColor = Color.Red;
                label15.Text = "characters<=50";
                label15.ForeColor = Color.Red;
                tx6 = false;

            }
            else
            {
                textBox6.BackColor = Color.White;
                label15.Text = "";
                tx6 = true;
                // label15.ForeColor = Color.Red;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text.Length >= 30)
            {
                textBox5.BackColor = Color.Red;
                label15.Text = "characters<=30";
                label15.ForeColor = Color.Red;
                tx5 = false;

            }
            else
            {
                textBox5.BackColor = Color.White;
                label15.Text = "";
                tx5 = true;
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (textBox10.Text.Length > 20)
            {
                textBox10.BackColor = Color.Red;
                label15.Text = "characters<=20";
                label15.ForeColor = Color.Red;
                // chs = false;
            }
            else
            {
                textBox10.BackColor = Color.White;
                label15.Text = "";
                //chs = true;
            }
        }



        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length > 30)
            {
                richTextBox1.BackColor = Color.Red;
                label15.Text = "characters<=30";
                label15.ForeColor = Color.Red;
                rtx1 = false;
            }
            else
            {
                richTextBox1.BackColor = Color.White;
                label15.Text = "";
                rtx1 = true;
            }
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                SearchBoxes();
            }
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                SearchBoxes();
            }
        }

        /***********************************************************************************************/
        /*                           COMBOBOX CHANGES                                              */

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Color k = Color.FromKnownColor(KnownColor.Control);
            if (comboBox6.Text.Equals("TAXI") && (Int32.Parse(comboBox1.Text) >= 5))
            {
                comboBox6.BackColor = Color.Red;
                comboBox1.BackColor = Color.Red;
                label5.BackColor = Color.Red;
                label23.BackColor = Color.Red;
                label15.Text = label15.Text + ". \n Taxi shoud have \n less than 4 people";
                // ch = false;
                ch2 = false;
            }
            else
            {
                comboBox6.BackColor = Color.White;
                comboBox1.BackColor = Color.White;
                label5.BackColor = k;
                label23.BackColor = k;
                label15.Text = "";
                //  ch = true;
                ch2 = true;
            }
            if (comboBox1.Text.Equals(""))
            {
                //  ch = false;
                ch2 = false;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text.Equals(""))
            {
                //   ch = false;
            }
            else
            {
                // ch = true;
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.Text.Equals(""))
            {
                //   ch = false;
            }
            else
            {
                comboBox9.SelectedIndex = comboBox5.SelectedIndex;
                comboBox9.BackColor = comboBox5.BackColor;
                //   ch = true;
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            Color k = Color.FromKnownColor(KnownColor.Control);
            if (comboBox6.Text.Equals("TAXI") && (Int32.Parse(comboBox1.Text) >= 5))
            {
                comboBox6.BackColor = Color.Red;
                comboBox1.BackColor = Color.Red;
                label5.BackColor = Color.Red;
                label23.BackColor = Color.Red;
                label15.Text = label15.Text + ". \n Taxi shoud have \n less than 4 people";
                //  ch = false;
                ch2 = false;
            }
            else {
                comboBox6.BackColor = Color.White;
                comboBox1.BackColor = Color.White;
                label15.Text = "";
                label5.BackColor = k;
                label23.BackColor = k;
                // ch = true;
                ch2 = true;
            }
            if (comboBox6.Text.Equals(""))
            {
                //   ch = false;
                ch2 = false;
            }
        }

        /***********************************************************************************************/
        /*                        DATETIMERPICKER                                                      */

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            Color k = Color.FromKnownColor(KnownColor.Control);
            if (DateTime.Compare(dateTimePicker1.Value, DateTime.Now.AddDays(2)) == -1)
            {
                label10.BackColor = Color.Red;
                label15.Text = label15.Text + ". \n Date should be greater \n than the day after tomorrow.";
                //  ch = false;
            }
            else {
                //     ch = true;
                label10.BackColor = k;
                label15.Text = "";
                dateTimePicker7.Value = dateTimePicker1.Value;
            }
        }

        /***********************************************************************************************/
        /*                        CHECKBOX CHANGES                                                     */

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                dateTimePicker4.Show();
                dateTimePicker4.Value = DateTime.Today.AddMonths(-1);
                if (ID.Equals("Admin")) {
                    label36.Show();
                    comboBox11.Show();
                }
            }
            else
            {
                dateTimePicker4.Hide();
                label36.Hide();
                comboBox11.Hide();
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                Color k = Color.FromKnownColor(KnownColor.Control);
                label29.Show();
                label28.Show();
                label30.Show();
                label31.Show();
                dateTimePicker7.Show();
                comboBox8.Show();
                comboBox9.Show();
                comboBox10.Show();
                label28.BackColor = k;
                label29.BackColor = k;
                label31.BackColor = k;
                comboBox8.BackColor = Color.White;
                //comboBox9.BackColor = Color.White;
                comboBox10.BackColor = Color.White;
                comboBox8.SelectedIndex = 0;
                //comboBox9.SelectedIndex = 0;
                comboBox10.SelectedIndex = (4 * 12) + 1;
            }
            else
            {
                label29.Hide();
                label28.Hide();
                label30.Hide();
                label31.Hide();
                dateTimePicker7.Hide();
                comboBox8.Hide();
                comboBox9.Hide();
                comboBox10.Hide();
                checkBox4.Checked = true;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox4.Checked)
                checkBox3.Checked = true;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                checkBox2.Checked = false;
                Properties.Settings.Default.checkBox5 = checkBox5.Checked;
                Properties.Settings.Default.Save();
            }
            else
            {
                checkBox2.Checked = true;
                Properties.Settings.Default.checkBox5 = checkBox5.Checked;
                Properties.Settings.Default.Save();
            }
        }

        /**********************************************************************************************/
        /*                             DATAGRIDVIEW CLICK                                            */


        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                try
                {
                    /*FIND COlUMN WITH IDBOOK*/
                    SelectedId = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                }
                catch { }
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                ContextMenuStrip m = new ContextMenuStrip();

                int currentMouseOverRow = dataGridView1.HitTest(e.X, e.Y).RowIndex;
                if (currentMouseOverRow >= 0)
                {
                    m.Items.Add("Delete").Name = "Delete";
                    m.Items.Add("Update").Name = "Update";
                    m.Items.Add("Print Voucher ").Name = "Print_Voucher";
                    if (ID.Equals("Admin"))
                    {
                        m.Items.Add("Driver").Name = "Driver";
                        for (int i = 0; i < Drivers.Count; i++)
                        {
                            (m.Items[3] as ToolStripMenuItem).DropDownItems.Add(Drivers[i].ToString(), null, new System.EventHandler(this.SubItem_OnClick));
                        }
                    }
                }
                m.Show(dataGridView1, new Point(e.X, e.Y));
                m.ItemClicked += new ToolStripItemClickedEventHandler(menuItemClicked);
            }
        }

        private void dataGridView1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var p = this.dataGridView1.PointToClient(Cursor.Position);
                int rowIndex = dataGridView1.HitTest(p.X, p.Y).RowIndex;
                if (rowIndex >= 0)
                {
                    string id = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                    if (!Rowsids.Contains(id) && !((ModifierKeys & Keys.Alt) == Keys.Alt))
                    {
                        if (Control.ModifierKeys == Keys.Control)
                        {

                            //   label15.Text = label15.Text + "," + id;
                            Rowsids.Add(id);
                            dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Green;
                        }

                    }
                    else
                    {
                        if ((ModifierKeys & Keys.Alt) == Keys.Alt)
                        {
                            Rowsids.Remove(id);
                            //    label15.Text = label15.Text.Replace("," + id, "");
                            dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.White;
                        }
                    }

                    if (p.Y > 130 && p.Y < 136)
                        dataGridView1.FirstDisplayedScrollingRowIndex += 1;

                }
            }
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                MySqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "update DeleteBook set HaveSeen = 1 where IDBook=@id;";
                cmd.Parameters.AddWithValue("@id", dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString());
                cmd.ExecuteNonQuery();
                Refresh2();
            }
            catch {
                MessageBox.Show("Error occurred");
            }
        }

        /**********************************************************************************************/
        /*                             TIMER TICK                                                     */

        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh2();
        }

        /*****************************************************************************************/
        /*                          METHODS                                                     */
        private void EmptyBoxes()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.SelectedIndex = 0;
            comboBox6.SelectedIndex = 0;
            textBox4.Text = "";
            textBox5.Text = "";
            comboBox5.SelectedIndex = 0;
            comboBox8.SelectedIndex = 0;
            comboBox9.SelectedIndex = 0;
            textBox6.Text = "";
            comboBox3.SelectedIndex = 0;
            richTextBox1.Text = "";
            dateTimePicker1.Value = DateTime.Today.AddDays(3);
            comboBox2.SelectedIndex = (4 * 12) + 1;
            comboBox10.SelectedIndex = (4 * 12) + 1;
            Color k = Color.FromKnownColor(KnownColor.Control);
            label11.BackColor = k;
            label14.BackColor = k;
            label5.BackColor = k;
            label2.BackColor = k;
            label8.BackColor = k;
            label9.BackColor = k;
            label10.BackColor = k;
            label23.BackColor = k;
            label28.BackColor = k;
            label29.BackColor = k;
            label31.BackColor = k;
            textBox2.BackColor = Color.White;
            comboBox1.BackColor = Color.White;
            comboBox5.BackColor = Color.White;
            comboBox6.BackColor = Color.White;
            comboBox3.BackColor = Color.White;
            comboBox2.BackColor = Color.White;
            comboBox8.BackColor = Color.White;
            comboBox9.BackColor = Color.White;
            comboBox10.BackColor = Color.White;
        }

        private Boolean CheckingValues()
        {
            Color k = Color.FromKnownColor(KnownColor.Control);
            Boolean t = true;
            if ((comboBox3.SelectedIndex == 0))
            {
                comboBox3.BackColor = Color.Red;
                label9.BackColor = Color.Red;
                t = false;
            }
            else
            {
                comboBox3.BackColor = Color.White;
                label9.BackColor = k;
                // t = false;
            }

            if ((comboBox5.SelectedIndex == 0))
            {
                comboBox5.BackColor = Color.Red;
                label8.BackColor = Color.Red;
                t = false;
            }
            else
            {
                comboBox5.BackColor = Color.White;
                label8.BackColor = k;
            }
            if ((comboBox1.SelectedIndex == 0) || (comboBox6.Text.Equals("TAXI") && (Int32.Parse(comboBox1.Text) >= 5)))
            {
                comboBox1.BackColor = Color.Red;
                label5.BackColor = Color.Red;
                t = false;
            }
            else
            {
                comboBox1.BackColor = Color.White;
                label5.BackColor = k;
            }

            if (comboBox2.Text == "12:01")
            {
                comboBox2.BackColor = Color.Red;
                t = false;
                label15.Text = " Red =Required field(*) \n Arrival Time other than 12:01 \n TAXI SHOULD HAVE LESS THAN 5 PEOPLE";
                label15.ForeColor = Color.Red;
                label11.BackColor = Color.Red;
            }
            else
            {
                comboBox2.BackColor = Color.Red;
                label11.BackColor = k;
                label15.Text = "";
            }

            if (comboBox6.Text.Equals("")  || (comboBox6.Text.Equals("TAXI") && (Int32.Parse(comboBox1.Text) >= 5)))
            {
                comboBox6.BackColor = Color.Red;
                label23.BackColor = Color.Red;
                t = false;
            }
            else
            {
                comboBox6.BackColor = Color.White;
                label23.BackColor = k;
            }

            if (textBox2.Text.Equals("")) {
                label2.BackColor = Color.Red;
                t = false;
            }else
            {
                label2.BackColor = k;
            }

            if (DateTime.Compare(dateTimePicker1.Value, DateTime.Now.AddDays(2)) == -1) {
                label10.BackColor = Color.Red;
                t = false;
            }
            else
            {
                label10.BackColor = k;
            }
            if (!t)
            {
                label15.Text = " Red =Required field(*) \n Arrival Time other than 12:01 \n TAXI SHOULD HAVE LESS THAN 5 PEOPLE";
                Thread piThread = new Thread(new ThreadStart(Pic));
                piThread.Start();
            }

            return t;
        }

        private Boolean UpdateSetValues()
        {
            try
            {
                MySqlCommand cmd = cnn.CreateCommand();

                cmd.CommandText = string.Format("select DAddress,Remarks,ADate,ATime,MOArrival,FirstName,LastName,NumPeople,Car,phone,Email,DAInfo from Book where  IDBook='{0}' ; ", SelectedId);

                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                //CREATE DT TO SAVE DATA AND PROCCESS
                DataTable sa = new DataTable();
                sa.Clear();
                //SAVE DATA FROM SELECT TO DT sa
                adap.Fill(sa);
                //ONLY ONE ROW WE WILL FIND
                foreach (DataRow row in sa.Rows)
                {
                    textBox1.Text = row["FirstName"].ToString();
                    textBox2.Text = row["LastName"].ToString();
                    comboBox1.Text = row["NumPeople"].ToString();
                    comboBox6.Text = row["Car"].ToString();
                    textBox4.Text = row["phone"].ToString();
                    textBox5.Text = row["Email"].ToString();
                    textBox6.Text = row["DAInfo"].ToString();
                    comboBox5.Text = row["DAddress"].ToString();
                   // textBox12.Text = row["DAInfo2"].ToString();
                    //comboBox7.Text = row["DAddress2"].ToString();
                    comboBox3.Text = row["MOArrival"].ToString();
                    comboBox2.Text = row["ATime"].ToString();
                    richTextBox1.Text = row["Remarks"].ToString();

                    // DateTime a = (DateTime)row["ADate"];
                    //MessageBox.Show(a.ToString());
                    dateTimePicker1.Value = (DateTime)row["ADate"];

                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        DataTable tmp1;// = new DataTable();
        DataTable tmp;// = new DataTable();

        private void SearchBoxes() {
            try
            {
                ds.Clear();
                tmp = new DataTable();
                tmp1 = new DataTable();
                tmp1.Clear();
                tmp.Clear();
                if (checkBox1.Checked)
                {
                    dt.Rows.Clear();
                    checkBox1.Checked = false;
                    MySqlCommand cmd = cnn.CreateCommand();
                    DateTime date1 = new DateTime(dateTimePicker4.Value.Year, dateTimePicker4.Value.Month, 1);
                    if (!ID.Equals("Admin"))
                    {
                        cmd.CommandText = string.Format("select ID,Car,DAddress,Count(DAddress) as C from Book where ID='{0}' and DateAdded >= '{1}' and DateAdded < '{2}' group by ID,Car,DAddress ; ", ID, date1.ToString("yyyy/MM/dd"), date1.AddMonths(1).ToString("yyyy/MM/dd"));
                    }
                    else
                    {
                        cmd.CommandText = string.Format("select ID,Car,DAddress,Count(DAddress) as C from Book where  DateAdded >= '{0}' and DateAdded < '{1}' group by ID,Car,DAddress ; ", date1.ToString("yyyy/MM/dd"), date1.AddMonths(1).ToString("yyyy/MM/dd"));
                    }

                    MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                    //CREATE DT TO SAVE DATA AND PROCCESS
                    DataTable sa = new DataTable();
                    sa.Clear();
                    //SAVE DATA FROM SELECT TO DT sa
                    adap.Fill(sa);

                    MySqlCommand cmd1 = cnn.CreateCommand();
                    string asss = string.Format("SELECT ID,Car,DAddress,Count(DAddress) as C FROM DeleteBook where month(DateAdded)!=month(ADate)  and month(curdate()- interval 30 day)=month(ADate) and year(ADate)=year(curdate()- interval 30 day)  group by ID, DAddress, Car;");
                   // string asss = string.Format("select ID,Car,DAddress,Count(DAddress) as C from DeleteBook where month(ADate) = month(curdate() - interval 30 day)     and month(DateAdded) = month(curdate() - interval 60 day)  group by ID, DAddress, Car; ");
                    cmd1.CommandText = asss;
                    MySqlDataAdapter adap1 = new MySqlDataAdapter(cmd1);
                    DataTable saDel = new DataTable();
                    adap1.Fill(saDel);

                    tmp.Columns.Add("User", typeof(String));
                    tmp.Columns.Add("Car", typeof(String));
                    foreach (string s in DAddress)
                        tmp.Columns.Add(s, typeof(String));
                    DataRow dr = null;
                    if (ID.Equals("Admin"))
                    {
                        foreach (string idss in ids)
                        {
                            //if admin and have not select user then do it for everyone
                            if (comboBox11.Text.Equals(""))
                            {
                                
                                MonthlyCheck(idss, tmp, dr, sa, "");
                                MonthlyCheck(idss, tmp, dr, saDel, "/Deleted");

                            }
                            //if admin and have select user 
                            else if (comboBox11.Text.Equals(idss))
                            {
                                MonthlyCheck(idss, tmp, dr, sa, "");
                                MonthlyCheck(idss, tmp, dr, saDel, "/Deleted");
                            }
                        }
                    }
                    else
                    {
                        MonthlyCheck(ID, tmp, dr, sa, "");
                        MonthlyCheck(ID, tmp, dr, saDel, "/Deleted");
                    }
                    dataGridView1.DataSource = tmp;
                    comboBox11.SelectedIndex = 0;
                }
                else if (!textBox8.Text.Equals(""))
                {

                    MySqlCommand cmd = cnn.CreateCommand();
                    string selects = "";
                    if (ID.Equals("Admin"))
                    {
                        selects = string.Format("SELECT  IDBook, Book.FirstName, Book.LastName, NumPeople, Car, phone, Email, DAddress, DAInfo,Remarks, MOArrival, DATE_FORMAT(ADate,'%d/%m/%Y') as Arrival, ATime,DATE_FORMAT(DateAdded,'%d/%m/%Y') as Added, Book.ID,Drivers.LastName as Driver FROM Book left join Drivers on Book.Driver=Drivers.ID  where IDBook={0} ;", (textBox8.Text));
                    }
                    else
                    {
                        selects = string.Format("SELECT  IDBook, Book.FirstName, Book.LastName, NumPeople, Car, phone, Email, DAddress, DAInfo,Remarks, MOArrival, DATE_FORMAT(ADate,'%d/%m/%Y') as Arrival, ATime,DATE_FORMAT(DateAdded,'%d/%m/%Y') as Added, Book.ID FROM Book where IDBook={0} and ID='{1}' ;", textBox6.Text, ID);
                    }
                    try
                    {
                        // MessageBox.Show();
                        cmd.CommandText = selects;
                        MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                        DataSet das = new DataSet();
                        adap.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0].DefaultView;
                        FixCollumn();
                        textBox8.Text = "";
                    }
                    catch { }
                }
                else if (!textBox10.Text.Equals("") /*&& chs*/)
                {
                    //  MessageBox.Show("te8");
                    MySqlCommand cmd = cnn.CreateCommand();
                    string selcts = "";
                    if (ID.Equals("Admin"))
                    {

                        selcts = string.Format("SELECT  IDBook, Book.FirstName, Book.LastName, NumPeople, Car, phone, Email, DAddress, DAInfo,Remarks, MOArrival, DATE_FORMAT(ADate,'%d/%m/%Y') as Arrival, ATime,DATE_FORMAT(DateAdded,'%d/%m/%Y') as Added, Book.ID,Drivers.LastName as Driver FROM Book left join Drivers on Book.Driver=Drivers.ID where Book.LastName LIKE CONCAT('{0}', '%') ;", textBox10.Text);
                    }
                    else
                    {

                        selcts = string.Format("SELECT  IDBook, Book.FirstName, Book.LastName, NumPeople, Car, phone, Email, DAddress, DAInfo,Remarks, MOArrival, DATE_FORMAT(ADate,'%d/%m/%Y') as Arrival, ATime,DATE_FORMAT(DateAdded,'%d/%m/%Y') as Added, Book.ID FROM Book where LastName LIKE CONCAT('{0}', '%')  and ID='{1}' ;", textBox10.Text, ID);
                    }
                    try
                    {
                        //MessageBox.Show(selcts);
                        cmd.CommandText = selcts;
                        MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                        DataSet ddas = new DataSet();
                        adp.Fill(ddas);
                        dataGridView1.DataSource = ddas.Tables[0].DefaultView;
                        FixCollumn();
                        textBox10.Text = "";
                    }
                    catch { }
                }
                else if (!(dateTimePicker2.Value.Date == DateTime.Now.AddDays(1).Date && dateTimePicker3.Value.Date == DateTime.Now.AddDays(1).Date))
                {
                    // MessageBox.Show((dateTimePicker2.Value == DateTime.Now.AddDays(1)).ToString());
                    MySqlCommand cmd = cnn.CreateCommand();
                    string selects = "";
                    if (ID.Equals("Admin"))
                    {
                        selects = string.Format("SELECT  IDBook, FirstName, LastName, NumPeople, Car, phone, Email, DAddress, DAInfo, MOArrival, DATE_FORMAT(ADate,'%d/%m/%Y') as Arrival, ATime,DATE_FORMAT(DateAdded,'%d/%m/%Y') as Added, ID from Book where DateAdded >='{0}' and DateAdded <= '{1}' ;", (dateTimePicker2.Value.Date.ToString("yyyy/MM/dd")), (dateTimePicker3.Value.Date.ToString("yyyy/MM/dd")));
                    }
                    else
                    {
                        selects = string.Format("SELECT  IDBook, FirstName, LastName, NumPeople, Car, phone, Email, DAddress, DAInfo, MOArrival, DATE_FORMAT(ADate,'%d/%m/%Y') as Arrival, ATime,DATE_FORMAT(DateAdded,'%d/%m/%Y') as Added, ID from Book where DateAdded  >='{0}' and DateAdded <= '{1}' and ID='{2}' ;", (dateTimePicker2.Value.Date.ToString("yyyy/MM/dd")), (dateTimePicker3.Value.Date.ToString("yyyy/MM/dd")), ID);
                    }
                    try
                    {
                        // MessageBox.Show();
                        cmd.CommandText = selects;
                        MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                        DataSet das = new DataSet();
                        adap.Fill(das);
                        dataGridView1.DataSource = das.Tables[0].DefaultView;
                        FixCollumn();
                        dateTimePicker2.Value = DateTime.Today.AddDays(1);
                        dateTimePicker3.Value = DateTime.Today.AddDays(1);
                    }
                    catch { }
                }
            }
            catch { MessageBox.Show("Error occurred"); }
        }

        private void MonthlyCheck(string idss, DataTable tmp, DataRow dr, DataTable sa, string param)
        {
            dr = tmp.NewRow();
            dr["User"] = idss + param;
            dr["Car"] = "TAXI";
            DataColumnCollection columns = tmp.Columns;
            foreach (DataRow row in sa.Rows)
            {
                if (row["ID"].Equals(idss) && row["Car"].Equals("TAXI"))
                {
                    if (columns.Contains(row["DAddress"].ToString()))
                    {
                        dr[row["DAddress"].ToString()] = row["C"];
                    }
                    else
                    {
                        tmp.Columns.Add(row["DAddress"].ToString(), typeof(String));
                        dr[row["DAddress"].ToString()] = row["C"];
                    }
                }
            }

            tmp.Rows.Add(dr);

            dr = tmp.NewRow();
            dr["User"] = idss + param;
            dr["Car"] = "VAN";

            foreach (DataRow row in sa.Rows)
            {

                if (row["ID"].Equals(idss) && row["Car"].Equals("VAN"))
                {
                    if (columns.Contains(row["DAddress"].ToString()))
                    {
                        dr[row["DAddress"].ToString()] = row["C"];
                    }
                    else
                    {
                        tmp.Columns.Add(row["DAddress"].ToString(), typeof(String));
                        dr[row["DAddress"].ToString()] = row["C"];
                    }

                }
            }
            tmp.Rows.Add(dr);
        }

        private void SelectPrinter()

            {  
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo()
                {
                    CreateNoWindow = true,
                    // Arguments = pd.PrinterSettings.PrinterName,//selected printer
                    Verb = "print",
                    FileName = file //put the correct path here
                };
                p.Start();

                Printer.SetDefaultPrinter(DefPrinter);       
        }

        private void FixCollumn()
        {
            try
            {
                DataGridViewColumn column0 = dataGridView1.Columns[0];
                column0.Width = 40;
                DataGridViewColumn column1 = dataGridView1.Columns[1];
                column1.Width = 130;
                DataGridViewColumn column2 = dataGridView1.Columns[2];
                column2.Width = 130;
                DataGridViewColumn column3 = dataGridView1.Columns[3];
                column3.Width = 15;
                DataGridViewColumn column4 = dataGridView1.Columns[4];
                column4.Width = 35;
                DataGridViewColumn column5 = dataGridView1.Columns[5];
                column5.Width = 75;
                DataGridViewColumn column6 = dataGridView1.Columns[6];
                column6.Width = 115;
                DataGridViewColumn column7 = dataGridView1.Columns[7];
                column7.Width = 85;
                DataGridViewColumn column8 = dataGridView1.Columns[8];
                column8.Width = 70;
                DataGridViewColumn column9 = dataGridView1.Columns[9];
                column9.Width = 70;
                DataGridViewColumn column10 = dataGridView1.Columns[10];
                column10.Width = 70;
                DataGridViewColumn column11 = dataGridView1.Columns[11];
                column11.Width = 70;
                DataGridViewColumn column12 = dataGridView1.Columns[12];
                column12.Width = 100;
                DataGridViewColumn column13 = dataGridView1.Columns[13];
                column13.Width = 70;
                DataGridViewColumn column14 = dataGridView1.Columns[14];
                column14.Width = 40;
                DataGridViewColumn column15 = dataGridView1.Columns[15];
                column15.Width = 70;
                DataGridViewColumn column16 = dataGridView1.Columns[16];
                column16.Width = 55;
            }
            catch { }
        }

        private void Refresh1()
        {
            try
            {
                ds.Clear();
                DataGridViewCellStyle CellStyleR = new DataGridViewCellStyle();
                CellStyleR.BackColor = Color.Red;
                DataGridViewCellStyle CellStyleG = new DataGridViewCellStyle();
                CellStyleG.BackColor = Color.Green;

                MySqlCommand cmd = cnn.CreateCommand();
                if (ID.Equals("Admin"))
                {
                    cmd.CommandText = "SELECT  IDBook, Book.FirstName, Book.LastName, NumPeople, Car, phone, Email, DAddress, DAInfo,Remarks, MOArrival, DATE_FORMAT(ADate,'%d/%m/%Y') as Arrival, ATime,DATE_FORMAT(DateAdded,'%d/%m/%Y') as Added, Book.ID,Drivers.LastName as Driver FROM Book left join Drivers on Book.Driver=Drivers.ID ;";
                    // cmd.CommandText = "select * from Book where ID=@id ORDER BY HaveSeen ASC;";
                }
                else
                {
                   String s  = String.Format("SELECT  IDBook, Book.FirstName, Book.LastName, NumPeople, Car, phone, Email, DAddress, DAInfo,Remarks, MOArrival, DATE_FORMAT(ADate,'%d/%m/%Y') as Arrival, ATime,DATE_FORMAT(DateAdded,'%d/%m/%Y') as Added, Book.ID FROM Book  where Book.ID='{0}' ;",ID);
                   cmd.CommandText = s;
                }
               
                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                adap.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
                FixCollumn();
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
            catch { }

        }

        private void Refresh2()
        {
            try
            {
                dt12.Clear();
                dataGridView2.ClearSelection();
                MySqlCommand cmd = cnn.CreateCommand();
                string asss = string.Format("Select IDBook,ADate,DateAdded,DeleteBook.ID,Drivers.LastName,DAddress from DeleteBook left join Drivers on DeleteBook.Driver=Drivers.ID where HaveSeen='0';");
                cmd.CommandText = asss;
                MySqlDataAdapter adap1 = new MySqlDataAdapter(cmd);
                adap1.Fill(dt12);
                dataGridView2.DataSource = dt12.Tables[0].DefaultView;
                DataGridViewColumn column0 = dataGridView2.Columns[0];
                column0.Width = 45;
                DataGridViewColumn column1 = dataGridView2.Columns[1];
                column1.Width = 72;
                DataGridViewColumn column2 = dataGridView2.Columns[2];
                column2.Width = 72;
                DataGridViewColumn column3 = dataGridView2.Columns[3];
                column3.Width = 60;
                DataGridViewColumn column4 = dataGridView2.Columns[4];
                column4.Width = 85;

                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    row.DefaultCellStyle.BackColor = Color.Red;
                }
            }
            catch { }
        }

        private Bitmap ResizeNow(int target_width, int target_height)
        {
            Rectangle dest_rect = new Rectangle(0, 0, target_width, target_height);
            Bitmap destImage = new Bitmap(target_width, target_height);
            destImage.SetResolution(target_image.HorizontalResolution, target_image.VerticalResolution);
            using (var g = Graphics.FromImage(destImage))
            {
                g.CompositingMode = CompositingMode.SourceCopy;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (var wrapmode = new ImageAttributes())
                {
                    wrapmode.SetWrapMode(WrapMode.TileFlipXY);
                    g.DrawImage(target_image, dest_rect, 0, 0, target_image.Width, target_image.Height, GraphicsUnit.Pixel, wrapmode);
                }
            }
            return destImage;
        }

        private void PrinterOnline() {
           
            bool online = false;
            try
            {
                PrinterSettings prname = new PrinterSettings();
                PrintDocument printDocument = new PrintDocument();
                printDocument.PrinterSettings.PrinterName = prname.PrinterName;
                online = printDocument.PrinterSettings.IsValid;
            }
            catch
            {
                online = false;
            }
           // MessageBox.Show(online.ToString());

        }

        private Boolean CheckingValuesReturn()
        {
            Color k = Color.FromKnownColor(KnownColor.Control);
            Boolean t = true;

            if ((comboBox8.SelectedIndex == 0))
            {
                comboBox8.BackColor = Color.Red;
                label28.BackColor = Color.Red;
                label15.Text = " Red =Required field(*) \n Arrival Time other than 12:01";
                t = false;
            }
            else
            {
                comboBox8.BackColor = Color.White;
                label28.BackColor = k;
                // t = false;
            }

            if ((comboBox9.SelectedIndex == 0))
            {
                comboBox9.BackColor = Color.Red;
                label29.BackColor = Color.Red;
                label15.Text = " Red =Required field(*) \n Arrival Time other than 12:01";
                t = false;
            }
            else
            {
                comboBox9.BackColor = Color.White;
                label29.BackColor = k;
            }

            if ((comboBox1.SelectedIndex == 0))
            {
                comboBox1.BackColor = Color.Red;
                label5.BackColor = Color.Red;
                label15.Text = " Red =Required field(*) \n Arrival Time other than 12:01";
                t = false;
            }
            else
            {
                comboBox1.BackColor = Color.White;
                label5.BackColor = k;
            }

            if (comboBox10.Text == "12:01")
            {
                comboBox10.BackColor = Color.Red;
                t = false;
                label15.Text = " Red =Required field(*) \n Arrival Time other than 12:01";
                label15.ForeColor = Color.Red;
                label31.BackColor = Color.Red;
            }
            else
            {
                comboBox10.BackColor = Color.Red;
                label31.BackColor = k;
                label15.Text = "";
            }

            if (comboBox6.Text.Equals(""))
            {
                comboBox6.BackColor = Color.Red;
                label23.BackColor = Color.Red;
                label15.Text = " Red =Required field(*) \n Arrival Time other than 12:01";
                t = false;
            }
            else
            {
                comboBox6.BackColor = Color.White;
                label23.BackColor = k;
            }

            if (textBox2.Text.Equals(""))
            {
                label2.BackColor = Color.Red;
                label15.Text = " Red =Required field(*) \n Arrival Time other than 12:01";
                t = false;
            }
            else
            {
                label2.BackColor = k;
            }

           /* if (DateTime.Compare(dateTimePicker7.Value, DateTime.Now.AddDays(2)) == -1)
            {
                label10.BackColor = Color.Red;
                t = false;
            }
            else
            {
                label10.BackColor = k;
            }//*/

            return t;
        }

        private void PrintVoucherOnline(int lid) {
            try
            {
                MySqlCommand cmd = cnn.CreateCommand();
                string c = String.Format("Select Book.FirstName as FN,Book.LastName as LN,Car,NumPeople,ATime,ADate,Drivers.LastName as DLN,Drivers.FirstName as DFN,phone,DAddress,MOArrival from Book left join Drivers on Book.Driver=Drivers.ID where IDBook={0}", lid);
                cmd.CommandText = c;
                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adap.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    // string x= dt.Rows[0]["d"].ToString();
                    var newFile = new FileInfo(file);
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                    {
                        ExcelWorksheet oSheet = xlPackage.Workbook.Worksheets[1];
                        //Phone
                        oSheet.Cells[4, 8].Value = "'" + dt.Rows[0]["phone"].ToString();
                        //last and first name
                        oSheet.Cells[8, 5].Value = dt.Rows[0]["LN"].ToString() + " " + dt.Rows[0]["FN"].ToString();
                        //MOArrival
                        oSheet.Cells[9, 2].Value = dt.Rows[0]["MOArrival"].ToString();
                        //Date and day
                        DateTime t = Convert.ToDateTime(dt.Rows[0]["ADate"].ToString());
                        oSheet.Cells[13, 2].Value = t.ToString("dddd");
                        oSheet.Cells[14, 2].Value = t.ToString("d/MM/yyyy");
                        //Arrival time
                        oSheet.Cells[15, 2].Value = dt.Rows[0]["ATime"].ToString();
                        //DAddress
                        oSheet.Cells[18, 2].Value = dt.Rows[0]["DAddress"].ToString();
                        //oSheet.Cells[20, 2].Value = dt.Rows[0]["DAddress2"].ToString();
                        //Car
                        oSheet.Cells[33, 3].Value = dt.Rows[0]["Car"].ToString();
                        //NumPeople
                        oSheet.Cells[34, 3].Value = dt.Rows[0]["NumPeople"].ToString();
                        //ID
                        oSheet.Cells[1, 8].Value = lid;
                        oSheet.Cells[40, 3].Value = lid;
                       // MessageBox.Show(dt.Rows[0]["DLN"].ToString());
                        if (ID.Equals("Admin") && !dt.Rows[0]["DLN"].ToString().Equals(""))
                        {
                        //    MessageBox.Show(dt.Rows[0]["DLN"].ToString());
                            oSheet.Cells[3, 5].Value = dt.Rows[0]["DLN"].ToString() + " " + dt.Rows[0]["DFN"].ToString(); ;
                        }
                        else
                        {
                            oSheet.Cells[3, 5].Value = "";
                        }

                        xlPackage.Save();

                    }
                    SelectPrinter();

                }
            }
            catch {
                MessageBox.Show("Error");
            }
            

        }

        private int AcceptEvent(string lid) {
            try
            {
                if (ID.Equals("Admin"))
                {
                    return 1;
                }
                MySqlCommand cmd = cnn.CreateCommand();
                string c = String.Format("Select ADate from Book  where IDBook='{0}'", lid);
                cmd.CommandText = c;
                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                DataTable dt1 = new DataTable();
                adap.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    DateTime t = Convert.ToDateTime(dt1.Rows[0]["ADate"].ToString());
                    return DateTime.Compare(t.Date, DateTime.Now.AddDays(1).Date);
                }
                return 0;
            }
            catch { return 0; }
                
        }

        private void Pic()
        {
            System.Threading.Thread.Sleep(5000);
            pictureBox4.Hide();
            label19.Text = "";
            label15.Text = "";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connetionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ExportToFile(BackupFIle);

                        conn.Close();
                    }
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            StreamReader myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = CurPath;
            openFileDialog1.Filter = "SQl files (*.sql)|*.sql|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                 MySqlScript script = new MySqlScript(cnn, File.ReadAllText(openFileDialog1.FileName));
                  script.Delimiter = "$$";
                  script.Execute();
               // MessageBox.Show(openFileDialog1.FileName);
            }
        }

        
        public static class Printer
        {
            [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool SetDefaultPrinter(string pr);

        }

        private void comboBox12_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pr = comboBox12.Text;
            Properties.Settings.Default.Printer = pr;
            Properties.Settings.Default.Save();
            Printer.SetDefaultPrinter(pr);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox5.Checked = false;
                comboBox12.SelectedIndex = 0;
                Printer.SetDefaultPrinter(DefPrinter);

            }
            else
            {
                checkBox5.Checked = true;
            }
        }

        /*****************************************************************************************/
        /*                          END                                                          */
        /*
		SELECT * FROM DeleteBook where year(DateAdded)<year(curdate()) and month(DateAdded) > month(curdate()) or month(ADate) = month(curdate() - interval 30 day ) and month(DateAdded) <= month(curdate()-interval 60 day)
		
		SELECT * FROM DeleteBook where month(DateAdded)!=month(ADate)  and month(curdate()- interval 30 day)=month(ADate) and year(ADate)=year(curdate()- interval 30 day)
		*/
    }
}
