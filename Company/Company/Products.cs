using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Company
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
        }

        // FOR: Products Form  ( Load ) StatusComboBox Show "Active"
        private void Products_Load(object sender, EventArgs e)
        {
            StatusComboBox.SelectedIndex = 0;
            LoadData();
        }
        // FOR: AddButton_Click
        private void AddButton_Click(object sender, EventArgs e)
        {
            if (validation())
            {
                SqlConnection con = Connection.GetConnection();
                //SqlConnection con = new SqlConnection(@"Data Source=TAJ-PC\SQLEXPRESS;Initial Catalog=Company;Integrated Security=True");
                // FOR: INSERT / UPDATE Logic
                con.Open();
                bool status = false;
                if (StatusComboBox.SelectedIndex == 0)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }

                var sqlQuery = "";
                if (IfProductsExists(con, ProductCodeTextBox.Text))
                {
                    sqlQuery = @"UPDATE [Product] SET [ProductName] = '" + ProductNameTextBox.Text + "', [ProductStatus] = '" + status + "' WHERE [ProductCode] = '" + ProductCodeTextBox.Text + "'";
                }
                else
                {
                    sqlQuery = (@"INSERT INTO [Company].[dbo].[Product]([ProductCode],[ProductName],[ProductStatus])
            VALUES
           ('" + ProductCodeTextBox.Text + "','" + ProductNameTextBox.Text + "','" + status + "')");
                }
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.ExecuteNonQuery();
                con.Close();

                //Reading Data
                LoadData();
                ResetRecords(); 
            }
        }
        private bool IfProductsExists(SqlConnection con,string ProductCode)
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select 1 From [Product] WHERE [ProductCode] = '" + ProductCode + "'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        public void LoadData()
        {
            SqlConnection con = Connection.GetConnection();

            SqlDataAdapter sda = new SqlDataAdapter("Select * From [Company].[dbo].[Product]", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            ProductDataGridView.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = ProductDataGridView.Rows.Add();
                ProductDataGridView.Rows[n].Cells[0].Value = item["ProductCode"].ToString();
                ProductDataGridView.Rows[n].Cells[1].Value = item["ProductName"].ToString();
                if ((bool)item["ProductStatus"])
                {
                    ProductDataGridView.Rows[n].Cells[2].Value = "Active";
                }
                else
                {
                    ProductDataGridView.Rows[n].Cells[2].Value = "Deactive";
                }
            }
        }
        // FOR:  MouseDoubleClick on DataGridView THEN ShowData In All TextBoxes
        private void ProductDataGridView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            AddButton.Text = "Update";
            ProductCodeTextBox.Text = ProductDataGridView.SelectedRows[0].Cells[0].Value.ToString();
            ProductNameTextBox.Text = ProductDataGridView.SelectedRows[0].Cells[1].Value.ToString();
            if (ProductDataGridView.SelectedRows[0].Cells[2].Value.ToString() == "Active")
            {
                StatusComboBox.SelectedIndex=0;
            }
            else
            {
                StatusComboBox.SelectedIndex=1;
            }
        }
        // FOR: DeleteButton_Click
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are You Sure Want to Delete?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                if (validation())
                {
                    // For:  DELETE FROM Product TABLE
                    SqlConnection con = Connection.GetConnection();
                    var sqlQuery = "";
                    if (IfProductsExists(con, ProductCodeTextBox.Text))
                    {
                        con.Open();
                        sqlQuery = @"DELETE FROM [Product] WHERE [ProductCode] = '" + ProductCodeTextBox.Text + "'";
                        SqlCommand cmd = new SqlCommand(sqlQuery, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    else
                    {
                        MessageBox.Show("Record Not Exits...!");
                    }
                    //Reading Data
                    LoadData();
                    ResetRecords();
                }
            }
        }
        // FOR: ResetRecords For All Buttons Applay
        private void ResetRecords()
        {
            ProductCodeTextBox.Clear();
            ProductNameTextBox.Clear();
            StatusComboBox.SelectedIndex = -1;
            AddButton.Text = "Add";
            ProductCodeTextBox.Focus();
        }
        // FOR: ResetButton_Click
        private void ResetButton_Click(object sender, EventArgs e)
        {
            ResetRecords();
        }
        // FOR: Set validation For All Buttons Applay
        private bool validation()
        {
            bool result = false;
            if (string.IsNullOrEmpty(ProductCodeTextBox.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(ProductCodeTextBox, "Product Code Required");
            }
            else if (string.IsNullOrEmpty(ProductNameTextBox.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(ProductCodeTextBox, "Product Name Required");
            }
            else if (StatusComboBox.SelectedIndex == -1)
            {
                errorProvider1.Clear();
                errorProvider1.SetError(StatusComboBox, "Selecte Status");
            }
            else
            {
                result = true;
            }

            if (!string.IsNullOrEmpty(ProductCodeTextBox.Text) && !string.IsNullOrEmpty(ProductNameTextBox.Text) && StatusComboBox.SelectedIndex > -1)
            {
                result = true;
            }
            return result;
        }
    }
}
