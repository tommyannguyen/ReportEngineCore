using Fluid;
using Reporting.HtmlEngine;
using System.Threading.Tasks;

namespace Reporting
{
    public sealed class HtmlViewEngine : IHtmlRenderEngine
    {
        public const string _MODEL_ = "model"; 
        public async Task<string> RenderAsync(object model, string template)
        {
            if (FluidTemplate.TryParse(template, out var templateResult))
            {
                var context = new TemplateContext();
                context.MemberAccessStrategy.Register(model.GetType()); 
                context.SetValue(_MODEL_, model);         
                return await templateResult.RenderAsync(context);
            }
            return string.Empty;
        }
    }
}
