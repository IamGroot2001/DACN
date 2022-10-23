using System.Web.Mvc;

namespace DoAn_Web_SellClothes.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "LogIn", controller = "Account", id = UrlParameter.Optional },
                namespaces: new[] { "DoAn_Web_SellClothes.Areas.Admin.Controllers"}
            ) ;
        }
    }
}