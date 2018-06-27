using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers
{
    public class UserController : Controller
    {
        //Registration Action
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        //Register POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(User user, UserData userD)
        {
            bool Status = false;
            string message = "";

            // Model validation
            if (ModelState.IsValid)
            {

                #region // Email is in use
                var ifExists = IfEmailExists(user.EmailID);
                if (ifExists)
                {
                    ModelState.AddModelError("EmailExists", "Email is alredy in use!");
                    return View(user);
                }
                #endregion

                #region // Password hashing
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                #endregion

                #region // Save data to db
                using (MyDatabaseEntities dc = new MyDatabaseEntities())
                {
                    dc.Users.Add(user);
                    dc.SaveChanges();
                    userD.Id = user.UserID;
                    userD.ProfileImage = "~/Image/facebook-avatar184953217.jpg";
                    dc.UserDatas.Add(userD);
                    dc.SaveChanges();
                    //send verification email

                    message = "Registration successfully done!";
                    Status = true;

                }
                #endregion
            }
            else
            {
                message = "Invalid request!";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(user);
        }


        //Verify Email

        //Verify Email link

        //Login
        [HttpGet]
        public  ActionResult Login()
        {
            return View();
        }


        //Login post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl)
        {
            string message = "";
            using(MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var v = dc.Users.Where(a => a.EmailID == login.EmailID).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(Crypto.Hash(login.Password),v.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20; // 525600 -> 1 an
                        var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            Session["id"] = dc.Users.Where(u => u.EmailID == login.EmailID).FirstOrDefault().UserID;
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        message = " Invalid username or password!";
                    }
                }
                else
                {
                    message = " Invalid username or password!";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }
        



        //smth
        [NonAction]
        public bool IfEmailExists(string emailID)
        {
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var v = dc.Users.Where(a => a.EmailID == emailID).FirstOrDefault();
                return v != null;
            }

        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            var verifyUrl = "User/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("thecrewsocialnetwork", "The Crew Social Network");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "the@crew@socialnetwork"; // maybe?
            string subject = "Your account is successfully created!";

            string body = "<br/><br/>We are exited to tell you that your account on The Crew Social Network is" +
                "successfully createt! Please click on the link below to verify your account" +
                "<br/><br/><a href='"+link+"'>'"+link+"</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            })

                smtp.Send(message);


        }
    }


}