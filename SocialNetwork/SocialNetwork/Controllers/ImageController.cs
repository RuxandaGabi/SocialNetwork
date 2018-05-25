using SocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialNetwork.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Picture imageModel)
        {
            string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            string extension = Path.GetExtension(imageModel.ImageFile.FileName);
            

            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imageModel.ImagePath = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            imageModel.ImageFile.SaveAs(fileName);
            imageModel.UserID = Convert.ToInt32(Session["id"]);
            imageModel.DatePosted = DateTime.Now;

            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                db.Pictures.Add(imageModel);
                db.SaveChanges();
            }
            ModelState.Clear();
          
            return View();
        }

        [HttpGet]
        public ActionResult View(int id)
        {
            Picture imageModel = new Picture();

            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                imageModel = db.Pictures.Where(x => x.PictureID == id).FirstOrDefault();
            }

            return View(imageModel);
        }

    }
}