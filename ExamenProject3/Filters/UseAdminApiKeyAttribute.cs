using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExamenProject3.Filters
{
    public class UseAdminApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //hämta key från appsettings.json
            var apiKey = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetValue<string>("ApiKeys:AdminApiKey");


            //Headers means te key will nor be seen in the url 
            //kode nedan beskrivervad som händer om vi inte hittar code som paramaeter i Urlen 
            if (!context.HttpContext.Request.Headers.TryGetValue("key", out var key))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            //koden nedan beskriver vad som händer om nyckeln inte stämmer överens med det vi har i vår apiKey
            if (!apiKey.Equals(key))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            await next();
        }
    }
}
