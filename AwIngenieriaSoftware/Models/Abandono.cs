using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AwIngenieriaSoftware.Models
{
    public class Abandono
    {
        public int Id { get; set; }

        public DateTime FechaRegistro { get; set; }

        public int AnimalId { get; set; }

        public string UsuarioId{ get; set; }


        #region Datos de Imagen
        [StringLength(255)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public FileType FileType { get; set; }
        #endregion

        #region Datos ubicacion
        [Display(Name = "Nombre ubicación"), MaxLength(120, ErrorMessage = "El nombre de ubicación es muy largo.")]
        public string Name { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string Description { get; set; }
        public string Tenure { get; set; }
        [DisplayFormat(DataFormatString = "{0:N10}", ApplyFormatInEditMode = true)]
        public decimal Longitude { get; set; }
        [DisplayFormat(DataFormatString = "{0:N10}", ApplyFormatInEditMode = true)]
        public decimal Latitude { get; set; }
        #endregion
        
        public string Estado{ get; set; }
        public string EstadoSalud{ get; set; }
        [Display(Name = "Animal")]
        public virtual Animal Animal { get; set; }

    }

    public class ImagenAnimal
    {
        public string type { get; set; }
        public string base64 { get; set; }
    }
}