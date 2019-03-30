using System.Threading.Tasks;

namespace Reporting
{
    public interface IHtmlRenderTask
    {
        Task<string> RenderAsync(object model, string template);
    }
}
