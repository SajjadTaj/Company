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
            SqlConnection con = new SqlConnection(@"Data Source=TAJ-PC\SQLEXPRESS;Initial Catalog=Company;Integrated Security=True");
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

            var sqlQuery="";
            if (IfProductsExists(con, ProductCodeTextBox.Text))
            {
                sqlQuery = @"UPDATE [Product] SET [ProductName] = '" + ProductNameTextBox.Text + "', [ProductStatus] = '" + status + "' WHERE [ProductCode] = '" + ProductCodeTextBox.Text + "'";
            }
            else
            {
                sqlQuery=(@"INSERT INTO [Company].[dbo].[Product]([ProductCode],[ProductName],[ProductStatus])
            VALUES
           ('" + ProductCodeTextBox.Text + "','" + ProductNameTextBox.Text + "','" + status + "')");
            }
            SqlCommand cmd = new SqlCommand(sqlQuery, con);
            cmd.ExecuteNonQuery();
            con.Close();

            //Reading Data
            LoadData();
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
            SqlConnection con = new SqlConnection(@"Data Source=TAJ-PC\SQLEXPRESS;Initial Catalog=Company;Integrated Security=True");

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
        private void ProductDataGridView_MouseDoubleClick(object sender, MouseEventArgs e)0
        {
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
            // For:  DELETE FROM Product TABLE

            SqlConnection con = new SqlConnection(@"Data Source=TAJ-PC\SQLEXPRESS;Initial Catalog=Company;Integrated Security=True");
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
            //ProductCodeTextBox.Clear();
            //ProductNameTextBox.Clear();
            //StatusComboBox.SelectedIndex = 0;
        }
    }
}
