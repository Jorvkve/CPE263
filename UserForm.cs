using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; //libary SQL
using System.Collections;

namespace project_263
{
    public partial class UserForm : Form
    {
        public UserForm()
        {
            InitializeComponent();
        }
        // สร้าง object con เพื่อเชื่อมต่อกับฐานข้อมูล
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\CPE263\project 263\project 263\obj\Debug\SQL\Data Server Cafe.mdf"";Integrated Security=True;Connect Timeout=30"); // สร้างobject con และทำให้เป็นตัวเชื่อมต่อ
        void populate() // ฟังก์ชันเพื่อแสดงข้อมูลใน DataGridView
        {
            try
            {
                con.Open(); //เปิดการเชื่อมต่อกับฐานข้อมูล SQL Server
                string query = "select * from Usertbls"; //สร้างคำสั่ง SQL select * from Usertbls เพื่อดึงข้อมูลทั้งหมดจากตาราง Itemtbls
                SqlDataAdapter sda = new SqlDataAdapter(query, con); //ดึงข้อมูลจากฐานข้อมูลโดยใช้คำสั่ง SQL และจัดการกับข้อมูลใน DataSet
                SqlCommandBuilder builder = new SqlCommandBuilder(sda); //สร้างคำสั่ง SQL สำหรับการ Update, Insert, Delete โดยอัตโนมัติจาก DataAdapter
                var ds = new DataSet(); //สร้างDataSet ที่สามารถเก็บข้อมูลจากหลายตาราง
                sda.Fill(ds, "UsersTbl"); //ดึงข้อมูลจากฐานข้อมูลโดยใช้ SqlDataAdapter และเก็บข้อมูลที่ได้ลงใน DataSet(ds)
                UsersGV.DataSource = ds.Tables["UsersTbl"]; //แสดงข้อมูลจาก UsersTbl ภายใน DataSet (ds) โดยใช้ DataSource ของ ItemsGV เพื่อแสดงข้อมูลในรูปแบบตารางที่ให้ผู้ใช้มองเห็น.
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); // เกิดข้อผิดพลาดในการ query หรือการเชื่อมต่อกับฐานข้อมูลไม่ได้ จะทำการแสดง MessageBox
            }
            finally
            {
                con.Close(); //ปิดการเชื่อมต่อกับฐานข้อมูล SQL Server
            }
        }
        private void Orderbtn_Click(object sender, EventArgs e) // คลิกปุ่ม Orderbtn เพื่อเปิดหน้า UserOrder
        {
            UserOrder uorder = new UserOrder(); // สร้างอ็อบเจกต์ของคลาส UserOrder
            uorder.Show(); // แสดง UserOrder form
            this.Hide(); // ซ่อน form ปัจจุบัน
        }

        private void Itemsbtn_Click(object sender, EventArgs e) // ปุ่ม Itemsbtn เพื่อเปิดหน้า itemForm
        {
            itemForm item = new itemForm(); // สร้างอ็อบเจกต์ของคลาส itemForm
            item.Show(); // แสดง itemForm
            this.Hide(); // ซ่อน form ปัจจุบัน
        }

        private void Logout_Click(object sender, EventArgs e) // ปุ่ม Logout เพื่อออกจากระบบและกลับสู่หน้า Login
        {
            this.Hide(); // ซ่อน form ปัจจุบัน
            Form1 login = new Form1(); // สร้างอ็อบเจกต์ของคลาส Form1
            login.Show(); // แสดงฟอร์มหน้า Login
        }

        private void Exit_Click(object sender, EventArgs e) // ปุ่ม Exit เพื่อปิดโปรแกรม
        {
            Application.Exit();
        }

        private void Add_Click(object sender, EventArgs e) // ปุ่ม Add เพื่อเพิ่มผู้ใช้งาน
        {
            con.Open(); // เปิดการเชื่อมต่อกับฐานข้อมูล
            string query = "insert into Usertbls (Uname, Uphone, Upassword) values('" + UnameTb.Text + "','" + UphoneTb.Text + "','" + UpassTb.Text + "')"; // สร้างคำสั่ง SQL เพื่อเพิ่มข้อมูลผู้ใช้ใหม่ลงในตาราง Usertbls
            SqlCommand cmd = new SqlCommand(query, con); // สร้าง SqlCommand เพื่อประมวลผลคำสั่ง SQL
            cmd.ExecuteNonQuery(); // ประมวลผลคำสั่ง SQL เพื่อเพิ่มข้อมูลลงในฐานข้อมูล
            MessageBox.Show("User Successfully Created"); // แสดงข้อความแจ้งเตือนว่าผู้ใช้ถูกสร้างเรียบร้อยแล้ว
            con.Close(); // ปิดการเชื่อมต่อกับฐานข้อมูล
        }
        private void UserForm_Load(object sender, EventArgs e) // เมื่อโหลด Form UserForm
        {
            populate();
        }

