using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [Authorize]
        public ActionResult Index()
        {
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                Session["id"] = db.Users.Where(u => u.EmailID == User.Identity.Name).FirstOrDefault().UserID;
                var id = Convert.ToInt32(Session["id"]);
                UserData usrD = new UserData
                {
                    ProfileImage = db.UserDatas.Where(x => x.Id == id).FirstOrDefault().ProfileImage
                };
                ViewBag.Pic = usrD;
                

                User usr = new User
                {   
                    FirstName = db.Users.Where(f => f.UserID == id).FirstOrDefault().FirstName,
                    LastName = db.Users.Where(f => f.UserID == id).FirstOrDefault().LastName
                };
                ViewBag.Name = usr;
                
            }
            return View();
        }

        public ActionResult Fullname()
        {
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                var FirstName = db.Users.Where(f => f.UserID == Convert.ToInt32(Session["id"])).FirstOrDefault().FirstName;
                var LastName = db.Users.Where(f => f.UserID == Convert.ToInt32(Session["id"])).Select(f => f.LastName);
                var FullName = FirstName + " " + LastName;
                return View(FullName);
            }
            //return View();
        }

        public ActionResult Search(string search)
        {
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                var id = Convert.ToInt32(Session["id"]);
                User usr = new User
                {
                    FirstName = db.Users.Where(f => f.UserID == id).FirstOrDefault().FirstName,
                    LastName = db.Users.Where(f => f.UserID == id).FirstOrDefault().LastName
                };
                ViewBag.Name = usr;

                UserData usrD = new UserData
                {
                    ProfileImage = db.UserDatas.Where(x => x.Id == id).FirstOrDefault().ProfileImage
                };
                ViewBag.Pic = usrD;

                if (search.Contains(' '))
                {
                    string[] name = search.Split(' ');
                    string name1 = name[0];
                    string name2 = name[1];
                    return View(db.Users.Where(x => x.FirstName.StartsWith(name1) && x.LastName.StartsWith(name2) || x.LastName.StartsWith(name1) && x.FirstName.StartsWith(name2) || search == null).ToList());
                }
                else
                {
                    return View(db.Users.Where(x => x.FirstName.StartsWith(search) || x.LastName.StartsWith(search) || search == null).ToList());
                }
                
                //return View(db.Users.ToList());
            }
            
        }

        public ActionResult Chat()
        {
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                Session["id"] = db.Users.Where(u => u.EmailID == User.Identity.Name).FirstOrDefault().UserID;
                var id = Convert.ToInt32(Session["id"]);
                UserData usrD = new UserData
                {
                    ProfileImage = db.UserDatas.Where(x => x.Id == id).FirstOrDefault().ProfileImage
                };
                ViewBag.Pic = usrD;


                User usr = new User
                {
                    FirstName = db.Users.Where(f => f.UserID == id).FirstOrDefault().FirstName,
                    LastName = db.Users.Where(f => f.UserID == id).FirstOrDefault().LastName
                };
                ViewBag.Name = usr;

            }
            return View("Chat");
        }

    }
}