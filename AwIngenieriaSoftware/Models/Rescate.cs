using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AwIngenieriaSoftware.Models
{
    public class Rescate
    {
        public int Id { get; set; }

        public DateTime FechaRescate { get; set; }

        public int AnimalId { get; set; }

        public string UsuarioId { get; set; }
        

        #region Datos de Imagen
        [StringLength(255)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public FileType FileType { get; set; }
        #endregion


        [Required]
        [StringLength(256, MinimumLength = 5, ErrorMessage = "El campo {0} debe tener una longitud mínima de {2} y máxima de {1}")]
        public string EstadoSalud { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El campo {0} debe tener una longitud mínima de {2} y máxima de {1}")]
        public string Estado { get; set; }
    }

    public class ImagenRescate
    {
        public string type { get; set; }
        public string base64 { get; set; }
    }
}