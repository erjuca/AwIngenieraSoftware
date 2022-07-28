using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AwIngenieriaSoftware.Models
{
    [Authorize]
    public class ClienteController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cliente
        public ActionResult Index()
        {
            return View(db.Cliente.ToList());
        }

        // GET: Cliente/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            //cliente.vehiculos = db.Vehiculo.Where(x => x.Cliente.Id == cliente.Id).ToList();
            //List<OrdenTrabajo> ordenes = db.OrdenTrabajo.Where(x => x.CotizacionCabecera.Cliente.Id == id).ToList();            
            //ordenes.ForEach(x => x.CotizacionCabecera.Trabajos = db.TrabajoCabecera.Where(y => y.CotizacionCabecera.Id == x.Id).ToList());
            //ordenes.ForEach(x => x.CotizacionCabecera.Trabajos.ForEach(y => y.Detalle = db.TrabajoDetalle.Where(z => z.TrabajoCabecera.Id== y.Id).ToList()));
            //cliente.ordenes = ordenes;
            //if (cliente == null)
            //{
            //    return HttpNotFound();
            //}
            return View(cliente);
        }

        // GET: Cliente/Create
        public ActionResult Create()
        {
            ViewBag.Estados = new SelectList(db.Catalogo.Where(x => x.Codigo == 1 && !x.Argumento.Trim().Equals("")), "Valor", "Descripcion", "ACT");
            ViewBag.Generos = new SelectList(db.Catalogo.Where(x => x.Codigo == 4 && !x.Argumento.Trim().Equals("")), "Valor", "Descripcion");
            return View();
        }

        // POST: Cliente/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Identificacion,Nombres,Apellidos,Direccion,Telefono,Genero,Mail,Estado")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                if (!db.Cliente.Any(x => x.Identificacion.Equals(cliente.Identificacion.Trim())))
                {
                    db.Cliente.Add(cliente);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Identificacion", "El cliente ya existe.");
                }

            }
            ViewBag.Estados = new SelectList(db.Catalogo.Where(x => x.Codigo == 1 && !x.Argumento.Trim().Equals("")), "Valor", "Descripcion", "ACT");
            ViewBag.Generos = new SelectList(db.Catalogo.Where(x => x.Codigo == 4 && !x.Argumento.Trim().Equals("")), "Valor", "Descripcion");
            return View(cliente);
        }


        // GET: Cliente/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.Estados = new SelectList(db.Catalogo.Where(x => x.Codigo == 1 && !x.Argumento.Trim().Equals("")), "Valor", "Descripcion", cliente.Estado);
            ViewBag.Generos = new SelectList(db.Catalogo.Where(x => x.Codigo == 4 && !x.Argumento.Trim().Equals("")), "Valor", "Descripcion", cliente.Genero);
            return View(cliente);
        }

        // POST: Cliente/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Identificacion,Nombres,Apellidos,Direccion,Telefono,Genero,Mail,Estado")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {

                if (!db.Cliente.Any(x => x.Identificacion.Equals(cliente.Identificacion.Trim()) && x.Id != cliente.Id))
                {
                    db.Entry(cliente).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Identificacion", "El cliente ya existe.");
                }
            }
            ViewBag.Estados = new SelectList(db.Catalogo.Where(x => x.Codigo == 1 && !x.Argumento.Trim().Equals("")), "Valor", "Descripcion", cliente.Estado);
            ViewBag.Generos = new SelectList(db.Catalogo.Where(x => x.Codigo == 4 && !x.Argumento.Trim().Equals("")), "Valor", "Descripcion", cliente.Genero);
            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost]
        public bool Delete(int id, string estado)
        {
            try
            {

                Cliente cliente = db.Cliente.Find(id);
                cliente.Estado = (estado.Equals("INA")) ? "ACT" : "INA";
                db.Entry(cliente).State = EntityState.Modified;
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
