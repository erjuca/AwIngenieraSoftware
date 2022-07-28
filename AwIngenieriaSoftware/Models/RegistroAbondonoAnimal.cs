using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AwIngenieriaSoftware.Models
{
    public class RegistroAbondonoAnimal
    {
        public int Id { get; set; }
        public int IdDatosAdicional { get; set; }

        //[Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener una longitud mínima de {2} y máxima de {1}")]
        public string Nombre { get; set; }

        //[Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener una longitud mínima de {2} y máxima de {1}")]
        public string Raza { get; set; }

        //[Required]        
        public int Edad { get; set; }        

        //[Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener una longitud mínima de {2} y máxima de {1}")]
        public string Color { get; set; }

        //[Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener una longitud mínima de {2} y máxima de {1}")]
        public string Sexo { get; set; }

        //public int UsuarioId { get; set; }

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
        public bool Editable { get; set; }
        public bool Rescate { get; set; }
        #endregion


        [Required]
        [StringLength(256, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener una longitud mínima de {2} y máxima de {1}")]
        public string EstadoSalud { get; set; }

        public string EstadoAbandono { get; set; }

        public string EstadoSaludNuevo { get; set; }

        public int IdRescate{ get; set; }
        public DateTime FechaRescate{ get; set; }
        public string FundacionRescate{ get; set; }


    }

    public class ImagenAbandonoRegistro
    {
        public string type { get; set; }
        public string base64 { get; set; }
    }

    public class BusquedaCabeceraAnimal
    {
        public string Filtro { get; set; }
        public List<RegistroAbondonoAnimal> detalle { get; set; }
    }
}