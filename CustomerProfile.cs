using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharedData;
using SignUp;


namespace CrumbleAndCoForms
{
    public partial class CustomerProfile : Form
    {

        private string connectionString = "Data Source=143.128.146.30\\istn2;Initial Catalog=ist2kr;User ID=ist2kr;Password=pihqje";
        private string currentCustomerID = "";
        public CustomerProfile()
        {
            InitializeComponent();
            currentCustomerID=GlobalVariables.CustomerID;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void CustomerProfile_Load(object sender, EventArgs e)
        {
           // using (SqlConnection con = new SqlConnection(connectionString))
          //  {
             //   con.Open();

               // while (string.IsNullOrEmpty(currentCustomerID))
              //  {
                   // string inputId = Microsoft.VisualBasic.Interaction.InputBox(
                      //  "Enter your Customer ID:", "Customer Login", "");

                  //  if (string.IsNullOrEmpty(inputId))
                    {
                 //       MessageBox.Show("Customer ID is required to use the system.");
                       // continue;
                   // }

                    // Check if this ID exists in the Customer table
                  //  string query = "SELECT COUNT(*) FROM Customer WHERE CustomerID = @CustomerID";
                 //   using (SqlCommand cmd = new SqlCommand(query, con))
                //    {
                       // cmd.Parameters.AddWithValue("@CustomerID", inputId);
                    //    int count = (int)cmd.ExecuteScalar();

                       // if (count > 0)
                       // {
                       //     currentCustomerID = inputId; // valid ID → store it
                      //  }
                        //else
                        //{
                            //MessageBox.Show("Customer ID not found. Please enter a valid ID.");
                       // }
                    }
                }
            
        










        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }



        private void button1_Click_1(object sender, EventArgs e)
        {
            CustomerOptions optionForm = new CustomerOptions();
            optionForm.Show();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            pnlEdit.Visible = false;
            pnlPassword.Visible = false;
           
            pnlDetails.Visible = true;

            LoadCustomerDetails();


        }







