using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project_263
{
    public partial class itemForm : Form
    {
        public itemForm()
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
                string query = "select * from Itemtbls"; //สร้างคำสั่ง SQL select * from Itemtbls เพื่อดึงข้อมูลทั้งหมดจากตาราง Itemtbls
                SqlDataAdapter sda = new SqlDataAdapter(query, con); //ดึงข้อมูลจากฐานข้อมูลโดยใช้คำสั่ง SQL และจัดการกับข้อมูลใน DataSet
                SqlCommandBuilder builder = new SqlCommandBuilder(sda); //สร้างคำสั่ง SQL สำหรับการ Update, Insert, Delete โดยอัตโนมัติจาก DataAdapter
                var ds = new DataSet(); //สร้างDataSet ที่สามารถเก็บข้อมูลจากหลายตาราง
                sda.Fill(ds, "UsersTbl"); //ดึงข้อมูลจากฐานข้อมูลโดยใช้ SqlDataAdapter และเก็บข้อมูลที่ได้ลงใน DataSet(ds)
                ItemsGV.DataSource = ds.Tables["UsersTbl"]; //แสดงข้อมูลจาก UsersTbl ภายใน DataSet (ds) โดยใช้ DataSource ของ ItemsGV เพื่อแสดงข้อมูลในรูปแบบตารางที่ให้ผู้ใช้มองเห็น.
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); //เกิดข้อผิดพลาดในการ query หรือการเชื่อมต่อกับฐานข้อมูลไม่ได้ จะทำการแสดง MessageBox
            }
            finally
            {
                con.Close(); //ปิดการเชื่อมต่อกับฐานข้อมูล SQL Server
            }
        }
        private void Userbtn_Click(object sender, EventArgs e) // เมื่อคลิกปุ่มย้อนกลับไปหน้า UserForm
        {
            this.Hide(); //ซ่อนหน้าปัจจุบันที่กำลังรัน
            UserForm user = new UserForm(); //สร้างการเรียกหน้า UserForm 
            user.Show(); //แสดงหน้า UserForm
        }

        private void Orderbtn_Click(object sender, EventArgs e) // เมื่อคลิกปุ่มไปยังหน้า UserOrder
        {
            this.Hide(); //ซ่อนหน้าปัจจุบันที่กำลังรัน
            UserOrder order = new UserOrder(); //สร้างการเรียกหน้า UserOrder
            order.Show(); //แสดงหน้า order
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit(); // เมื่อคลิก X เพื่อออกจากแอพพลิเคชั่น
        }

        private void LogOut_Click(object sender, EventArgs e) // เมื่อคลิก LogOut เพื่อกลับไปหน้า Login
        {
            this.Hide(); //ซ่อนหน้าปัจจุบันที่กำลังรัน
            Form1 login =  new Form1(); //สร้างการเรียกหน้า Form1
            login.Show(); //แสดงหน้า login
        }

        private void Addbtn_Click(object sender, EventArgs e) //คลิกปุ่ม Addbtn เพื่อเพิ่มรายการสินค้า
        {
            //ตรวจสอบว่ามีการกรอกข้อมูลครบถ้วนหรือไม่ ถ้ามีข้อมูลใดข้อมูลหนึ่งขาดหายไป จะแสดง MessageBox
            if (ItemnameTb.Text == "" || ItemnumTb.Text == "" || ItempriceTb.Text == "")
            {
                MessageBox.Show("Fill All The Data");
            }
            else
            {
                try
                {
                    //เมื่อข้อมูลถูกกรอกครบถ้วน จะทำการเชื่อมต่อกับฐานข้อมูล เพื่อทำการเพิ่มรายการสินค้าลงในตาราง Itemtbls
                    con.Open();
                    // ตรวจสอบการเลือกหมวดหมู่
                    if (CategolyCb.SelectedItem != null) // ตรวจสอบว่ามีการเลือกหมวดหมู่หรือไม่ ถ้ามีการเลือกหมวดหมู่จะทำการเพิ่มข้อมูลลงในฐานข้อมูล
                    {
                        string query = "insert into Itemtbls (ItemNum, ItemName, ItemCategoly, ItemPrice) values(@ItemNum, @ItemName, @ItemCategory, @ItemPrice)";
                        SqlCommand cmd = new SqlCommand(query, con); //สร้างSqlCommand เพื่อเตรียมส่งคำสั่งSQL ไปฐานข้อมูล โดยใช้คำสั่ง SQL และเชื่อมต่อฐานข้อมูลที่เรากำหนดไว้ในตัวแปร con
                        cmd.Parameters.AddWithValue("@ItemNum", ItemnumTb.Text); //กำหนดค่าพารามิเตอร์ของคำสั่ง SQL เพื่อป้องกันการโจมตีด้วย SQL Injection โดยกำหนดค่าของแต่ละพารามิเตอร์ให้เท่ากับข้อมูลที่ผู้ใช้ป้อนเข้ามา
                        cmd.Parameters.AddWithValue("@ItemName", ItemnameTb.Text);
                        cmd.Parameters.AddWithValue("@ItemCategory", CategolyCb.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@ItemPrice", ItempriceTb.Text);
                        cmd.ExecuteNonQuery(); //ประมวลผลคำสั่งSQL: การเพิ่มข้อมูลไปยังฐานข้อมูล
                        MessageBox.Show("Item Successfully Created"); //แสดง MessageBox เพื่อแจ้งให้ทราบว่าการเพิ่มรายการสินค้าสำเร็จ
                    }
                    else //แสดง MessageBox เพื่อแจ้งให้ผู้ใช้ทราบว่าต้องเลือกหมวดหมู่สินค้า
                    {
                        MessageBox.Show("Please select a category");
                    }
                }
                catch (Exception ex) //เมื่อพบข้อผิดพลาด จะจัดเก็บข้อมูลไว้ใน ex
                {
                    MessageBox.Show("Error: " + ex.Message); //MessageBox แสดงข้อความที่บอกถึงข้อผิดพลาดที่เกิดขึ้น ข้อความที่แสดงจะประกอบด้วยคำว่า "Error: " ตามด้วยข้อความของข้อผิดพลาดที่ถูกเก็บไว้ในตัวแปร
                }
                finally
                {
                    con.Close(); //ปิดการเชื่อมต่อกับฐานข้อมูล
                }
            }
        }
        // เมื่อโหลด Form itemForm
        private void itemForm_Load(object sender, EventArgs e)
        {
            populate(); // เรียกใช้ฟังก์ชัน populate เพื่อแสดงข้อมูลใน DataGridView
        }

        private void Refreshbtn_Click(object sender, EventArgs e) // เมื่อคลิกปุ่ม Refreshbtn เพื่อรีเฟรชข้อมูล
        {
            populate();
        }
        // เมื่อเลือกแถวใน DataGridView ItemsGV
        private void ItemsGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // กำหนดข้อมูลที่เลือกจาก DataGridView ไปยัง TextBox และ ComboBox ที่เก็บข้อมูลรายการสินค้า
            ItemnumTb.Text = ItemsGV.SelectedRows[0].Cells[0].Value.ToString(); // กำหนดข้อมูลเลขที่สินค้าลงใน TextBox
            ItemnameTb.Text = ItemsGV.SelectedRows[0].Cells[1].Value.ToString(); // กำหนดข้อมูลชื่อสินค้าลงใน TextBox
            CategolyCb.SelectedItem = ItemsGV.SelectedRows[0].Cells[2].Value.ToString(); // กำหนดข้อมูลหมวดหมู่สินค้าลงใน ComboBox
            ItempriceTb.Text = ItemsGV.SelectedRows[0].Cells[3].Value.ToString(); // กำหนดข้อมูลราคาสินค้าลงใน TextBox
        }
        private void Deletebtn_Click(object sender, EventArgs e)
        {
            if (ItemnumTb.Text == "")
            {
                MessageBox.Show("Select The Item to be Deleted");
            }
            else
            {
                con.Open(); // เปิดการเชื่อมต่อกับฐานข้อมูล
                // สร้างคำสั่ง SQL เพื่อลบรายการสินค้าที่มี ItemNum ตรงกับที่ผู้ใช้เลือก
                string query = "delete from Itemtbls Where ItemNum = '" + ItemnumTb.Text + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery(); // สั่งให้คำสั่ง SQL ทำงาน
                MessageBox.Show("Item Successfully Deleted"); // แสดงข้อความแจ้งเตือนการลบรายการสินค้าสำเร็จ
                con.Close(); // ปิดการเชื่อมต่อกับฐานข้อมูล
            }
        }
        // เมื่อคลิกปุ่ม Editbtn เพื่อแก้ไขรายการสินค้า
        private void Editbtn_Click(object sender, EventArgs e)
        {
            if (ItemnumTb.Text == "" || ItemnameTb.Text == "" || ItempriceTb.Text == "" || CategolyCb.Text == "")
            {
                MessageBox.Show("Fill All The fields");
            }
            else
            {
                con.Open(); // เปิดการเชื่อมต่อกับฐานข้อมูล
                // สร้างคำสั่ง SQL เพื่ออัปเดตข้อมูลในตาราง Itemtbls โดยใช้เงื่อนไข ItemNum เป็นตัวกำหนด
                string query = "UPDATE Itemtbls SET ItemName = @ItemName, ItemPrice = @ItemPrice, ItemCategoly = @ItemCategoly WHERE ItemNum = @ItemNum";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ItemName", ItemnameTb.Text);
                cmd.Parameters.AddWithValue("@ItemPrice", ItempriceTb.Text);
                cmd.Parameters.AddWithValue("@ItemCategoly", CategolyCb.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@ItemNum", ItemnumTb.Text);
                int rowsAffected = cmd.ExecuteNonQuery(); // ส่งคำสั่ง SQL ไปทำงานและบันทึกจำนวนแถวที่ถูกอัปเดต
                if (rowsAffected > 0) // ถ้ามีการอัปเดตข้อมูล
                {
                    MessageBox.Show("Item Successfully Updated");
                }
                else // ถ้าไม่มีการอัปเดตข้อมูล
                {
                    MessageBox.Show("Update failed");
                }
                con.Close(); // ปิดการเชื่อมต่อกับฐานข้อมูล
            }
        }
    }
}