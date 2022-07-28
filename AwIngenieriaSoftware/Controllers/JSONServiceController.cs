using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AwIngenieriaSoftware.Models;

namespace AwIngenieriaSoftware.Services
{
    public class JSONServiceController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //public JsonResult BuscarProveedor(String filtro)
        //{
        //    return Json(db.Proveedor.Where(x => (x.Ruc.Contains(filtro) || x.NombreComercial.Contains(filtro) || x.RepresentanteLegal.Contains(filtro)) && x.Estado.Equals("ACT")).ToList(), JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult BuscarArticulo(String filtro)
        //{
        //    return Json(db.Articulo.Where(x => (x.Marca.Contains(filtro) || x.Descripcion.Contains(filtro)) && x.Estado.Equals("ACT")).ToList(), JsonRequestBehavior.AllowGet);
        //}

        public JsonResult BuscarCliente(String filtro)
        {
            return Json(db.Cliente.Where(x => (x.Nombres.Contains(filtro) || x.Apellidos.Contains(filtro) || x.Identificacion.Contains(filtro)) && x.Estado.Equals("ACT")).ToList(), JsonRequestBehavior.AllowGet);
        }

        //public JsonResult BuscarVehiculo(int clienteId, String filtro)
        //{
        //    return Json(db.Vehiculo.Where(x => x.Cliente.Id == clienteId && x.Estado.Equals("ACT") && x.Matricula.Contains(filtro)).ToList(), JsonRequestBehavior.AllowGet);
        //}
        public JsonResult ObtenerClientes()
        {
            return Json(db.Cliente.Where(x => x.Estado.Equals("ACT")).ToList(), JsonRequestBehavior.AllowGet);
        }
        //public JsonResult ObtenerVehiculos(int clienteId)
        //{
        //    return Json(db.Vehiculo.Where(x => x.Cliente.Id == clienteId && x.Estado.Equals("ACT")).ToList(), JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult ObtenerArticulos()
        //{
        //    return Json(db.Articulo.Where(x => x.Estado.Equals("ACT")).ToList(), JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult ObtenerTecnicos()
        //{
        //    return Json(db.PersonalTecnico.Where(x => x.Estado.Equals("ACT")).ToList(), JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult BuscarCotizacion(String filtro)
        //{
        //    var result = db.CotizacionCabecera.Where(x => (x.Id.ToString().Contains(filtro)
        //    || x.Cliente.Identificacion.Contains(filtro)) && !x.Estado.Equals("ANU")).ToList();

        //    var lista = new List<JsonCotizacion>();
        //    foreach (var item in result)
        //    {
        //        lista.Add(new JsonCotizacion()
        //        {
        //            Id = item.Id,
        //            Cliente = item.Cliente.Nombres + " " + item.Cliente.Apellidos,
        //        });
        //    }
        //    return Json(lista, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult BuscarPersonalTecnico(String filtro)
        //{
        //    var lista = db.PersonalTecnico.Where(x => (x.Identificacion.Contains(filtro) || x.Nombres.Contains(filtro) || x.Apellidos.Contains(filtro)) && x.Estado.Equals("ACT")).ToList();
        //    return Json(lista, JsonRequestBehavior.AllowGet);
        //}


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
    public class JsonCotizacion
    {
        public int Id { get; set; }
        public String Cliente { get; set; }
    }
}