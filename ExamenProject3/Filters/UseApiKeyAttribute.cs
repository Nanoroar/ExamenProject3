using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExamenProject3.Filters
{
    public class UseApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        //add async because we have a task
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //hämta key från appsettings.json
            var apiKey = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetValue<string>("ApiKeys:ApiKey");


            //Query = parameter i url  ex. https://domain.com/api/products?code=f325c406-4758-43f4-9253-1b7b4b7ce8de
            //kode nedan beskrivervad som händer om vi inte hittar code som paramaeter i Urlen 
            if (!context.HttpContext.Request.Query.TryGetValue("key", out var key))
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
