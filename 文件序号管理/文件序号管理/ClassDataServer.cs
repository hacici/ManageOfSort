using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.IO;
using System.Text; 



 


/// <summary>
///ClassDataServer 的摘要说明
/// </summary>
public class ClassDataServer
{
    private static String strConn = ConfigurationManager.ConnectionStrings["DataServices"].ConnectionString;  
    private static char[] KeyinRandomPassword = {'a','b','c','d','e','f','g','h','i','j','k','l','m'
                                                 ,'A','B','C','D','E','F','G','H','I','J','K','L','M'
                                                 ,'n','o','p','q','r','s','t','u','v','w','x','y','z'
                                                 ,'N','O','P','Q','R','S','T','U','V','W','X','Y','Z' 
                                                 ,'1','2','3','4','5','6','7','8','9','0'};
    private const int MaxKey = 62;//以上数组的个数
    private const int Minlong = 8;//最短密码
    private const int Maxlong = 12;//最长密码
	public ClassDataServer()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
      //  strConn =
	}
    public static SqlConnection DBCon()
    {
        return new  SqlConnection(strConn);
    }

    public static String generateapassword(int max = MaxKey)//根据MaxKey产生随机书
    {

        Random reum = new Random(DateTime.Now.Second);
        int randomdata;
        randomdata = reum.Next(Minlong,Maxlong);
        char[] cc = new char[randomdata];

        Random reum1 = new Random(DateTime.Now.Millisecond);
        int randomdata1;

        for (int i = 0; i < randomdata; i++)
        {
            randomdata1 = reum1.Next(max);
            cc[i] = KeyinRandomPassword[randomdata1];
        } 
        string ss = new string(cc);

            return ss;
    }
    public static int generatrandom(int max = MaxKey)//根据MaxKey产生随机书
    {

        Random reum = new Random(DateTime.Now.Millisecond);
        int randomdata;
        randomdata = reum.Next(max);
        return randomdata;
    }
    public static String decrypt(string str2,int key)
    {
        char[] cc = str2.ToCharArray();
        int j = 0;
        foreach (char s in str2)
        {
            int i = 0;
            while (s != KeyinRandomPassword[i])
                i++;

            cc[j] = KeyinRandomPassword[((i + MaxKey - key) % MaxKey)];
            j++;
        }
        string ss = new string(cc);

        return ss; 
    }
    public static String encrypt(string str2,int key)
    {
        char[] cc = str2.ToCharArray();
        int j = 0;
        foreach (char s in str2)
        {
            int i = 0;
            while (s != KeyinRandomPassword[i])
                i++;

            cc[j] = KeyinRandomPassword[((i + key) % MaxKey)];
            j++;
        }
        string ss = new string(cc);

        return ss;
    }


    public static String Update_Bit_in_table(string table, string pk, string pkidvalue,  string field, bool bvalue = false/*clear image*/)
    {

        SqlConnection conn = ClassDataServer.DBCon();
        SqlCommand mycmd = conn.CreateCommand();
        string sql = "update_bit_in_table";
        mycmd.CommandType = CommandType.StoredProcedure;
        mycmd.CommandText = sql;
        mycmd.Parameters.Add("@table", SqlDbType.NVarChar, 50);
        mycmd.Parameters["@table"].Value = table;

        mycmd.Parameters.Add("@pk", SqlDbType.NVarChar, 50);
        mycmd.Parameters["@pk"].Value = pk;
        mycmd.Parameters.Add("@id", SqlDbType.NVarChar, 50);
        mycmd.Parameters["@id"].Value = pkidvalue;

        mycmd.Parameters.Add("@field", SqlDbType.NVarChar, 50);
        mycmd.Parameters["@field"].Value = field;
        mycmd.Parameters.Add("@bvalue", SqlDbType.Bit,1);
        mycmd.Parameters["@bvalue"].Value = bvalue;

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
            return "Error:" + ex.ToString();
        }
        return "";
    }
    public static bool find_recorder_in_table(string table, string pk, string field)
    {
        SqlConnection conn = ClassDataServer.DBCon();

        conn.Open();
        SqlCommand mycmd = conn.CreateCommand();
        string sql = "select * from " + table + " where " + pk +"='" + field + "'";
        mycmd.CommandType = CommandType.Text;
        mycmd.CommandText = sql;

        SqlDataReader reader = mycmd.ExecuteReader();

        bool temp = false;
        if (reader.Read())
        {
            temp = true;
        }
        if (conn.State == ConnectionState.Open)
        {
            conn.Close();

        }
        return temp;

    }
    public static bool find_recorder_in_table(string table, string pk, int field)
    {
        SqlConnection conn = ClassDataServer.DBCon();

        conn.Open();
        SqlCommand mycmd = conn.CreateCommand();
        string sql = "select * from " + table + " where " + pk + "='" + field + "'";
        mycmd.CommandType = CommandType.Text;
        mycmd.CommandText = sql;

        SqlDataReader reader = mycmd.ExecuteReader();

        bool temp = false;
        if (reader.Read())
        {
            temp = true;
        }
        if (conn.State == ConnectionState.Open)
        {
            conn.Close();

        }
        return temp;

    }
    public static String UpdateImageInTable(string pk, int pkid, FileUpload filename, string table, string field, bool bdelete = false/*clear image*/)
    {

        bool flag = false;
        if (bdelete)
        {
            flag = true;
        }
        else
        {
            if (filename.HasFile)
            {
                string fileExtension = Path.GetExtension(filename.FileName).ToUpper();
                //只允许上传格式
                string[] allowExtension = { ".JPG", ".GIF", ".PNG" };
                for (int i = 0; i < allowExtension.Length; i++)
                {
                    if (fileExtension == allowExtension[i])
                        flag = true;
                }
                if (!flag)
                {
                    return "图片不属于可支持图片格式，没有提交到服务器.";

                }
            }
            else
            {
                return "没有选图，没的上传啊.";


            }
        }
        //上传 
        int imgSize;
        string imgType;
        Stream imgStream;
        byte[] imgContent = null;

        //准备图片 
        if (flag && !bdelete)
        {//需要上传
            imgSize = filename.PostedFile.ContentLength;
            imgType = filename.PostedFile.ContentType;
            imgStream = filename.PostedFile.InputStream;
            imgContent = new byte[imgSize];
            imgStream.Read(imgContent, 0, imgSize);
            imgStream.Close();
        }
        ///准备图片
        ///

        SqlConnection conn = ClassDataServer.DBCon();
        SqlCommand mycmd = conn.CreateCommand();
        string sql = "update_tupian";
        mycmd.CommandType = CommandType.StoredProcedure;
        mycmd.CommandText = sql;
        mycmd.Parameters.Add("@id", SqlDbType.Int, 4);
        mycmd.Parameters["@id"].Value = pkid;
        mycmd.Parameters.Add("@field", SqlDbType.NVarChar, 50);
        mycmd.Parameters["@field"].Value = field;
        mycmd.Parameters.Add("@pk", SqlDbType.NVarChar, 50);
        mycmd.Parameters["@pk"].Value = pk;
        mycmd.Parameters.Add("@table", SqlDbType.NVarChar, 50);
        mycmd.Parameters["@table"].Value = table;

        if (flag && !bdelete)
        {
            mycmd.Parameters.Add("@img1", SqlDbType.Image);

            mycmd.Parameters["@img1"].Value = imgContent;
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
            return "Error:" + ex.ToString();
        }
        return "";
    }
    public static String UpdateImageInTablenvarchar(string pk, string pkid, FileUpload filename, string table, string field, bool bdelete = false/*clear image*/)
    {

        bool flag = false;
        if (bdelete)
        {
            flag = true;
        }
        else
        {
            if (filename.HasFile)
            {
                string fileExtension = Path.GetExtension(filename.FileName).ToUpper();
                //只允许上传格式
                string[] allowExtension = { ".JPG", ".GIF", ".PNG" };
                for (int i = 0; i < allowExtension.Length; i++)
                {
                    if (fileExtension == allowExtension[i])
                        flag = true;
                }
                if (!flag)
                {
                    return "图片不属于可支持图片格式，没有提交到服务器.";

                }
            }
            else
            {
                return "没有选图，没的上传啊.";


            }
        }
        //上传 
        int imgSize;
        string imgType;
        Stream imgStream;
        byte[] imgContent = null;

        //准备图片 
        if (flag && !bdelete)
        {//需要上传
            imgSize = filename.PostedFile.ContentLength;
            imgType = filename.PostedFile.ContentType;
            imgStream = filename.PostedFile.InputStream;
            imgContent = new byte[imgSize];
            imgStream.Read(imgContent, 0, imgSize);
            imgStream.Close();
        }
        ///准备图片
        ///

        SqlConnection conn = ClassDataServer.DBCon();
        SqlCommand mycmd = conn.CreateCommand();
        string sql = "update_tupian_nvarchar";
        mycmd.CommandType = CommandType.StoredProcedure;
        mycmd.CommandText = sql;
        mycmd.Parameters.Add("@id", SqlDbType.NVarChar, 50);
        mycmd.Parameters["@id"].Value = pkid;
        mycmd.Parameters.Add("@field", SqlDbType.NVarChar, 50);
        mycmd.Parameters["@field"].Value = field;
        mycmd.Parameters.Add("@pk", SqlDbType.NVarChar, 50);
        mycmd.Parameters["@pk"].Value = pk;
        mycmd.Parameters.Add("@table", SqlDbType.NVarChar, 50);
        mycmd.Parameters["@table"].Value = table;

        if (flag && !bdelete)
        {
            mycmd.Parameters.Add("@img1", SqlDbType.Image);

            mycmd.Parameters["@img1"].Value = imgContent;
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
            return "Error:" + ex.ToString();
        }
        return "照片更新成功！";
    }
    public static String findtixingofshiti(int shitiid)
    {
        SqlConnection conn = ClassDataServer.DBCon();
        string tixing = "";
        conn.Open();
        SqlCommand mycmd = conn.CreateCommand();
        string sql = "select tixing from shiti where shiti_id = '" + shitiid + "'";
        mycmd.CommandType = CommandType.Text;
        mycmd.CommandText = sql;

        SqlDataReader reader = mycmd.ExecuteReader();


        if (reader.Read())
        {
            if (System.DBNull.Value != (reader["tixing"]))
            {
                tixing = (string)reader["tixing"];
            }
            else
                tixing = "";

        } 
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            } 
            return tixing;
    }
    public static bool findtixingiskeguanti(string tixing)
    {
        SqlConnection conn = ClassDataServer.DBCon();

        conn.Open();
        SqlCommand mycmd = conn.CreateCommand();
        string sql = "select iskeguanti from tixing where tixing = '" + tixing + "'";
        mycmd.CommandType = CommandType.Text;
        mycmd.CommandText = sql;

        SqlDataReader reader = mycmd.ExecuteReader();

        bool temp=false;
        if (reader.Read())
        {

                if (System.DBNull.Value != (reader["iskeguanti"]))
                {
                    temp = (bool)reader["iskeguanti"];
                }else
                  temp = false;

        }
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }  
            return temp;
    }

    public static int calculatfenshu(int shitiid, string studentanswer)
    {
        SqlConnection conn = ClassDataServer.DBCon();

        conn.Open();
        SqlCommand mycmd = conn.CreateCommand();
        string sql = "select cankaodaan,fenshu from View_CalculateDefen where shiti_id = '" + shitiid + "'";
        mycmd.CommandType = CommandType.Text;
        mycmd.CommandText = sql;

        SqlDataReader reader = mycmd.ExecuteReader();

        int temp = 0;
        string tempcankaodaan = "";
        int jianyifenshu = 0;
        if (reader.Read())
        {
            if (System.DBNull.Value != (reader["cankaodaan"]))
            {
                jianyifenshu = (int)reader["fenshu"];
                tempcankaodaan = (string)reader["cankaodaan"];
            }
            else
                temp = 0;

            //if ( 0 == string.Compare(studentanswer, tempcankaodaan))
            if (CalculateDefen(tempcankaodaan.Trim().ToUpper(), studentanswer.Trim().ToUpper()))
                temp = jianyifenshu;
            else
                temp = 0;
        }
        if (conn.State == ConnectionState.Open)
        {
            conn.Close();
        }
        return temp;
    }
    public static bool panduanduicuo(int shitiid, string studentanswer)
    {
        SqlConnection conn = ClassDataServer.DBCon();

        conn.Open();
        SqlCommand mycmd = conn.CreateCommand();
        string sql = "select cankaodaan from shiti where shiti_id = '" + shitiid + "'";
        mycmd.CommandType = CommandType.Text;
        mycmd.CommandText = sql;

        SqlDataReader reader = mycmd.ExecuteReader();

        bool temp = false;
        string tempcankaodaan = "";
        //int jianyifenshu = 0;
        if (reader.Read())
        {
            if (System.DBNull.Value != (reader["cankaodaan"]))
            { 
                tempcankaodaan = (string)reader["cankaodaan"];

            //if ( 0 == string.Compare(studentanswer, tempcankaodaan))
            if (CalculateDefen(tempcankaodaan.Trim().ToUpper(), studentanswer.Trim().ToUpper()))
                temp = true;
            else
                temp = false;
            }
        }
        if (conn.State == ConnectionState.Open)
        {
            conn.Close();
        }
        return temp;
    }
    public static String GetSystemInfo(string systeminfoname)
    {
        SqlConnection conn = ClassDataServer.DBCon();
        string tixing = "";
        conn.Open();
        SqlCommand mycmd = conn.CreateCommand();
        string sql = "select systeminfodetail from systeminfo where systeminfoname = '" + systeminfoname + "'";
        mycmd.CommandType = CommandType.Text;
        mycmd.CommandText = sql;

        SqlDataReader reader = mycmd.ExecuteReader();


        if (reader.Read())
        {
            if (System.DBNull.Value != (reader["tixing"]))
            {
                tixing = (string)reader["tixing"];
            }
            else
                tixing = "";

        }
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            return tixing;
    }
    public static String Md5hash_String(string InputString)
    {
        InputString = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(InputString,"MD5");
        return InputString;
    }

    public static String findshenfenzheng(int shenfenzheng,string ziduan)
    {
        SqlConnection conn = ClassDataServer.DBCon();

        SqlCommand mycmd = conn.CreateCommand();
        string sql = "select " + ziduan + "  from teacher where teacher_id = '" + shenfenzheng + "'";
        mycmd.CommandType = CommandType.Text;
        mycmd.CommandText = sql;

        string temp = "";
        try
        {
            conn.Open();
            SqlDataReader reader = mycmd.ExecuteReader();

            if (reader.Read())
            {
                if (System.DBNull.Value != (reader[ziduan]))
                {
                    temp = (string)reader[ziduan];
                }
            }
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();

            }
        }
        catch (Exception ex)
        {
            return "Error:" + ex.ToString();
        }
        return temp;
    }
    public static bool CalculateDefen(string cankaodaan, string studentanswer)
    { 

        bool flag = true;
        foreach (char s in studentanswer)
        {
            foreach (char s1 in cankaodaan)
                if (s == s1)
                {
                    flag = true;
                    break;
                }
                else
                    flag = false;

            if (flag)
                continue;
            else
                break;
        }

        if (cankaodaan.Length > studentanswer.Length )
                    flag = false;
            //答案不完全
          return flag;
        //tr2.ToCharArray();
        //int j = 0;
        //foreach (char s in str2)
        //{
        //    int i = 0;
        //    while (s != KeyinRandomPassword[i])
        //        i++;

        //    cc[j] = KeyinRandomPassword[((i + MaxKey - key) % MaxKey)];
        //    j++;
        //}
        //string ss = new string(cc);
    }
    public static String findanswer(int shitiid)
    {
        SqlConnection conn = ClassDataServer.DBCon();

        SqlCommand mycmd = conn.CreateCommand();
        string sql = "select cankaodaan from shiti where shiti_id = '" + shitiid + "'";
        mycmd.CommandType = CommandType.Text;
        mycmd.CommandText = sql;

        string temp = "";
        try
        {
            conn.Open();
            SqlDataReader reader = mycmd.ExecuteReader();

            if (reader.Read())
            {
                if (System.DBNull.Value != (reader["cankaodaan"]))
                {
                    temp = (string)reader["cankaodaan"];
                }
            }
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();

            }
        }
        catch (Exception ex)
        {
            return "Error:" + ex.ToString();
        }
        return temp;
    }
    public static DateTime GetDateTimeofkaoshiname(string kaoshiname,string timefield)
    {
        SqlConnection conn = ClassDataServer.DBCon();

        conn.Open();
        SqlCommand mycmd = conn.CreateCommand();
        string sql = "select " + timefield + " from kaoshi where kaoshiname = '" + kaoshiname + "'";
        mycmd.CommandType = CommandType.Text;
        mycmd.CommandText = sql;

        SqlDataReader reader = mycmd.ExecuteReader();

        DateTime temp = DateTime.Now;//肯定会小于返回后的now
        if (reader.Read())
        {
            if (System.DBNull.Value != (reader[timefield]))
            {
                temp = (DateTime)reader[timefield];

            }
        }
        if (conn.State == ConnectionState.Open)
        {
            conn.Close();

        }
        return temp;
    }
    public static bool findstudent(string xuehao)
    {
        SqlConnection conn = ClassDataServer.DBCon();

        conn.Open();
        SqlCommand mycmd = conn.CreateCommand();
        string sql = "select * from student where xuehao = '" + xuehao + "'";
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
    public static String UpdateFile(FileUpload FileUpload1, string webPath, bool isOLDName)    
    {//附件相对路径

        if (FileUpload1.HasFile)
        {
            string phyPath = HttpContext.Current.Server.MapPath(webPath);
            string fileName = FileUpload1.FileName;
            string extName = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();
            if (GetFuJianAllowSize(extName) > FileUpload1.PostedFile.ContentLength)//尺寸被允许
            {
                string saveName = string.Empty;
                if (isOLDName)
                {
                    saveName = fileName.Substring(fileName.LastIndexOf("/") + 1);
                }
                else
                {
                    string strDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    saveName = strDateTime + Guid.NewGuid().ToString().Substring(0, 8);

                }
                try
                {
                    FileUpload1.SaveAs(System.Web.HttpContext.Current.Request.PhysicalApplicationPath +
                        "uploaddirectory" + webPath + "\\" + saveName + "." + extName);
                }
                catch { return ""; }
                return "~/uploaddirectory/" + saveName + "." + extName;
            }
        }
        return "";


    }

    public static int GetCeLue(string CeLueName)
    //获取策略值，0为无此策略
    {
        SqlConnection conn = ClassDataServer.DBCon();
        conn.Open();
        SqlCommand mycmd = conn.CreateCommand();
        string sql = "select  *  from celue where celue_name = '" + CeLueName + "'";
        mycmd.CommandType = CommandType.Text;
        mycmd.CommandText = sql;

        SqlDataReader reader = mycmd.ExecuteReader();

        int temp = 0;
        if (reader.Read())
        {
            if (System.DBNull.Value != (reader["celue"]))
            {
                temp = (int)reader["celue"];
            }
        }
        if (conn.State == ConnectionState.Open)
        {
            conn.Close();
        }
        return temp;
    }

    public static int GetFuJianAllowSize(string fujian_leixing)
    { //获取策略值，0为无此策略
   
        SqlConnection conn = ClassDataServer.DBCon();
        conn.Open();
        SqlCommand mycmd = conn.CreateCommand();
        string sql = "select  *  from fujianleixing where fujian_leixing = '" + fujian_leixing + "'";
        mycmd.CommandType = CommandType.Text;
        mycmd.CommandText = sql;

        SqlDataReader reader = mycmd.ExecuteReader();

        int temp = 0;
        if (reader.Read())
        {
            if (System.DBNull.Value != (reader["biggest"]))
            {
                temp = (int)reader["biggest"];
            }
        }
        if (conn.State == ConnectionState.Open)
        {
            conn.Close();
        }
        if (temp == 0)//不在表的附件类型
            if (GetCeLue("fujian") != 1)//附件策略允许不列决的类型
                temp = GetCeLue("other_fujian_max_size");
        return temp;
    }

    public static String GetStudentName(string xuehao)
    {
        SqlConnection conn = ClassDataServer.DBCon();

        conn.Open();
        SqlCommand mycmd = conn.CreateCommand();
        string sql = "select studentname from student where xuehao = '" + xuehao + "'";
        mycmd.CommandType = CommandType.Text;
        mycmd.CommandText = sql;

        SqlDataReader reader = mycmd.ExecuteReader();


        string temp = "";
        if (reader.Read())
        {

            if (System.DBNull.Value != (reader["studentname"]))
            {
                temp = (string)reader["studentname"];
            }
        }
        if (conn.State == ConnectionState.Open)
        {
            conn.Close();

        }
        return temp;
    }
    public static String Getstudentanswer(string kaoshiname, int shitiid, string xuehao)
    {

        SqlConnection conn = ClassDataServer.DBCon();

        conn.Open();
        SqlCommand mycmd = conn.CreateCommand();
        string sql = "select * from studentanswer where xuehao = '" + xuehao + "' and shiti_id = '" + shitiid + "' and kaoshiname = '" + kaoshiname + "'";
        mycmd.CommandType = CommandType.Text;
        mycmd.CommandText = sql;

        SqlDataReader reader = mycmd.ExecuteReader();

        string temp = "";
        if (reader.Read())
        {
            if (System.DBNull.Value != (reader["answerzhengwen"]))
            {
                string i = (string)reader["answerzhengwen"];
                temp = i;
            }
        }
        if (conn.State == ConnectionState.Open)
        {
            conn.Close();

        }
        return temp;
    }
    public static int Getstudentanswerid(string kaoshiname, int shitiid, string xuehao)
    {

        SqlConnection conn = ClassDataServer.DBCon();

        conn.Open();
        SqlCommand mycmd = conn.CreateCommand();
        string sql = "select * from studentanswer where xuehao = '" + xuehao + "' and shiti_id = '" + shitiid + "' and kaoshiname = '" + kaoshiname + "'";
        mycmd.CommandType = CommandType.Text;
        mycmd.CommandText = sql;

        SqlDataReader reader = mycmd.ExecuteReader();

        int temp = -1;
        if (reader.Read())
        {
            if (System.DBNull.Value != (reader["studentanswer_id"]))
            {
                int i = (int)reader["studentanswer_id"];
                temp = i;
            }
        }
        if (conn.State == ConnectionState.Open)
        {
            conn.Close();

        }
        return temp;
    }
    public static String CheckCidInfo(string cid)
    {
        string[] aCity = new string[] { null, null, null, null, null, null, null, null, null, null, null, "北京", "天津", "河北", "山西", "内蒙古", null, null, null, null, null, "辽宁", "吉林", "黑龙江", null, null, null, null, null, null, null, "上海", "江苏", "浙江", "安微", "福建", "江西", "山东", null, null, null, "河南", "湖北", "湖南", "广东", "广西", "海南", null, null, null, "重庆", "四川", "贵州", "云南", "西藏", null, null, null, null, null, null, "陝西", "甘肃", "青海", "宁夏", "新疆", null, null, null, null, null, "台湾", null, null, null, null, null, null, null, null, null, "香港", "澳门", null, null, null, null, null, null, null, null, "国外" };
        double iSum = 0;
        System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"^\d{17}(\d|x)$");
        System.Text.RegularExpressions.Match mc = rg.Match(cid);
        if (!mc.Success)
        {
            return "";
        }
        cid = cid.ToLower();
        cid = cid.Replace("x", "a");
        if (aCity[int.Parse(cid.Substring(0, 2))] == null)
        {
            return "非法地区";
        }
        try
        {
            DateTime.Parse(cid.Substring(6, 4) + "-" + cid.Substring(10, 2) + "-" + cid.Substring(12, 2));
        }
        catch
        {
            return "非法生日";
        }
        for (int i = 17; i >= 0; i--)
        {
            iSum += (System.Math.Pow(2, i) % 11) * int.Parse(cid[17 - i].ToString(), System.Globalization.NumberStyles.HexNumber);
        }
        if (iSum % 11 != 1)
        {
            return ("非法证号");
        }

        return  ("地区:" + aCity[int.Parse(cid.Substring(0, 2))] + "," + cid.Substring(6, 4) + "-" + cid.Substring(10, 2) + "-" + cid.Substring(12, 2) + "," + (int.Parse(cid.Substring(16, 1)) % 2 == 1 ? "男" : "女"));

    }
    public static bool bCheckCidInfo(string cid)
    {
        string[] aCity = new string[] { null, null, null, null, null, null, null, null, null, null, null, "北京", "天津", "河北", "山西", "内蒙古", null, null, null, null, null, "辽宁", "吉林", "黑龙江", null, null, null, null, null, null, null, "上海", "江苏", "浙江", "安微", "福建", "江西", "山东", null, null, null, "河南", "湖北", "湖南", "广东", "广西", "海南", null, null, null, "重庆", "四川", "贵州", "云南", "西藏", null, null, null, null, null, null, "陝西", "甘肃", "青海", "宁夏", "新疆", null, null, null, null, null, "台湾", null, null, null, null, null, null, null, null, null, "香港", "澳门", null, null, null, null, null, null, null, null, "国外" };
        double iSum = 0;
        System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"^\d{17}(\d|X)$");
        System.Text.RegularExpressions.Match mc = rg.Match(cid);
        if (!mc.Success)
        {
            return false;
        }
        cid = cid.ToLower();
        cid = cid.Replace("x", "a");
        if (aCity[int.Parse(cid.Substring(0, 2))] == null)
        {
            return true;
        }
        try
        {
            DateTime.Parse(cid.Substring(6, 4) + "-" + cid.Substring(10, 2) + "-" + cid.Substring(12, 2));
        }
        catch
        {
            return false;
        }
        for (int i = 17; i >= 0; i--)
        {
            iSum += (System.Math.Pow(2, i) % 11) * int.Parse(cid[17 - i].ToString(), System.Globalization.NumberStyles.HexNumber);
        }
        if (iSum % 11 != 1)
        {
            return false;
        }

        return true;
        //(aCity[int.Parse(cid.Substring(0, 2))] + "," + cid.Substring(6, 4) + "-" + cid.Substring(10, 2) + "-" + cid.Substring(12, 2) + "," + (int.Parse(cid.Substring(16, 1)) % 2 == 1 ? "男" : "女"));

    }
}