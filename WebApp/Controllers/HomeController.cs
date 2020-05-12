using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RedisStudy.DAL.Abstraction.Models;
using RedisStudy.DAL.EF;
using RedisStudy.Services;
using RedisCache;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private UserService demoService = new UserService();
        private UserCache userCache = new UserCache();


        //private EFDbContext db = new EFDbContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(userCache.GetList());
        }


        //public ActionResult Index()
        //{
        //    //var list = new List<User>();

        //    //using (var tran = RedisManager.GetClient().CreateTransaction())
        //    //{
        //    //    try
        //    //    {
        //    //        tran.QueueCommand(p => {
        //    //            list = new DoRedisHash().GetHashToListCache<User>("UserCache");
        //    //        });
        //    //        tran.Commit();
        //    //    }
        //    //    catch
        //    //    {
        //    //        tran.Rollback();
        //    //    }
        //    //}

        //    var list = redisHash.GetHashToListCache<User>("UserCache");

        //    return View(list);
        //}


        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //User user = db.User.Find(id);
            User user = demoService.GetEntity(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                //db.User.Add(user);
                //db.SaveChanges();
                demoService.SaveForm(user);

                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //User user = db.User.Find(id);
            User user = demoService.GetEntity(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(user).State = EntityState.Modified;
                //db.SaveChanges();
                demoService.SaveForm(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //User user = db.User.Find(id);
            User user = demoService.GetEntity(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            //User user = db.User.Find(id);
            //db.User.Remove(user);
            //db.SaveChanges();

            demoService.DeleteForm(id);
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
