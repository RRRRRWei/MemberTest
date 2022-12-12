using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using System.Data.SqlClient;
using MemberTest.Models;
using System.Security.Cryptography;
using System.Text;

namespace MemberTest.Services
{
    public class MemberDBService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["MemberDB"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);

        #region 註冊
        public void Register(Member newmember)
        {
            newmember.Password = HashPassword(newmember.Password);
            string sql = $@"insert into MemberTest(Account,Password,Username)
                        values('{newmember.Account}','{newmember.Password}','{newmember.Username}')";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region Hash密碼
        public string HashPassword(string password)
        {
            string saltkey = "qwertyuiop1234567890";
            string saltAndpassword = string.Concat(password, saltkey);
            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            byte[] passworddata=Encoding.Default.GetBytes(saltAndpassword);
            byte[] hashdata = sha256.ComputeHash(passworddata);
            string hashresult = Convert.ToBase64String(hashdata);
            return hashresult;
        }
        #endregion

        #region 查詢帳號
        private Member GetAccountData(string Account)
        {
            Member Data = new Member();
            string sql = $@"select * from MemberTest where Account = '{Account}'";
            try
            {
                conn.Open();
                SqlCommand cmd=new SqlCommand(sql, conn);
                SqlDataReader dr=cmd.ExecuteReader();
                dr.Read();
                Data.Account = dr["Account"].ToString();
                Data.Password = dr["Password"].ToString();
                Data.Username = dr["Username"].ToString();
            }
            catch
            {
                Data=null;
            }
            finally
            {
                conn.Close();
            }
            return Data;
        }
        #endregion

        #region 帳號重複確認
        public bool AccountCheck(string Account)
        {
            Member Data = GetAccountData(Account);
            if(Data!=null)
                return(false);
            else
                return(true);
        }
        #endregion

        #region 登入確認
        public string LoginCheck(string account,string password)
        {
            Member LoginMember = GetAccountData(account);
            if (LoginMember != null)
            {
                if (PasswordCheck(LoginMember, password)) { return LoginMember.Username; }                
                else { return "密碼錯誤"; }
            }
            else
                return "無此帳號";
        }
        #endregion

        #region 密碼確認
        public bool PasswordCheck(Member member, string password)
        {
            bool result = member.Password.Equals(HashPassword(password));
            return result;
        }
        #endregion
    }
}