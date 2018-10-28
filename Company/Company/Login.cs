using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
//using System.Threading.Taskus;

namespace Company
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            UserNameTextBox.Clear();
            PasswordTextBox.Clear();
            UserNameTextBox.Focus();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            //To Do: Check Or Connect SqlConnection with database
            SqlConnection con = new SqlConnection(@"Data Source=TAJ-PC\SQLEXPRESS;Initial Catalog=Company;Integrated Security=True");           
            SqlDataAdapter sda = new SqlDataAdapter(@"SELECT *
            FROM [Company].[dbo].[Login] where UserName='"+ UserNameTextBox.Text +"' and Password='"+ PasswordTextBox.Text +"'",con);
            
            DataTable dt = new DataTable("dbo.Login");
            sda.Fill(dt);

            //Condition check [ True ] Login UserName & Password
            if (dt.Rows.Count == 1)
            {
                this.Hide();
                StockMain main = new StockMain();
                main.Show();
            }
            //Condition check [ False ] Login UserName & Password
            else
            {
                MessageBox.Show("Invalid UserName Or Password..!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                ClearButton_Click(sender, e);
            }
        }
    }
}
