using Authen.Application.Queries;
using Core.Infrastructure.Services;
using Infrastructure.EF;
using System.Reflection;

namespace Authen.API.Configurations
{
    public static class DIAssemblies
    {
        internal static Assembly[] AssembliesToScan = new Assembly[]
        {
            Assembly.GetExecutingAssembly(),
            Assembly.GetAssembly(typeof(BaseDbContext)),
            Assembly.GetAssembly(typeof(BaseService)),
            Assembly.GetAssembly(typeof(TestQuery))
        };
    }
}
