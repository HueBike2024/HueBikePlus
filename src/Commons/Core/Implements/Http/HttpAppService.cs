using Microsoft.Extensions.DependencyInjection;
using System;

namespace Core.Implements.Http
{
    public static class HttpAppService
    {
        private static IServiceProvider _serviceProvider;

        /// <summary>
        /// Get Application Collection Service
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void Configure(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static IServiceProvider ServiceProvider => _serviceProvider;

        public static TService GetRequestService<TService>()
        {
            return HttpAppContext.Current.RequestServices.GetService<TService>();
        }
    }
}
