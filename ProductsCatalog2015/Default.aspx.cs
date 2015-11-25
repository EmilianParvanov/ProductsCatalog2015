using ProductsCatalog2015.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProductsCatalog2015
{

    public partial class _Default : Page
    {
        Entities context = new Entities();
        string Sort_Direction = "name ASC";

        protected void Page_PreRender(object sender, EventArgs e)
        {

        }
          
        protected void gridViewProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridViewProducts.PageIndex = e.NewPageIndex;
            DataView dvEmployee = Getdata();
            GridVwPagingSorting.DataSource = dvEmployee;
            GridVwPagingSorting.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //original, conventional GV
                var products = context.Products.OrderBy(x => x.id);
                GridVwPagingSorting.DataSource = products.ToList();
                GridVwPagingSorting.DataBind();
                
                ViewState["SortExpr"] = Sort_Direction;
                DataView dvZakuski = Getdata();
                GridVwPagingSorting.DataSource = dvZakuski;
                GridVwPagingSorting.DataBind();

                //AddNewZakuskaGrid();

                //jquery ajax GV
                this.BindDummyRow();
                InvokeNewZakuskaInputFieldsView.Visible = false;
            }
        }

        private void BindDummyRow()
        {
            DataTable dummy = new DataTable();
            dummy.Columns.Add("id");
            dummy.Columns.Add("name");
            dummy.Columns.Add("price");
            dummy.Rows.Add();
            gvProducts.DataSource = dummy;
            gvProducts.DataBind();
        }
     
        private DataView Getdata()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                DataSet dsEmployee = new DataSet();
                string strSelectCmd = "SELECT [id],[name], [price] FROM [product]";
                SqlDataAdapter da = new SqlDataAdapter(strSelectCmd, conn);
                da.Fill(dsEmployee, "product");
                DataView dvEmp = dsEmployee.Tables["product"].DefaultView;
                dvEmp.Sort = ViewState["SortExpr"].ToString();
                return dvEmp;
            }
        }

        protected void PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridVwPagingSorting.PageIndex = e.NewPageIndex;
            DataView dvEmployee = Getdata();
            GridVwPagingSorting.DataSource = dvEmployee;
            GridVwPagingSorting.DataBind();
        }

        protected void Sorting(object sender, GridViewSortEventArgs e)
        {
            string[] SortOrder = ViewState["SortExpr"].ToString().Split(' ');
            if (SortOrder[0] == e.SortExpression)
            {
                if (SortOrder[1] == "ASC")
                {
                    ViewState["SortExpr"] = e.SortExpression + " " + "DESC";
                }
                else
                {
                    ViewState["SortExpr"] = e.SortExpression + " " + "ASC";
                }
            }
            else
            {
                ViewState["SortExpr"] = e.SortExpression + " " + "ASC";
            }
            GridVwPagingSorting.DataSource = Getdata();
            GridVwPagingSorting.DataBind();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridVwPagingSorting.EditIndex = e.NewEditIndex;
            ((BoundField)GridVwPagingSorting.Columns[0]).ReadOnly = true;
            DataView dvZakuski = Getdata();
            GridVwPagingSorting.DataSource = dvZakuski;
            GridVwPagingSorting.DataBind();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int userid = Convert.ToInt32(GridVwPagingSorting.DataKeys[e.RowIndex].Value.ToString());
            GridViewRow row = (GridViewRow)GridVwPagingSorting.Rows[e.RowIndex];
            Label lblID = (Label)row.FindControl("lblID");
            
            TextBox newName = (TextBox)row.Cells[1].Controls[0];
            TextBox newPrice = (TextBox)row.Cells[2].Controls[0];

            string newPr = newPrice.Text.Replace(",",".");
           
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
            conn.Open();
            SqlCommand cmd = new SqlCommand("update product set name='" + newName.Text + "', price='" + newPr + "'" +
                                                             " where id='" + (GridVwPagingSorting.DataKeys[e.RowIndex].Value) + "';", conn);
            cmd.ExecuteNonQuery();
            GridVwPagingSorting.EditIndex = -1;
            conn.Close();

            DataView dvZakuski = Getdata();
           
            GridVwPagingSorting.DataSource = dvZakuski;
            GridVwPagingSorting.DataBind();
        }

        //delete conventional
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = (GridViewRow)GridVwPagingSorting.Rows[e.RowIndex];
            Label lbldeleteid = (Label)row.FindControl("lblID");
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
            conn.Open();
            SqlCommand cmd = new SqlCommand("delete FROM Product where id='" + Convert.ToInt32(GridVwPagingSorting.DataKeys[e.RowIndex].Value.ToString()) + "'", conn);
            cmd.ExecuteNonQuery();

            DataView dvZakuski = Getdata();
            GridVwPagingSorting.DataSource = dvZakuski;
            GridVwPagingSorting.DataBind();

            conn.Close();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridVwPagingSorting.EditIndex = -1;
            DataView dvZakuski = Getdata();
            GridVwPagingSorting.DataSource = dvZakuski;
            GridVwPagingSorting.DataBind();
        }
        /*
        protected void InvokeNewZakuskaView(object sender, EventArgs e)
        {
            addNewZakuskaGrid.Visible = true;
        }
        */
