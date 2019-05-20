using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Portal.DataLayer.DataContext;
using Portal.DomainClasses.Models;

namespace Portal.Areas.Tools.Controllers
{
    public class WeblogController : Controller
    {
        private PortalDb db = new PortalDb();

        //
        // GET: /Weblog/
        public ActionResult Index(int CurrentPageIndex = 1, int EachPageItems = 3)
        {
            ViewBag.CurrentPageIndex = CurrentPageIndex;
            ViewBag.LastPageIndex = Math.Ceiling((decimal)(db.WeblogPosts.ToList().Count / EachPageItems));
            return View(db.WeblogPosts.ToList().OrderByDescending(r => r.PostDateTime).Skip((CurrentPageIndex - 1) * EachPageItems).Take(EachPageItems));
        }

        //
        // GET: /Weblog/Details/5

        public ActionResult Details(int id = 0)
        {
            WeblogPost weblogpost = db.WeblogPosts.Find(id);
            if (weblogpost == null)
            {
                return HttpNotFound();
            }
            return View(weblogpost);
        }

        //
        // GET: /Weblog/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Weblog/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WeblogPost weblogpost)
        {
            //weblogpost.PostDateTime = DateTime.Now;
            weblogpost.PostDateTime = new Portal.CommonLibrary.PersianDateTime().GregorianToShamsi(DateTime.Now);
            weblogpost.Writer = User.Identity.Name;
            //weblogpost.PostContent = Server.HtmlEncode(weblogpost.PostContent);
            if (ModelState.IsValid)
            {
                db.WeblogPosts.Add(weblogpost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(weblogpost);
        }

        //
        // GET: /Weblog/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id = 0)
        {
            WeblogPost weblogpost = db.WeblogPosts.Find(id);
            if (weblogpost == null)
            {
                return HttpNotFound();
            }
            return View(weblogpost);
        }

        //
        // POST: /Weblog/Edit/5

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WeblogPost weblogpost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(weblogpost).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(weblogpost);
        }

        //
        // GET: /Weblog/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id = 0)
        {
            WeblogPost weblogpost = db.WeblogPosts.Find(id);
            if (weblogpost == null)
            {
                return HttpNotFound();
            }
            return View(weblogpost);
        }

        //
        // POST: /Weblog/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WeblogPost weblogpost = db.WeblogPosts.Find(id);
            db.WeblogPosts.Remove(weblogpost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Search(string searchTerm = null)
        {
            var model = db.WeblogPosts
                .OrderBy(r => r.PostDateTime)
                .Where(r => searchTerm == null || r.Writer.Equals(searchTerm));
            if (Request.IsAjaxRequest())
            {
                return PartialView("_SearchWeblog", model);
            }
            return View(model);
        }

        public ActionResult Autocomplete(string searchTerm = null)
        {
            var model = db.WeblogPosts
                .Where(r => r.Writer.StartsWith(searchTerm))
                .Take(100)
                .Select(r => new { label = r.Writer });
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Comment()
        {
            return View();
        }
	}
}