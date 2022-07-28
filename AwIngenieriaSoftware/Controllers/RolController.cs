using AwIngenieriaSoftware.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AwIngenieriaSoftware.Controllers
{
    [Authorize]
    public class RolController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationRoleManager _roleManager;
        private ApplicationUserManager _userManager;
        // GET: Rol
        public ActionResult Index()
        {
            List<Rol> lista = new List<Rol>();
            var result = RoleManager.Roles;
            foreach (var item in result)
            {
                Rol r = new Rol();
                r.Id = item.Id;
                r.Name = item.Name;
                lista.Add(r);
            }
            return View(lista);
        }

        // GET: Rol/Details/
        public ActionResult Details(String Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var result = RoleManager.FindById(Id);
            if (result == null)
            {
                return HttpNotFound();
            }
            Rol rol = new Rol
            {
                Id = result.Id,
                Name = result.Name
            };
            return View(rol);
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Rol rol)
        {
            if (ModelState.IsValid)
            {
                var Name = rol.Name.Trim();
                var nuevo = new IdentityRole { Name = Name };

                if (!RoleManager.RoleExists(Name))
                {
                    var resultadoCreacion = RoleManager.Create(nuevo);
                    if (resultadoCreacion.Succeeded == true)
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "El rol " + Name + " ya existe.");
                    return View(rol);
                }

            }
            return View(rol);
        }

        // GET: Rol/Edit
        public ActionResult Edit(String Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var result = RoleManager.FindById(Id);
            if (result == null)
            {
                return HttpNotFound();
            }
            Rol Rol = new Rol() { Id = result.Id, Name = result.Name };
            return View(Rol);
        }

        // POST: Rol/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Rol rol)
        {
            if (ModelState.IsValid)
            {
                var result = RoleManager.FindById(rol.Id);
                if (result == null)
                {
                    return HttpNotFound();
                }
                result.Name = rol.Name;
                RoleManager.Update(result);
                return RedirectToAction("Index");
            }
            return View(rol);
        }

        // POST: Usuario/Delete
        [HttpPost]
        public bool Delete(string Id)
        {
            var Rol = RoleManager.FindById(Id);
            if (Rol == null)
            {
                return false;
            }

            var users = UserManager.Users.Where(x => x.Roles.Any(y => y.RoleId.Equals(Rol.Id))).ToList();
            foreach (var item in users)
            {
                UserManager.RemoveFromRole(item.Id, Rol.Name);

            }
            var menuRoles = db.MenuRol.Where(x => x.Rol.Id == Rol.Id);
            foreach (var item in menuRoles)
            {
                db.MenuRol.Remove(item);
            }
            db.SaveChanges();
            RoleManager.Delete(Rol);
            return true;
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


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                RoleManager.Dispose();
                UserManager.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}