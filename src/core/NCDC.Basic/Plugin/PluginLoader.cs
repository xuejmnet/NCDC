using System.Reflection;

namespace NCDC.Basic.Plugin;

public class PluginLoader
{
    private static PluginLoadContext _pluginLoadContext;
    public static PluginLoadContext Instance => _pluginLoadContext;

    private List<Assembly> _assemblies = new List<Assembly>();

    public static void Init(string path)
    {
        _pluginLoadContext = new PluginLoadContext(path);
    }

    // public static Assembly Load(string fullClassName)
    // {
    //    return _pluginLoadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(fullClassName)));
    // }
    public  Assembly LoadFromFileName(string fileName)
    {
       return _pluginLoadContext.LoadFromAssemblyPath(fileName);
    }
}