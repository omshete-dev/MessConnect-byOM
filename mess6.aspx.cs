using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class mess6 : System.Web.UI.Page
{
    SqlConnection connection = new SqlConnection(DbConfig.GetConnectionString("MessConnect"));

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["CustomId"] == null)
        {
            Response.Redirect("mess2.aspx");
            return;
        }

        if (!IsPostBack)
        {
            ShowCustomerName();
            LoadMess();
        }
    }

    void ShowCustomerName()
    {
        if (Session["CustomId"] == null)
        {
            lblName.Text = "Customer";
            return;
        }

        SqlCommand cmd = new SqlCommand(
            "SELECT FullName FROM mst_customer WHERE CustomId=@CustomId",
            connection);

        cmd.Parameters.AddWithValue("@CustomId", Session["CustomId"]);

        connection.Open();

        object result = cmd.ExecuteScalar();

        if (result != null)
            lblName.Text = result.ToString();
        else
            lblName.Text = "Customer";

        connection.Close();
    }

    void LoadMess()
    {
        SqlDataAdapter da = new SqlDataAdapter(
            @"SELECT MessId, MessName, Location, Description,
                     PricePerMonth, AvailableSeats
              FROM mst_MessInfo
              WHERE IsActive = 1",
            connection);

        DataTable dt = new DataTable();
        da.Fill(dt);

        rptMess.DataSource = dt;
        rptMess.DataBind();
    }

    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        SqlDataAdapter da = new SqlDataAdapter(
            @"SELECT MessId, MessName, Location, Description,
                     PricePerMonth, AvailableSeats
              FROM mst_MessInfo
              WHERE IsActive=1
              AND (MessName LIKE @search
                   OR Location LIKE @search)",
            connection);

        da.SelectCommand.Parameters.AddWithValue(
            "@search", "%" + txtSearch.Text + "%");

        DataTable dt = new DataTable();
        da.Fill(dt);

        rptMess.DataSource = dt;
        rptMess.DataBind();
    }

    protected void lnkMess_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;

        string messId = btn.CommandArgument;

        Response.Redirect("messdetails.aspx?MessId=" + messId);
    }

    protected void btnFav_Click(object sender, EventArgs e)
    {
        if (Session["CustomId"] == null)
        {
            Response.Redirect("mess2.aspx");
            return;
        }

        LinkButton btn = (LinkButton)sender;

        int messId = Convert.ToInt32(btn.CommandArgument);
        int customerId = Convert.ToInt32(Session["CustomId"]);

        SqlCommand cmd = new SqlCommand(@"
            IF NOT EXISTS
            (
                SELECT *
                FROM mst_favorite
                WHERE CustomId=@CustomId
                AND MessId=@MessId
            )
            BEGIN
                INSERT INTO mst_favorite(CustomId, MessId)
                VALUES(@CustomId, @MessId)
            END",
            connection);

        cmd.Parameters.AddWithValue("@CustomId", customerId);
        cmd.Parameters.AddWithValue("@MessId", messId);

        connection.Open();
        cmd.ExecuteNonQuery();
        connection.Close();

        ClientScript.RegisterStartupScript(
            this.GetType(),
            "msg",
            "alert('Added to Favorite Successfully');",
            true);
    }
}