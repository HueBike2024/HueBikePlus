using System.Threading.Tasks;

namespace Core.Interfaces.Html
{
    public interface ITemplateService
    {
        Task<string> GetTemplateHtmlAsStringAsync<T>(string viewName, T model) where T : class, new();
    }
}
