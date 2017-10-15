using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace 文件序号管理
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (FindItemContent("单位") == "")
            {
                Label2.Text = "系统未初始化，请联系管理员！";// lblInfor0.Text = "";
                return;
            }
            else
            { 
                 //Response.Redirect("./SimpleDataItemManage.aspx#tips");
            
            }
                

        }
        private String FindItemContent(string ItemName)
        {
            SqlConnection conn = ClassDataServer.DBCon();
            string SDanWei = "";

            conn.Open();
            SqlCommand mycmd = conn.CreateCommand();
            string sql = "select * from SimpleData where ItemName = '" + ItemName + "'";
            mycmd.CommandType = CommandType.Text;
            mycmd.CommandText = sql;

            SqlDataReader reader = mycmd.ExecuteReader();


            if (reader.Read())
            {
                if (System.DBNull.Value != (reader["ItemName"]))
                {
                    SDanWei = (string)reader["ItemName"];
                }
                else
                    SDanWei = "";

            }
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            return SDanWei;
        }
    }
}