/*
        protected void AddNewZakuska(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)addNewZakuskaGrid.Rows[0];
            TextBox txtBox = row.FindControl("txtName") as TextBox;
            TextBox txtPrice = row.FindControl("txtPrice") as TextBox;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Product (name, price) VALUES ('" + txtBox.Text + "', '" + txtPrice.Text + "');", conn);
            cmd.ExecuteNonQuery();
            GridVwPagingSorting.EditIndex = -1;
            conn.Close();

            DataView dvZakuski = Getdata();
            GridVwPagingSorting.DataSource = dvZakuski;
            GridVwPagingSorting.DataBind();
            addNewZakuskaGrid.Visible = false;
            txtBox.Text = string.Empty;
            txtPrice.Text = string.Empty;
        }
        */
        /*
        private void AddNewZakuskaGrid()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("nameCol", typeof(string)));
            dt.Columns.Add(new DataColumn("priceCol", typeof(string)));

            dr = dt.NewRow();
            dr["nameCol"] = string.Empty;
            dr["priceCol"] = string.Empty;

            dt.Rows.Add(dr);

            ViewState["CurrentTable"] = dt;

            addNewZakuskaGrid.DataSource = dt;
            addNewZakuskaGrid.DataBind();
        }

        protected void CanlcelNewZakuska(object sender, EventArgs e)
        {
            addNewZakuskaGrid.Visible = false;
            GridViewRow row = (GridViewRow)addNewZakuskaGrid.Rows[0];
            TextBox txtBox = row.FindControl("txtName") as TextBox;
            TextBox txtPrice = row.FindControl("txtPrice") as TextBox;
            txtBox.Text = string.Empty;
            txtPrice.Text = string.Empty;
        }
        */

        //1st jQueryAJAX try

        protected void InvokeNewZakuskaInputFieldsBtnFunc(object sender, EventArgs e)
        {
            InvokeNewZakuskaInputFieldsView.Visible = true;
        }

        protected void CancelNewZakuska(object sender, EventArgs e)
        {
            InvokeNewZakuskaInputFieldsView.Visible = false;

            txtName.Text = string.Empty;
            txtPrice.Text = string.Empty;
        }

        [WebMethod]
        public static string GetProducts()
        {
            string query = "select id, name, convert(decimal(10,2),price) as price from product";
            SqlCommand cmd = new SqlCommand(query);
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        return ds.GetXml();
                    }
                }
            }
        }

        [WebMethod]
        public static void DeleteProduct(int id)
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("delete FROM product WHERE id = @id"))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        
        [WebMethod]
        public static void UpdateProduct(int id, string name, string price)
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("update product SET name = @name, price = @price WHERE id = @id"))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        [WebMethod]
        public static int InsertProduct(string name, string price)
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Product (name, price) VALUES (@name, @price) SELECT SCOPE_IDENTITY()"))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Connection = con;
                    con.Open();
                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                    return id;
                }
            }
        }
    }
}