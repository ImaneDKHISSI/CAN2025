using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Projet_pfa.Filters
{
    public class AuthentificationFilter:ActionFilterAttribute
    {
        public string role { get; set; }
        public string role2 { get; set; }
        public AuthentificationFilter(string role)
        {

            this.role = role;

        }
        public AuthentificationFilter(string role,string role2)
        {

            this.role = role;
            this.role2 = role2;

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if(role !=null && role2 != null)
            {
                if(context.HttpContext.Session.GetString("IdUtilisateur") == null)
                {
                    context.Result = new RedirectResult("/Utilisateurs/Authentification");
                }
            }
            else if(context.HttpContext.Session.GetString("IdUtilisateur") == null || context.HttpContext.Session.GetString("role") == null || context.HttpContext.Session.GetString("role") != role)
            {
               context.Result = new RedirectResult("/Utilisateurs/Authentification");
            }
        }
    }
}
