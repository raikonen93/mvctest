using MvcTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace MvcTest.Helpers
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

        public static List<User> FilteringAsideBar(string elemscount, string text, string bold, UserContext context)
        {
            bool state;
            int visibleUsersCount = 20;
            if (bold == null)
                state = true;
            else
                state = bold == "Enabled" ? true : false;

            if (!string.IsNullOrEmpty(elemscount) && elemscount != "0")
                visibleUsersCount = int.Parse(elemscount)+3;

            List<User> usersList;

            if (!string.IsNullOrEmpty(text))
                return usersList = context.Users.Where(t => (t.Name.ToUpper().Contains(text.ToUpper()) && (t.IsEnabled == state))).Take(visibleUsersCount).OrderByDescending(t => t.Id).ToList();
            else
            {
                return usersList = context.Users.Where(t => t.IsEnabled == state).Take(visibleUsersCount).OrderByDescending(t => t.Id).ToList();
            }
        }
    }
}