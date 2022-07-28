using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AwIngenieriaSoftware.Models
{
    public class MenuRol
    {
        //public int MyProperty { get; set; }

        [Key]
        [Column(Order = 0)]
        [Display(Name = "Menu")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int MenuId { get; set; }        
        public virtual Menu Menu { get; set; }


        [Key]
        [Column(Order = 1)]
        [Display(Name = "Rol")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string RolId { get; set; }        
        public virtual IdentityRole Rol { get; set; }

        public virtual ICollection<Menu> MenuLista { get; set; }
        public virtual ICollection<IdentityRole> RolLista { get; set; }
    }
}