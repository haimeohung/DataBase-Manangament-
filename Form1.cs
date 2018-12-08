using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BT6._1
{

    public partial class Form1 : Form
    {
        static String connString = @"Data Source=VTHCOMPUTER;Initial Catalog=Thongtinsinhvien;Integrated Security=True";
        SqlConnection connection = new SqlConnection(connString);
        int index;
        public Form1()
        {
            InitializeComponent();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private SqlCommand open()
        {
            String sqlQuery = "select * from [Thongtinsinhvien].[dbo].[SINHVIEN]";
            //Tao mot Sqlcommand de thuc hien cau lenh truy van da chuan bi voi ket noi hien tai
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            //Thuc hien cau truy van va nhan ve mot doi tuong reader ho tro do du lieu  
            return command;
        }
        //Query
        private void insert()
        {
            String sqlQuery = "insert into [Thongtinsinhvien].[dbo].[SINHVIEN] values (@masv, @hoten, @ngsinh)";
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            //Thuc hien cau truy van va nhan ve mot doi tuong reader ho tro do du lieu
            command.Parameters.Add("@masv", SqlDbType.Char, 4).Value = txt_masv.Text.ToString();
            command.Parameters.Add("@hoten", SqlDbType.VarChar, 40).Value = txt_ten.Text.ToString();
            command.Parameters.Add("@ngsinh", SqlDbType.SmallDateTime, 4).Value = txt_ngsinh.Text.ToString();
            try
            {
                command.ExecuteNonQuery();

            }
            catch (Exception)
            {
                MessageBox.Show("Trùng khóa, hãy nhập lại");
            }
        }
        private void delete()
        {           
            int CurrentIndex = dataGridView1.CurrentRow.Index;
            string id = dataGridView1.Rows[CurrentIndex].Cells[0].Value.ToString();
            String sqlQuery = "delete from [Thongtinsinhvien].[dbo].[SINHVIEN] where MASV = '" + id + "'";
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            try
            {
                command.ExecuteNonQuery();

            }
            catch (Exception)
            {
                MessageBox.Show("Feo");
            }
        }
        private void update()
        {
         
            int CurrentIndex = dataGridView1.CurrentRow.Index;
            string id = dataGridView1.Rows[CurrentIndex].Cells[0].Value.ToString();
            String sqlQuery = "update [Thongtinsinhvien].[dbo].[SINHVIEN] set HOTEN = '" + txt_ten.Text + "', NGSINH = '"
                + txt_ngsinh.Text + "' where MASV = '" + id + "'";
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            try
            {
                command.ExecuteNonQuery();

            }
            catch (Exception)
            {
                MessageBox.Show("Update Failed!");
            }
        }
        private void Connect()
        {        
            try
            {
                //Mo ket noi
                connection.Open();
                //Chuan bi cau lenh query viet bang SQL               
            }
            catch (InvalidOperationException ex)
            {
                //xu ly khi ket noi co van de
                //MessageBox.Show("Khong the mo ket noi hoac ket noi da mo truoc do");
            }
            catch (Exception ex)
            {
                //xu ly khi ket noi co van de
                MessageBox.Show("Ket noi xay ra loi hoac doc du lieu bi loi");
            }
            finally
            {
                //Dong ket noi sau khi thao tac ket thuc
                //connection.Close();

            }
        }
        //btn_Connect
        private void btn_Connect_Click(object sender, EventArgs e)
        {
            connection.Close();
            button8.Enabled = false;                     
            Connect();
            SqlCommand command = open(); 
            SqlDataReader reader = command.ExecuteReader();
            dataGridView1.Rows.Clear();
            //Su dung reader de doc tung dong du lieu
            //va thuc hien thao tac xu ly mong muon voi du lieu doc len
            while (reader.HasRows)//con dong du lieu thi doc tiep
            {
                if (reader.Read() == false) return;//doc ko duoc thi return
                                                   //xu ly khi da doc du lieu len
                dataGridView1.Rows.Add(reader.GetString(0), reader.GetString(1), reader.GetDateTime(2));
            }
            reader.Close();
            connection.Close();
        }
        //btn
        private void btn_Save_Click(object sender, EventArgs e)
        {    
            try
            {
                Connect();
                if (index == 1)
                {
                    insert();
                }
                if (index == 2)
                {
                    update();
                }

                dataGridView1.Rows.Clear();
                SqlCommand command = open();
                SqlDataReader reader = command.ExecuteReader();
                //Su dung reader de doc tung dong du lieu
                //va thuc hien thao tac xu ly mong muon voi du lieu doc len
                while (reader.HasRows)//con dong du lieu thi doc tiep
                {
                    if (reader.Read() == false) break;//doc ko duoc thi return
                                                      //xu ly khi da doc du lieu len
                    dataGridView1.Rows.Add(reader.GetString(0), reader.GetString(1), reader.GetDateTime(2));
                }
                reader.Close();
                connection.Close();
                index = 0;
            }
            catch (Exception) { }
        }
       
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            Connect();
            index = 3;
            delete();
            dataGridView1.Rows.Clear();
            SqlCommand command = open();
            SqlDataReader reader = command.ExecuteReader();
            //Su dung reader de doc tung dong du lieu
            //va thuc hien thao tac xu ly mong muon voi du lieu doc len
            while (reader.HasRows)//con dong du lieu thi doc tiep
            {
                if (reader.Read() == false) break;//doc ko duoc thi return
                                                   //xu ly khi da doc du lieu len
                dataGridView1.Rows.Add(reader.GetString(0), reader.GetString(1), reader.GetDateTime(2));
            }
            reader.Close();
            connection.Close();
            btn_Save.Enabled = false;

        }
        private void btn_Add_Click(object sender, EventArgs e)
        {
            index = 1;
            txt_masv.Clear();
            txt_ngsinh.Clear();
            txt_ten.Clear();
        }
        private void btn_Update_Click(object sender, EventArgs e)
        {
            index = 2;
        }

        private void button8_Click(object sender, EventArgs e)
        {

            btn_Connect.Enabled = false;
            String value = ".";
            FileStream fs = new FileStream("data.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader rd = new StreamReader(fs);
            value = rd.ReadLine();
            String[] col = value.Split(',');

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                int CurrentIndex = dataGridView1.CurrentRow.Index;
                txt_masv.Text = dataGridView1.Rows[CurrentIndex].Cells[0].Value.ToString();
                txt_ten.Text = dataGridView1.Rows[CurrentIndex].Cells[1].Value.ToString();
                txt_ngsinh.Text = dataGridView1.Rows[CurrentIndex].Cells[2].Value.ToString();
            }
            catch (Exception) { }
        }
        private void button7_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (txt_search.Text == "") btn_Search.Enabled = false;
            else btn_Search.Enabled = true;
            if (index == 0)
            {
                btn_Add.Enabled = true;
                btn_Delete.Enabled = true;
                btn_Update.Enabled = true;
                btn_Save.Enabled = false;

                txt_masv.Enabled = false;
                txt_ngsinh.Enabled = false;
                txt_ten.Enabled = false;
            }
            if (index == 1)
            {
             
                btn_Add.Enabled = false;
                btn_Delete.Enabled = false;
                btn_Update.Enabled = false;
                btn_Save.Enabled = true;

                txt_masv.Enabled = true;
                txt_ngsinh.Enabled = true;
                txt_ten.Enabled = true;
            }
            if (index == 2)
            {
                btn_Add.Enabled = false;
                btn_Delete.Enabled = false;
                btn_Update.Enabled = false;
                btn_Save.Enabled = true;

                txt_masv.Enabled = false;
                txt_ngsinh.Enabled = true;
                txt_ten.Enabled = true;

            }
            if (index == 3)
            {
                btn_Add.Enabled = false;
                btn_Delete.Enabled = false;
                btn_Update.Enabled = false;
                btn_Save.Enabled = true;

                txt_masv.Enabled = false;
                txt_ngsinh.Enabled = false;
                txt_ten.Enabled = false;

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            btn_Connect.PerformClick();
            String searchvalue = txt_search.Text.ToString();
            int []index = new int[50];
            string[] id = new string[50];
            string[] name = new string[50];
            string[] day = new string[50];
            int i = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                try
                {
                    if (row.Cells[1].Value.ToString().Equals(searchvalue) || row.Cells[0].Value.ToString().Equals(searchvalue))
                    {
                        id[i] = dataGridView1.Rows[row.Index].Cells[0].Value.ToString();
                        name[i] = dataGridView1.Rows[row.Index].Cells[1].Value.ToString();
                        day[i] = dataGridView1.Rows[row.Index].Cells[2].Value.ToString();
                        i++;
                    }
                }
                catch (Exception) {
                }
               
            }
            dataGridView1.Rows.Clear();
            for (int j = 0; j < i; j ++)
            {
                dataGridView1.Rows.Add(id[j], name[j], day[j]);
            }
            connection.Close();
        }
    }
}
