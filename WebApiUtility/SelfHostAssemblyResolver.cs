using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Dispatcher;

namespace WebApiUtility
{
    public class SelfHostAssemblyResolver : IAssembliesResolver
    {
        private readonly string _path;

        public SelfHostAssemblyResolver(string path)
        {
            _path = path;
        }

        public ICollection<Assembly> GetAssemblies()
        {
            var assembiles = new List<Assembly>();
            assembiles.Add(Assembly.LoadFrom(_path));
            return assembiles;
        }
    }
}