        public void LoadCustomerDetails()
        {

            if (string.IsNullOrEmpty(currentCustomerID))
            {
                MessageBox.Show("No Customer ID found. Please log in first.");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    string query = @"SELECT CustomerID, FirstName, LastName, Email, PhoneNumber,
                                    NoOfOrdersPlaced,
                                    CASE WHEN LoyaltyMember = 1 THEN 'Yes' ELSE 'No' END AS LoyaltyMember
                             FROM Customer
                             WHERE CustomerID = @CustomerID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", currentCustomerID);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            richTextBox1.Clear();
                            richTextBox1.AppendText(
                                $"ID: {reader["CustomerID"]}\n" +
                                $"First Name: {reader["FirstName"]}\n" +
                                $"Last Name: {reader["LastName"]}\n" +
                                $"Email: {reader["Email"]}\n" +
                                $"Phone: {reader["PhoneNumber"]}\n" +
                                $"Orders Placed: {reader["NoOfOrdersPlaced"]}\n" +
                                $"Loyalty Member: {reader["LoyaltyMember"]}\n"
                            );
                        }
                        else
                        {
                            MessageBox.Show("Customer not found. Please check your ID.");
                            currentCustomerID = "";
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading customer details: " + ex.Message);
                }
            }
        }





        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }






        private void pnlDetails_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnUpdatePassword_Click(object sender, EventArgs e)
        {
            {

                pnlDetails.Visible = false;       // hide other panels
                pnlPassword.Visible = true;       // show password panel
                pnlPassword.BringToFront();       // bring it to front
                txtOldPassword.Clear();           // clear textboxes
                txtNewPassword.Clear();
                txtOldPassword.Focus();           // focus old password box
            }





        }




        public bool IsValidPassword(string password)
        {
            if (password.Length < 7)
                return false;

            bool hasUpper = false;
            bool hasNumber = false;
            bool hasSpecial = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpper = true;
                else if (char.IsDigit(c)) hasNumber = true;
                else if (!char.IsLetterOrDigit(c)) hasSpecial = true;
            }

            return hasUpper && hasNumber && hasSpecial;
        }
        public string GetPasswordValidationMessage(string password)
        {
            List<string> errors = new List<string>();

            if (password.Length < 7) errors.Add("- Minimum 7 characters");
            if (!password.Any(char.IsUpper)) errors.Add("- At least 1 uppercase letter");
            if (!password.Any(char.IsDigit)) errors.Add("- At least 1 number");
            if (!password.Any(c => !char.IsLetterOrDigit(c))) errors.Add("- At least 1 special character");

            return errors.Count == 0 ? "" : "Password must meet the following criteria:\n" + string.Join("\n", errors);
        }

        public void UpdatePasswordWithOld(string customerId, string newPassword, string newPassword1)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    // Only update if old password matches
                    string query = @"UPDATE Customer
                             SET Password = @NewPassword
                             WHERE CustomerID = @CustomerID AND Password = @OldPassword";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@NewPassword", newPassword);
                        cmd.Parameters.AddWithValue("@CustomerID", customerId);
                        cmd.Parameters.AddWithValue("@OldPassword", txtOldPassword);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Password updated successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Old password is incorrect or customer not found.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating password: " + ex.Message);
                }
            }
        }


        private void btnNewPassword_Click(object sender, EventArgs e)
        {
            pnlDetails.Visible = false;
            pnlPassword.Visible = true;
            pnlPassword.BringToFront();

            string oldPassword = txtOldPassword.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();

            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Please fill in both Old and New passwords.");
                return;
            }

            // Check if new password is the same as old password
            if (oldPassword == newPassword)
            {
                MessageBox.Show("New password cannot be the same as the old password. Please choose a different one.");
                return;
            }

            // Validate new password strength
            if (!IsValidPassword(newPassword))
            {
                string msg = GetPasswordValidationMessage(newPassword);
                MessageBox.Show("New password is not strong enough:\n" + msg);
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    // Check if old password matches the database
                    string checkQuery = "SELECT COUNT(*) FROM Customer WHERE CustomerID = @CustomerID AND Password = @OldPassword";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("@CustomerID", currentCustomerID);
                        checkCmd.Parameters.AddWithValue("@OldPassword", oldPassword);

                        int count = (int)checkCmd.ExecuteScalar();
                        if (count == 0)
                        {
                            MessageBox.Show("Old password is incorrect. Please try again.");
                            return;
                        }
                    }

                    // Update password in the database
                    string updateQuery = "UPDATE Customer SET Password = @NewPassword WHERE CustomerID = @CustomerID";
                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, con))
                    {
                        updateCmd.Parameters.AddWithValue("@NewPassword", newPassword);
                        updateCmd.Parameters.AddWithValue("@CustomerID", currentCustomerID);

                        int rowsAffected = updateCmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Password updated successfully!");
                            pnlPassword.Visible = false;
                            txtOldPassword.Clear();
                            txtNewPassword.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Error updating password. Please try again.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating password: " + ex.Message);
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            {pnlDetails.Visible = false;
                pnlPassword.Visible=false;
                
                string password = Microsoft.VisualBasic.Interaction.InputBox(
                    "Enter your Password:", "Delete Profile", "");

                if (string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Password is required.");
                    return;
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    try
                    {
                        con.Open();

                        // Check if password matches
                        string checkPasswordQuery = "SELECT COUNT(*) FROM Customer WHERE CustomerID = @CustomerID AND Password = @Password";
                        using (SqlCommand cmdCheck = new SqlCommand(checkPasswordQuery, con))
                        {
                            cmdCheck.Parameters.AddWithValue("@CustomerID", currentCustomerID);
                            cmdCheck.Parameters.AddWithValue("@Password", password);

                            int valid = (int)cmdCheck.ExecuteScalar();
                            if (valid == 0)
                            {
                                MessageBox.Show("Password is incorrect. Cannot delete profile.");
                                return;
                            }
                        }

                        // Check for outstanding orders
                        string statusCheckQuery = @"SELECT OrderID, Status 
                                        FROM [Orders] 
                                        WHERE CustomerID = @CustomerID 
                                          AND Status IN ('Being Prepared', 'Ready for Collection')";
                        using (SqlCommand cmdStatus = new SqlCommand(statusCheckQuery, con))
                        {
                            cmdStatus.Parameters.AddWithValue("@CustomerID", currentCustomerID);
                            using (SqlDataReader reader = cmdStatus.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    string msg = "You cannot delete your profile because you have outstanding orders:\n\n";
                                    while (reader.Read())
                                    {
                                        msg += $"OrderID: {reader["OrderID"]}, Status: {reader["Status"]}\n";
                                    }
                                    msg += "\nPlease collect or wait for all orders to be completed before deleting your account.";
                                    MessageBox.Show(msg);
                                    return;
                                }
                            }
                        }

                        // Confirm deletion
                        var confirm = MessageBox.Show(
      "We are sad to see you leave our family.\n\nAre you sure you want to permanently delete your profile? This action cannot be undone.",
      "Confirm Delete",
      MessageBoxButtons.YesNo,
      MessageBoxIcon.Warning);

                        if (confirm == DialogResult.Yes)
                        {
                            
                        }
                        else
                        {
                            MessageBox.Show(
                                "We’re happy you decided to stay in the family! 💙",
                                "Thank You",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                        
                        using (SqlTransaction transaction = con.BeginTransaction())
                        {
                            try
                            {
                                // Delete from OrderItem first
                                string deleteOrderItemsQuery = @"DELETE oi
                                                     FROM OrderItem oi
                                                     INNER JOIN [Orders] o ON oi.OrderID = o.OrderID
                                                     WHERE o.CustomerID = @CustomerID";
                                using (SqlCommand cmdDeleteItems = new SqlCommand(deleteOrderItemsQuery, con, transaction))
                                {
                                    cmdDeleteItems.Parameters.AddWithValue("@CustomerID", currentCustomerID);
                                    cmdDeleteItems.ExecuteNonQuery();
                                }

                                // Delete from Order table
                                string deleteOrdersQuery = @"DELETE FROM [Orders] WHERE CustomerID = @CustomerID";
                                using (SqlCommand cmdDeleteOrders = new SqlCommand(deleteOrdersQuery, con, transaction))
                                {
                                    cmdDeleteOrders.Parameters.AddWithValue("@CustomerID", currentCustomerID);
                                    cmdDeleteOrders.ExecuteNonQuery();
                                }

                                // Delete customer
                                string deleteCustomerQuery = @"DELETE FROM Customer WHERE CustomerID = @CustomerID";
                                using (SqlCommand cmdDeleteCustomer = new SqlCommand(deleteCustomerQuery, con, transaction))
                                {
                                    cmdDeleteCustomer.Parameters.AddWithValue("@CustomerID", currentCustomerID);
                                    cmdDeleteCustomer.ExecuteNonQuery();
                                }

                                transaction.Commit();

                                MessageBox.Show("Profile and all related orders deleted successfully!");
                                richTextBox1.Clear();
                                pnlDetails.Visible = false;
                            }
                            catch
                            {
                                transaction.Rollback();
                                MessageBox.Show("An error occurred while deleting your profile. No changes were made.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting profile: " + ex.Message);
                    }
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            pnlPassword.Visible = false;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pnlPassword_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    string query = @"SELECT OrderID, CustomerID, BranchID, OrderDate, 
                                    NoOfItems, Status, TotalCost
                             FROM dbo.[Orders]
                             WHERE CustomerID = @CustomerID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = currentCustomerID;

                        SqlDataReader reader = cmd.ExecuteReader();

                        // Clear old text
                        richTextBox1.Clear();

                        if (reader.HasRows)
                        {
                            
                            richTextBox1.AppendText(
                                $"{"OrderID",-10} {"BranchID",-10} {"Date",-15} {"Items",-8} {"Status",-12} {"TotalCost",-10}\n");
                            richTextBox1.AppendText(new string('-', 80) + "\n");

                            while (reader.Read())
                            {
                                richTextBox1.AppendText(
                                    $"{reader["OrderID"],-10} " +
                                    $"{reader["BranchID"],-10} " +
                                    $"{Convert.ToDateTime(reader["OrderDate"]).ToShortDateString(),-15} " +
                                    $"{reader["NoOfItems"],-8} " +
                                    $"{reader["Status"],-12} " +
                                    $"{reader["TotalCost"],-10}\n"
                                );
                            }

                            
                            pnlDetails.Visible = true;
                            pnlDetails.BringToFront();
                        }
                        else
                        {
                            MessageBox.Show("No orders found for this customer.");
                            pnlDetails.Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading orders: " + ex.Message);
                    pnlDetails.Visible = false;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
          
        
            DialogResult result = MessageBox.Show(
                "Are you sure you want to sign out?\n\nSigning out will close the apllication and you will be required to re-enter your details.",
                "Confirm Sign Out",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                // Hide current form
                Application.Exit();

                
            }
            else
            {
                
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
         
        
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string phoneNumber = txtPhoneNumber.Text.Trim();
            string email = txtEmail.Text.Trim();

            // Validation
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                MessageBox.Show("First Name and Last Name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (phoneNumber.Length != 10 || !phoneNumber.All(char.IsDigit))
            {
                MessageBox.Show("Please enter a valid phone number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(email) || !email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

           
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Customer 
                         SET FirstName = @FirstName, 
                             LastName = @LastName, 
                             PhoneNumber = @PhoneNumber, 
                             Email = @Email
                         WHERE CustomerID = @CustomerID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@CustomerID", currentCustomerID);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show("Your profile has been updated successfully.", "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pnlEdit.Visible = false; // hide panel again
                    }
                    else
                    {
                        MessageBox.Show("Update failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }




        private void LoadCustomerDetails(string customerId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT FirstName, LastName, PhoneNumber, Email FROM Customer WHERE CustomerID = @CustomerID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        txtFirstName.Text = reader["FirstName"].ToString();
                        txtLastName.Text = reader["LastName"].ToString();
                        txtPhoneNumber.Text = reader["PhoneNumber"].ToString();
                        txtEmail.Text = reader["Email"].ToString();
                    }
                }
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click_1(object sender, EventArgs e)
        
           
        {
            pnlDetails.Visible = false;
            pnlPassword.Visible = false;

            var confirm = MessageBox.Show(
       "Are you sure you want to update your details?\n\nThese changes will be permanent and cannot be reverted.",
       "Confirm Update",
       MessageBoxButtons.YesNo,
       MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
            {
                
                MessageBox.Show(
                    "No changes were made. Your profile remains the same.",
                    "Update Cancelled",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            pnlEdit.Visible = true;   // show edit panel
            pnlEdit.BringToFront();   // bring it in front

            LoadCustomerDetails(currentCustomerID); 
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }
    }
}

















