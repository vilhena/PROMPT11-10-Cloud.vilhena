using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Ana.IoC;
using StructureMap;

namespace Ana.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                "WebApiCardDone",
                "api/Card/{id}/done",
                new { action = "Done" });

            
            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                            "WebApi",
                            "api/{controller}/{id}",
                            new {id = RouteParameter.Optional});


            routes.MapRoute(
                "CreateCardActions", // Route name
                "Board/{board_url_name}/{board_id}/CreateCard/", // URL with parameters
                new { controller = "Card", action = "Create" } // Parameter defaults
            );

            routes.MapRoute(
                "Index", // Route name
                "Boards", // URL with parameters
                new { controller = "Board", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
                "CustomActionsWithUser", // Route name
                "{user_name}/Shares/Board/{url_name}/{id}/{action}", // URL with parameters
                new { action = "Details", controller="Board" } // Parameter defaults
            );

            routes.MapRoute(
                "CustomActions", // Route name
                "{controller}/{url_name}/{id}/{action}", // URL with parameters
                new { action = "Details" } // Parameter defaults
            );


            routes.MapRoute(
                "UserIndex", // Route name
                "{user_name}/Boards", // URL with parameters
                new { action = "Details" } // Parameter defaults
            );

            routes.MapRoute(
                "UserCustomActions", // Route name
                "{user_name}/{controller}/{url_name}/{id}/{action}", // URL with parameters
                new { action = "Details" } // Parameter defaults
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {

            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
                configSetter(RoleEnvironment.GetConfigurationSettingValue(configName)));

            
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            IoC.DependencyController.ConfigureForMVC();

        }
    }
}