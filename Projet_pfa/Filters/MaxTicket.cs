using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Projet_pfa.Filters
{
    public class MaxTicket : ActionFilterAttribute
    {
        //public int TicketLimit { get; set; }
        public MaxTicket()
        {
            //TicketLimit = ticketLimit;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            int NbrTicketMadeByUser = int.Parse(context.HttpContext.Session.GetString("NbrTicketMadeByUser"));
            if (NbrTicketMadeByUser > 4)
            {
                // Ensure this redirect doesn't get caught in a loop
                if (!context.HttpContext.Request.Path.Value.Contains("BuyLimitExceeded"))
                {
                    context.Result = new RedirectResult("/Ticket/BuyLimitExceeded");
                }
            }
            
        }
    }
}
