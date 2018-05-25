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
                User usr = new User
                {
                    
                    FirstName = db.Users.Where(f => f.UserID == id).FirstOrDefault().FirstName,
                    LastName = db.Users.Where(f => f.UserID == id).FirstOrDefault().LastName
                };
                ViewBag.Name = usr;
                return View();
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
            return View();
        }
    }
}