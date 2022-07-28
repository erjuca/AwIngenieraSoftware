using Microsoft.Owin;
using Owin;


[assembly: OwinStartupAttribute(typeof(AwIngenieriaSoftware.Startup))]
namespace AwIngenieriaSoftware
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            //app.MapSignalR();
            ConfigureAuth(app);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            
        }
       
    }
}
