using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace project_263
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // สร้าง object con และทำให้เป็นตัวเชื่อมต่อ Database
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\CPE263\project 263\project 263\obj\Debug\SQL\Data Server Cafe.mdf"";Integrated Security=True;Connect Timeout=30");
        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit(); // เมื่อกดปุ่มออกจากแอพพลิเคชั่น
        }

        private void Guest_Click(object sender, EventArgs e)
        {
            this.Hide(); //ซ่อนฟอร์มปัจจุบันหรือทำให้ไม่แสดงผลบนหน้าจอ
            GuestOrder guest = new GuestOrder(); //เป็นหน้าต่างหรือฟอร์มที่จะแสดงหลังจากที่ฟอร์มปัจจุบันถูกซ่อน
            guest.Show(); //ทำให้ฟอร์มของGuestOrderแสดงผลบนหน้าจอโดยใช้เมทอดShow()
        }
        public static string user;// สร้างตัวแปรสำหรับเก็บชื่อผู้ใช้
        private void Loginbtn_Click(object sender, EventArgs e)
        {
            user = UnameTb.Text; // เก็บค่าชื่อผู้ใช้จาก TextBox
            if (UnameTb.Text == "" || passwordTb.Text == "")// ตรวจสอบว่าช่องว่างหรือไม่ ถ้ามีให้แสดง MessageBox
            {
                MessageBox.Show("Enter A Username Or password");
            }
            else
            {
                con.Open(); //เปิดการเชื่อมต่อกับฐานข้อมูล
                //สร้าง SqlDataAdapter เพื่อดึงข้อมูลจำนวนแถวที่มี Username และ Password ตรงกับที่ผู้ใช้ป้อนเข้ามาใน TextBoxes
                SqlDataAdapter sda = new SqlDataAdapter("select count(*) from Usertbls where Uname = '" + UnameTb.Text + "' and Upassword = '" + passwordTb.Text + "'", con);
                DataTable dt = new DataTable(); //สร้าง DataTable ชื่อ dt เพื่อเก็บผลลัพธ์ที่ได้จากการ query ที่ทำโดย SqlDataAdapter
                sda.Fill(dt); //นำข้อมูลที่ได้จากการ query มาเติมใน DataTable
                if (dt.Rows[0][0].ToString()=="1") //ตรวจสอบผลลัพธ์ที่ได้จากการ query
                {
                    // ถ้ามีผลลัพธ์ที่เป็น 1 แสดง UserOrder Form และซ่อน Form ปัจจุบัน
                    UserOrder uorder = new UserOrder();
                    uorder.Show();
                    this.Hide();
                }
                else
                {
                    // แสดง MessageBox กรณีผู้ใช้ใส่ชื่อผู้ใช้หรือรหัสผ่านผิด
                    MessageBox.Show("Wrong Username or Password");
                }
                con.Close(); //ปิดการเชื่อมต่อกับฐานข้อมูล SQL Server
            }
        }
    }
}