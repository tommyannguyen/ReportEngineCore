using System.Threading.Tasks;

namespace Reporting.HtmlEngine
{
    public interface IHtmlRenderEngine
    {
        Task<string> RenderAsync(object model, string template);
    }
}
