using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers
{
    public class FriendController : Controller
    {
        [HttpGet]
        public ActionResult _AddFriend()
        {
            return View();
        }



        [HttpPost]
        public ActionResult _AddFriend(Friend friend)
        {
            var id = Convert.ToInt32(Session["id"]);
            var iduser = Convert.ToInt32(Session["iduser"]);


            if (id != iduser)
            {
                using (MyDatabaseEntities db = new MyDatabaseEntities())
                {

                    Friend beFriend = new Friend
                    {
                        UserId1 = id,
                        UserId2 = iduser,
                        Status = 1
                    };
                    //ViewBag.Status =  1;
                    db.Friends.Add(beFriend);
                    db.SaveChanges();
                        
                    
                }
            }
            return RedirectToAction("LastPosts","Profile", new { id = iduser});
        }

        [HttpGet]
        public ActionResult _CheckFriend()
        {
            var idx = Convert.ToInt32(Session["id"]);
            var iduser = Convert.ToInt32(Session["iduser"]);
            //Friend friend = new Friend();

            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {

                bool exist = db.Friends.Any(x => x.UserId1 == idx && x.UserId2 == iduser || x.UserId1 == iduser && x.UserId2 == idx);
                if (exist == true)
                {
                    Friend friend = new Friend()
                    {
                        UserId1 = db.Friends.Where(x => x.UserId1 == idx && x.UserId2 == iduser || x.UserId1 == iduser && x.UserId2 == idx).FirstOrDefault().UserId1,
                        UserId2 = db.Friends.Where(x => x.UserId1 == idx && x.UserId2 == iduser || x.UserId1 == iduser && x.UserId2 == idx).FirstOrDefault().UserId2

                    };
                    ViewBag.Friend = friend;


                    ViewBag.Status = db.Friends.Where(x => x.UserId1 == idx && x.UserId2 == iduser || x.UserId1 == iduser && x.UserId2 == idx).FirstOrDefault().Status;
                    //friend = db.Friends.Where(x => x.UserId1 == idx && x.UserId2 == iduser || x.UserId1 == iduser && x.UserId2 == idx).FirstOrDefault();

                    ViewBag.User1 = db.Friends.Where(x => x.UserId1 == idx && x.UserId2 == iduser || x.UserId1 == iduser && x.UserId2 == idx).FirstOrDefault().UserId1;
                    ViewBag.User2 = db.Friends.Where(x => x.UserId1 == idx && x.UserId2 == iduser || x.UserId1 == iduser && x.UserId2 == idx).FirstOrDefault().UserId2;

                }
                else
                {
                    ViewBag.Status = 0;
                }
            }

            return View();
        }


        [HttpPost]
        public ActionResult _CheckFriend(Friend friend)
        {
            var id = Convert.ToInt32(Session["id"]);
            var iduser = Convert.ToInt32(Session["iduser"]);


            if (id != iduser)
            {
                using (MyDatabaseEntities db = new MyDatabaseEntities())
                {
                    if (db.Friends.Where(x => x.UserId1 == id && x.UserId2 == iduser || x.UserId1 == iduser && x.UserId2 == id).FirstOrDefault().Status != null)
                    {
                        var status = db.Friends.Where(x => x.UserId1 == id && x.UserId2 == iduser || x.UserId1 == iduser && x.UserId2 == id).FirstOrDefault().Status;

                        //ViewBag.Status = status;

                        //if (status == 2)
                        //{
                        //    // do nothing
                        //    return RedirectToAction("LastPosts", "Profile", new { id = iduser });
                        //}
                        if (status == 1)
                        {

                            var friendship = db.Friends.Where(x => x.UserId1 == id && x.UserId2 == iduser || x.UserId1 == iduser && x.UserId2 == id).FirstOrDefault();

                            friendship.Status = 2;
                            db.Entry(friendship).Property("Status").IsModified = true;
                            //ViewBag.Status = 2;

                            db.SaveChanges();
                            return RedirectToAction("LastPosts", "Profile", new { id = iduser });
                        }
                    }
                }
            }
            return View();
        }


    }
}