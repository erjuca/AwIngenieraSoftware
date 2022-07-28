using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AwIngenieriaSoftware.Models
{
    public class Usuario
    {
        public string Id { get; set; }

        //[Required(ErrorMessage = "El campo {0} es requerido.")]
        //[EmailAddress]
        public string Email { get; set; }
        
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]        
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Finalización Bloqueo")]
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nombre usuario")]
        public string UserName { get; set; }        

        public string RolId { get; set; }

        [Display(Name = "Accesos fallidos")]
        public int AccessFailedCount { get; set; }

        [Display(Name = "Bloqueo habilitado")]
        public bool LockoutEnabled { get; set; }
        
        [Display(Name = "Id Cliente")]
        public int? ClienteId { get; set; }

        [Display(Name = "Nombre Cliente")]
        public string ClienteNombre { get; set; }

        [Display(Name = "Cliente")]
        public IEnumerable<SelectListItem> Clientes { get; set; }

    }

    public class Rol
    {
        public string Id { get; set; }
        [Display(Name = "Rol")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Name { get; set; }
    }

    public class UsuarioRol
    {        
        [Display(Name = "Usuario")]     
        public Usuario Usuario { get; set; }
        [Display(Name = "Rol")]        
        public Rol Rol { get; set; }

        [Display(Name = "Usuario")]
        public string SelectedUsuario { get; set; }        

        [Display(Name = "Rol")]
        public string SelectedRol { get; set; }        
    }

}