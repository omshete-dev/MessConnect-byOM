using System;
using System.Data.SqlClient;


public partial class messdetails : System.Web.UI.Page
{

    SqlConnection connection = new SqlConnection(DbConfig.GetConnectionString("MessConnect"));


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            LoadMess();
        }

    }



    void LoadMess()
    {

        int id;
        if (!int.TryParse(Request.QueryString["MessId"], out id) || id <= 0)
        {
            Response.Redirect("mess6.aspx");
            return;
        }


        string query = "SELECT * FROM mst_messinfo WHERE MessId=@id";


        SqlCommand cmd = new SqlCommand(query, connection);

        cmd.Parameters.AddWithValue("@id", id);


        connection.Open();

        SqlDataReader dr = cmd.ExecuteReader();


        if (dr.Read())
        {

            lblName.Text = dr["MessName"].ToString();

            lblLocation.Text = dr["Location"].ToString();

            lblDescription.Text = dr["Description"].ToString();

            lblPrice.Text = dr["PricePerMonth"].ToString();

            lblSeats.Text = dr["AvailableSeats"].ToString();

        }


        connection.Close();


    }



    protected void btnBook_Click(object sender, EventArgs e)
    {

        Response.Redirect(
        "booking.aspx?MessId=" + Request.QueryString["MessId"]
        );

    }


}