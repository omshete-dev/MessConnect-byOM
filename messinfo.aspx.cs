using System;
using System.Data.SqlClient;
using System.Web.UI;

public partial class messinfo : System.Web.UI.Page
{
    string connectionString = DbConfig.GetConnectionString("MessConnect");

    protected void Page_Load(object sender, EventArgs e)
    {
        // Owner Login नसल्यास Login Page वर पाठव
        if (Session["OwnerId"] == null)
        {
            Response.Redirect("mess21.aspx");
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int totalSeats;
        int availableSeats;
        decimal pricePerMonth;

        if (string.IsNullOrWhiteSpace(txtMessName.Text) ||
            string.IsNullOrWhiteSpace(txtLocation.Text) ||
            !decimal.TryParse(txtPricePerMonth.Text.Trim(), out pricePerMonth) || pricePerMonth <= 0 ||
            !int.TryParse(txtTotalSeats.Text.Trim(), out totalSeats) || totalSeats <= 0 ||
            !int.TryParse(txtAvailableSeats.Text.Trim(), out availableSeats) ||
            availableSeats < 0 || availableSeats > totalSeats)
        {
            Response.Write("<script>alert('Please enter valid mess details, price and seat values.');</script>");
            return;
        }

        try
        {
            int ownerId = Convert.ToInt32(Session["OwnerId"]);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string str = @"INSERT INTO mst_MessInfo
                (
                    OwnerId,
                    MessName,
                    Location,
                    Description,
                    ContactNo,
                    PricePerMonth,
                    TotalSeats,
                    AvailableSeats,
                    Veg,
                    Nonveg,
                    WiFi,
                    Parking,
                    ACDining,
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
                    @PricePerMonth,
                    @TotalSeats,
                    @AvailableSeats,
                    @Veg,
                    @Nonveg,
                    @WiFi,
                    @Parking,
                    @ACDining,
                    @Status,
                    @isActive
                )";

                SqlCommand com = new SqlCommand(str, connection);

                com.Parameters.AddWithValue("@OwnerId", ownerId);
                com.Parameters.AddWithValue("@MessName", txtMessName.Text);
                com.Parameters.AddWithValue("@Location", txtLocation.Text);
                com.Parameters.AddWithValue("@Description", txtDescription.Text);
                com.Parameters.AddWithValue("@ContactNo", txtContact.Text);
                com.Parameters.AddWithValue("@PricePerMonth", pricePerMonth);
                com.Parameters.AddWithValue("@TotalSeats", totalSeats);
                com.Parameters.AddWithValue("@AvailableSeats", availableSeats);
                com.Parameters.AddWithValue("@Veg", chkVeg.Checked);
                com.Parameters.AddWithValue("@Nonveg", chkNonVeg.Checked);
                com.Parameters.AddWithValue("@WiFi", chkWifi.Checked);
                com.Parameters.AddWithValue("@Parking", chkParking.Checked);
                com.Parameters.AddWithValue("@ACDining", chkACDining.Checked);
                com.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                com.Parameters.AddWithValue("@isActive", true);

                int result = com.ExecuteNonQuery();

                if (result > 0)
                {
                    Response.Write("<script>alert('Mess Information Saved Successfully');window.location='ownerdash.aspx';</script>");
                }
                else
                {
                    Response.Write("<script>alert('Data not saved');</script>");
                }
            }
        }
        catch (Exception)
        {
            Response.Write("<script>alert('Unable to save mess information. Please check the entered values and database connection.');</script>");
        }
    }
}