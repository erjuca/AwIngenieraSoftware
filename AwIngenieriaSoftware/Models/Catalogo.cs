using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AwIngenieriaSoftware.Models
{
    public class Catalogo
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Código")]
        public int Codigo { get; set; }
        public string Argumento { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public string Valor { get; set; }
        public string Tipo { get; set; }

        //[NotMapped]
        //public bool IsAdministrator { set; get; }
    }

}