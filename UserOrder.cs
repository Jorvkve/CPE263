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
    public partial class UserOrder : Form
    {
        public UserOrder()
        {
            InitializeComponent();
        }
        // สร้าง object con เพื่อเชื่อมต่อกับฐานข้อมูล
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\CPE263\project 263\project 263\obj\Debug\SQL\Data Server Cafe.mdf"";Integrated Security=True;Connect Timeout=30"); // สร้างobject con และทำให้เป็นตัวเชื่อมต่อ
        void populate() //ฟังก์ชันเพื่อแสดงข้อมูลใน DataGridView ของรายการสินค้า
        {
            try
            {
                con.Open(); // เปิดการเชื่อมต่อกับฐานข้อมูล SQL Server
                string query = "select * from Itemtbls"; // สร้างคำสั่ง SQL เพื่อดึงข้อมูลทั้งหมดจากตาราง Itemtbls
                SqlDataAdapter sda = new SqlDataAdapter(query, con); // ดึงข้อมูลจากฐานข้อมูลโดยใช้คำสั่ง SQL และจัดการกับข้อมูลใน DataSet
                SqlCommandBuilder builder = new SqlCommandBuilder(sda); // สร้างคำสั่ง SQL สำหรับการ Update, Insert, Delete จาก DataAdapter
                var ds = new DataSet(); // สร้าง DataSet เพื่อเก็บข้อมูลที่ได้จากหลายตาราง
                sda.Fill(ds, "UsersTbl"); // ดึงข้อมูลจากฐานข้อมูลโดยใช้ SqlDataAdapter และเก็บข้อมูลลงใน DataSet (ds)
                ItemsGV.DataSource = ds.Tables["UsersTbl"]; // กำหนดข้อมูลใน DataGridView (ItemsGV) ให้เป็นข้อมูลในตาราง UsersTbl ใน DataSet
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); // แสดงข้อความแจ้งเตือนเมื่อเกิดข้อผิดพลาด
            }
            finally
            {
                con.Close(); // ปิดการเชื่อมต่อกับฐานข้อมูล
            }
        }
        void filterbycategory() // ฟังก์ชันสำหรับกรองรายการสินค้าตามหมวดหมู่
        {
            try
            {
                con.Open(); // เปิดการเชื่อมต่อกับฐานข้อมูล
                string query = "select * from Itemtbls where ItemCategoly = '" + categolycb.SelectedItem.ToString() + "'"; // สร้างคำสั่ง SQL เพื่อดึงรายการสินค้าจากตาราง Itemtbls โดยกรองตามหมวดหมู่ที่เลือกจาก ComboBox
                SqlDataAdapter sda = new SqlDataAdapter(query, con); // ดึงข้อมูลจากฐานข้อมูลโดยใช้คำสั่ง SQL และจัดการข้อมูลใน DataSet
                SqlCommandBuilder builder = new SqlCommandBuilder(sda); // สร้างคำสั่ง SQL สำหรับการ Update, Insert, Delete จาก DataAdapter
                var ds = new DataSet(); // สร้าง DataSet เพื่อเก็บข้อมูลที่ได้จากการดึงข้อมูล
                sda.Fill(ds, "UsersTbl"); // ดึงข้อมูลจากฐานข้อมูลโดยใช้ SqlDataAdapter และเก็บข้อมูลลงใน DataSet (ds)
                ItemsGV.DataSource = ds.Tables["UsersTbl"]; // กำหนดข้อมูลใน DataGridView (ItemsGV) ให้เป็นข้อมูลในตาราง UsersTbl ใน DataSet
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); // แสดงข้อความแจ้งเตือนเมื่อเกิดข้อผิดพลาด
            }
            finally
            {
                con.Close(); // ปิดการเชื่อมต่อกับฐานข้อมูล
            }
        }
        private void Logout_Click(object sender, EventArgs e) // ปุ่ม Logout เพื่อออกจากระบบและเข้าสู่หน้า Login
        {
            this.Hide(); // ซ่อนหน้าปัจจุบัน
            Form1 login = new Form1(); //สร้างอ็อบเจกต์ของคลาส Form1
            login.Show(); // แสดงหน้า Login
        }
        private void Itemsbtn_Click(object sender, EventArgs e) // ปุ่ม Itemsbtn เพื่อเปิดหน้า itemForm
        {
            this.Hide(); //ซ่อนหน้าปัจจุบัน
            itemForm Item = new itemForm(); //สร้างอ็อบเจกต์ของคลาส itemForm
            Item.Show(); //แสดงหน้า Item
        }

        private void Usersbtn_Click(object sender, EventArgs e) // ปุ่ม Usersbtn เพื่อเปิดหน้า UserForm
        {
            this.Hide(); //ซ่อนหน้าปัจจุบัน
            UserForm user = new UserForm(); //สร้างอ็อบเจกต์ของคลาส UserForm
            user.Show(); //แสดงหน้า user
        }

        private void Exit_Click(object sender, EventArgs e) // ปุ่ม Exit เพื่อออกจากโปรแกรม
        {
            Application.Exit();
        }
        // ตัวแปรสำหรับการจัดการรายการสินค้าที่ใส่ในตะกร้า
        int num = 0;
        int price, total;
        string item, cat;

        private void AddToCartbtn_Click(object sender, EventArgs e) // ปุ่ม AddToCartbtn เพื่อเพิ่มรายการสินค้าเข้าตะกร้า
        {
            if (QuantityTb.Text == "")
            {
                MessageBox.Show("What is The Quantity of item?"); // แสดงข้อความเมื่อไม่ได้ใส่จำนวนสินค้า
            }
            else if (flag == 0)
            {
                MessageBox.Show("Select The Quantity To be Ordered"); // แสดงข้อความเมื่อยังไม่ได้เลือกจำนวนสินค้า
                total = 0; // กำหนดให้ราคาเป็นศูนย์เมื่อยังไม่ได้เลือกจำนวนสินค้า
            }
            else
            {
                num = num + 1; // เพิ่มหมายเลขรายการในตะกร้า
                total = price * Convert.ToInt32(QuantityTb.Text); // คำนวณราคารวมของรายการนี้
                table.Rows.Add(num, item, cat, price, total); // เพิ่มรายการสินค้าในตะกร้า
                OrderGv.DataSource = table; // กำหนดตารางข้อมูลที่แสดงใน DataGridView เป็นตารางรายการสินค้า
                flag = 0; // รีเซ็ตค่า flag ให้กลับเป็นศูนย์
            }
            sum = sum + total; // นับยอดรวมของรายการที่เพิ่มเข้าตะกร้า
            labelOrderAmount.Text = ""+ sum; // แสดงยอดรวมใน labelOrderAmount
        }
        // ตารางข้อมูลสำหรับการจัดเก็บรายการสินค้าที่ใส่ในตะกร้า
        DataTable table = new DataTable();
        int flag = 0;
        int sum = 0;

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)// การกรองรายการสินค้าตามหมวดหมู่ที่เลือก
        {
            filterbycategory();
        }

        private void Refreshbtn_Click(object sender, EventArgs e)// ปุ่ม Refreshbtn เพื่อรีเฟรชข้อมูลรายการสินค้า
        {
            populate();
        }
        private void Placetheorderbtn_Click(object sender, EventArgs e) // ปุ่ม Placetheorderbtn เพื่อทำการสั่งซื้อสินค้า
        {
            con.Open(); // เปิดการเชื่อมต่อกับฐานข้อมูล
            string query = "INSERT INTO Ordertbls VALUES ('" + OrdernumTb.Text + "','" + Datelbl.Text + "', '" + SellernameTb.Text + "', '" + labelOrderAmount.Text + "')"; // สร้างคำสั่ง SQL สำหรับการเพิ่มข้อมูลการสั่งซื้อ
            SqlCommand cmd = new SqlCommand(query, con); // สร้าง SqlCommand เพื่อทำการ execute คำสั่ง SQL ในการเพิ่มข้อมูล
            cmd.ExecuteNonQuery(); // ประมวลผล execute คำสั่ง SQL
            MessageBox.Show("Order Successfully Created"); // แสดงข้อความแจ้งเตือนว่าการสั่งซื้อสำเร็จ
            con.Close(); // ปิดการเชื่อมต่อกับฐานข้อมูล
        }

        private void Viewsorderbtn_Click(object sender, EventArgs e) // เมื่อคลิกปุ่ม Viewsorderbtn เพื่อดูรายการสั่งซื้อ
        {
            ViewOrders view = new ViewOrders(); // สร้าง Instance ของ ViewOrders
            view.Show(); // แสดงหน้าต่าง ViewOrders
        }
        private void UserOrder_Load(object sender, EventArgs e) // เมื่อโหลด UserOrder Form
        {
            populate(); // เรียกใช้เมธอด populate เพื่อดึงข้อมูล
            table.Columns.Add("Num", typeof(int)); // เพิ่มคอลัมน์ Num ใน DataTable ชื่อ table โดยมีชนิดข้อมูลเป็น int
            table.Columns.Add("Item", typeof(string)); // เพิ่มคอลัมน์ Item ใน DataTable ชื่อ table โดยมีชนิดข้อมูลเป็น string
            table.Columns.Add("Categoly", typeof(string)); // เพิ่มคอลัมน์ Category ใน DataTable ชื่อ table โดยมีชนิดข้อมูลเป็น string
            table.Columns.Add("UnitPrice", typeof(int)); // เพิ่มคอลัมน์ UnitPrice ใน DataTable ชื่อ table โดยมีชนิดข้อมูลเป็น int
            table.Columns.Add("Total", typeof(int)); // เพิ่มคอลัมน์ Total ใน DataTable ชื่อ table โดยมีชนิดข้อมูลเป็น int
            OrderGv.DataSource = table; // กำหนด DataTable table ให้เป็นแหล่งข้อมูลของ DataGridView ชื่อ OrderGv
            Datelbl.Text = DateTime.Now.ToString("dd/MM/yyyy") + "\n\t" + DateTime.Now.ToString("HH:mm:ss"); // กำหนดข้อความใน Datelbl เป็นวันที่และเวลาปัจจุบัน
            SellernameTb.Text = Form1.user; // กำหนดชื่อผู้ขายจาก Form1.user ให้กับ TextBox ชื่อ SellernameTb
        }
        private void ItemsGV_CellContentClick(object sender, DataGridViewCellEventArgs e) // การเลือก Item จาก DataGridView ItemsGV
        {
            // เก็บข้อมูลของ Item ที่เลือกลงในตัวแปรต่างๆ
            item = ItemsGV.SelectedRows[0].Cells[1].Value.ToString(); // เก็บชื่อ Item จากคอลัมน์ที่ 1
            cat = ItemsGV.SelectedRows[0].Cells[2].Value.ToString(); // เก็บหมวดหมู่ของ Item จากคอลัมน์ที่ 2
            price = Convert.ToInt32(ItemsGV.SelectedRows[0].Cells[3].Value.ToString()); // เก็บราคาของ Item จากคอลัมน์ที่ 3 โดยแปลงให้อยู่ในรูปแบบ int
            flag = 1; // ตั้งค่า flag เป็น 1 เพื่อบ่งชี้ว่าได้ทำการเลือก Item แล้ว
        }
    }
}