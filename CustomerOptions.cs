using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ISTNPROJECT;
using Branch1;


namespace CrumbleAndCoForms
{
    public partial class CustomerOptions : Form

    {
        public CustomerOptions()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            CustomerProfile profileForm = new CustomerProfile();
            profileForm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();

        }

        private void button7_Click(object sender, EventArgs e)
        {


        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblwhichmenu.Visible = true;
            btnBakedG.Visible = true;
            btnBev.Visible = true;
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            PlaceOrderForm orderForm  = new PlaceOrderForm();
            orderForm.Show();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void CustomerOptions_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnBakedG_Click(object sender, EventArgs e)
        {
            BakedgoodsMenu menu = new BakedgoodsMenu();
            menu.Show();
        }

        private void btnBev_Click(object sender, EventArgs e)
        {
            BeverageMenu bvgmenu = new BeverageMenu(); 
            bvgmenu.Show();
        }
    }
}
