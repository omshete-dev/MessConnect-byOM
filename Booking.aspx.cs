using System;
using System.Data.SqlClient;
using System.Globalization;


public partial class Booking : System.Web.UI.Page
{


    SqlConnection connection = new SqlConnection(DbConfig.GetConnectionString("MessConnect"));


    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
            return;

        int messId;
        if (!int.TryParse(Request.QueryString["MessId"], out messId) || messId <= 0)
            return;

        SqlCommand cmd = new SqlCommand(
            "SELECT PricePerMonth FROM mst_messinfo WHERE MessId=@MessId",
            connection);
        cmd.Parameters.AddWithValue("@MessId", messId);

        try
        {
            connection.Open();
            object price = cmd.ExecuteScalar();
            if (price != null && price != DBNull.Value)
                txtAmount.Text = Convert.ToDecimal(price).ToString("0.##", CultureInfo.InvariantCulture);
        }
        finally
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }
    }



    protected void btnConfirm_Click(object sender, EventArgs e)
    {

        if (Session["CustomId"] == null)
        {
            Response.Write("<script>alert('Please login first');</script>");
            return;
        }


        int customId;
        int messId;
        decimal amount;

        if (!int.TryParse(Convert.ToString(Session["CustomId"]), out customId) ||
            !int.TryParse(Request.QueryString["MessId"], out messId) ||
            !decimal.TryParse(txtAmount.Text.Trim(), out amount) || amount <= 0)
        {
            Response.Write("<script>alert('Invalid booking details. Please check the mess and amount.');</script>");
            return;
        }



        string query = @"
    INSERT INTO mst_bookingInfo
    (CustomId, MessId, MealType, Date, Amount, Status)

    VALUES
    (@CustomerId,@MessId,@MealType,@Date,@Amount,@Status)";


        SqlCommand cmd = new SqlCommand(query, connection);


        cmd.Parameters.AddWithValue("@CustomerId", customId);

        cmd.Parameters.AddWithValue("@MessId", messId);

        cmd.Parameters.AddWithValue("@MealType", ddlMeal.Text);

        cmd.Parameters.AddWithValue("@Date", DateTime.Now);

        cmd.Parameters.AddWithValue("@Amount", amount);

        cmd.Parameters.AddWithValue("@Status", "Pending");


        try
        {
            connection.Open();
            cmd.ExecuteNonQuery();
        }
        finally
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }


        Response.Write("<script>alert('Booking Successful');window.location='mess6.aspx';</script>");

    }

}