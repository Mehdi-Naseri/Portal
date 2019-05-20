using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Portal.DataLayer.DataContext;
using Portal.DomainClasses.Models;

namespace Portal.Areas.Tools.Controllers
{
    public class UploadController : Controller
    {
        private PortalDb db = new PortalDb();

        //
        // GET: /Upload/

        public ActionResult Index()
        {
            return View(db.Uploads.ToList());
        }

        //
        // GET: /Upload/Details/5

        public ActionResult Details(int id = 0)
        {
            Upload upload = db.Uploads.Find(id);
            if (upload == null)
            {
                return HttpNotFound();
            }
            return View(upload);
        }

        //
        // GET: /Upload/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Upload/Create

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(IEnumerable<HttpPostedFileBase> files1)
        {
            foreach (var file1 in files1)
            {
                if ((file1 != null) && (file1.ContentLength > 0) && (file1.ContentLength < 20000000))
                {
                    string stringfileName1 = System.IO.Path.GetFileName(file1.FileName);
                    string stringRandomFileName = System.IO.Path.GetRandomFileName();
                    string stringPath1 = System.IO.Path.Combine(Server.MapPath("~/Files/Upload/Files"), stringRandomFileName);
                    if (!System.IO.File.Exists(stringPath1))
                    {
                        file1.SaveAs(stringPath1);
                        //save file name to database
                        //var ImagesInfo1 = MoviesMrEntities1.ImagesInfoes;
                        var UploadedFile = new Upload();
                        UploadedFile.UploadDateTime = DateTime.Now;
                        UploadedFile.FileName = stringfileName1;
                        UploadedFile.RandomName = stringRandomFileName;
                        UploadedFile.ContentLength = file1.ContentLength;
                        UploadedFile.ContentType = file1.ContentType;
                        //Uploder name must be read automatically
                        if (User.Identity.IsAuthenticated == false)
                            UploadedFile.UploadedBy = "Guest";
                        else
                            UploadedFile.UploadedBy = User.Identity.Name;
                        db.Uploads.Add(UploadedFile);
                        db.SaveChanges();
                    }
                }
                else
                {
                    ModelState.AddModelError("files1", "Can't upload your file");
                }
            }
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View("Create");
        }

        //
        // GET: /Upload/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id = 0)
        {
            Upload upload = db.Uploads.Find(id);
            if (upload == null)
            {
                return HttpNotFound();
            }
            return View(upload);
        }

        //
        // POST: /Upload/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Upload upload)
        {
            if (ModelState.IsValid)
            {
                db.Entry(upload).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(upload);
        }

        //
        // GET: /Upload/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id = 0)
        {
            Upload upload = db.Uploads.Find(id);
            if (upload == null)
            {
                return HttpNotFound();
            }
            return View(upload);
        }

        //
        // POST: /Upload/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Upload upload = db.Uploads.Find(id);
            db.Uploads.Remove(upload);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Download(int id)
        {
            Upload upload = db.Uploads.Find(id);
            string stringPath = System.IO.Path.Combine(Server.MapPath("~/Files/Upload/Files"), upload.RandomName);
            return File(stringPath, upload.ContentType, upload.FileName);
        }
	}
}