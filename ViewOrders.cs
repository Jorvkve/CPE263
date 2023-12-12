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
    public partial class ViewOrders : Form
    {
        public ViewOrders()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\CPE263\project 263\project 263\obj\Debug\SQL\Data Server Cafe.mdf"";Integrated Security=True;Connect Timeout=30"); // สร้างobject con และทำให้เป็นตัวเชื่อมต่อ
        private void Closebtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        void populate()
        {
            try
            {
                con.Open(); //เปิดการเชื่อมต่อกับฐานข้อมูล SQL Server
                string query = "select * from Ordertbls"; //สร้างคำสั่ง SQL select * from Ordertbls เพื่อดึงข้อมูลทั้งหมดจากตาราง Itemtbls
                SqlDataAdapter sda = new SqlDataAdapter(query, con); //ดึงข้อมูลจากฐานข้อมูลโดยใช้คำสั่ง SQL และจัดการกับข้อมูลใน DataSet
                SqlCommandBuilder builder = new SqlCommandBuilder(sda); //สร้างคำสั่ง SQL สำหรับการ Update, Insert, Delete โดยอัตโนมัติจาก DataAdapter
                var ds = new DataSet(); //สร้างDataSet ที่สามารถเก็บข้อมูลจากหลายตาราง
                sda.Fill(ds, "UsersTbl"); //ดึงข้อมูลจากฐานข้อมูลโดยใช้ SqlDataAdapter และเก็บข้อมูลที่ได้ลงใน DataSet(ds)
                OrdersGV.DataSource = ds.Tables["UsersTbl"];
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
        private void ViewOrders_Load(object sender, EventArgs e)
        {
            populate(); // เรียกใช้ฟังก์ชัน populate() เพื่อโหลดข้อมูลในการดูรายการสั่งซื้อ
        }
        private void OrdersGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // หากผู้ใช้กด OK ใน dialog แสดงตัวอย่างก่อนพิมพ์
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                // ทำการพิมพ์เมื่อผู้ใช้กด OK
                printDocument1.Print();
            }
        }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // กำหนดข้อความส่วนหัว
            e.Graphics.DrawString("=====MyCafe SoftWare=====", new Font("Britannic", 20, FontStyle.Bold), Brushes.Red, new Point(200, 40));
            e.Graphics.DrawString("=====Order Summary=====", new Font("Britannic", 20, FontStyle.Bold), Brushes.Red, new Point(208, 70));
            // ดึงข้อมูลการสั่งซื้อที่ถูกเลือกในตาราง OrdersGV และแสดงในเอกสารที่กำลังพิมพ์
            e.Graphics.DrawString("Number :" + OrdersGV.SelectedRows[0].Cells[0].Value.ToString(), new Font("Britannic", 14, FontStyle.Regular), Brushes.Black, new Point(120, 135));
            e.Graphics.DrawString("Date :" + OrdersGV.SelectedRows[0].Cells[1].Value.ToString(), new Font("Britannic", 14, FontStyle.Regular), Brushes.Black, new Point(120, 170));
            e.Graphics.DrawString("Seller:" + OrdersGV.SelectedRows[0].Cells[2].Value.ToString(), new Font("Britannic", 14, FontStyle.Regular), Brushes.Black, new Point(120, 205));
            e.Graphics.DrawString("Amount :" + OrdersGV.SelectedRows[0].Cells[3].Value.ToString(), new Font("Britannic", 14, FontStyle.Regular), Brushes.Black, new Point(120, 240));
            // ข้อความส่วนท้ายของเอกสาร
            e.Graphics.DrawString("=====Tewit Chankong 6504384=====", new Font("Britannic", 20, FontStyle.Bold), Brushes.Red, new Point(208, 340));
        }
        private void Exit_Click(object sender, EventArgs e) //กด X เพื่อออกจากโปรแกรม
        {
            Application.Exit();
        }
    }
}