using TallentexResult.Entities;

namespace TallentexResult.Interface
{
    public interface ISMSService
    {
        bool Send(int templateid, string to, HttpRequest request);
        bool Send(string to, string message, string appname, HttpRequest request);
    }
}