        private void UsersGV_CellContentClick(object sender, DataGridViewCellEventArgs e) // เมื่อคลิกเลือกแถวใน DataGridView UsersGV
        {
            // กำหนดค่าของช่อง UnameTb เป็นค่าที่ถูกเลือกในแถวแรกและคอลัมน์แรกในตารางข้อมูล
            UnameTb.Text = UsersGV.SelectedRows[0].Cells[0].Value.ToString();
            // กำหนดค่าของช่อง UphoneTb เป็นค่าที่ถูกเลือกในแถวแรกและคอลัมน์ที่สองในตารางข้อมูล
            UphoneTb.Text = UsersGV.SelectedRows[0].Cells[1].Value.ToString();
            // กำหนดค่าของช่อง UpassTb เป็นค่าที่ถูกเลือกในแถวแรกและคอลัมน์ที่สามในตารางข้อมูล
            UpassTb.Text = UsersGV.SelectedRows[0].Cells[2].Value.ToString();
        }

        private void Deletebtn_Click(object sender, EventArgs e) // ปุ่ม Deletebtn เพื่อลบผู้ใช้งาน
        {
            // ตรวจสอบว่ากล่องข้อความ UphoneTb มีค่าว่างหรือไม่
            if (UphoneTb.Text == "")
            {
                // หากไม่ได้เลือกข้อมูลผู้ใช้ แสดงข้อความเตือนให้ผู้ใช้ทราบ
                MessageBox.Show("Select The User to be Deleted");
            }
            else
            {
                con.Open(); // เปิดการเชื่อมต่อกับฐานข้อมูล

                // สร้างคำสั่ง SQL สำหรับลบข้อมูลผู้ใช้งาน
                string query = "delete from Usertbls Where Uphone = '" + UphoneTb.Text + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery(); // ทำการลบข้อมูลผู้ใช้งาน

                // แสดงข้อความแจ้งเตือนเมื่อลบข้อมูลผู้ใช้งานสำเร็จ
                MessageBox.Show("User Successfully Deleted");

                con.Close(); // ปิดการเชื่อมต่อกับฐานข้อมูล
            }
        }

        private void Refreshbtn_Click(object sender, EventArgs e) // ปุ่ม Refreshbtn เพื่อรีเฟรชข้อมูล
        {
            populate(); //เรียกฟังก์ชัน populate เพื่อรีเฟรชข้อมูล
        }

        private void Editbtn_Click(object sender, EventArgs e)
        {
            // ตรวจสอบว่ากล่องข้อความแต่ละช่องมีการกรอกข้อมูลหรือไม่
            if (UphoneTb.Text == "" || UpassTb.Text == "" || UnameTb.Text == "")
            {
                // หากข้อมูลในกล่องข้อความบางช่องยังไม่ได้กรอก จะแสดงข้อความเตือนให้กรอกข้อมูลให้ครบถ้วน
                MessageBox.Show("Fill All The fields");
            }
            else
            {
                con.Open(); // เปิดการเชื่อมต่อกับฐานข้อมูล
                // สร้างคำสั่ง SQL สำหรับอัปเดตข้อมูลผู้ใช้งาน
                string query = "UPDATE Usertbls SET Uname = @Uname, Upassword = @Upassword WHERE Uphone = @Uphone";
                SqlCommand cmd = new SqlCommand(query, con);
                // เพิ่มค่าพารามิเตอร์ที่ใช้ในการอัปเดตข้อมูลผู้ใช้งาน
                cmd.Parameters.AddWithValue("@Uname", UnameTb.Text);
                cmd.Parameters.AddWithValue("@Upassword", UpassTb.Text);
                cmd.Parameters.AddWithValue("@Uphone", UphoneTb.Text);
                int rowsAffected = cmd.ExecuteNonQuery(); // ทำการอัปเดตข้อมูลและเก็บจำนวนแถวที่ถูกอัปเดต
                if (rowsAffected > 0)
                {
                    // ถ้ามีการอัปเดตข้อมูลผู้ใช้สำเร็จ จะแสดงข้อความยืนยัน
                    MessageBox.Show("User Successfully Updated");
                }
                else
                {
                    // ถ้าไม่มีการอัปเดตข้อมูลผู้ใช้ จะแสดงข้อความแจ้งเตือนว่าการอัปเดตไม่สำเร็จ
                    MessageBox.Show("Update failed");
                }
                con.Close(); // ปิดการเชื่อมต่อกับฐานข้อมูล
            }
        }
    }
}
