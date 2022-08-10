using Incedo_Octavius_Demo_2.Data;
using Incedo_Octavius_Demo_2.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Incedo_Octavius_Demo_2.Controllers
{
    public class KOL_ImageController : Controller
    {
        private Incedo_Octavius_Demo_2_kol_degree_map_table_Context db = new Incedo_Octavius_Demo_2_kol_degree_map_table_Context();
        ProfileStatusModel model = new ProfileStatusModel();
        List<ProfileStatusModel> profiles = new List<ProfileStatusModel>();

        int chosenProfileID;
        List<int> KOL_Count = new List<int>();

        [ChildActionOnly]
        public ActionResult RenderProfile()
        {
            return PartialView("ProfileIndex");
        }

        // GET: ProfileStatus
        public ActionResult ProfileIndex()
        {
            Console.WriteLine("Inside profileIndex");
            //KOL_With_Degree_List kolList = new KOL_With_Degree_List();
            string constr = ConfigurationManager.ConnectionStrings["Incedo_Octavius_Demo_2_kol_table_Context"].ConnectionString;
            GetProfiles();
            

            for (int i = profiles.Count; i > 0; i--)
            {

                using (MySqlConnection dbConnection = new MySqlConnection(constr))
                {
                    try
                    {
                        dbConnection.Open();
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = dbConnection;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "ProfileCount";
                        cmd.Parameters.AddWithValue("id", i-1);
                        //cmd.ExecuteReader();

                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
                        DataSet dataSetObject = new DataSet();
                        dataAdapter.Fill(dataSetObject);

                        //ViewData[i.ToString()] = Convert.ToInt32(dataSetObject.Tables[0].Rows[0]["kolCount"]);
                        KOL_Count.Add(Convert.ToInt32(dataSetObject.Tables[0].Rows[0]["kolCount"]));
                        //profiles.
                    }
                    catch (Exception Ex)
                    {

                        Console.WriteLine("Error : " + Ex.Message);
                    }

                }
            }
            
            ViewBag.KOLCount = KOL_Count;
            return View(profiles);
        }

        // GET: KOL_Image
        public ActionResult Index()
        {
            Console.WriteLine("Inside Index GEt");
            int profile = 2;
            List<KOL_Image> kolNameImageList = new List<KOL_Image>();
            string constr = ConfigurationManager.ConnectionStrings["Incedo_Octavius_Demo_2_kol_table_Context"].ConnectionString;
            GetProfiles();
            SetProfileId(profile);
            ViewBag.Profiles = profiles;
            ViewBag.Profile = profiles[chosenProfileID].ProfileStatus;
            //chosenProfileID = profile;
            // Stored Procedures
            using (MySqlConnection dbConnection = new MySqlConnection(constr))
            {
                try
                {
                    dbConnection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "KOL_Name_Image";
                    cmd.Parameters.AddWithValue("profileStatus",profile);
                    //cmd.ExecuteReader();

                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
                    DataSet dataSetObject = new DataSet();
                    dataAdapter.Fill(dataSetObject);

                    if (dataSetObject.Tables[0].Rows.Count > 0)
                    {
                        for (int iCout = 0; iCout < dataSetObject.Tables[0].Rows.Count; iCout++)
                        {
                            KOL_Image kolImage= new KOL_Image();
                            kolImage.kolID = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["kolID"]);
                            kolImage.First_Name = dataSetObject.Tables[0].Rows[iCout]["First_Name"].ToString();
                            kolImage.Last_Name = dataSetObject.Tables[0].Rows[iCout]["Last_Name"].ToString();
                            kolImage.Image_URL = dataSetObject.Tables[0].Rows[iCout]["Image_Link"].ToString();

                            kolNameImageList.Add(kolImage);
                        }
                    }
                }
                catch (Exception Ex)
                {

                    Console.WriteLine("Error : " + Ex.Message);
                }

            }
            //profiles = ViewBag.Profiles;
            //ViewBag.Profile = profiles[chosenProfileID].ProfileStatus;
            return View(kolNameImageList);
        }

        [HttpPost]
        public ActionResult Index(int profile)
        {
            Console.WriteLine("Inside index post");
            List<KOL_Image> kolNameImageList = new List<KOL_Image>();
            string constr = ConfigurationManager.ConnectionStrings["Incedo_Octavius_Demo_2_kol_table_Context"].ConnectionString;
            GetProfiles();
            SetProfileId(profile);
            ViewBag.Profiles = profiles;
            ViewBag.Profile = profiles[chosenProfileID].ProfileStatus;
            //chosenProfileID = profile;
            // Stored Procedures
            using (MySqlConnection dbConnection = new MySqlConnection(constr))
            {
                try
                {
                    dbConnection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "KOL_Name_Image";
                    cmd.Parameters.AddWithValue("profileStatus", profile);
                    //cmd.ExecuteReader();

                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
                    DataSet dataSetObject = new DataSet();
                    dataAdapter.Fill(dataSetObject);

                    if (dataSetObject.Tables[0].Rows.Count > 0)
                    {
                        for (int iCout = 0; iCout < dataSetObject.Tables[0].Rows.Count; iCout++)
                        {
                            KOL_Image kolImage = new KOL_Image();
                            kolImage.kolID = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["kolID"]);
                            kolImage.First_Name = dataSetObject.Tables[0].Rows[iCout]["First_Name"].ToString();
                            kolImage.Last_Name = dataSetObject.Tables[0].Rows[iCout]["Last_Name"].ToString();
                            kolImage.Image_URL = dataSetObject.Tables[0].Rows[iCout]["Image_Link"].ToString();

                            kolNameImageList.Add(kolImage);
                        }
                    }
                }
                catch (Exception Ex)
                {

                    Console.WriteLine("Error : " + Ex.Message);
                }

            }
            //profiles = ViewBag.Profiles;
            //ViewBag.Profile = profiles[chosenProfileID].ProfileStatus;
            return View(kolNameImageList);
        }
         
        public void GetProfiles()
        {
            string constr = ConfigurationManager.ConnectionStrings["Incedo_Octavius_Demo_2_kol_table_Context"].ConnectionString;
            List<ProfileStatusModel> innerProfiles = new List<ProfileStatusModel>();
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "SELECT * FROM octavius_db.profile_status_master_table order by ProfileStatusID desc";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            //string name = sdr["first_name"].ToString();

                            innerProfiles.Add(new ProfileStatusModel
                            {
                                ProfileStatusID = Convert.ToInt32(sdr["ProfileStatusID"]),
                                ProfileStatus = sdr["ProfleStatus"].ToString(),

                            });

                        }
                    }
                    con.Close();
                }
            }
            profiles = innerProfiles;
        }

        public void SetProfileId(int id)
        {
            if(id==0)
            {
                id = 2;
            }
            else if(id==2)
            {
                id = 0;
            }
            chosenProfileID = id;
        }
    }
}