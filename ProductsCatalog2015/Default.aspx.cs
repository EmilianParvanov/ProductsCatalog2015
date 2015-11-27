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
        protected void Page_PreRender(object sender, EventArgs e)
        {

        }
        
        private static int PageSize = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
        public static string FetchProducts(int pageIndex, int sortExpr)
        {
            string query = "[GetProductsWithPaging]";
            SqlCommand cmd = new SqlCommand(query);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
            cmd.Parameters.AddWithValue("@PageSize", PageSize);
            cmd.Parameters.AddWithValue("@SortExpr", sortExpr);
            cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
            return GetData(cmd, pageIndex).GetXml();
        }

        private static DataSet GetData(SqlCommand cmd, int pageIndex)
        {
            string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            using (SqlConnection con = new SqlConnection(strConnString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds, "product");
                        DataTable dt = new DataTable("Pager");
                        dt.Columns.Add("PageIndex");
                        dt.Columns.Add("PageSize");
                        dt.Columns.Add("RecordCount");
                        dt.Rows.Add();
                        dt.Rows[0]["PageIndex"] = pageIndex;
                        dt.Rows[0]["PageSize"] = PageSize;
                        dt.Rows[0]["RecordCount"] = cmd.Parameters["@RecordCount"].Value;
                        ds.Tables.Add(dt);
                        return ds;
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