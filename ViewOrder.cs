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

namespace CrumbAndCo
{
    public partial class ViewOrder : Form
    {
        public ViewOrder()
        {
            InitializeComponent();
        }

        private void ViewOrder_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'ist2krDataSet.Order' table. You can move, or remove it, as needed.
            this.orderTableAdapter.Fill(this.ist2krDataSet.Orders);

        }

        private void orderBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.orderBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.ist2krDataSet);

        }

        private void btnBackLogin_Click(object sender, EventArgs e)
        {
            Selection selectionForm = new Selection();
            this.Hide();
            selectionForm.Show();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string enteredOrderID = tBoxOrderID.Text.Trim();

            if (string.IsNullOrEmpty(enteredOrderID))
            {
                MessageBox.Show("Please enter an Order ID.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Connection string
            string connectionString = @"Server=143.128.146.30\istn2;Database=ist2kr;User Id=ist2kr;Password=pihqje;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // SQL query to find the order
                    string query = "SELECT * FROM Orders WHERE OrderID = @orderID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderID", enteredOrderID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Order found 
                                rDisplay.Text =
                                    $"Order ID: {reader["OrderID"]}\n" +
                                    $"Customer ID: {reader["CustomerID"]}\n" +
                                    $"Branch ID: {reader["BranchID"]}\n" +
                                    $"Order Date: {reader["OrderDate"]}\n" +
                                    $"Number of Items: {reader["NoOfItems"]}\n" +
                                    $"Status: {reader["Status"]}\n" +
                                    $"Total Cost: {reader["TotalCost"]}";
                            }
                            else
                            {
                                // OrderID not found 
                                MessageBox.Show("The entered Order ID does not exist.", "Order Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                rDisplay.Clear();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error connecting to database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string orderID = tBoxOrderID.Text.Trim();
            string newStatus = cbxStatus.Text.Trim();

            if (string.IsNullOrEmpty(orderID))
            {
                MessageBox.Show("Please enter an Order ID.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(newStatus))
            {
                MessageBox.Show("Please select a status from the combobox.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1. Connection string 
            string connectionString = @"Server=143.128.146.30\istn2;Database=ist2kr;User Id=ist2kr;Password=pihqje;";
   

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // 2. SQL command to update the Status field
                    string updateQuery = "UPDATE Orders SET Status = @status WHERE OrderID = @orderID";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@status", newStatus);
                        cmd.Parameters.AddWithValue("@orderID", orderID);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Order status updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            RefreshOrderGrid();
                        }
                        else
                        {
                            MessageBox.Show("The entered Order ID does not exist.", "Order Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating order: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

       
        private void RefreshOrderGrid()
        {
            try
            {
                string connectionString = @"Server=143.128.146.30\istn2;Database=ist2kr;User Id=ist2kr;Password=pihqje;";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string selectQuery = "SELECT * FROM Orders";
                    SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    orderDataGridView.DataSource = dt;
                }
            }
            catch
            {
               
            }
        }
    }
}
