using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AwIngenieriaSoftware.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Código")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "¿Recordar este explorador?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "¿Recordar cuenta?")]
        public bool RememberMe { get; set; }
        
        public string FullName { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nombre usuario")]
        public string UserName { get; set; }

        //[Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required]        
        [Display(Name = "Identificación de Cliente")]
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
        
    }

    public class RegisterViewModel2
    {

        //DATOS CLIENTE
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

        
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Género")]
        public string Genero { get; set; }

        //[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Rol")]
        public string Rol { get; set; }


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
                return Identificacion + " - " + Nombres + " " + Apellidos;
            }
        }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nombre usuario")]
        public string UserName { get; set; }

        //[Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }
    }
}
