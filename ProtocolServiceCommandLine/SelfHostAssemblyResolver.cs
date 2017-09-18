using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Dispatcher;

namespace ProtocolServiceCommandLine
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
            var assemblies = new List<Assembly> {Assembly.LoadFrom(_path)};
            return assemblies;
        }
    }
}
