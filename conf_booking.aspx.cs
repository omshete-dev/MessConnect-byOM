using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class conf_booking : System.Web.UI.Page
{
  

    SqlConnection con = new SqlConnection(DbConfig.GetConnectionString("MessConnect"));

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["OwnerId"] == null)
            {
                Response.Redirect("mess21.aspx");
                return;
            }
            LoadConfirmed();
        }
    }

    void LoadConfirmed()
    {
        int OwnerId = Convert.ToInt32(Session["OwnerId"]);

        SqlDataAdapter da = new SqlDataAdapter(@"
SELECT
c.FullName,
c.PhoneNo,
m.MessName,
b.MealType,
b.Date,
b.Amount

FROM mst_BookingInfo b

INNER JOIN mst_customer c
ON b.CustomId=c.CustomId

INNER JOIN mst_MessInfo m
ON b.MessId=m.MessId

WHERE
m.OwnerId=@OwnerId
AND b.Status='Confirmed'", con);

        da.SelectCommand.Parameters.AddWithValue("@OwnerId", OwnerId);

        DataTable dt = new DataTable();

        da.Fill(dt);

        rptConfirmed.DataSource = dt;
        rptConfirmed.DataBind();
    }
}
