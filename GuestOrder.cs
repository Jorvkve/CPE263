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
    public partial class GuestOrder : Form
    {
        public GuestOrder()
        {
            InitializeComponent();
        }
        // สร้าง object con เพื่อเชื่อมต่อกับฐานข้อมูล
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\CPE263\project 263\project 263\obj\Debug\SQL\Data Server Cafe.mdf"";Integrated Security=True;Connect Timeout=30"); // สร้างobject con และทำให้เป็นตัวเชื่อมต่อ
        
        // ฟังก์ชันที่ใช้ในการดึงข้อมูลจากฐานข้อมูลและแสดงใน DataGridView
        void populate()
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
        void filterbycategory()// ฟังก์ชันสำหรับกรองข้อมูลตามหมวดหมู่
        {
            try
            {
                con.Open(); //เปิดการเชื่อมต่อกับฐานข้อมูล
                string query = "select * from Itemtbls where ItemCategoly = '" + categolycb.SelectedItem.ToString() + "'"; //ดึงข้อมูลที่ตรงกับหมวดหมู่ที่เลือกจาก ComboBox
                SqlDataAdapter sda = new SqlDataAdapter(query, con); //นำข้อมูลที่ได้จากการ query มาเก็บใน DataSet(ds) โดยกำหนดชื่อตารางเป็น UsersTbl
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet(); //สร้างDataSet ที่สามารถเก็บข้อมูลจากหลายตาราง
                sda.Fill(ds, "UsersTbl");
                ItemsGV.DataSource = ds.Tables["UsersTbl"]; //แสดงข้อมูลที่อยู่ในตารางที่ชื่อ UsersTbl ภายใน DataSet(ds)
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
        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit(); // เมื่อกดปุ่มออกจากแอพพลิเคชั่น
        }

        private void Logout_Click(object sender, EventArgs e) // เมื่อคลิกที่ Label เพื่อกลับไปหน้า Login
        {
            this.Hide(); //ซ่อน Form ปัจจุบันที่กำลังรัน
            Form1 login = new Form1(); //สร้างอ็อบเจกต์ใหม่ของ Form1 เป็นหน้าที่จะนำผู้ใช้กลับไปยังหน้า Login
            login.Show(); //แสดงหน้าlogin
        }

        private void GuestOrder_Load(object sender, EventArgs e) // เมื่อโหลด Form GuestOrder
        {
            populate();// เรียกใช้ฟังก์ชัน populate เพื่อแสดงข้อมูลใน DataGridView
            // กำหนดคอลัมน์ของตาราง OrderGv
            table.Columns.Add("Num", typeof(int));
            table.Columns.Add("Item", typeof(string));
            table.Columns.Add("Categoly", typeof(string));
            table.Columns.Add("UnitPrice", typeof(int));
            table.Columns.Add("Total", typeof(int));
            OrderGv.DataSource = table;
            Datelbl.Text = DateTime.Now.ToString("dd/MM/yyyy") + "\n\t" + DateTime.Now.ToString("HH:mm:ss");
        }
        int num = 0;
        int price, total;
        string item, cat;
        DataTable table = new DataTable();
        int flag = 0;
        int sum = 0;
        // เมื่อมีการคลิกเซลล์ใน DataGridView ItemsGV
        private void ItemsGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            item = ItemsGV.SelectedRows[0].Cells[1].Value.ToString();
            //กำหนดค่าของตัวแปร item เป็นค่าที่ถูกเลือกในเซลล์แถวที่ 1 ของ DataGridView ซึ่งเก็บชื่อสินค้า
            cat = ItemsGV.SelectedRows[0].Cells[2].Value.ToString();
            //กำหนดค่าของตัวแปร cat เป็นค่าที่ถูกเลือกในเซลล์แถวที่ 2 ของ DataGridView ซึ่งเก็บหมวดหมู่ของสินค้า
            price = Convert.ToInt32(ItemsGV.SelectedRows[0].Cells[3].Value.ToString());
            //กำหนดค่าของตัวแปร price เป็นค่าที่ถูกเลือกในเซลล์แถวที่ 3 ของ DataGridView ซึ่งเก็บราคาของสินค้าโดยแปลงเป็น integer
            flag = 1;
            //กำหนดค่า flag เป็น 1 เพื่อบ่งชี้ว่ามีการเลือกรายการสินค้าแล้ว.
        }
        // เมื่อคลิกปุ่ม PlaceTheOrderbtn
        private void PlaceTheOrderbtn_Click(object sender, EventArgs e)
        {
            con.Open(); //เปิดการเชื่อมต่อกับฐานข้อมูล
            string query = "INSERT INTO Ordertbls VALUES ('" + OrderNumTb.Text + "','" + Datelbl.Text + "', '" + SellerNameTb.Text + "', '" + labelOrderAmount.Text + "')"; //คำสั่ง SQL ประเภท INSERT เพิ่มข้อมูลเข้าไปในตาราง Ordertbls ในฐานข้อมูล
            SqlCommand cmd = new SqlCommand(query, con); //สร้างอ็อบเจกต์ SqlCommand เพื่อทำการสร้างคำสั่ง SQL ในการเพิ่มข้อมูลลงในฐานข้อมูล
            cmd.ExecuteNonQuery(); //ส่งคำสั่ง SQL ไปยังฐานข้อมูลเพื่อทำการ ประมวลผลคำสั่ง SQL ที่สร้างขึ้น คือการเพิ่มข้อมูลเข้าไปในตาราง Ordertbls
            MessageBox.Show("Order Successfully Created"); //แสดง MessageBox แจ้งผู้ใช้ว่าคำสั่งซื้อได้ถูกสร้างเรียบร้อยแล้ว
            con.Close(); //ปิดการเชื่อมต่อกับฐานข้อมูล
        }

        // เมื่อคลิกปุ่ม AddToCartbtn
        private void AddToCartbtn_Click(object sender, EventArgs e)
        {
            if (QuantityTb.Text == "") //ตรวจสอบมีจำนวนสินค้าหรือไม่ ถ้าไม่มี จะแสดง MessageBox บอกให้ใส่จำนวนสินค้า
            {
                MessageBox.Show("What is The Quantity of item?");
            }
            else if (flag == 0) //ตรวจสอบว่า flag มีค่าเป็น 0 หรือไม่ ถ้าเป็น 0 จะแสดง MessageBox บอกให้เลือกจำนวนสินค้าที่ต้องการสั่งซื้อ
            {
                MessageBox.Show("Select The Quantity To be Ordered");
                total = 0; // กำหนดให้ราคาเป็นศูนย์เมื่อยังไม่ได้เลือกจำนวนสินค้า
            }
            else
            {
                num = num + 1; //ค่าnumที่เป็นตัวแปรที่ใช้เก็บจำนวนสินค้า จะถูกเพิ่มขึ้นทีละหนึ่งหน่วย เพื่อให้ทราบถึงจำนวนสินค้าที่ถูกเพิ่มในแต่ละครั้งเมื่อสินค้าถูกเพิ่มเข้าไปในตะกร้า
                total = price * Convert.ToInt32(QuantityTb.Text); //ราคาต่อหน่วยของสินค้า*จำนวนสินค้าที่ผู้ใช้กรอกไว้ แล้วนำมาเก็บไว้ในตัวแปร total เพื่อใช้ในการแสดงผลหรือประมวลผลต่อไป
                table.Rows.Add(num, item, cat, price, total); //ข้อมูลเกี่ยวกับสินค้าที่ถูกเลือก(num, item, cat, price, total) จะถูกเพิ่มลงในตารางข้อมูล
                OrderGv.DataSource = table; //อัพเดทข้อมูลและแสดงผลข้อมูลที่มีอยู่ในตาราง
                flag = 0; //จะถูกกำหนดเป็น 0 เพื่อรองรับการเพิ่มสินค้าใหม่ลงในตะกร้าในครั้งต่อไป
            }
            sum = sum + total; //รวมราคาสินค้าทั้งหมดที่อยู่ในตะกร้า
            labelOrderAmount.Text = ""+sum; //แสดงราคาสินค้าทั้งหมด
        }
        // เมื่อเลือกหมวดหมู่ใน ComboBox categolycb  
        private void categolycb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            filterbycategory();
        }
    }
}