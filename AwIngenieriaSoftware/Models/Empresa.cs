using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AwIngenieriaSoftware.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        [Identificacion]
        [Display(Name = "Ruc")]
        public string Ruc { get; set; }
        [Required(ErrorMessage="El campo {0} es requerido")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Teléfono")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Ingrese un teléfono válido.")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Mail { get; set; }


        #region Datos de Imagen
        [StringLength(255)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public FileType FileType { get; set; }        
        #endregion  
    }
    public class LogoEmpresa
    {
        public string type { get; set; }
        public string base64 { get; set; }
    }
}