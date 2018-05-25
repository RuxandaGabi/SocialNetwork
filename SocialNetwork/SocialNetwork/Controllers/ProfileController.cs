using SocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialNetwork.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        [HttpGet]
        public ActionResult LastPosts(int id)
        {
            User user = new User();

            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                user = db.Users.Where(x => x.UserID == id).FirstOrDefault();
            }

            return View(user);
        }

        [HttpGet]
        public ActionResult Bio(int id)
        {
            User user = new User();

            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                user = db.Users.Where(x => x.UserID == id).FirstOrDefault();
            }

            return View(user);
        }

        [HttpGet]
        public ActionResult Gallery(int id)
        {
   
            //id = Convert.ToInt32(Session["iduser"]);

            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                return View(db.Posts.Where(x => x.UserID == id && x.ImagePath != null).ToList().OrderByDescending(c => c.DatePosted));
            }
        }
    }
}