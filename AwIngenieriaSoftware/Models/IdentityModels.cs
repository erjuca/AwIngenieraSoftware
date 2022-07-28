using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
namespace AwIngenieriaSoftware.Models
{
    // Para agregar datos de perfil del usuario, agregue más propiedades a su clase ApplicationUser. Visite https://go.microsoft.com/fwlink/?LinkID=317594 para obtener más información.
    public class ApplicationUser : IdentityUser
    {
        
        public int? ClienteId { get; set; }
        public string ClienteNombre { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tenga en cuenta que el valor de authenticationType debe coincidir con el definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Agregar aquí notificaciones personalizadas de usuario
            //userIdentity.AddClaim(new Claim("FullName", userIdentity.));
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //public virtual DbSet<Articulo> Articulo { get; set; }
        public virtual DbSet<Catalogo> Catalogo { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        //public virtual DbSet<CotizacionCabecera> CotizacionCabecera { get; set; }
        //public virtual DbSet<CotizacionDetalle> CotizacionDetalle { get; set; }        
        public virtual DbSet<Menu> Menu { get; set; }
        //public virtual DbSet<OrdenTrabajo> OrdenTrabajo { get; set; }
        //public virtual DbSet<Proveedor> Proveedor { get; set; }
        //public virtual DbSet<ProveedorArticulo> ProveedorArticulo { get; set; }
        //public virtual DbSet<Sugerencia> Sugerencia { get; set; }
        //public virtual DbSet<Vehiculo> Vehiculo { get; set; }
        public virtual DbSet<MenuRol> MenuRol { get; set; }
        //public virtual DbSet<PersonalTecnico> PersonalTecnico { get; set; }
        //public virtual DbSet<TrabajoCabecera> TrabajoCabecera { get; set; }
        //public virtual DbSet<TrabajoDetalle> TrabajoDetalle { get; set; }
        //public virtual DbSet<AyudaUsuario> AyudaUsuario { get; set; }
        public virtual DbSet<Empresa> Empresa { get; set; }
        public virtual DbSet<Animal> Animal { get; set; }
        public virtual DbSet<Abandono> Abandono { get; set; }
        public virtual DbSet<Donacion> Donacion { get; set; }
        public virtual DbSet<Rescate> Rescate { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection"/*, throwIfV1Schema: false*/)
        {
            //Configuration.ProxyCreationEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.IncludeMetadataInDatabase = false;
            modelBuilder.Conventions.AddFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();


            base.OnModelCreating(modelBuilder);            
            //modelBuilder.Entity<Articulo>().Ignore(x => x.NombreProveedor);
            //modelBuilder.Entity<Articulo>().Ignore(x => x.ProveedorId);
            modelBuilder.Entity<Menu>().Ignore(x => x.Estado);
            modelBuilder.Entity<Menu>().Ignore(x => x.Root);
            //modelBuilder.Entity<Menu>().HasRequired<Menu>(s => s.MenuPadre).WithMany(g => g.MenuLista).HasForeignKey<int>(s => s.MenuPadre);
            modelBuilder.Entity<ApplicationUser>().ToTable("Usuario");
            modelBuilder.Entity<IdentityRole>().ToTable("Rol");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("ReclamoUsuario");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UsuarioSesion");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UsuarioRol").HasKey(k => new { k.RoleId, k.UserId });            

            ////modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaim");
            ////modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserToken");
        }
    }
}
