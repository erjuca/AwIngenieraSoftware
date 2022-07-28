using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AwIngenieriaSoftware.Models
{
    [Authorize]
    public class DonacionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Donacion
        public ActionResult Index()
        {
            var usuarioId = User.Identity.GetUserId();
            var roles = UserManager.GetRoles(usuarioId);
            string rolName = roles[0];
            BusquedaCabeceraDonacion busquedaCabecera = new BusquedaCabeceraDonacion();
            List<RegistroDonacion> listaRegistro = new List<RegistroDonacion>();
            try
            {
                List<Donacion> listaDonaciones = null;
                if (!rolName.Equals("FUNDACION"))
                {
                    listaDonaciones = db.Donacion.Where(x => x.UsuarioId.Equals(usuarioId)).ToList();
                }
                else
                {
                    listaDonaciones = db.Donacion.Where(x => x.UsuarioBeneficiarioId.Equals(usuarioId)).ToList();
                }                
                foreach (var item in listaDonaciones)
                {
                    RegistroDonacion registro = new RegistroDonacion();
                    if (!rolName.Equals("FUNDACION") && !item.Estado.Equals("RECIBIDA"))
                    {
                        registro.Editable = true;
                    }
                    else
                    {
                        registro.Editable = false;
                    }

                    if (rolName.Equals("FUNDACION"))
                    {
                        if (item.Estado.Equals("REGISTRADO"))
                        {
                            registro.Recibir = true;
                        }
                        else
                        {
                            registro.Recibir = false;
                        }
                    }
                    else
                    {
                        registro.Recibir = false;
                    }
                    //registro.Recibir = true;
                    registro.FechaDonacion = item.FechaDonacion.Value;
                    registro.Id = item.Id;
                    registro.Descripcion = item.Descripcion;
                    registro.Cantidad = item.Cantidad;
                    registro.Latitude = item.Latitude;
                    registro.Longitude = item.Longitude;
                    registro.Estado = item.Estado;
                    registro.FileName = item.FileName;
                    var donante = UserManager.Users.Where(x => x.Id.Equals(item.UsuarioId)).FirstOrDefault();
                    string nombreDonante = "";
                    if (donante != null && donante.ClienteId != null)
                    {
                        Cliente cliente = db.Cliente.Where(x => x.Id == donante.ClienteId).FirstOrDefault();
                        nombreDonante = cliente.FullName;
                    }
                    else
                    {
                        nombreDonante = donante.UserName;
                    }
                    registro.Donante = nombreDonante;

                    var beneficiario = UserManager.Users.Where(x => x.Id.Equals(item.UsuarioId)).FirstOrDefault();
                    string nombreBeneficiario = "";
                    if (beneficiario != null && beneficiario.ClienteId != null)
                    {
                        Cliente cliente = db.Cliente.Where(x => x.Id == donante.ClienteId).FirstOrDefault();
                        nombreBeneficiario = cliente.FullName;
                    }
                    else
                    {
                        nombreBeneficiario = donante.UserName;
                    }
                    registro.Beneficiario = nombreBeneficiario;
                    registro.ContentType = item.ContentType;
                    registro.FileType = item.FileType;
                    listaRegistro.Add(registro);
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("CustomError", exception.Message);
            }
            busquedaCabecera.detalle = listaRegistro;
            return View("ListaCabecera", busquedaCabecera);
        }
        private IEnumerable<SelectListItem> GetUsuarios(string idUsuario = "")
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

            List<UsuarioRol> listaUsuarios = lista.Where(x => x.Rol.Name.Equals("FUNDACION")).ToList();
            List<Usuario> usuarios = new List<Usuario>();
            foreach (var item in listaUsuarios)
            {
                var u = UserManager.FindById(item.Usuario.Id);
                Usuario us = new Usuario();
                us.ClienteNombre = u.ClienteNombre;
                us.Id = u.Id;
                us.Email = u.Email;
                us.UserName = u.UserName;
                usuarios.Add(us);
            }

            List<SelectListItem> items = new List<SelectListItem>();
         
            foreach (var x in usuarios)
            {
                SelectListItem item = new SelectListItem
                {
                    Value = x.Id.Trim(),
                    Text = x.ClienteNombre,
                    Selected=false
                };                
                item.Selected = x.Id.Equals(idUsuario);
                //if (x..Equals("INA"))
                //{
                //    item.Disabled = true;
                //}
                items.Add(item);
            }
            return new SelectList(items, "Value", "Text");
        }

        // GET: Donacion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donacion item = db.Donacion.Find(id);
            RegistroDonacion registro = new RegistroDonacion();
            registro.FechaDonacion = item.FechaDonacion.Value;
            registro.Id = item.Id;
            registro.Descripcion = item.Descripcion;
            registro.Cantidad = item.Cantidad;
            registro.Latitude = item.Latitude;
            registro.Longitude = item.Longitude;
            registro.Estado = item.Estado;
            registro.FileName = item.FileName;
            var donante = UserManager.Users.Where(x => x.Id.Equals(item.UsuarioId)).FirstOrDefault();
            string nombreDonante = "";
            if (donante != null && donante.ClienteId != null)
            {
                Cliente cliente = db.Cliente.Where(x => x.Id == donante.ClienteId).FirstOrDefault();
                nombreDonante = cliente.FullName;
            }
            else
            {
                nombreDonante = donante.UserName;
            }
            registro.Donante = nombreDonante;

            var beneficiario = UserManager.Users.Where(x => x.Id.Equals(item.UsuarioBeneficiarioId)).FirstOrDefault();
            string nombreBeneficiario = "";
            if (beneficiario != null && beneficiario.ClienteId != null)
            {
                Cliente cliente = db.Cliente.Where(x => x.Id == donante.ClienteId).FirstOrDefault();
                nombreBeneficiario = cliente.FullName;
            }
            else
            {
                nombreBeneficiario = donante.UserName;
            }
            registro.Beneficiario = nombreBeneficiario;
            registro.ContentType = item.ContentType;
            registro.FileType = item.FileType;
            return View(registro);
        }

        // GET: Donacion/Create
        public ActionResult Create()
        {
            RegistroDonacion registro = new RegistroDonacion();
            registro.Latitude = (decimal)-2.897069;
            registro.Longitude = (decimal)-79.004671;
            ViewBag.UsuarioLista = GetUsuarios();
            return View(registro);
        }

        // POST: Donacion/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "Id,Identificacion,Nombres,Apellidos,Direccion,Telefono,Genero,Mail,Estado")]*/ RegistroDonacion registro, HttpPostedFileBase upload)
        {
            try
            {
                string usuarioBeneficiarioId = Convert.ToString(Request.Form["UsuarioBeneficiarioId"]);
                if (ModelState.IsValid)
                {
                    using (ApplicationDbContext context = new ApplicationDbContext())
                    {
                        using (var transaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                Donacion donacion = new Donacion();
                                donacion.FechaDonacion = DateTime.Now;
                                donacion.Descripcion = registro.Descripcion;
                                donacion.Cantidad = registro.Cantidad;
                                var idUser = UserManager.Users.Where(x => x.UserName.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Id;
                                donacion.UsuarioId = idUser;
                                donacion.UsuarioBeneficiarioId = usuarioBeneficiarioId;
                                if (string.IsNullOrEmpty(donacion.UsuarioBeneficiarioId)) throw new Exception("Elija una fundación beneficiaria");
                                donacion.Estado = "REGISTRADO";
                                donacion.Latitude = registro.Latitude;
                                donacion.Longitude = registro.Longitude;
                                if (upload == null) throw new Exception("Elija una imagen para continuar");
                                donacion.FileName = System.IO.Path.GetFileName(upload.FileName);
                                donacion.FileType = FileType.Logo;
                                donacion.ContentType = upload.ContentType;
                                using (var reader = new System.IO.BinaryReader(upload.InputStream))
                                {
                                    donacion.Content = reader.ReadBytes(upload.ContentLength);
                                }
                                db.Donacion.Add(donacion);
                                db.SaveChanges();
                                transaction.Commit();
                                return RedirectToAction("Index");

                            }
                            catch (DbEntityValidationException e)
                            {
                                foreach (var eve in e.EntityValidationErrors)
                                {
                                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                    foreach (var ve in eve.ValidationErrors)
                                    {
                                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                                        ModelState.AddModelError("CustomError", ve.ErrorMessage);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ModelState.AddModelError("CustomError", ex.Message);
                                transaction.Rollback();
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("CustomError", exception.Message);
            }
            ViewBag.UsuarioLista = GetUsuarios();
            return View(registro);
        }


        // GET: Donacion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donacion item = db.Donacion.Find(id);
            RegistroDonacion registro = new RegistroDonacion();
            registro.FechaDonacion = item.FechaDonacion.Value;
            registro.Id = item.Id;
            registro.UsuarioBeneficiarioId = item.UsuarioBeneficiarioId;
            registro.Descripcion = item.Descripcion;
            registro.Cantidad = item.Cantidad;
            registro.Latitude = item.Latitude;
            registro.Longitude = item.Longitude;
            registro.Estado = item.Estado;
            registro.FileName = item.FileName;
            var donante = UserManager.Users.Where(x => x.Id.Equals(item.UsuarioId)).FirstOrDefault();
            string nombreDonante = "";
            if (donante != null && donante.ClienteId != null)
            {
                Cliente cliente = db.Cliente.Where(x => x.Id == donante.ClienteId).FirstOrDefault();
                nombreDonante = cliente.FullName;
            }
            else
            {
                nombreDonante = donante.UserName;
            }
            registro.Donante = nombreDonante;

            var beneficiario = UserManager.Users.Where(x => x.Id.Equals(item.UsuarioId)).FirstOrDefault();
            string nombreBeneficiario = "";
            if (beneficiario != null && beneficiario.ClienteId != null)
            {
                Cliente cliente = db.Cliente.Where(x => x.Id == donante.ClienteId).FirstOrDefault();
                nombreBeneficiario = cliente.FullName;
            }
            else
            {
                nombreBeneficiario = donante.UserName;
            }
            registro.Beneficiario = nombreBeneficiario;
            registro.ContentType = item.ContentType;
            registro.FileType = item.FileType;
            ViewBag.UsuarioLista = GetUsuarios(registro.UsuarioBeneficiarioId.Trim());
            return View(registro);
        }

        // POST: Donacion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RegistroDonacion registro, HttpPostedFileBase upload)
        {
            string usuarioBeneficiario = Convert.ToString(Request.Form["UsuarioBeneficiarioId"]);
            try
            {
                if (ModelState.IsValid)
                {
                    using (ApplicationDbContext context = new ApplicationDbContext())
                    {
                        using (var transaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                Donacion donacion = db.Donacion.Where(x => x.Id == registro.Id).FirstOrDefault();
                                db.Entry(donacion).State = EntityState.Modified;
                                donacion.FechaDonacion = DateTime.Now;
                                donacion.Descripcion = registro.Descripcion;
                                donacion.Cantidad = registro.Cantidad;
                                donacion.Longitude = registro.Longitude;
                                donacion.Latitude = registro.Latitude;
                                donacion.Estado = registro.Estado;
                                donacion.UsuarioBeneficiarioId = usuarioBeneficiario;
                                if (string.IsNullOrEmpty(donacion.UsuarioBeneficiarioId)) throw new Exception("Elija una fundación beneficiaria");
                                if (upload != null)
                                {
                                    //throw new Exception("Elija una imagen para continuar");
                                    donacion.FileName = System.IO.Path.GetFileName(upload.FileName);
                                    donacion.FileType = FileType.Logo;
                                    donacion.ContentType = upload.ContentType;
                                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                                    {
                                        donacion.Content = reader.ReadBytes(upload.ContentLength);
                                    }
                                }
                                db.SaveChanges();
                                transaction.Commit();
                                return RedirectToAction("Index");
                            }
                            catch (DbEntityValidationException e)
                            {
                                transaction.Rollback();
                                foreach (var eve in e.EntityValidationErrors)
                                {
                                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                    foreach (var ve in eve.ValidationErrors)
                                    {
                                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                                        ModelState.AddModelError("CustomError", ve.ErrorMessage);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                ModelState.AddModelError("CustomError", ex.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("CustomError", exception.Message);
            }
            ViewBag.UsuarioLista = GetUsuarios(registro.UsuarioBeneficiarioId.Trim());
            return View(registro);
        }


        public ActionResult Recibir(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donacion item = db.Donacion.Find(id);
            RegistroDonacion registro = new RegistroDonacion();
            registro.FechaDonacion = item.FechaDonacion.Value;
            registro.Id = item.Id;
            registro.UsuarioBeneficiarioId = item.UsuarioBeneficiarioId;
            registro.Descripcion = item.Descripcion;
            registro.Cantidad = item.Cantidad;
            registro.Latitude = item.Latitude;
            registro.Longitude = item.Longitude;
            registro.Estado = item.Estado;
            registro.FileName = item.FileName;
            var donante = UserManager.Users.Where(x => x.Id.Equals(item.UsuarioId)).FirstOrDefault();
            string nombreDonante = "";
            if (donante != null && donante.ClienteId != null)
            {
                Cliente cliente = db.Cliente.Where(x => x.Id == donante.ClienteId).FirstOrDefault();
                nombreDonante = cliente.FullName;
            }
            else
            {
                nombreDonante = donante.UserName;
            }
            registro.Donante = nombreDonante;

            var beneficiario = UserManager.Users.Where(x => x.Id.Equals(item.UsuarioId)).FirstOrDefault();
            string nombreBeneficiario = "";
            if (beneficiario != null && beneficiario.ClienteId != null)
            {
                Cliente cliente = db.Cliente.Where(x => x.Id == donante.ClienteId).FirstOrDefault();
                nombreBeneficiario = cliente.FullName;
            }
            else
            {
                nombreBeneficiario = donante.UserName;
            }
            registro.Beneficiario = nombreBeneficiario;
            registro.ContentType = item.ContentType;
            registro.FileType = item.FileType;            
            return View(registro);
        }

        // POST: Donacion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Recibir(RegistroDonacion registro)
        {            
            try
            {
                if (ModelState.IsValid)
                {
                    using (ApplicationDbContext context = new ApplicationDbContext())
                    {
                        using (var transaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                Donacion donacion = db.Donacion.Where(x => x.Id == registro.Id).FirstOrDefault();
                                db.Entry(donacion).State = EntityState.Modified;
                                donacion.FechaRecepcion = DateTime.Now;
                                donacion.Estado = "RECIBIDO";
                                db.SaveChanges();
                                transaction.Commit();
                                return RedirectToAction("Index");
                            }
                            catch (DbEntityValidationException e)
                            {
                                transaction.Rollback();
                                foreach (var eve in e.EntityValidationErrors)
                                {
                                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                    foreach (var ve in eve.ValidationErrors)
                                    {
                                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                                        ModelState.AddModelError("CustomError", ve.ErrorMessage);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                ModelState.AddModelError("CustomError", ex.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("CustomError", exception.Message);
            }            
            return View(registro);
        }

        // POST: Donacion/Delete/5
        [HttpPost]
        public bool Delete(int id, string estado)
        {
            try
            {

                Donacion Donacion = db.Donacion.Find(id);
                Donacion.Estado = (estado.Equals("INA")) ? "ACT" : "INA";
                db.Entry(Donacion).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set { _roleManager = value; }
        }

        public ActionResult GetImagen(int? id)
        {
            Donacion registro = null;
            registro = db.Donacion.Where(x => x.Id == id).FirstOrDefault();
            if (registro != null)
            {
                if (registro.Content != null)
                {
                    return File(registro.Content, registro.ContentType);
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
