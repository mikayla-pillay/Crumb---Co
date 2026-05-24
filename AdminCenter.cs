using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ISTNPROJECT2
{
    public partial class AdminCenter : Form
    {
        // BindingSources
        private BindingSource ordersBinding = new BindingSource();
        private BindingSource inventoryBinding = new BindingSource();
        private BindingSource employeeBinding = new BindingSource();
        private BindingSource customerBinding = new BindingSource();

        public AdminCenter()
        {
            InitializeComponent();
        }

        private void AdminCenter_Load(object sender, EventArgs e)
        {
            LoadOrders();
            LoadInventory();
            LoadEmployees();
            LoadCustomers();
        }

        // _______ ORDER CODE ____________
        private void ApplyOrdersFilterAndSort()
        {
            string branchFilter = "";
            string statusFilter = "";
            string sortOrder = "";

            if (rbChatsOrd.Checked) branchFilter = "BranchID = 'B001'";
            else if (rbmalvernOrd.Checked) branchFilter = "BranchID = 'B002'";
            else if (rbKloofOrd.Checked) branchFilter = "BranchID = 'B003'";
            else if (rbUmhlangaOrd.Checked) branchFilter = "BranchID = 'B004'";

            if (rbCollected.Checked) statusFilter = "Status = 'Collected'";
            else if (rbReady.Checked) statusFilter = "Status = 'Ready for Collection'";
            else if (rbUnderPrep.Checked) statusFilter = "Status = 'Being Prepared'";

            if (rbOrdASC.Checked) sortOrder = "TotalCost ASC";
            else if (rbOrdDESC.Checked) sortOrder = "TotalCost DESC";

            string filter = "";
            if (!string.IsNullOrEmpty(branchFilter) && !string.IsNullOrEmpty(statusFilter))
                filter = branchFilter + " AND " + statusFilter;
            else if (!string.IsNullOrEmpty(branchFilter))
                filter = branchFilter;
            else if (!string.IsNullOrEmpty(statusFilter))
                filter = statusFilter;

            ordersBinding.Filter = filter;
            ordersBinding.Sort = sortOrder;
        }

        private void LoadOrders()
        {
            string query = "SELECT * FROM Orders";
            string connStr = ordersTableAdapter.Connection.ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                ordersBinding.DataSource = dt;
                dataViewOrders.DataSource = ordersBinding;
            }
        }

        // ORDERS event handlers
        private void rbChatsOrd_CheckedChanged(object sender, EventArgs e)
        {
            ApplyOrdersFilterAndSort();
        }

        private void rbmalvernOrd_CheckedChanged(object sender, EventArgs e)
        {
            ApplyOrdersFilterAndSort();
        }

        private void rbKloofOrd_CheckedChanged(object sender, EventArgs e)
        {
            ApplyOrdersFilterAndSort();
        }

        private void rbUmhlangaOrd_CheckedChanged(object sender, EventArgs e)
        {
            ApplyOrdersFilterAndSort();
        }

        private void rbCollected_CheckedChanged(object sender, EventArgs e)
        {
            ApplyOrdersFilterAndSort();
        }

        private void rbReady_CheckedChanged(object sender, EventArgs e)
        {
            ApplyOrdersFilterAndSort();
        }

        private void rbUnderPrep_CheckedChanged(object sender, EventArgs e)
        {
            ApplyOrdersFilterAndSort();
        }

        private void rbOrdASC_CheckedChanged(object sender, EventArgs e)
        {
            ApplyOrdersFilterAndSort();
        }

        private void rbOrdDESC_CheckedChanged(object sender, EventArgs e)
        {
            ApplyOrdersFilterAndSort();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            rbChatsOrd.Checked = false;
            rbmalvernOrd.Checked = false;
            rbKloofOrd.Checked = false;
            rbUmhlangaOrd.Checked = false;
            rbCollected.Checked = false;
            rbReady.Checked = false;
            rbUnderPrep.Checked = false;
            rbOrdASC.Checked = false;
            rbOrdDESC.Checked = false;

            ordersBinding.RemoveFilter();
            ordersBinding.Sort = "";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string orderID = Interaction.InputBox("Enter the OrderID to search for:", "Search Order");
            if (string.IsNullOrWhiteSpace(orderID))
            {
                MessageBox.Show("No OrderID entered.", "Search Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ordersBinding.Filter = "OrderID = '" + orderID + "'";
        }

        //__________INVENTORY CODE __________________
        private void ApplyInventoryFilterAndSort()
        {
            string filter = "";
            string sort = "";

            if (rbBaked.Checked) filter = "Category = 'Baked'";
            if (rbHBev.Checked) filter = "Category = 'Hot Beverage'";
            if (rbCBev.Checked) filter = "Category = 'Cold Beverage'";

            if (rbLowStock.Checked) sort = "Quantity ASC";
            else if (rbHighStock.Checked) sort = "Quantity DESC";

            inventoryBinding.Filter = filter;
            inventoryBinding.Sort = sort;
        }

        private void LoadInventory()
        {
            string query = "SELECT * FROM Inventory";
            string connStr = inventoryTableAdapter.Connection.ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                inventoryBinding.DataSource = dt;
                dataGridView2.DataSource = inventoryBinding;
            }
        }

        private void rbBaked_CheckedChanged(object sender, EventArgs e)
        {
            ApplyInventoryFilterAndSort();
        }

        private void rbHBev_CheckedChanged(object sender, EventArgs e)
        {
            ApplyInventoryFilterAndSort();
        }

        private void rbCBev_CheckedChanged(object sender, EventArgs e)
        {
            ApplyInventoryFilterAndSort();
        }

        private void rbLowStock_CheckedChanged(object sender, EventArgs e)
        {
            ApplyInventoryFilterAndSort();
        }

        private void rbHighStock_CheckedChanged(object sender, EventArgs e)
        {
            ApplyInventoryFilterAndSort();
        }

        private void btnResetInv_Click(object sender, EventArgs e)
        {
            rbBaked.Checked = false;
            rbHBev.Checked = false;
            rbCBev.Checked = false;
            rbLowStock.Checked = false;
            rbHighStock.Checked = false;

            inventoryBinding.RemoveFilter();
            inventoryBinding.Sort = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string productIDInput = Interaction.InputBox("Enter ProductID of item to restock:", "Restock Item");
            if (!int.TryParse(productIDInput, out int productID))
            {
                MessageBox.Show("ProductID must be a number.");
                return;
            }

            string qtyInput = Interaction.InputBox("Enter new quantity:", "Restock Item");
            if (!int.TryParse(qtyInput, out int newQty))
            {
                MessageBox.Show("Please enter a valid number for quantity.");
                return;
            }

            string connStr = inventoryTableAdapter.Connection.ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string checkSql = "SELECT Quantity FROM Inventory WHERE ProductID = @ProductID";
                SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                checkCmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = productID;
                object result = checkCmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                {
                    MessageBox.Show("ProductID not found in the database.");
                    return;
                }

                int currentQty = Convert.ToInt32(result);

                if (newQty <= currentQty)
                {
                    MessageBox.Show("Cannot decrease stock.");
                    return;
                }

                string updateSql = "UPDATE Inventory SET Quantity = @Qty WHERE ProductID = @ProductID";
                SqlCommand updateCmd = new SqlCommand(updateSql, conn);
                updateCmd.Parameters.Add("@Qty", SqlDbType.Int).Value = newQty;
                updateCmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = productID;
                updateCmd.ExecuteNonQuery();

                MessageBox.Show("Stock updated to " + newQty);
                LoadInventory();
            }
        }


        //_________________EMPLOYEE TAB CODE________________________
        private void ApplyEmployeeFilterAndSort()
        {
            string branchFilter = "";
            string sortOrder = "";

            if (rbChatsEmp.Checked) branchFilter = "BranchID = 'B001'";
            else if (rbMalvEmp.Checked) branchFilter = "BranchID = 'B002'";
            else if (rbKloofEmp.Checked) branchFilter = "BranchID = 'B003'";
            else if (rbUmhEmp.Checked) branchFilter = "BranchID = 'B004'";

            if (rbFNEmpASC.Checked) sortOrder = "FirstName ASC";
            else if (rbFNEmpDESC.Checked) sortOrder = "FirstName DESC";

            employeeBinding.Filter = branchFilter;
            employeeBinding.Sort = sortOrder;
        }

        private void LoadEmployees()
        {
            string query = "SELECT * FROM Employee";
            string connStr = employeeTableAdapter.Connection.ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                employeeBinding.DataSource = dt;
                dataGridView3.DataSource = employeeBinding;
            }
        }

        private void rbChatsEmp_CheckedChanged(object sender, EventArgs e)
        {
            ApplyEmployeeFilterAndSort();
        }

        private void rbMalvEmp_CheckedChanged(object sender, EventArgs e)
        {
            ApplyEmployeeFilterAndSort();
        }

        private void rbKloofEmp_CheckedChanged(object sender, EventArgs e)
        {
            ApplyEmployeeFilterAndSort();
        }

        private void rbUmhEmp_CheckedChanged(object sender, EventArgs e)
        {
            ApplyEmployeeFilterAndSort();
        }

        private void rbFNEmpASC_CheckedChanged(object sender, EventArgs e)
        {
            ApplyEmployeeFilterAndSort();
        }

        private void rbFNEmpDESC_CheckedChanged(object sender, EventArgs e)
        {
            ApplyEmployeeFilterAndSort();
        }

        private void btnResetEmp_Click(object sender, EventArgs e)
        {
            rbChatsEmp.Checked = false;
            rbMalvEmp.Checked = false;
            rbKloofEmp.Checked = false;
            rbUmhEmp.Checked = false;
            rbFNEmpASC.Checked = false;
            rbFNEmpDESC.Checked = false;

            employeeBinding.RemoveFilter();
            employeeBinding.Sort = "";
        }

        private void btnSearchEmp_Click(object sender, EventArgs e)
        {
            string empID = Interaction.InputBox("Enter the EmployeeID to search for:", "Search Employee");
            if (string.IsNullOrWhiteSpace(empID))
            {
                MessageBox.Show("No EmployeeID entered.");
                return;
            }

            employeeBinding.Filter = "EmployeeID = '" + empID + "'";
        }

        private void btnRemoveEmp_Click(object sender, EventArgs e)
        {
            string empID = Interaction.InputBox("Enter the EmployeeID of the employee to remove:", "Remove Employee");
            if (string.IsNullOrWhiteSpace(empID))
            {
                MessageBox.Show("No EmployeeID entered.");
                return;
            }

            DialogResult result = MessageBox.Show($"Are you sure you want to remove EmployeeID {empID}?", "Confirm Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string query = "DELETE FROM Employee WHERE EmployeeID = @EmployeeID";
                string connStr = employeeTableAdapter.Connection.ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@EmployeeID", empID);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                        MessageBox.Show($"EmployeeID '{empID}' not found.");
                    else
                        MessageBox.Show("Employee removed successfully.");
                }
                LoadEmployees();
            }
        }

        private void btnAddEmp_Click(object sender, EventArgs e)
        {
            string connStr = employeeTableAdapter.Connection.ConnectionString;
            string empID = "";
            string password;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                //generate EmployeeID
                string getMaxEmpQuery = "SELECT MAX(EmployeeID) FROM Employee";
                using (SqlCommand cmd = new SqlCommand(getMaxEmpQuery, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result == DBNull.Value || result == null)
                    {
                        empID = "Emp1";
                    }
                    else
                    {
                        string maxEmp = result.ToString();
                        int numPart = int.Parse(maxEmp.Substring(3));
                        empID = "Emp" + (numPart + 1);
                    }
                }

                //Vallidation
                string branchID = Interaction.InputBox($"Your EmployeeID is: {empID}\nEnter BranchID (B001–B004):", "Add Employee");
                if (string.IsNullOrWhiteSpace(branchID))
                {
                    MessageBox.Show("BranchID is required.");
                    return;
                }

                string[] validBranches = { "B001", "B002", "B003", "B004" };
                if (!validBranches.Contains(branchID))
                {
                    MessageBox.Show($"Invalid BranchID. Must be one of: {string.Join(", ", validBranches)}");
                    return;
                }

                string firstName = Interaction.InputBox($"Your EmployeeID is: {empID}\nEnter First Name:", "Add Employee");
                if (string.IsNullOrWhiteSpace(firstName) || Regex.IsMatch(firstName, @"\d"))
                {
                    MessageBox.Show("First Name is required.");
                    return;
                }

                string lastName = Interaction.InputBox($"Your EmployeeID is: {empID}\nEnter Last Name:", "Add Employee");
                if (string.IsNullOrWhiteSpace(lastName) || Regex.IsMatch(lastName, @"\d"))
                {
                    MessageBox.Show("Last Name is required.");
                    return;
                }

                // --- Phone Number ---
                string phone = Interaction.InputBox($"Your EmployeeID is: {empID}\nEnter Phone Number (10 digits):", "Add Employee");
                if (!System.Text.RegularExpressions.Regex.IsMatch(phone ?? "", @"^\d{10}$"))
                {
                    MessageBox.Show("Phone Number must be exactly 10 digits.");
                    return;
                }

                string email = Interaction.InputBox($"Your EmployeeID is: {empID}\nEnter Email:", "Add Employee");
                if (!string.IsNullOrWhiteSpace(email) && !email.Contains("@"))
                {
                    MessageBox.Show("Email must contain '@'.");
                    return;
                }

                string address = Interaction.InputBox($"Your EmployeeID is: {empID}\nEnter Home Address:", "Add Employee");
                if (string.IsNullOrWhiteSpace(address))
                {
                    MessageBox.Show("Home Address is required.");
                    return;
                }
                if (address.Length < 3)
                {
                    MessageBox.Show("Address seems too short.");
                    return;
                }

                string emergencyContactNumber = Interaction.InputBox($"Your EmployeeID is: {empID}\nEnter Emergency Contact Number (10 digits):", "Add Employee");
                if (!System.Text.RegularExpressions.Regex.IsMatch(emergencyContactNumber ?? "", @"^\d{10}$"))
                {
                    MessageBox.Show("Emergency Contact Number must be exactly 10 digits.");
                    return;
                }

                //generate password
                string firstTwo = firstName.Length >= 2 ? firstName.Substring(0, 2) : firstName;
                string lastThree = lastName.Length >= 3 ? lastName.Substring(0, 3) : lastName;
                string specials = "!@#$%&*?";
                Random rnd = new Random();
                char specialChar = specials[rnd.Next(specials.Length)];

                string basePassword = firstTwo + lastThree + specialChar;

                //shuffle passowrd
                char[] chars = basePassword.ToCharArray();
                for (int i = chars.Length - 1; i > 0; i--)
                {
                    int j = rnd.Next(i + 1);
                    char temp = chars[i];
                    chars[i] = chars[j];
                    chars[j] = temp;
                }
                password = new string(chars);


                //insert into DB
                string insertQuery = @"INSERT INTO Employee
                               (EmployeeID, BranchID, FirstName, LastName, PhoneNumber, Email, HomeAddress, EmergencyContactNumber, Password)
                               VALUES (@EmployeeID, @BranchID, @FirstName, @LastName, @PhoneNumber, @Email, @HomeAddress, @EmergencyContactNumber, @Password)";

                using (SqlCommand cmdInsert = new SqlCommand(insertQuery, conn))
                {
                    cmdInsert.Parameters.AddWithValue("@EmployeeID", empID);
                    cmdInsert.Parameters.AddWithValue("@BranchID", branchID);
                    cmdInsert.Parameters.AddWithValue("@FirstName", firstName);
                    cmdInsert.Parameters.AddWithValue("@LastName", lastName);
                    cmdInsert.Parameters.AddWithValue("@PhoneNumber", phone);
                    cmdInsert.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(email) ? DBNull.Value : (object)email);
                    cmdInsert.Parameters.AddWithValue("@HomeAddress", address);
                    cmdInsert.Parameters.AddWithValue("@EmergencyContactNumber", emergencyContactNumber);
                    cmdInsert.Parameters.AddWithValue("@Password", password);


                    cmdInsert.ExecuteNonQuery();
                }
            }

            MessageBox.Show($"Employee added successfully. EmployeeID: {empID}. Please inform them their password is: {password}");
            LoadEmployees();
        }


        //_______________CUSTOMER TAB CODE___________________________
        private void ApplyCustomerFilterAndSort()
        {
            string filter = "";
            string sort = "";

            if (rbLMYes.Checked) filter = "LoyaltyMember = 1";
            else if (rbLMNo.Checked) filter = "LoyaltyMember = 0";

            if (rbFNcustASC.Checked) sort = "FirstName ASC";
            else if (rbFNcustDESC.Checked) sort = "FirstName DESC";

            customerBinding.Filter = filter;
            customerBinding.Sort = sort;
        }

        private void LoadCustomers()
        {
            string query = "SELECT CustomerID, FirstName, LastName, Email, PhoneNumber, Password, NoOfOrdersPlaced, LoyaltyMember FROM Customer";
            string connStr = customerTableAdapter.Connection.ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                customerBinding.DataSource = dt;
                dataViewGrid4.DataSource = customerBinding;

                dataViewGrid4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataViewGrid4.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dataViewGrid4.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataViewGrid4.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataViewGrid4.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            }
        }

        private void rbLMYes_CheckedChanged(object sender, EventArgs e)
        {
            ApplyCustomerFilterAndSort();
        }

        private void rbLMNo_CheckedChanged(object sender, EventArgs e)
        {
            ApplyCustomerFilterAndSort();
        }

        private void rbFNcustASC_CheckedChanged(object sender, EventArgs e)
        {
            ApplyCustomerFilterAndSort();
        }

        private void rbFNcustDESC_CheckedChanged(object sender, EventArgs e)
        {
            ApplyCustomerFilterAndSort();
        }

        private void btnSearchCust_Click(object sender, EventArgs e)
        {
            string custID = Interaction.InputBox("Enter CustomerID to search:", "Search Customer");
            if (string.IsNullOrWhiteSpace(custID))
            {
                MessageBox.Show("No CustomerID entered.");
                return;
            }

            customerBinding.Filter = "CustomerID = '" + custID + "'";
        }

        private void btnResetCust_Click(object sender, EventArgs e)
        {
            rbLMYes.Checked = false;
            rbLMNo.Checked = false;
            rbFNcustASC.Checked = false;
            rbFNcustDESC.Checked = false;

            customerBinding.RemoveFilter();
            customerBinding.Sort = "";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

