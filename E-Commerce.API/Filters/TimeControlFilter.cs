using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace E_Commerce.API.Filters
{
    public class TimeControlFilter : ActionFilterAttribute
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        { 
            var now = DateTime.Now.TimeOfDay;

            StartTime = "23:00";
            EndTime = "23:59";

            if (now >= TimeSpan.Parse(StartTime) && now <= TimeSpan.Parse(EndTime))
            {
                base.OnActionExecuting(context);
            }
            else
            {
                context.Result = new ContentResult
                {
                    Content = "Cannot request this end-point now. Try to request between 23:00 and 23:59.",
                    StatusCode = 403
                };
            }
        }
    }
}
