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
    public class AnimalController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Animal
        public ActionResult Index()
        {
            var usuarioId = User.Identity.GetUserId();
            var roles = UserManager.GetRoles(usuarioId);
            string rolName = roles[0];
            BusquedaCabeceraAnimal busquedaCabecera = new BusquedaCabeceraAnimal();
            List<RegistroAbondonoAnimal> listaRegistro = new List<RegistroAbondonoAnimal>();
            try
            {

                List<Animal> listaAnimales = null;
                if (!rolName.Equals("FUNDACION"))
                {
                    listaAnimales = db.Animal.Where(x => x.UsuarioId.Equals(usuarioId)).ToList();
                }
                else
                {
                    listaAnimales = db.Animal.ToList();
                }
                foreach (var item in listaAnimales)
                {
                    RegistroAbondonoAnimal registro = new RegistroAbondonoAnimal();
                    if (!rolName.Equals("FUNDACION") && !item.Estado.Equals("RESCATADO"))
                    {
                        registro.Editable = true;
                    }
                    else
                    {
                        registro.Editable = false;
                    }
                    if (rolName.Equals("FUNDACION"))
                    {
                        if (item.Estado.Equals("ABANDONADO"))
                        {
                            registro.Rescate = true;
                        }
                        else
                        {
                            registro.Rescate = false;
                        }
                    }
                    else
                    {
                        registro.Rescate = false;
                    }

                    //registro.Rescate = true;
                    registro.Id = item.Id;
                    registro.Color = item.Color;
                    registro.Nombre = item.Nombre;
                    registro.Raza = item.Raza;
                    registro.Edad = item.Edad;
                    registro.Sexo = (item.Sexo.Equals("H")) ? "HEMBRA" : "MACHO";
                    registro.EstadoAbandono = item.Estado;
                    Abandono abandono = db.Abandono.Where(x => x.AnimalId == item.Id).FirstOrDefault();
                    if (abandono != null)
                    {
                        registro.EstadoSalud = abandono.EstadoSalud;
                        registro.Latitude = abandono.Latitude;
                        registro.Longitude = abandono.Longitude;
                        registro.FileName = abandono.FileName;
                        registro.ContentType = abandono.ContentType;
                        registro.FileType = abandono.FileType;
                    }
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

        // GET: Animal/Details/5
        public ActionResult Details(int? id)
        {
            RegistroAbondonoAnimal registro = new RegistroAbondonoAnimal();
            Animal item = db.Animal.Where(x => x.Id == id).FirstOrDefault();
            if (item != null)
            {
                registro.Id = item.Id;
                registro.Color = item.Color;
                registro.Nombre = item.Nombre;
                registro.Raza = item.Raza;
                registro.Edad = item.Edad;
                registro.Sexo = (item.Sexo.Equals("H")) ? "HEMBRA" : "MACHO";
                registro.EstadoAbandono = item.Estado;
                Abandono abandono = db.Abandono.Where(x => x.AnimalId == item.Id).FirstOrDefault();
                if (abandono != null)
                {
                    registro.EstadoSalud = abandono.EstadoSalud;
                    registro.Latitude = abandono.Latitude;
                    registro.Longitude = abandono.Longitude;
                    registro.FileName = abandono.FileName;
                    registro.ContentType = abandono.ContentType;
                    registro.FileType = abandono.FileType;
                }
                Rescate rescate = db.Rescate.Where(x => x.AnimalId == item.Id).FirstOrDefault();
                if (rescate != null)
                {
                    registro.FechaRescate = rescate.FechaRescate;
                    registro.EstadoSaludNuevo = rescate.EstadoSalud;

                    var fundacion = UserManager.Users.Where(x => x.Id.Equals(rescate.UsuarioId)).FirstOrDefault();
                    string nombreFundacion = "";
                    if (fundacion != null && fundacion.ClienteId != null)
                    {
                        Cliente cliente = db.Cliente.Where(x => x.Id == fundacion.ClienteId).FirstOrDefault();
                        nombreFundacion = cliente.FullName;
                    }
                    else
                    {
                        nombreFundacion = fundacion.UserName;
                    }
                    registro.FundacionRescate = nombreFundacion;
                }
            }
            return View(registro);
        }

        // GET: Animal/Create
        public ActionResult Create()
        {
            ViewBag.SexoLista = new SelectList(db.Catalogo.Where(x => x.Codigo == 11 && !x.Argumento.Trim().Equals("")), "Valor", "Descripcion", null);
            RegistroAbondonoAnimal registro = new RegistroAbondonoAnimal();
            registro.Latitude = (decimal)-2.897069;
            registro.Longitude = (decimal)-79.004671;
            return View(registro);
        }

        // POST: Animal/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "Id,Identificacion,Nombres,Apellidos,Direccion,Telefono,Genero,Mail,Estado")]*/ RegistroAbondonoAnimal registro, HttpPostedFileBase upload)
        {
            try
            {
                string sexo = Convert.ToString(Request.Form["Sexo"]);
                registro.Sexo = sexo;
                if (ModelState.IsValid)
                {
                    using (ApplicationDbContext context = new ApplicationDbContext())
                    {
                        using (var transaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                if (!db.Animal.Any(x => x.Nombre.Equals(registro.Nombre.Trim()) && x.Raza.Equals(registro.Raza) && x.Edad == registro.Edad))
                                {
                                    Animal animal = new Animal();
                                    animal.Nombre = registro.Nombre;
                                    animal.Raza = registro.Raza;
                                    animal.Edad = registro.Edad;
                                    animal.FechaRegistro = DateTime.Now;
                                    animal.Color = registro.Color;
                                    animal.Sexo = registro.Sexo;
                                    animal.Estado = "ABANDONADO";
                                    var idUser = UserManager.Users.Where(x => x.UserName.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Id;
                                    animal.UsuarioId = idUser;
                                    db.Animal.Add(animal);

                                    Abandono abandono = new Abandono();
                                    abandono.FechaRegistro = DateTime.Now;
                                    abandono.AnimalId = animal.Id;
                                    abandono.UsuarioId = System.Web.HttpContext.Current.User.Identity.Name;
                                    abandono.Latitude = registro.Latitude;
                                    abandono.Longitude = registro.Longitude;
                                    abandono.EstadoSalud = registro.EstadoSalud;
                                    abandono.Estado = "PENDIENTE";
                                    if (upload == null) throw new Exception("Elija una imagen para continuar");
                                    abandono.FileName = System.IO.Path.GetFileName(upload.FileName);
                                    abandono.FileType = FileType.Logo;
                                    abandono.ContentType = upload.ContentType;
                                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                                    {
                                        abandono.Content = reader.ReadBytes(upload.ContentLength);
                                    }
                                    db.Abandono.Add(abandono);
                                    db.SaveChanges();
                                    transaction.Commit();
                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    ModelState.AddModelError("CustomError", "El Animal ya existe.");
                                }
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
            ViewBag.SexoLista = new SelectList(db.Catalogo.Where(x => x.Codigo == 11 && !x.Argumento.Trim().Equals("")), "Valor", "Descripcion", registro.Sexo);
            return View(registro);
        }


        // GET: Animal/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistroAbondonoAnimal registro = new RegistroAbondonoAnimal();
            Animal item = db.Animal.Where(x => x.Id == id).FirstOrDefault();
            if (item != null)
            {
                registro.Id = item.Id;
                registro.Color = item.Color;
                registro.Nombre = item.Nombre;
                registro.Raza = item.Raza;
                registro.Edad = item.Edad;
                registro.Sexo = (item.Sexo.Equals("H")) ? "HEMBRA" : "MACHO";
                registro.EstadoAbandono = item.Estado;
                Abandono abandono = db.Abandono.Where(x => x.AnimalId == item.Id).FirstOrDefault();
                if (abandono != null)
                {
                    registro.IdDatosAdicional = abandono.Id;
                    registro.EstadoSalud = abandono.EstadoSalud;
                    registro.Latitude = abandono.Latitude;
                    registro.Longitude = abandono.Longitude;
                    registro.FileName = abandono.FileName;
                    registro.ContentType = abandono.ContentType;
                    registro.FileType = abandono.FileType;
                }
            }
            ViewBag.SexoLista = new SelectList(db.Catalogo.Where(x => x.Codigo == 11 && !x.Argumento.Trim().Equals("")), "Valor", "Descripcion", registro.Sexo.Trim());
            return View(registro);
        }

        // POST: Animal/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RegistroAbondonoAnimal registro, HttpPostedFileBase upload)
        {
            string sexo = Convert.ToString(Request.Form["Sexo"]);
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
                                Animal animal = db.Animal.Where(x => x.Id == registro.Id).FirstOrDefault();
                                db.Entry(animal).State = EntityState.Modified;
                                animal.Color = registro.Color;
                                animal.Nombre = registro.Nombre;
                                animal.Raza = registro.Raza;
                                animal.Edad = registro.Edad;
                                animal.Sexo = sexo;
                                animal.Estado = registro.EstadoAbandono;

                                Abandono abandono = db.Abandono.Where(x => x.Id == registro.IdDatosAdicional).FirstOrDefault();
                                db.Entry(abandono).State = EntityState.Modified;
                                abandono.FechaRegistro = DateTime.Now;
                                abandono.AnimalId = registro.Id;
                                abandono.UsuarioId = System.Web.HttpContext.Current.User.Identity.Name;
                                abandono.Latitude = registro.Latitude;
                                abandono.Longitude = registro.Longitude;
                                abandono.EstadoSalud = registro.EstadoSalud;
                                abandono.Estado = "PENDIENTE";
                                if (upload != null)
                                {
                                    //throw new Exception("Elija una imagen para continuar");
                                    abandono.FileName = System.IO.Path.GetFileName(upload.FileName);
                                    abandono.FileType = FileType.Logo;
                                    abandono.ContentType = upload.ContentType;
                                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                                    {
                                        abandono.Content = reader.ReadBytes(upload.ContentLength);
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
            ViewBag.SexoLista = new SelectList(db.Catalogo.Where(x => x.Codigo == 11 && !x.Argumento.Trim().Equals("")), "Valor", "Descripcion", registro.Sexo.Trim());
            return View(registro);
        }


        // GET: Animal/Edit/5
        public ActionResult Rescate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistroAbondonoAnimal registro = new RegistroAbondonoAnimal();
            Animal item = db.Animal.Where(x => x.Id == id).FirstOrDefault();
            if (item != null)
            {
                registro.Id = item.Id;
                registro.Color = item.Color;
                registro.Nombre = item.Nombre;
                registro.Raza = item.Raza;
                registro.Edad = item.Edad;
                registro.Sexo = item.Sexo.Trim();
                registro.EstadoAbandono = item.Estado;
                Abandono abandono = db.Abandono.Where(x => x.AnimalId == item.Id).FirstOrDefault();
                if (abandono != null)
                {
                    registro.IdDatosAdicional = abandono.Id;
                    registro.EstadoSalud = abandono.EstadoSalud;
                    registro.Latitude = abandono.Latitude;
                    registro.Longitude = abandono.Longitude;
                    registro.FileName = abandono.FileName;
                    registro.ContentType = abandono.ContentType;
                    registro.FileType = abandono.FileType;
                }
            }
            ViewBag.SexoLista = new SelectList(db.Catalogo.Where(x => x.Codigo == 11 && !x.Argumento.Trim().Equals("")), "Valor", "Descripcion", registro.Sexo.Trim());
            return View(registro);
        }

        // POST: Animal/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rescate(RegistroAbondonoAnimal registro, HttpPostedFileBase upload)
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
                                Animal animal = db.Animal.Where(x => x.Id == registro.Id).FirstOrDefault();
                                db.Entry(animal).State = EntityState.Modified;
                                animal.Estado = "RESCATADO";

                                Rescate rescate = new Rescate();
                                rescate.FechaRescate = DateTime.Now;
                                rescate.AnimalId = animal.Id;
                                rescate.UsuarioId = User.Identity.GetUserId();
                                rescate.Estado = "REGISTRADO";
                                rescate.EstadoSalud = registro.EstadoSaludNuevo;
                                if (upload != null)
                                {
                                    //throw new Exception("Elija una imagen para continuar");
                                    rescate.FileName = System.IO.Path.GetFileName(upload.FileName);
                                    rescate.FileType = FileType.Logo;
                                    rescate.ContentType = upload.ContentType;
                                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                                    {
                                        rescate.Content = reader.ReadBytes(upload.ContentLength);
                                    }
                                }
                                db.Rescate.Add(rescate);
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
            ViewBag.SexoLista = new SelectList(db.Catalogo.Where(x => x.Codigo == 11 && !x.Argumento.Trim().Equals("")), "Valor", "Descripcion", registro.Sexo.Trim());
            return View(registro);
        }

        // POST: Animal/Delete/5
        [HttpPost]
        public bool Delete(int id, string estado)
        {
            try
            {

                Animal Animal = db.Animal.Find(id);
                Animal.Estado = (estado.Equals("INA")) ? "ACT" : "INA";
                db.Entry(Animal).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // GET: obtiene ubicaciones para mapas
        [HttpGet]
        public ActionResult map(int animalId)
        {
            var q = from t in db.Abandono where t.AnimalId == animalId select new { t.Name, t.Latitude, t.Longitude, t.Description };
            //  return PartialView("_map", q.ToList());
            return Json(q, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetImagen(int? idAnimal)
        {
            Abandono registro = null;
            registro = db.Abandono.Where(x => x.AnimalId == idAnimal).FirstOrDefault();
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

        public ActionResult GetImagenRescate(int? idAnimal)
        {
            Rescate registro = null;
            registro = db.Rescate.Where(x => x.AnimalId == idAnimal).FirstOrDefault();
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
