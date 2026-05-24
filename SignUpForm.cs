using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SignUp
{
    public partial class SignUpForm : Form
    {
        public SignUpForm()
        {
            InitializeComponent();
        }

        private string GeneratePassword(string firstName, string lastName)
        {
            Random rnd = new Random();

            string part1 = lastName.Length >= 3 ? lastName.Substring(0, 3) : lastName.PadRight(3, 'X');
            part1 = char.ToUpper(part1[0]) + part1.Substring(1).ToLower();

            string part2 = firstName.Length >= 2 ? firstName.Substring(0, 2) : firstName.PadRight(2, 'X');

            string digit = rnd.Next(0, 10).ToString();

            string specialChars = "!@#$%^&*";
            string special = specialChars[rnd.Next(specialChars.Length)].ToString();

            List<string> parts = new List<string> { part1, part2, digit, special };

            for (int i = parts.Count - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                string temp = parts[i];
                parts[i] = parts[j];
                parts[j] = temp;
            }

            return string.Join("", parts);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fName = FNameBox.Text.Trim();
            string lName = SNameBox.Text.Trim();
            string email = EAddressBox.Text.Trim();
            string phone = PNoBox.Text.Trim();
            bool loyalty = LoyaltyCBox.Checked;
            DateTime dob = DateOfBirthBox.Value;

            if (string.IsNullOrWhiteSpace(fName) || string.IsNullOrWhiteSpace(lName) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!email.Contains("@"))
            {
                MessageBox.Show("Invalid email address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (phone.Length != 10 || !phone.All(char.IsDigit))
            {
                MessageBox.Show("Phone number must be exactly 10 digits.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!phone.StartsWith("0"))
            {
                MessageBox.Show("Phone number must start with 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int age = DateTime.Now.Year - dob.Year;
            if (dob > DateTime.Now.AddYears(-age)) age--;

            if (age < 16)
            {
                MessageBox.Show("You must be at least 16 years old to sign up.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {

                var allCustomers = customerTableAdapter1.GetData();

                bool emailExists = allCustomers.Any(c => c.Email == email);
                bool phoneExists = allCustomers.Any(c => c.PhoneNumber == phone);

                if (emailExists && phoneExists)
                {
                    MessageBox.Show("Both this email and phone number are already registered.","Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (emailExists)
                {
                    MessageBox.Show("This email is already registered.","Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (phoneExists)
                {
                    MessageBox.Show("This phone number is already registered.","Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int maxId = 0;
                foreach (var row in allCustomers)
                {
                    string username = row.CustomerID; 
                    if (username.StartsWith("Cust"))
                    {
                        if (int.TryParse(username.Substring(4), out int number))
                        {
                            if (number > maxId)
                                maxId = number;
                        }
                    }
                }
                string userName = $"Cust{(maxId + 1):D2}";

                string password;
                Random rnd = new Random();
                do
                {
                    password = GeneratePassword(fName, lName);

                    while (allCustomers.Any(c => c.Password == password))
                    {
                        string extraChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
                        password += extraChars[rnd.Next(extraChars.Length)];
                    }

                } while (allCustomers.Any(c => c.Password == password));

                customerTableAdapter1.InsertQuery(userName, fName, lName, email, phone, password, 0, loyalty);

                MessageBox.Show($"Your account has been created!\n\nUsername: {userName}\nPassword: {password}\n\nIf you wish to change your password, head to 'View Profile'.","Sign Up Successful!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                FNameBox.Clear();
                SNameBox.Clear();
                EAddressBox.Clear();
                PNoBox.Clear();
                LoyaltyCBox.Checked = false;
                DateOfBirthBox.Value = DateTime.Today;

                MessageBox.Show("Proceed to login");
                
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating account: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FNameBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void SNameBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void EAddressBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void PNOBOX_TextChanged(object sender, EventArgs e)
        {

        }

        private void DateOfBirthBox_ValueChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();

            conditions conditions = new conditions();
            conditions.Show();
        }
    }
}
