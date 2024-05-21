using Core.Interfaces.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Core.Implements.Html
{
    public class TemplateService : ITemplateService
    {
        private IRazorViewEngine _razorViewEngine;
        private IServiceProvider _serviceProvider;
        private ITempDataProvider _tempDataProvider;

        public TemplateService(IRazorViewEngine engine, IServiceProvider serviceProvider, ITempDataProvider tempDataProvider)
        {
            _razorViewEngine = engine;
            _serviceProvider = serviceProvider;
            _tempDataProvider = tempDataProvider;
        }

        public async Task<string> GetTemplateHtmlAsStringAsync<T>(string viewPath, T model) where T : class, new()
        {
            var httpContext = new DefaultHttpContext() { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            using StringWriter sw = new();
            var viewResult = _razorViewEngine.FindView(actionContext, viewPath, false);

            if (viewResult.View == null)
            {
                return string.Empty;
            }

            var metadataProvider = new EmptyModelMetadataProvider();
            var msDictionary = new ModelStateDictionary();
            var viewDataDictionary = new ViewDataDictionary(metadataProvider, msDictionary)
            {
                Model = model
            };

            var tempDictionary = new TempDataDictionary(actionContext.HttpContext, _tempDataProvider);
            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewDataDictionary,
                tempDictionary,
                sw,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return sw.ToString();
        }
    }
}
