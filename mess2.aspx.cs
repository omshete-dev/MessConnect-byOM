using System;
using System.Data.SqlClient;

public partial class mess2 : System.Web.UI.Page
{
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        SqlConnection connection = new SqlConnection(DbConfig.GetConnectionString("MessConnect"));

        string query = @"SELECT CustomId
                         FROM mst_customer
                         WHERE Email=@Email AND Password=@Password";

        SqlCommand cmd = new SqlCommand(query, connection);

        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
        cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());

        try
        {
            connection.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                Session["CustomId"] = dr["CustomId"].ToString();
                Response.Redirect("mess6.aspx");
            }
            else
            {
                Iblmsg.Text = "Invalid username and password";
            }

            dr.Close();
            connection.Close();
        }
        catch (Exception ex)
        {
            Iblmsg.Text = ex.Message;
        }
    }
}