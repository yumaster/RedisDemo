using RedisStudy.DAL.Abstraction.Models;
using RedisStudy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class HomeBackController : Controller
    {
        private UserService demoService = new UserService();

        private UserCache userCache = new UserCache();
        public ActionResult Index()
        {
            return View(userCache.GetList());
        }
        public ActionResult Edit(string id)
        {
            User user = demoService.GetEntity(id);
            user.Password = new Random().Next(100, 999).ToString();
            demoService.SaveForm(user);
            userCache.Remove();
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}