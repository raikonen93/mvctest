using MvcTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcTest.Helper;
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
            Helper.Helper.CheckIfImageIsEmpty(context);
            User user = null;
            if(id==null)
            user = context.Users.ToList().LastOrDefault();
            else
                user = context.Users.Where(t=>t.Id==id).ToList().FirstOrDefault();

            return View(user);
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
            ViewBag.CurrentPage = "Settings";
            User user = null;
            if (id == null)
                user = context.Users.ToList().LastOrDefault();
            else
                user = context.Users.Where(t => t.Id == id).ToList().FirstOrDefault();
            return View(user);
        }

        public ActionResult AsideBar(string elemscount, string text, string bold)
        {
            var usersList = FilteringAsideBar(elemscount, text, bold);

           Helper.Helper.CheckIfImageIsEmpty(context);
            return PartialView(usersList);
        }
        [HttpPost]
        public ActionResult ProfileForm()
        {
            return View();
        }

        public ActionResult Searching(string elemscount, string text, string bold)
        {
            var usersList = FilteringAsideBar(elemscount,text, bold);
            return PartialView("AsideBar",usersList);
        }

        public List<User> FilteringAsideBar(string elemscount,string text, string bold)
        {

            bool state;
            int visibleUsersCount = 15;
            if (bold == null)
                state = true;
            else
                state = bold == "Enabled" ? true : false;
           
            if (!string.IsNullOrEmpty(elemscount) && elemscount!="0")
                visibleUsersCount = int.Parse(elemscount);

            List<User> usersList;
            
            if (!string.IsNullOrEmpty(text))
               return usersList = context.Users.Where(t => (t.Name.ToUpper().Contains(text.ToUpper()) && (t.IsEnabled == state))).Take(visibleUsersCount).OrderByDescending(t=>t.Id).ToList();
            else
            {
               return usersList = context.Users.Where(t => t.IsEnabled == state).Take(visibleUsersCount).OrderByDescending(t => t.Id).ToList();
            }
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
            //instead of what augi wrote using the binarystreamresult this was the closest thing i found so i am assuming that this is what it evolved into 
            return new FileStreamResult(new System.IO.MemoryStream(imageData), "image/jpeg");
        }

        public void UploadFiles(int id)
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
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