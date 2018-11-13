using Fluid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Reporting
{
    public class HtmlViewEngine
    {
        public async Task<string> RenderAsync(object model, string viewName = "index")
        {
            var htmlBody = await File.ReadAllTextAsync($"Views\\{viewName}.html");
            if (FluidTemplate.TryParse(htmlBody, out var template))
            {
                var context = new TemplateContext();
                context.MemberAccessStrategy.Register(model.GetType()); 
                context.SetValue("model", model);
                
                return template.Render(context);
            }
            return string.Empty;
        }
    }
}
