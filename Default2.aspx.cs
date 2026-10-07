using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Default2 : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        SqlConnection connection = new SqlConnection(DbConfig.GetConnectionString("LegacyLogin"));
        SqlCommand com = new SqlCommand();
        SqlDataAdapter adp = new SqlDataAdapter();
        DataSet ds = new DataSet();
        String str = string.Empty;
        str = "select * from mst_login where UserName=@UserName AND Password=@Password";
        using (connection)
        {
            using (SqlCommand cmd = new SqlCommand(str, connection))
            {
                cmd.Parameters.AddWithValue("@UserName", usText.Text.Trim());
                cmd.Parameters.AddWithValue("@Password", paText.Text.Trim());
                adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);
            }
            connection.Close();

        }
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                Iblmsg.Text = "login successfull";
            }
            else
            {
                Iblmsg.Text = "invalid user name and password";
            }
        }
        else
        {
            Iblmsg.Text = "invalid user name and password";
        }
    }

    protected void btnReset_click(object sender, EventArgs e)
    {
        usText.Text = "";
        paText.Text = "";
        Iblmsg.Text = "";
            
    }
}