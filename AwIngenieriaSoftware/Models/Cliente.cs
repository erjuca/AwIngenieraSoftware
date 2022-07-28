using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AwIngenieriaSoftware.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Identificacion]

        public string Identificacion { get; set; }

        [StringLength(30, MinimumLength = 5, ErrorMessage = "El campo {0} debe tener una longitud mínima de {2} y máxima de {1}")]
        public string Nombres { get; set; }

        [StringLength(30, MinimumLength = 5, ErrorMessage = "El campo {0} debe tener una longitud mínima de {2} y máxima de {1}")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Ingrese un teléfono válido.")]
        public string Telefono { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Mail { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Género")]
        public string Genero { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Estado { get; set; }


        //public ICollection<Vehiculo> vehiculos { set; get; }
        //public ICollection<OrdenTrabajo> ordenes { set; get; }

        [Display(Name = "Cliente")]
        public string FullName
        {
            get
            {
                return Nombres + " " + Apellidos;
            }
        }
        [Display(Name = "Cliente")]
        public string FullName2
        {
            get
            {
                return Identificacion+" - "+ Nombres + " " + Apellidos;
            }
        }
    }
}