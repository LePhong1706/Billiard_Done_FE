
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BilliardShop.Web.Controllers;

public static class ControllerExtensions
{
    public static async Task<string> RenderViewToStringAsync(this Controller controller, string viewName, object model)
    {
        controller.ViewData.Model = model;
        using (var sw = new StringWriter())
        {
            IViewEngine? viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
            if (viewEngine == null)
            {
                return "A view engine could not be found";
            }
            ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);

            if (viewResult.Success == false)
            { 
                return $"A view with the name {viewName} could not be found";
            }

            ViewContext viewContext = new ViewContext(
                controller.ControllerContext,
                viewResult.View,
                controller.ViewData,
                controller.TempData,
                sw,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);

            return sw.ToString();
        }
    }
}
