using System;
using System.Drawing;
using System.Windows.Forms;
using SharedData;
using CrumbleAndCoForms;

//Username to log in: Cust06
//Password to log in: Tdm5&9

namespace Customer_Login
{
    public partial class LoginForm : Form
    {
        int attemptCount = 0;
        int lockoutTime = 30;
        Timer lockoutTimer = new Timer();

        public LoginForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            lockoutTimer.Interval = 1000; // 1 second
            lockoutTimer.Tick += LockoutTimer_Tick;
        }

        public static bool LoginSuccess = false;



        private void button1_Click(object sender, EventArgs e) {

            if (string.IsNullOrWhiteSpace(CustIDBox.Text))
            {
                MessageBox.Show("Please enter your Customer ID");
                return;
            }

            if (string.IsNullOrWhiteSpace(PWBox.Text))
            {
                MessageBox.Show("Please enter your Password"); 
                return;
            }

            customerTableAdapter1.CheckLogin(customerDS1.Customer, CustIDBox.Text, PWBox.Text);



            if (customerDS1.Customer.Rows.Count > 0)
            {
                GlobalVariables.CustomerID = CustIDBox.Text;
                LoginSuccess = true;

                var row = customerDS1.Customer[0];
                string firstName = row.FirstName;

                MessageBox.Show($"Welcome back, {firstName}!", "Login Successful!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //this.Close();
                CustomerOptions customerOptions = new CustomerOptions();
                customerOptions.Show();
                

            }
            else
            {
                attemptCount++;
                if (attemptCount >= 3)
                {

                    CustIDBox.Enabled = false;
                    PWBox.Enabled = false;
                    button1.Enabled = false;

                    lockoutTime = 30;
                    label6.Text = $"Retry in {lockoutTime} seconds";
                    label6.ForeColor = Color.Red;
                    label6.Font = new Font(label6.Font.FontFamily, 14, FontStyle.Bold);
                    label6.TextAlign = ContentAlignment.MiddleCenter;
                    lockoutTimer.Start();
                }
                else
                {
                    MessageBox.Show("Login Failed");
                }
            }
        }
        private void LockoutTimer_Tick(object sender, EventArgs e)
        {
            lockoutTime--;
            string newText = $"Retry in {lockoutTime} seconds";

            if (label6.Text != newText) // only update if different
            {
                label6.Text = newText;
            }

            if (lockoutTime <= 0)
            {
                lockoutTimer.Stop();
                attemptCount = 0;

                CustIDBox.Enabled = true;
                PWBox.Enabled = true;
                button1.Enabled = true;
                label6.Text = "";
            }
        }



        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }


        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SignUp.SignUpForm signUpForm = new SignUp.SignUpForm();


            signUpForm.Show();


        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            PWBox.PasswordChar = '\0';
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            PWBox.PasswordChar = '*';
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void EmailBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)

        {
            this.Hide();

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
