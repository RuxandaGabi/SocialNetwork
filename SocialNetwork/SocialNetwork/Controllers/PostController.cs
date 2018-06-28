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




        public FileResult Download(string location)
        {
            //string fileName = @"C:\Licenta\SocialNetwork\SocialNetwork"+location;
            //string fileName = @"C:\Licenta\SocialNetwork\SocialNetwork\Image\sub184727216.txt";
            //string fileName = @"~\Image\sub184727216.txt";
            //string contentType = "text/plain";
            //"application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            //return new FilePathResult(fileName, contentType);
            return new FilePathResult(Server.MapPath(location), "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        }

        [HttpPost]
        public ActionResult _AddPost(Post post, HttpPostedFileBase files)
        {
            post.UserID = Convert.ToInt32(Session["id"]);
            post.DatePosted = DateTime.Now;


            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    string imgFileName = Path.GetFileNameWithoutExtension(post.ImageFile.FileName);
                    string imgExtension = Path.GetExtension(post.ImageFile.FileName).ToLower();

                    if (imgExtension == ".png" || imgExtension == ".jpeg" || imgExtension == ".jpg")
                    {
                        imgFileName = imgFileName + DateTime.Now.ToString("yymmssfff") + imgExtension;
                        post.ImagePath = "~/Image/" + imgFileName;
                        imgFileName = Path.Combine(Server.MapPath("~/Image/"), imgFileName);
                        post.ImageFile.SaveAs(imgFileName);
                    }
                    else
                    {
                        imgFileName = imgFileName + DateTime.Now.ToString("yymmssfff") + imgExtension;
                        post.OtherFilePath = "~/Image/" + imgFileName;
                        imgFileName = Path.Combine(Server.MapPath("~/Image/"), imgFileName);
                        post.ImageFile.SaveAs(imgFileName);


                        //string otherFileName = Path.GetFileNameWithoutExtension(post.OtherFile.FileName);
                        //string otherExtension = Path.GetExtension(post.OtherFile.FileName).ToLower();
                        //otherFileName = otherFileName + DateTime.Now.ToString("yymmssfff") + otherExtension;
                        //post.OtherFilePath = "~/OtherFiles/" + otherFileName;
                        //otherFileName = Path.Combine(Server.MapPath("~/OtherFiles/"), otherFileName);
                        //post.OtherFile.SaveAs(otherFileName);

                    }
                    //fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    //post.ImagePath = "~/Image/" + fileName;
                    //fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                    //post.ImageFile.SaveAs(fileName);
                }
            }

            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                string FirstName = db.Users.Where(f => f.UserID == post.UserID).FirstOrDefault().FirstName;
                string LastName = db.Users.Where(f => f.UserID == post.UserID).FirstOrDefault().LastName;
                post.UserIdName = FirstName + " " + LastName;
                post.UserPicture = db.UserDatas.Where(f => f.Id == post.UserID).FirstOrDefault().ProfileImage;

                db.Posts.Add(post);
                db.SaveChanges();
            }
            ModelState.Clear();

            return RedirectToAction("Index", "Home");
        }


        //[HttpGet]
        public ActionResult _ViewPost()
        {
            using (MyDatabaseEntities db = new MyDatabaseEntities()){
                

                return View(db.Posts.ToList().OrderByDescending(c => c.DatePosted));
            }
        }


        [HttpGet]
        public ActionResult ViewPost(int id)
        {
            Post post = new Post();
            var userID = Convert.ToInt32(Session["id"]);
            bool exist;


            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                post = db.Posts.Where(x => x.PostID == id).FirstOrDefault();
                exist = db.Likes.Any(x => x.PostId == id && x.UserId == userID);
                if (exist == false)
                {
                    ViewBag.Like = false;
                }
                else
                {
                    ViewBag.Like = true;
                }

                post.Likes = db.Likes.Count(x => x.PostId == id);
                db.Entry(post).Property("Likes").IsModified = true;
                db.SaveChanges();


                post.Comments = db.Comments.Count(x => x.PostId == id);
                db.Entry(post).Property("Comments").IsModified = true;
                db.SaveChanges();
            }

            return View(post);
        }


        public ActionResult _LastPosts(int id)
        {
            id = Convert.ToInt32(Session["iduser"]);
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                UserData userDat = new UserData
                {

                    ProfileImage = db.UserDatas.Where(f => f.Id == id).FirstOrDefault().ProfileImage,
                    
                };
                ViewBag.Picture = userDat;


                return View(db.Posts.Where(d => d.UserID == id).ToList().OrderByDescending(c => c.DatePosted));
            }
        }


        public ActionResult Test()
        {
            //var asd = Temp;
            ViewBag.Test = "Ana are prune";
           // Session["1"] = sTest.ToString();
            return View();
        }

        public ActionResult Like(int postId)
        {
            var Uid = Convert.ToInt32(Session["id"]);
            var Pid = postId;

            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                Like like = new Like()
                {
                    UserId = Uid,
                    PostId = Pid
                };
                db.Likes.Add(like);
                db.SaveChanges();


            }
            return RedirectToAction("ViewPost", "Post", new { id = postId });
        }

        public ActionResult DisLike(int postId)
        {
            var Uid = Convert.ToInt32(Session["id"]);
            var Pid = postId;

            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {

                Like like = new Like();
                like = db.Likes.Where(x => x.PostId == Pid && x.UserId == Uid).FirstOrDefault();

                db.Likes.Remove(like);
                db.SaveChanges();

            }
            return RedirectToAction("ViewPost", "Post", new { id = postId });
        }


        public ActionResult _Comment(int postId)
        {
           // id = Convert.ToInt32(Session["iduser"]);
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                //UserData userDat = new UserData
                //{

                //    ProfileImage = db.UserDatas.Where(f => f.Id == id).FirstOrDefault().ProfileImage,

                //};
                //ViewBag.Picture = userDat;

                //User user = new User()
                //{

                //}


                return View(db.Comments.Where(x => x.PostId == postId).ToList().OrderByDescending(x => x.DatePosted));
            }
            //return View();
        }
        [HttpGet]
        public ActionResult Comment()
        {
            return View();
        }

        //[HttpPost]
        public ActionResult Comment(Comment comment)
        {
            //var pID = postId;
            var UsrID = Convert.ToInt32(Session["id"]);
            var pID = Convert.ToInt32(Session["postID"]);


            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                var fname = db.Users.Where(x => x.UserID == UsrID).FirstOrDefault().FirstName;
                var lname = db.Users.Where(x => x.UserID == UsrID).FirstOrDefault().LastName;


                comment.UserId = UsrID;
                comment.PostId = pID;
                comment.DatePosted = DateTime.Now;
                comment.PosterName = fname + " " + lname;

                db.Comments.Add(comment);
                db.SaveChanges();
            }

            return RedirectToAction("ViewPost", "Post", new { id = pID });
        }

    }
}