using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AwIngenieriaSoftware.Models
{
    public class Animal
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener una longitud mínima de {2} y máxima de {1}")]
        public string Nombre { get; set; }


        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener una longitud mínima de {2} y máxima de {1}")]
        public string Raza { get; set; }

        [Required]        
        public int Edad { get; set; }
        
        public DateTime FechaRegistro { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener una longitud mínima de {2} y máxima de {1}")]
        public string Color { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener una longitud mínima de {2} y máxima de {1}")]
        public string Sexo { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener una longitud mínima de {2} y máxima de {1}")]
        public string Estado { get; set; }

        public string UsuarioId { get; set; }

    }
}