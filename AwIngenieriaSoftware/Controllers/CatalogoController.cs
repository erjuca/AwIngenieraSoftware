using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AwIngenieriaSoftware.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace AwIngenieriaSoftware.Controllers
{
    [Authorize]
    public class CatalogoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationRoleManager _roleManager;
        private ApplicationUserManager _userManager;

        // GET: Catalogo
        public ActionResult Index()
        {
            var currentUser = System.Web.HttpContext.Current.User.Identity.Name;
            var user = UserManager.FindByName(currentUser);
            var roles = UserManager.GetRoles(user.Id)[0];
            return View((roles.Equals("Administrador")) ? db.Catalogo.ToList() : db.Catalogo.Where(x => x.Tipo == "U").ToList());
        }
        //public ActionResult Index()
        //{            
        //    return View();
        //}
        public ActionResult GetData()
        {
            return Json(db.Catalogo.ToList(), JsonRequestBehavior.AllowGet);
        }


        // GET: Catalogo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Catalogo catalogo = db.Catalogo.Find(id);
            if (catalogo == null)
            {
                return HttpNotFound();
            }
            return View(catalogo);
        }

        // GET: Catalogo/Create
        public ActionResult Create()
        {
            var currentUser = System.Web.HttpContext.Current.User.Identity.Name;
            var user = UserManager.FindByName(currentUser);
            var roles = UserManager.GetRoles(user.Id)[0];
            var TipoUsuario = (roles.Equals("Administrador")) ? "S" : "U";
            ViewBag.IsAdministrator = (roles.Equals("Administrador")) ? true : false;
            ViewBag.Tipos = new SelectList(db.Catalogo.Where(x => x.Codigo == 6 && !string.IsNullOrEmpty(x.Argumento)), "Valor", "Descripcion", TipoUsuario);
            return View();
        }

        // POST: Catalogo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Codigo,Argumento,Tipo, Descripcion,Valor")] Catalogo catalogo)
        {
            var currentUser = System.Web.HttpContext.Current.User.Identity.Name;
            var user = UserManager.FindByName(currentUser);
            var roles = UserManager.GetRoles(user.Id)[0];
            if (ModelState.IsValid)
            {
                db.Catalogo.Add(catalogo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var TipoUsuario = (roles.Equals("Administrador")) ? "S" : "U";
            ViewBag.IsAdministrator = (roles.Equals("Administrador")) ? true : false;
            ViewBag.Tipos = new SelectList(db.Catalogo.Where(x => x.Codigo == 6 && !string.IsNullOrEmpty(x.Argumento)), "Valor", "Descripcion", TipoUsuario);
            return View(catalogo);
        }

        // GET: Catalogo/Edit/5
        public ActionResult Edit(int? id)
        {
            var currentUser = System.Web.HttpContext.Current.User.Identity.Name;
            var user = UserManager.FindByName(currentUser);
            var roles = UserManager.GetRoles(user.Id)[0];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Catalogo catalogo = db.Catalogo.Find(id);
            if (catalogo == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsAdministrator = (roles.Equals("Administrador")) ? true : false;
            ViewBag.Tipos = new SelectList(db.Catalogo.Where(x => x.Codigo == 6 && !string.IsNullOrEmpty(x.Argumento)), "Valor", "Descripcion", catalogo.Tipo);
            return View(catalogo);
        }

        // POST: Catalogo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Codigo,Argumento, Tipo, Descripcion,Valor")] Catalogo catalogo)
        {
            var currentUser = System.Web.HttpContext.Current.User.Identity.Name;
            var user = UserManager.FindByName(currentUser);
            var roles = UserManager.GetRoles(user.Id)[0];
            if (ModelState.IsValid)
            {
                db.Entry(catalogo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IsAdministrator = (roles.Equals("Administrador")) ? true : false;
            ViewBag.Tipos = new SelectList(db.Catalogo.Where(x => x.Codigo == 6 && !string.IsNullOrEmpty(x.Argumento)), "Valor", "Descripcion", catalogo.Tipo);
            return View(catalogo);
        }
        
        // POST: Catalogo/Delete/5
        [HttpPost]        
        public bool Delete(int id)
        {
            try
            {
                Catalogo catalogo = db.Catalogo.Find(id);
                db.Catalogo.Remove(catalogo);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
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

        #region Get Managers
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ??
                    HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set { _roleManager = value; }
        }
        #endregion
    }
}
