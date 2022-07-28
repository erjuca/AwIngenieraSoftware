using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AwIngenieriaSoftware.Models
{
    public partial class Menu
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Tipo de menú")]
        public string Tipo { get; set; }
        [Display(Name = "Nombre menú")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Nombre { get; set; }
        [Display(Name = "Controlador")]
        public string Controlador { get; set; }
        [Display(Name = "Página Inicial")]
        public string Accion { get; set; }
        [Display(Name = "Menú Padre")]
        public Nullable<int> MenuPadreId { get; set; }
        public virtual Menu MenuPadre { get; set; }
        [Display(Name = "Ícono")]
        public string Icono { get; set; }
        public string Estado { get; set; }

        
        public String GetNombre {
            get {
                if (MenuPadre!=null)
                {
                    return MenuPadre.Nombre + " -> " + Nombre;
                }
                else {
                    return Nombre;
                }
            }
        }
        public ICollection<Menu> Items { get; set; }
        public int Root { get; set; }
    }
}