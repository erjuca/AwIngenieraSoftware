using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AwIngenieriaSoftware.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace AwIngenieriaSoftware.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationRoleManager _roleManager;
        private ApplicationUserManager _userManager;

        // GET: Menu
        public ActionResult Index()
        {
            var menu = db.Menu.Include(m => m.MenuPadre);
            return View(menu.ToList());
        }

        // GET: Menu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menu.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // GET: Menu/Create
        public ActionResult Create()
        {
            ViewBag.Menus = new SelectList(db.Menu, "Id", "GetNombre");
            ViewBag.Tipos = new SelectList(db.Catalogo.Where(x => x.Codigo == 2 && !String.IsNullOrEmpty(x.Valor)), "Valor", "Descripcion");
            ViewBag.Iconos = new SelectList(db.Catalogo.Where(x => x.Codigo == 10 && !String.IsNullOrEmpty(x.Valor)), "Valor", "Descripcion");
            return View(new Menu { Icono = "fa fa-circle-o" });
        }

        // POST: Menu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Tipo,Controlador,Accion,Icono,Nombre,MenuPadreId")] Menu menu)
        {
            string tipo = Convert.ToString(Request.Form["Tipo"]);
            menu.Tipo = tipo;
            if (ModelState.IsValid)
            {
                menu.MenuPadre = db.Menu.Find(menu.MenuPadreId);
                db.Menu.Add(menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Menus = new SelectList(db.Menu, "Id", "GetNombre");
            ViewBag.Tipos = new SelectList(db.Catalogo.Where(x => x.Codigo == 2 && !String.IsNullOrEmpty(x.Valor)), "Valor", "Descripcion");
            ViewBag.Iconos = new SelectList(db.Catalogo.Where(x => x.Codigo == 10 && !String.IsNullOrEmpty(x.Valor)), "Valor", "Descripcion");
            return View(menu);
        }

        // GET: Menu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menu.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            ViewBag.Menus = new SelectList(db.Menu, "Id", "GetNombre", menu.MenuPadreId);
            ViewBag.Tipos = new SelectList(db.Catalogo.Where(x => x.Codigo == 2 && !String.IsNullOrEmpty(x.Valor)), "Valor", "Descripcion", menu.Tipo);
            ViewBag.Iconos = new SelectList(db.Catalogo.Where(x => x.Codigo == 10 && !String.IsNullOrEmpty(x.Valor)), "Valor", "Descripcion", menu.Icono);
            return View(menu);
        }

        // POST: Menu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Tipo,Controlador,Accion,Nombre,Icono,Menu,MenuPadreId")] Menu menu)
        {
            string tipo = Convert.ToString(Request.Form["Tipo"]);
            menu.Tipo = tipo;
            if (ModelState.IsValid)
            {
                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Menus = new SelectList(db.Menu, "Id", "GetNombre", menu.MenuPadreId);
            ViewBag.Tipos = new SelectList(db.Catalogo.Where(x => x.Codigo == 2 && !String.IsNullOrEmpty(x.Valor)), "Valor", "Descripcion", menu.Tipo);
            ViewBag.Iconos = new SelectList(db.Catalogo.Where(x => x.Codigo == 10 && !String.IsNullOrEmpty(x.Valor)), "Valor", "Descripcion", menu.Icono);
            return View(menu);
        }

        // POST: Usuario/Delete
        [HttpPost]
        public bool Delete(int id)
        {
            //Menu menu = db.Menu.Find(id);
            try
            {
                //var menuRol = db.MenuRol.Where(x => x.Menu.Id == menu.Id).ToList();
                //var menuDependientes = db.Menu.Where(x => x.MenuPadre != null && x.MenuPadre.Id == menu.Id).ToList();
                //foreach (var item in menuDependientes)
                //{
                //    var menuRolDep = db.MenuRol.Where(x => x.Menu.Id == item.Id).ToList();
                //    foreach (var itemDep in menuRolDep)
                //    {
                //        db.MenuRol.Remove(itemDep);
                //    }
                //    db.Menu.Remove(item);
                //}
                //foreach (var item in menuRol)
                //{
                //    db.MenuRol.Remove(item);
                //}
                //db.Menu.Remove(menu);
                //db.SaveChanges();
                EliminarMenus(id);
                return true;
            }

            catch (Exception ex)
            {

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Error no se puede eliminar este registro");
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.InnerException.Message);
                ModelState.AddModelError("", sb.ToString());
                return false;
            }
        }
        private void EliminarMenus(int id)
        {            
            Menu menu = db.Menu.Find(id);
            var menuDependientes = db.Menu.Where(x => x.MenuPadre != null && x.MenuPadre.Id == menu.Id).ToList();          
            foreach (var item in menuDependientes)
            {
                var menuRolDep = db.MenuRol.Where(x => x.Menu.Id == item.Id).ToList();
                foreach (var itemDep in menuRolDep)
                {
                    db.MenuRol.Remove(itemDep);
                }
                db.Menu.Remove(item);
                EliminarMenus(item.Id);
            }
            var menuRol = db.MenuRol.Where(x => x.Menu.Id == menu.Id).ToList();
            foreach (var item in menuRol)
            {
                db.MenuRol.Remove(item);
            }
            db.Menu.Remove(menu);
            db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult MenuDinamico()
        {
            var currentUser = System.Web.HttpContext.Current.User.Identity.Name;
            var user = UserManager.FindByName(currentUser);
            var roles = UserManager.GetRoles(user.Id);
            List<Menu> ListaMenus = db.Menu.ToList();
            ListaMenus.ConvertAll(x => x.Estado = "false");
            List<Menu> Items = new List<Menu>();
            foreach (var r in roles)
            {
                var result = db.MenuRol.Where(x => x.Rol.Name == r).ToList();
                foreach (var menusAsignados in result)
                {
                    ListaMenus.Where(x => x.Id == menusAsignados.MenuId).ToList().ConvertAll(y => y.Estado = "true");
                }
            }
            ViewBag.Root = ListaMenus.Where(x => x.MenuPadre == null).First().Id;
            ListaMenus.RemoveAll(x => x.Estado == "false" || x.MenuPadre == null);
            return PartialView("MenuDinamico", ListaMenus);
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
