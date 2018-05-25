using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialNetwork.Models;
using System.IO;


namespace SocialNetwork.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        [HttpGet]
        public ActionResult _AddPost()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult _AddPost(Post post)
        {
            post.UserID = Convert.ToInt32(Session["id"]);
            post.DatePosted = DateTime.Now;

            if(Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(post.ImageFile.FileName);
                    string extension = Path.GetExtension(post.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    post.ImagePath = "~/Image/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                    post.ImageFile.SaveAs(fileName);
                }
            }

            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                db.Posts.Add(post);
                db.SaveChanges();
            }
            ModelState.Clear();

            return RedirectToAction("Index", "Home");
        }


        //[HttpGet]
        public ActionResult _ViewPost()
        {
            MyDatabaseEntities db = new MyDatabaseEntities();
            return View(db.Posts.ToList().OrderByDescending(c => c.DatePosted));
        }

        [HttpGet]
        public ActionResult ViewPost(int id)
        {
            Post post = new Post();

            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                post = db.Posts.Where(x => x.PostID == id).FirstOrDefault();
            }

            return View(post);
        }


        public ActionResult _LastPosts(int id)
        {
            id = Convert.ToInt32(Session["iduser"]);
            MyDatabaseEntities db = new MyDatabaseEntities();
            return View(db.Posts.Where(d => d.UserID == id).ToList().OrderByDescending(c => c.DatePosted));
        }


    }
}