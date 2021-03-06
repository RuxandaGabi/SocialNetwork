﻿using SocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
            var idx = Convert.ToInt32(Session["id"]);
            
            var iduser = Convert.ToInt32(Session["iduser"]);
            bool exist;
            

            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                UserData usrD = new UserData()
                {
                    ProfileImage = db.UserDatas.Where(x => x.Id == id).FirstOrDefault().ProfileImage
                };

                exist = db.Friends.Any(x => x.UserId1 == idx && x.UserId2 == iduser || x.UserId1 == iduser && x.UserId2 == idx);
                if (exist == true)
                {
                    ViewBag.Status = db.Friends.Where(x => x.UserId1 == idx && x.UserId2 == iduser || x.UserId1 == iduser && x.UserId2 == idx).FirstOrDefault().Status;
                }
                else
                {
                    ViewBag.Status = 0;
                }



                user = db.Users.Where(x => x.UserID == id).FirstOrDefault();
                ViewBag.Pic = usrD;
            }



            return View(user);
        }

        [HttpGet]
        public ActionResult Bio(int id)
        {
            User user = new User();

            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                UserData usrD = new UserData()
                {
                    ProfileImage = db.UserDatas.Where(x => x.Id == id).FirstOrDefault().ProfileImage
                };
                user = db.Users.Where(x => x.UserID == id).FirstOrDefault();
                ViewBag.Pic = usrD;
            }

            return View(user);
        }

        [HttpGet]
        public ActionResult Friends(int id)
        {
            User user = new User();
           
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                UserData usrD = new UserData()
                {
                    ProfileImage = db.UserDatas.Where(x => x.Id == id).FirstOrDefault().ProfileImage
                };
                user = db.Users.Where(x => x.UserID == id).FirstOrDefault();
                ViewBag.Pic = usrD;

            }
            return View(user);
        }

        [HttpGet]
        public ActionResult _Friends(int id)
        {
            bool exist;

            id = Convert.ToInt32(Session["iduser"]);
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                exist = db.Friends.Any(x => x.UserId1 == id || x.UserId2 == id);
                if (exist == true)
                {
                    return View(db.Friends.Where(x => x.UserId1 == id && x.UserId2 != id || x.UserId1 != id && x.UserId2 == id).ToList());
                }
                
            }
            return View();
        }

        [HttpGet]
        public ActionResult Uploads(int id)
        {
            User user = new User();

            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                UserData usrD = new UserData()
                {
                    ProfileImage = db.UserDatas.Where(x => x.Id == id).FirstOrDefault().ProfileImage
                };
                user = db.Users.Where(x => x.UserID == id).FirstOrDefault();
                ViewBag.Pic = usrD;
            }

            return View(user);
        }

        [HttpGet]
        public ActionResult _Uploads(int id)
        {
            id = Convert.ToInt32(Session["iduser"]);
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                return View(db.Posts.Where(x => x.UserID == id && x.OtherFilePath != null).ToList().OrderByDescending(c => c.DatePosted));
            }
        }


        [HttpGet]
        public ActionResult Gallery(int id)
        {
            User user = new User();

            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                UserData usrD = new UserData()
                {
                    ProfileImage = db.UserDatas.Where(x => x.Id == id).FirstOrDefault().ProfileImage
                };
                user = db.Users.Where(x => x.UserID == id).FirstOrDefault();
                ViewBag.Pic = usrD;
            }

            return View(user);
        }

        [HttpGet]
        public ActionResult _Gallery(int id)
        {
            id = Convert.ToInt32(Session["iduser"]);
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                return View(db.Posts.Where(x => x.UserID == id && x.ImagePath != null).ToList().OrderByDescending(c => c.DatePosted));
            }
        }

        [HttpGet]
        public ActionResult Picture(int id)
        {
            Post post = new Post();

            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                post = db.Posts.Where(x => x.PostID == id).FirstOrDefault();
            }

            return View(post);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Gallery()
        {
            //using (MyDatabaseEntities db = new MyDatabaseEntities())
            //{
            //    Session["id"] = db.Users.Where(u => u.EmailID == User.Identity.Name).FirstOrDefault().UserID;
            //    var id = Convert.ToInt32(Session["id"]);
            //    UserData usrD = new UserData
            //    {
            //        ProfileImage = db.UserDatas.Where(x => x.Id == id).FirstOrDefault().ProfileImage
            //    };
            //    ViewBag.Pic = usrD;
            //}
            try { 
                int id = Convert.ToInt32(Session["id"]);
                string image = Session["img"].ToString();
                using (MyDatabaseEntities db = new MyDatabaseEntities())
                {
                    var user = db.UserDatas.Where(x => x.Id == id).FirstOrDefault();
                    
                    user.ProfileImage = image;
                    foreach(var item in db.Posts)
                    {
                        if(item.UserID == id)
                        {
                            item.UserPicture = image;
                            db.Entry(item).Property("UserPicture").IsModified = true;
                            //db.SaveChanges();
                        }
                    }

                    db.Entry(user).Property("ProfileImage").IsModified = true;
                    
                    
                    db.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }


            return RedirectToAction("Index", "Home");
        }
    }
}