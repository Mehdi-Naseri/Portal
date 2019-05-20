using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Portal.DomainClasses.Models;
using Portal.DataLayer.DataContext;

namespace Portal.Areas.Tools.Controllers
{
    public class WeblogCommentController : Controller
    {
        private PortalDb db = new PortalDb();

        // GET: /Tools/WeblogComment/
        public ActionResult Index(int WeblogPostId)
        {
            var weblogcomments = db.WeblogComments.Include(w => w.WeblogPost);
            var weblogcommentsFiltered = db.WeblogComments
                .Where(r => r.WeblogPost.Id == WeblogPostId);
            ViewBag.WeblogPostId = WeblogPostId;
            return View(weblogcommentsFiltered.ToList());
        }
        public ActionResult CommentsAll()
        {
            var weblogcomments = db.WeblogComments.Include(w => w.WeblogPost);
            return View(weblogcomments.ToList());
        }

        // GET: /Tools/WeblogComment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeblogComment weblogcomment = db.WeblogComments.Find(id);
            if (weblogcomment == null)
            {
                return HttpNotFound();
            }
            ViewBag.WeblogPostId = weblogcomment.WeblogPostId;
            return View(weblogcomment);
        }

        // GET: /Tools/WeblogComment/Create
        public ActionResult Create(int? WeblogPostId)
        {
            if (WeblogPostId == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            //ViewBag.WeblogId = new SelectList(db.WeblogPosts, "Id", "Title");
            ViewBag.WeblogPostId = WeblogPostId;
            return View();
        }

        // POST: /Tools/WeblogComment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Writer,Comment,WeblogPostId")] WeblogComment weblogcomment)
        {
            if (ModelState.IsValid)
            {
                weblogcomment.CommentDateTime = new Portal.CommonLibrary.PersianDateTime().GregorianToShamsi(DateTime.Now);
                db.WeblogComments.Add(weblogcomment);
                db.SaveChanges();
                return RedirectToAction("Index", new { WeblogPostId = weblogcomment.WeblogPostId });
            }

            //ViewBag.WeblogPostId = new SelectList(db.WeblogPosts, "Id", "Writer", weblogcomment.WeblogPostId);
            ViewBag.WeblogPostId = weblogcomment.WeblogPostId;
            return View(weblogcomment);
        }

        // GET: /Tools/WeblogComment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeblogComment weblogcomment = db.WeblogComments.Include(w => w.WeblogPost).Where(r => r.Id == id).SingleOrDefault();
            if (weblogcomment == null)
            {
                return HttpNotFound();
            }
            //ViewBag.WeblogPostId = new SelectList(db.WeblogPosts, "Id", "Writer", weblogcomment.WeblogPostId);
            //ViewBag.WeblogPostId = weblogcomment.WeblogPost.Id;
            //int i = weblogcomment.WeblogPost.Id;
            return View(weblogcomment);
        }

        // POST: /Tools/WeblogComment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CommentDateTime,Writer,Comment,Timestamp,WeblogPostId")] WeblogComment weblogcomment)
        {
            weblogcomment.WeblogPost = db.WeblogPosts.Find(weblogcomment.WeblogPostId);
            //Where(w => w.Id == weblogcomment.WeblogPostId);
            if (ModelState.IsValid)
            {
                db.Entry(weblogcomment).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { WeblogPostId = weblogcomment.WeblogPostId });
            }
            //ViewBag.WeblogPostId = new SelectList(db.WeblogPosts, "Id", "Writer", weblogcomment.WeblogPostId);
            //ViewBag.WeblogPostId = weblogcomment.WeblogPostId;
            return View(weblogcomment);
        }

        // GET: /Tools/WeblogComment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeblogComment weblogcomment = db.WeblogComments.Find(id);
            if (weblogcomment == null)
            {
                return HttpNotFound();
            }
            ViewBag.WeblogPostId = weblogcomment.WeblogPost.Id;
            return View(weblogcomment);
        }

        // POST: /Tools/WeblogComment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WeblogComment weblogcomment = db.WeblogComments.Find(id);
            int WeblogPostId = weblogcomment.WeblogPost.Id;
            db.WeblogComments.Remove(weblogcomment);
            db.SaveChanges();
            return RedirectToAction("Index", new { WeblogPostId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
