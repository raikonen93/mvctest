using MvcTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcTest.Helpers;
using System.IO;


namespace MvcTest.Controllers
{
    public class HomeController : Controller
    {
        
        UserContext context = new UserContext();      
        
        public ActionResult NewProfile()
        {
            User us = new User();
            return PartialView(us);
        }

        public ActionResult Profile(int? id)
        {
            if (id == 0)
            {
                User us = new User();
                return PartialView("NewProfile",us);
            }
            Helper.CheckIfImageIsEmpty(context);
            User user = null;
            if(id==null)
            user = context.Users.Where(t=>t.IsEnabled).ToList().LastOrDefault();
            else
                user = context.Users.Where(t=>t.Id==id).ToList().FirstOrDefault();

            return View(user);
        }

        public ActionResult UserRole(int? id)
        {
            User user = null;
            if (id == null)
                user = context.Users.ToList().LastOrDefault();
            else
                user = context.Users.Where(t => t.Id == id).ToList().FirstOrDefault();
            return View(user);
        }
        public ActionResult Settings(int? id)
        {
            User user = null;
            if (id == null)
                user = context.Users.ToList().LastOrDefault();
            else
                user = context.Users.Where(t => t.Id == id).ToList().FirstOrDefault();
            return View(user);
        }

        public ActionResult AsideBar(string elemscount, string text, string bold)
        {
            Helper.CheckIfImageIsEmpty(context);
            var usersList = Helper.FilteringAsideBar(elemscount, text, bold,context);
            return PartialView(usersList);
        }

        public void RefreshSettings(int? id, string name,  string email, string skype, string signature)
        {
            User us;
            if (id == null || id==0)
            {
                us= new User { Name = name, Email = email, Skype = skype, Signature = signature, UserRole = new UserRole { Role = Roles.User.ToString() }, IsEnabled = true };
                if (us.Avatar == null)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + @"\\Images\\Portrait.png";
                    us.Avatar= System.IO.File.ReadAllBytes(path);
                }
                context.Users.Add(us);
            }
            else
            {
                us = context.Users.Where(t => t.Id == id).FirstOrDefault();
                if (us != null)
                {
                    us.Name = name;
                    us.Email = email;
                    us.Skype = skype;
                    us.Signature = signature;
                }
            }
            context.SaveChanges();
           
        }

        public JsonResult CreateNewProfile(int? id, string name, string email, string skype, string signature)
        {
            User us;
            if (id == null || id == 0)
            {
                us = new User { Name = name, Email = email, Skype = skype, Signature = signature, UserRole = new UserRole { Role = Roles.User.ToString() }, IsEnabled = true };
                if (us.Avatar == null)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + @"\\Images\\Portrait.png";
                    us.Avatar = System.IO.File.ReadAllBytes(path);
                }
                context.Users.Add(us);
            }
            else
            {
                us = context.Users.Where(t => t.Id == id).FirstOrDefault();
                if (us != null)
                {
                    us.Name = name;
                    us.Email = email;
                    us.Skype = skype;
                    us.Signature = signature;
                }
            }
            context.SaveChanges();
            return Json(us.Id);
        }

        public ActionResult Searching(string elemscount, string text, string bold)
        {
            var usersList = Helper.FilteringAsideBar(elemscount,text, bold,context);
            return PartialView("AsideBar",usersList);
        }

       

        public ActionResult GetImage(int id)
        {
            var user = context.Users.Where(t => t.Id == id).ToList().FirstOrDefault();
            byte[] imageData = new byte[0];
            if (user != null)
            {
                if(user.Avatar==null)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + @"\\Images\\Portrait.png";                   
                    user.Avatar = System.IO.File.ReadAllBytes(path);                    
                }

                imageData = user.Avatar;
            }
            context.SaveChanges();
            return new FileStreamResult(new System.IO.MemoryStream(imageData), "image/jpeg");
        }

        public void UploadFiles(int? id)
        {            
            if (Request.Files.Count > 0)
            {
                try
                {                   
                    HttpFileCollectionBase files = Request.Files;

                    var file = files[0];
                    byte[] data;
                    using (Stream inputStream = file.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        data = memoryStream.ToArray();
                    }
                    if (id == null)
                        return;
                    var user = context.Users.Where(t => t.Id == id).FirstOrDefault();
                    if (user != null)
                        user.Avatar = data;
                    context.SaveChanges();
                    
                }
                catch (Exception ex)
                {
                   
                }
            }           
        }

        public void EnabledOrDisabledChanging(string id,string state)
        {
            if (!string.IsNullOrEmpty(id))
            {
                User user = context.Users.Where(t => t.Id.ToString() == id).FirstOrDefault();
                if (user != null)
                {
                    if (state == "checked")
                        user.IsEnabled = true;
                    else
                        user.IsEnabled = false;
                    context.SaveChanges();
                }
            }
        }

        public void UserRoleChanging(string id, Roles role)
        {
            if (!string.IsNullOrEmpty(id))
            {
                User user = context.Users.Where(t => t.Id.ToString() == id).FirstOrDefault();
                if (user != null)
                {
                    user.UserRole.Role = role.ToString();
                    context.SaveChanges();
                }
            }
        }
    }
}