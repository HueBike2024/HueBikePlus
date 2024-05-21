using MasterData.Application.Queries;
using Core.Infrastructure.Services;
using Infrastructure.EF;
using System.Reflection;

namespace MasterData.API.Configurations
{
    public static class DIAssemblies
    {
        public static Assembly[] AssembliesToScan = new Assembly[]
        {
            Assembly.GetExecutingAssembly(),
            Assembly.GetAssembly(typeof(BaseDbContext)),
            Assembly.GetAssembly(typeof(BaseService)),
            Assembly.GetAssembly(typeof(TestQuery))
        };
    }
}
