using MvcTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace MvcTest.Helper
{
    public class Helper
    {
        public static void CheckIfImageIsEmpty(UserContext context)
        {           
            string path = AppDomain.CurrentDomain.BaseDirectory+ @"\\Images\\Portrait.png";
            foreach (var item in context.Users.ToList())
            {
                if(item.Avatar==null)
                    item.Avatar = File.ReadAllBytes(path);
            }
            context.SaveChanges();
        }
    }
}