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

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    private bool FindItem(string ItemName)
    {
        SqlConnection conn = ClassDataServer.DBCon();

        conn.Open();
        SqlCommand mycmd = conn.CreateCommand();
        string sql = "select * from SimpleDataItem where ItemName = '" + ItemName + "'";
        mycmd.CommandType = CommandType.Text;
        mycmd.CommandText = sql;

        SqlDataReader reader = mycmd.ExecuteReader();


        if (reader.Read())
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();

            }
            return true;
        }
        return false;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        lblInfor.Text = "";
        if (TextBox_jiaoyanshi.Text.Trim()=="")
        { 
            return;
        }
        if (FindItem(TextBox_jiaoyanshi.Text.Trim()))
        {
            lblInfor.Text = "条目已存在！";
            return;
        }

        SqlConnection conn = ClassDataServer.DBCon();
        SqlCommand mycmd = conn.CreateCommand();

        //判断上传格式是否符合 

        string sql = "AddSimpleDataItem";
        mycmd.CommandType = CommandType.StoredProcedure;
        mycmd.CommandText = sql;

        mycmd.Parameters.Add("@ItemName", SqlDbType.NVarChar, 50);
        mycmd.Parameters["@ItemName"].Value = TextBox_jiaoyanshi.Text.Trim();
         

        if (TextBox_leader1.Text != "")
        {
            mycmd.Parameters.Add("@ItemDescript", SqlDbType.NVarChar, 1000);
            mycmd.Parameters["@ItemDescript"].Value = TextBox_leader1.Text.Trim();
        } 
        try
        {
            conn.Open();
            mycmd.ExecuteNonQuery();
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();

            }
        }
        catch (Exception ex)
        {
            lblInfor.Text = lblInfor.Text + "Error:" + ex.ToString();
        }
        
         
        SqlDataSource1.DataBind();
        GridView1.DataBind();
    } 
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void LinkButton1_Click1(object sender, EventArgs e)
    {


        LinkButton lb1 = (LinkButton)sender;
        TextBox_ItemName.Text = (lb1.CommandArgument);
        TextBox_ItemContent.Text = "";
        lblInfor0.Text = "";

        Page.ClientScript.RegisterStartupScript(this.GetType(), "alt", "document.getElementById('clickLink').click();", true);
        //跳转到锚点
        //Response.Redirect("./SimpleDataItemManage.aspx#tips");
         
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alt", "document.getElementById('clickLink').click();", true);
        //跳转到锚点
        lblInfor.Text = "";
        if (TextBox_ItemName.Text.Trim() == "")
        {
            return;
        }
        if (TextBox_ItemContent.Text.Trim() == "")
        {
            return;
        }
        if (FindItemContent(TextBox_ItemName.Text.Trim(), TextBox_ItemContent.Text.Trim()))
        {
            lblInfor0.Text = "条目内容已存在！";// lblInfor0.Text = "";
            return;
        }

        SqlConnection conn = ClassDataServer.DBCon();
        SqlCommand mycmd = conn.CreateCommand();

        //判断上传格式是否符合 

        string sql = "AddSimpleData";
        mycmd.CommandType = CommandType.StoredProcedure;
        mycmd.CommandText = sql;

        mycmd.Parameters.Add("@ItemName", SqlDbType.NVarChar, 50);
        mycmd.Parameters["@ItemName"].Value = TextBox_ItemName.Text.Trim();

        mycmd.Parameters.Add("@ItemContent", SqlDbType.NVarChar, 50);
        mycmd.Parameters["@ItemContent"].Value = TextBox_ItemContent.Text.Trim();


        if (TextBox_Content_descript.Text != "")
        {
            mycmd.Parameters.Add("@ItemContentDescript", SqlDbType.NVarChar, 1000);
            mycmd.Parameters["@ItemContentDescript"].Value = TextBox_Content_descript.Text.Trim();
        }
        try
        {
            conn.Open();
            mycmd.ExecuteNonQuery();
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();

            }
        }
        catch (Exception ex)
        {
            lblInfor0.Text = lblInfor0.Text + "Error:" + ex.ToString();
        }
        lblInfor0.Text = "";
         
        SqlDataSource3.DataBind();
        GridView2.DataBind();
    }
    private bool FindItemContent(string ItemName, string ItemContent)
    {
        SqlConnection conn = ClassDataServer.DBCon();

        conn.Open();
        SqlCommand mycmd = conn.CreateCommand();
        string sql = "select * from SimpleData where ItemName = '" + ItemName + "' and ItemContent = '" + ItemContent + "'";
        mycmd.CommandType = CommandType.Text;
        mycmd.CommandText = sql;

        SqlDataReader reader = mycmd.ExecuteReader();


        if (reader.Read())
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();

            }
            return true;
        }
        return false;
    }
}
