using AwIngenieriaSoftware.Models;
using Microsoft.AspNet.Identity;
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
    public class UsuarioController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Usuario
        public ActionResult Index()
        {
            List<Usuario> lista = new List<Usuario>();
            var result = UserManager.Users;
            foreach (var item in result)
            {
                Usuario u = new Usuario();
                u.Id = item.Id;
                u.UserName = item.UserName;
                u.Email = item.Email;
                u.LockoutEndDateUtc = item.LockoutEndDateUtc;
                u.ClienteId = item.ClienteId;
                u.ClienteNombre = item.ClienteNombre;
                lista.Add(u);
            }
            return View(lista);
        }

        // GET: Articulo/Details/5
        public ActionResult Details(String Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var result = UserManager.FindById(Id);
            if (result == null)
            {
                return HttpNotFound();
            }
            Usuario usuario = new Usuario
            {
                Id = result.Id,
                UserName = result.UserName,
                Email = result.Email,
                LockoutEndDateUtc = result.LockoutEndDateUtc,
                LockoutEnabled = result.LockoutEnabled,
                AccessFailedCount = result.AccessFailedCount,
                ClienteId = result.ClienteId,
                ClienteNombre = result.ClienteNombre
            };
            return View(usuario);
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            ViewBag.ListaClientes = new SelectList(db.Cliente.Where(x => !x.Estado.Equals("INA")), "Id", "FullName");
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,Email,Password,ClienteId, ClienteNombre")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {                
                if (string.IsNullOrEmpty(usuario.Password))
                {
                    ModelState.AddModelError("", "El campo contraseña es requerido");
                    ViewBag.ListaClientes = new SelectList(db.Cliente.Where(x => !x.Estado.Equals("INA")), "Id", "FullName", usuario.ClienteId);
                    return View(usuario);
                }
                var Email = usuario.Email;
                var UserName = usuario.UserName.Trim();
                var Password = usuario.Password.Trim();
                var ClienteId = usuario.ClienteId;
                var cliente = db.Cliente.Find(usuario.ClienteId);
                var ClienteNombre = "";
                if (cliente != null)
                {
                    ClienteNombre = cliente.Nombres + " " + cliente.Apellidos;
                }
                var nuevoUsuario = new ApplicationUser { UserName = UserName, Email = Email, ClienteId = ClienteId, ClienteNombre = ClienteNombre };
                if (UserManager.FindByName(UserName) == null)
                {
                    var resultadoCreacion = UserManager.Create(nuevoUsuario, Password);
                    if (resultadoCreacion.Succeeded == true)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in resultadoCreacion.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                        ViewBag.ListaClientes = new SelectList(db.Cliente.Where(x => !x.Estado.Equals("INA")), "Id", "FullName", usuario.ClienteId);
                        return View(usuario);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "El usuario " + UserName + " ya existe.");
                    ViewBag.ListaClientes = new SelectList(db.Cliente.Where(x => !x.Estado.Equals("INA")), "Id", "FullName", usuario.ClienteId);
                    return View(usuario);
                }
            }
            else
            {
                var query = from state in ModelState.Values
                            from error in state.Errors
                            select error.ErrorMessage;
                var errorList = query.ToList();
                foreach (var item in errorList)
                {
                    ModelState.AddModelError("", item.ToString());
                }                
            }
            ViewBag.ListaClientes = new SelectList(db.Cliente.Where(x => !x.Estado.Equals("INA")), "Id", "FullName",usuario.ClienteId);
            return View(usuario);
        }
        // GET: Usuario/Edit
        public ActionResult Edit(String Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var result = UserManager.FindById(Id);
            if (result == null)
            {
                return HttpNotFound();
            }
            Usuario usuario = new Usuario { Id = result.Id, UserName = result.UserName, Email = result.Email, ClienteId = result.ClienteId, ClienteNombre = result.ClienteNombre };
            ViewBag.ListaClientes = new SelectList(db.Cliente.Where(x => !x.Estado.Equals("INA")), "Id", "FullName", usuario.ClienteId);
            return View(usuario);
        }

        // POST: Usuario/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, Username, Email, Password, ClienteId, ClienteNombre")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var UsuarioId = UserManager.FindById(usuario.Id);
                if (UsuarioId == null)
                {
                    return HttpNotFound();
                }                
                //if (usuario.ClienteId <= 0)
                //{
                //    ModelState.AddModelError("", "El campo Cliente es requerido");
                //    //ViewBag.Rol = GetRoles();
                //    return View(usuario);
                //}
                UsuarioId.UserName = usuario.UserName;
                UsuarioId.Email = usuario.Email;
                UsuarioId.ClienteId = usuario.ClienteId;
                var cliente = db.Cliente.Find(usuario.ClienteId);
                if (cliente!=null)
                {
                    UsuarioId.ClienteNombre = cliente.Nombres + " " + cliente.Apellidos;
                }                
                if (!string.IsNullOrEmpty(usuario.Password))
                {
                    var result = UserManager.RemovePassword(UsuarioId.Id);
                    if (result.Succeeded)
                    {
                        var AddPassword = UserManager.AddPassword(UsuarioId.Id, usuario.Password);
                        if (AddPassword.Errors.Count() > 0)
                        {
                            foreach (var item in AddPassword.Errors)
                            {
                                ModelState.AddModelError("", item.ToString());
                            }
                            ViewBag.ListaClientes = new SelectList(db.Cliente.Where(x => !x.Estado.Equals("INA")), "Id", "FullName", usuario.ClienteId);
                            return View(usuario);
                        }
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                        ViewBag.ListaClientes = new SelectList(db.Cliente.Where(x => !x.Estado.Equals("INA")), "Id", "FullName", usuario.ClienteId);
                        return View(usuario);
                    }
                }
                UserManager.Update(UsuarioId);
                return RedirectToAction("Index");
            }
            ViewBag.ListaClientes = new SelectList(db.Cliente.Where(x => !x.Estado.Equals("INA")), "Id", "FullName", usuario.ClienteId);
            return View(usuario);
        }      
        // POST: Usuario/Delete
        [HttpPost]        
        public bool Delete(string Id)
        {
            var usuario = UserManager.FindById(Id);
            if (usuario == null)
            {
                return false;
            }

            var roles = UserManager.GetRoles(usuario.Id);
            foreach (var item in roles)
            {
                UserManager.RemoveFromRole(usuario.Id, item);
            }
            UserManager.Delete(usuario);
            return true;
        }

        private IEnumerable<SelectListItem> GetClientes()
        {
            var clientes = db.Cliente.Where(x => !x.Estado.Equals("INA"));
            List<SelectListItem> items = new List<SelectListItem>();
            SelectListItem first = new SelectListItem
            {
                Value = "0",
                Text = "NINGUNO"
            };
            first.Selected = true;            
            items.Add(first);
            foreach (var x in clientes)
            {
                SelectListItem item = new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Nombres + " " + x.Apellidos + ((x.Estado.Equals("INA")) ? " (Inactivo)" : "")                    
                };
                if (x.Estado.Equals("INA"))
                {
                    item.Disabled = true;                    
                }
                items.Add(item);
            }
            

            return new SelectList(items, "Value", "Text");
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