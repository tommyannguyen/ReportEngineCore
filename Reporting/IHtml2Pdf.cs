namespace Reporting
{
    public interface IHtml2Pdf
    {
        byte[] ExportFromHtml(string html);
        byte[] ExportFromUri(string uri);
    }
}