using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddMess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["OwnerId"] == null)
        {
            Response.Redirect("mess21.aspx");
            return;
        }
    }
protected void btnSave_Click(object sender, EventArgs e)
{
    long price;
    int totalSeats;
    int availableSeats;

    if (string.IsNullOrWhiteSpace(txtMessName.Text) ||
        string.IsNullOrWhiteSpace(txtLocation.Text) ||
        !long.TryParse(txtPrice.Text.Trim(), out price) || price <= 0 ||
        !int.TryParse(txtTotalSeats.Text.Trim(), out totalSeats) || totalSeats <= 0 ||
        !int.TryParse(txtAvailableSeats.Text.Trim(), out availableSeats) ||
        availableSeats < 0 || availableSeats > totalSeats)
    {
        Response.Write("<script>alert('Please enter valid mess details, price and seat values.');</script>");
        return;
    }

    SqlConnection con = new SqlConnection(DbConfig.GetConnectionString("MessConnect"));

    string query = @"INSERT INTO mst_MessInfo
    (
        OwnerId,
        MessName,
        Location,
        Description,
        ContactNo,
        Veg,
        NonVeg,
        wiFi,
        Parking,
        ACDining,
        PricePerMonth,
        TotalSeats,
        AvailableSeats,
        Status,
        isActive
    )
    VALUES
    (
        @OwnerId,
        @MessName,
        @Location,
        @Description,
        @ContactNo,
        @Veg,
        @NonVeg,
        @wiFi,
        @Parking,
        @ACDining,
        @PricePerMonth,
        @TotalSeats,
        @AvailableSeats,
        @Status,
        @isActive
    )";

    SqlCommand cmd = new SqlCommand(query, con);

    cmd.Parameters.AddWithValue("@OwnerId", Session["OwnerId"]);
    cmd.Parameters.AddWithValue("@MessName", txtMessName.Text);
    cmd.Parameters.AddWithValue("@Location", txtLocation.Text);
    cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
    cmd.Parameters.AddWithValue("@ContactNo", txtContactNo.Text);

    cmd.Parameters.AddWithValue("@Veg", chkVeg.Checked);
    cmd.Parameters.AddWithValue("@NonVeg", chkNonVeg.Checked);
    cmd.Parameters.AddWithValue("@wiFi", chkWifi.Checked);
    cmd.Parameters.AddWithValue("@Parking", chkParking.Checked);
    cmd.Parameters.AddWithValue("@ACDining", chkACDining.Checked);

    cmd.Parameters.AddWithValue("@PricePerMonth", price);
    cmd.Parameters.AddWithValue("@TotalSeats", totalSeats);
    cmd.Parameters.AddWithValue("@AvailableSeats", availableSeats);

    cmd.Parameters.AddWithValue("@Status", "Available");
    cmd.Parameters.AddWithValue("@isActive", true);

    try
    {
        con.Open();
        cmd.ExecuteNonQuery();
            Response.Write("<script>alert('Mess Information Saved Successfully');window.location='ownerdash.aspx';</script>");
    }
    catch (Exception ex)
    {
        Response.Write("<script>alert('Unable to save mess information. Please check the entered values and database connection.');</script>");
    }
    finally
    {
        if (con.State == System.Data.ConnectionState.Open) con.Close();
        con.Dispose();
    }
}
}