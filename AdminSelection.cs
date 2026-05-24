using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Customer_Login;
using ISTNPROJECT2;

//Employee Password: Ma#3Gar
//Admin Password: Dj7#6

namespace CrumbAndCo
{
    public partial class Selection : Form
    {
        public Selection()
        {
            InitializeComponent();
        }

        private void AdminSelection_Load(object sender, EventArgs e)
        {

        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            bool correct = false;

            while (!correct)
            {
                // 1. Ask for password
                string enteredPassword = Microsoft.VisualBasic.Interaction.InputBox("Enter Admin Password:", "Admin Login");

                if (string.IsNullOrWhiteSpace(enteredPassword))
                {
                    // User pressed Cancel or left empty
                    MessageBox.Show("No password entered. Login cancelled.");
                    return;
                }

                // 2. Query the Admin table for this password
                DataTable dt = adminTableAdapter1.GetData(); // load Admin table

                DataRow[] foundRows = dt.Select("Password = '" + enteredPassword + "'");

                if (foundRows.Length > 0)
                {
                    // 3. Match found 
                    string firstName = foundRows[0]["FirstName"].ToString().Trim();
                    string lastName = foundRows[0]["LastName"].ToString().Trim();

                    MessageBox.Show("Welcome " + firstName + " " + lastName + "!", "Success");

                    // 4. Open Form1 and hide current form
                    //Form1 f1 = new Form1();
                    //f1.Show();
                    AdminCenter adminCenter = new AdminCenter();
                    adminCenter.Show();

                    correct = true; // exit loop
                }
                else
                {
                    // 5. No match
                    DialogResult dr = MessageBox.Show(
                        "Password incorrect. Do you want to try again?",
                        "Error",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Error
                    );

                    if (dr == DialogResult.No)
                    {
                        return; // stop login
                    }
                }
            }
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            bool loginSuccess = false;


            while (!loginSuccess)
            {
                // Ask employee for password
                string enteredPassword = Interaction.InputBox("Enter Employee Password:", "Employee Login");

                if (string.IsNullOrWhiteSpace(enteredPassword))
                {
                    MessageBox.Show("Login cancelled.");
                    return;
                }

                //Load the Employee table from the dataset
                DataTable dt = employeeTableAdapter1.GetData();

                //Search for a row where password matches
                DataRow[] foundRows = dt.Select("Password = '" + enteredPassword + "'");

                if (foundRows.Length > 0)
                {
                    //When match found then get FirstName and LastName
                    string firstName = foundRows[0]["FirstName"].ToString();
                    string lastName = foundRows[0]["LastName"].ToString();

                    MessageBox.Show("Welcome " + firstName + " " + lastName + "!", "Success");

                    //Open ViewOrder form and hide this one
                    ViewOrder vo = new ViewOrder();
                    vo.Show();
                    this.Hide();

                    loginSuccess = true; // stop loop
                }
                else
                {
                    //ask to retry
                    DialogResult dr = MessageBox.Show(
                        "Password incorrect. Do you want to try again?",
                        "Error",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Error
                    );

                    if (dr == DialogResult.No)
                    {
                        return; // stop login
                    }
                }
            }
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            LoginForm customer_Login=new LoginForm();
            customer_Login.Show();
           
           
        }

        private void btnEmployee_MouseLeave(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void hhToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
