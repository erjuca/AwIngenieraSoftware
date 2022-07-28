using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AwIngenieriaSoftware.Models;

namespace AwIngenieriaSoftware
{
    public class EmpresaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Empresa
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Empresa.ToList());
        }
        [Authorize]
        // GET: Empresa/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empresa empresa = db.Empresa.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }
        [Authorize]
        // GET: Empresa/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Empresa/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Ruc,Nombre,Direccion,Telefono,Mail")] Empresa empresa, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {

                    empresa.FileName = System.IO.Path.GetFileName(upload.FileName);
                    empresa.FileType = FileType.Logo;
                    empresa.ContentType = upload.ContentType;
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        empresa.Content = reader.ReadBytes(upload.ContentLength);
                    }
                }
                db.Empresa.Add(empresa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(empresa);
        }
        [Authorize]
        // GET: Empresa/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empresa empresa = db.Empresa.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }

        // POST: Empresa/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Ruc,Nombre,Direccion,Telefono,Mail")] Empresa empresa, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {

                    empresa.FileName = System.IO.Path.GetFileName(upload.FileName);
                    empresa.FileType = FileType.Logo;
                    empresa.ContentType = upload.ContentType;
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        empresa.Content = reader.ReadBytes(upload.ContentLength);
                    }
                    db.Entry(empresa).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    var oldValues = db.Empresa.Find(empresa.Id);
                    oldValues.Ruc = empresa.Ruc;
                    oldValues.Nombre = empresa.Nombre;
                    oldValues.Direccion = empresa.Direccion;
                    oldValues.Telefono = empresa.Telefono;
                    oldValues.Mail = empresa.Mail;
                    db.Entry(oldValues).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            return View(empresa);
        }
        [Authorize]
        // GET: Empresa/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empresa empresa = db.Empresa.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }

        // POST: Empresa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Empresa empresa = db.Empresa.Find(id);
            db.Empresa.Remove(empresa);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GetLogo(int? id)
        {
            Empresa empresa = null;
            if (id != null)
            {
                empresa = db.Empresa.Find(id);
            }
            else
            {
                empresa = db.Empresa.FirstOrDefault();
            }

            if (empresa != null)
            {
                if (empresa.Content != null)
                {
                    return File(empresa.Content, "image/" + empresa.ContentType);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
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
