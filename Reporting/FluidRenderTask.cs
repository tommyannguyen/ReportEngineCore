using Fluid;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Reporting
{
    public sealed class FluidRenderTask : IHtmlRenderTask
    {
        public const string _MODEL_ = "model";
        private readonly ILogger _logger;

        public FluidRenderTask(ReportContext reportContext)
        {
            _logger = reportContext.LoggerFactory.CreateLogger<FluidRenderTask>();
        }
        public async Task<string> RenderAsync(object model, string template)
        {
            try
            {
                if (FluidTemplate.TryParse(template, out var templateResult))
                {
                    var context = new TemplateContext();
                    context.MemberAccessStrategy.Register(model.GetType());
                    context.SetValue(_MODEL_, model);
                    return await templateResult.RenderAsync(context);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("Error html parser", ex);
            }
            return string.Empty;
        }
    }
}
