using AwIngenieriaSoftware.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AwIngenieriaSoftware.Controllers
{
    [Authorize]
    public class UsuarioRolController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        // GET: UsuarioRol
        public ActionResult Index()
        {
            List<UsuarioRol> lista = new List<UsuarioRol>();
            var users = UserManager.Users;
            foreach (var iu in users)
            {
                Usuario u = new Usuario();
                u.Id = iu.Id;
                u.UserName = iu.UserName;
                u.Email = iu.Email;
                u.LockoutEndDateUtc = iu.LockoutEndDateUtc;
                var roles = UserManager.GetRoles(u.Id);
                foreach (var ir in roles)
                {
                    var r = RoleManager.FindByName(ir.ToString());
                    Rol rol = new Rol() { Id = r.Id, Name = r.Name };
                    lista.Add(new UsuarioRol() { Usuario = u, Rol = rol });
                }
            }
            return View(lista);
        }

        // GET: UsuarioRol/Create
        public ActionResult Create()
        {
            ViewBag.UsuarioLista = GetUsuarios();
            ViewBag.RolLista = GetRoles();
            return View();
        }
        // POST: UsuarioRol/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsuarioRol usuarioRol)
        {
            string usuarioId = Convert.ToString(Request.Form["UsuarioLista"]);
            string rolId = Convert.ToString(Request.Form["RolLista"]);

            if (string.IsNullOrEmpty(usuarioId))
            {
                ModelState.AddModelError("", "El campo Usuario es requerido.");
            }
            if (string.IsNullOrEmpty(rolId))
            {
                ModelState.AddModelError("", "El campo Rol es requerido.");
            }
            var result = UserManager.AddToRole(usuarioId, rolId);

            if (result.Succeeded == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                ViewBag.UsuarioLista = GetUsuarios();
                ViewBag.RolLista = GetRoles();
                return View(usuarioRol);
            }
        }       
        // POST: UsuarioRol/Delete
        [HttpPost]        
        public bool Delete(string Id)
        {
            if (Id == null)
            {
                return false;
            }
            String[] ids = Id.Split(',');
            String usuarioId = ids[0];
            String rolId = ids[1];
            UserManager.RemoveFromRole(usuarioId, rolId);
            return true;
        }

        #region Get Listas
        private List<SelectListItem> GetRoles()
        {
            List<SelectListItem> Roles = new List<SelectListItem>();
            Roles.Add(
                new SelectListItem
                {
                    Text = "Seleccione uno",
                    Value = ""
                });

            var result = RoleManager.Roles.ToList();
            foreach (var item in result)
            {
                Roles.Add(
                       new SelectListItem
                       {
                           Text = item.Name.ToString(),
                           Value = item.Name.ToString()
                       });
            }
            return Roles;
        }
        private List<SelectListItem> GetUsuarios()
        {
            List<SelectListItem> Users = new List<SelectListItem>();
            Users.Add(
                new SelectListItem
                {
                    Text = "Seleccione uno",
                    Value = ""
                });

            var result = UserManager.Users.ToList();
            foreach (var item in result)
            {
                Users.Add(
                       new SelectListItem
                       {
                           Text = item.UserName.ToString(),
                           Value = item.Id.ToString()
                       });
            }
            return Users;
        }
        #endregion

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
            }
            base.Dispose(disposing);
        }
    }
}