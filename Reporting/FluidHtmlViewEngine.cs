using Fluid;
using Microsoft.Extensions.Logging;
using Reporting.HtmlEngine;
using System;
using System.Threading.Tasks;

namespace Reporting
{
    public sealed class FluidHtmlViewEngine : IHtmlRenderEngine
    {
        public const string _MODEL_ = "model";
        private readonly ILogger _logger;

        public FluidHtmlViewEngine(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FluidHtmlViewEngine>();
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
