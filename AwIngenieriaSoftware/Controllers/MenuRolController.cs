using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using AwIngenieriaSoftware.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AwIngenieriaSoftware.Controllers
{
    [Authorize]
    public class MenuRolController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MenuRol
        public ActionResult Index()
        {
            return View(db.MenuRol.ToList());
        }


        // GET: MenuRol/Create
        public ActionResult Create()
        {
            ViewBag.Menus = new SelectList(db.Menu.ToList(), "Id", "GetNombre");
            ViewBag.Roles = new SelectList(db.Roles.ToList(), "Id", "Name");
            return View();
        }

        // POST: MenuRol/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Menu,Rol,MenuId,RolId")] MenuRol menuRol)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var rol = db.Roles.Find(menuRol.RolId);
                    Menu menu = db.Menu.Find(menuRol.MenuId);
                    menuRol.Menu = menu;
                    menuRol.Rol = rol;
                    List<MenuRol> asignaciones = new List<MenuRol>();

                    var existeAsignacion = db.MenuRol.Any(x => x.Menu.Id == menu.Id && x.Rol.Id == rol.Id);
                    if (!existeAsignacion)
                    {
                        asignaciones.Add(menuRol);
                    }
                    var check = Request.Form.GetValues("AgregarSubmenus")[0];
                    if (check.Equals("true"))
                    {
                        var submenus = db.Menu.Where(x => x.MenuPadre.Id == menuRol.MenuId).ToList();
                        foreach (var item in submenus)
                        {
                            var existeAsignacionSubmenu = db.MenuRol.Any(x => x.Menu.Id == item.Id && x.Rol.Id == rol.Id);
                            if (!existeAsignacionSubmenu)
                            {
                                MenuRol submenu = new MenuRol { Menu = item, Rol = rol };
                                asignaciones.Add(submenu);
                            }
                        }
                    }
                    db.MenuRol.AddRange(asignaciones); 
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var failure in ex.EntityValidationErrors)
                    {
                        sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                        foreach (var error in failure.ValidationErrors)
                        {
                            sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                            sb.AppendLine();
                        }
                    }
                    ModelState.AddModelError("", sb.ToString());
                    ViewBag.Menus = new SelectList(db.Menu.Where(x => x.Tipo == "SUBMENU"), "Id", "Nombre", menuRol.MenuId);
                    ViewBag.Roles = new SelectList(db.Roles.ToList(), "Id", "Name", menuRol.RolId);
                    return View(menuRol);
                }
                return RedirectToAction("Index");
            }
            ViewBag.Menus = new SelectList(db.Menu.Where(x => x.Tipo == "SUBMENU"), "Id", "GetNombre", menuRol.MenuId);
            ViewBag.Roles = new SelectList(db.Roles.ToList(), "Id", "Name", menuRol.RolId);
            return View(menuRol);
        }


        // POST: MenuRol/Delete/5
        [HttpPost]
        public bool Delete(string id)
        {
            try
            {
                String[] ids = id.Split(',');
                IdentityRole rol = db.Roles.Find(ids[0]);
                Menu menu = db.Menu.Find(int.Parse(ids[1]));
                var menuRol = db.MenuRol.Find(int.Parse(ids[1]), ids[0]);
                db.MenuRol.Remove(menuRol);
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
    }
}
