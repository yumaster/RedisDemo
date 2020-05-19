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
using RedisStudy.Services.Service;

namespace WebApp.Controllers
{
    public class FunctionsController : Controller
    {
        private FunService demoService = new FunService();
        public ActionResult Index()
        {
            return View(demoService.GetList());
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Function fun = demoService.GetEntity(id);
            if (fun == null)
            {
                return HttpNotFound();
            }
            return View(fun);
        }

        public ActionResult Create()
        {
            return View();
        }

        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FunName,FunType,IsUsed")] Function fun)
        {
            if (ModelState.IsValid)
            {
                demoService.SaveForm(fun);
                return RedirectToAction("Index");
            }
            return View(fun);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Function fun = demoService.GetEntity(id);
            if (fun == null)
            {
                return HttpNotFound();
            }
            return View(fun);
        }

        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FunName,FunType,IsUsed")] Function fun)
        {
            if (ModelState.IsValid)
            {
                demoService.SaveForm(fun);
                return RedirectToAction("Index");
            }
            return View(fun);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Function fun = demoService.GetEntity(id);
            if (fun == null)
            {
                return HttpNotFound();
            }
            return View(fun);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            demoService.DeleteForm(id);
            return RedirectToAction("Index");
        }
    }
}
