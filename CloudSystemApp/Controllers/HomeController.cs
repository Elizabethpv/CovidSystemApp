using CloudSystemApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudSystemApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginView(Person person)
        {

            string strcon = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
            SqlConnection connect = new SqlConnection(strcon);
            connect.Open();
            SqlCommand cmd = new SqlCommand("AccountLogin", connect);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserName", person.UserName);
            cmd.Parameters.AddWithValue("@Password", person.Password);
            cmd.Parameters.AddWithValue("@UserType", person.UserType);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            String dataSetUserName;
            String dataSetPassword;

            if (ds.Tables[0].Rows.Count > 0)
            {

                dataSetUserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                dataSetPassword = ds.Tables[0].Rows[0]["Password"].ToString();

                if (dataSetUserName == person.Email && dataSetPassword == person.Password)
                {

                    return View("UserAccountRegistration");
                }
                else
                {
                    person.Message = "Invalid Login";
                }
            }
            else
            {
                person.Message = "Invalid UserName and Password";
            }

            return View("UserAccountRegistration");
        }
        public ActionResult UserRegistration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserAccountRegistration(Person person)
        {

            string strcon = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
            SqlConnection connect = new SqlConnection(strcon);
            connect.Open();
            SqlCommand cmd = new SqlCommand("UserAccountSave", connect);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", person.Name);
            cmd.Parameters.AddWithValue("@Address", person.Address);
            cmd.Parameters.AddWithValue("@AdharNumber", person.AdharNumber);
            cmd.Parameters.AddWithValue("@DateOfBirth", person.DateOfBirth);
            cmd.Parameters.AddWithValue("@UserName", person.UserName);
            cmd.Parameters.AddWithValue("@Password", person.Password);
            cmd.ExecuteNonQuery();
            connect.Close();
            return View("UserVaccinationRegistration");
        }
        public ActionResult UserVaccinationRegistration()
        { 
            return View();
        }
        public ActionResult VaccinRegistration(Vaccine vaccine, Hospital hospital)
        {
            
            string strcon = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
            SqlConnection connect = new SqlConnection(strcon);
            connect.Open();
            SqlCommand cmd = new SqlCommand("", connect);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Dose", vaccine.Dose);
            

            SqlCommand cmmd = new SqlCommand("", connect);
            cmmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmmd.ExecuteReader();
            while (reader.Read())
            {
                
               hospital.Name = reader["Name"].ToString();
               
            }
                connect.Close();
            return View("UserVaccinationRegistration");
        }
    }
}