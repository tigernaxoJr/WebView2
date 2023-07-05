using System;
using System.IO;
using System.Reflection;

public class AssemblyHelper
{
    public string Name
    {
        get
        {
            return this.assembly.GetName().Name;
        }
    }
    public string AppDataPath { get; set; }
    private Assembly assembly;

    public AssemblyHelper()
    {
        assembly = Assembly.GetCallingAssembly();
        AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        AppDataPath = Path.Combine(AppDataPath, Name);
    }

    /// <summary>
    /// Extract embeded dll to target path
    /// </summary>
    /// <param name="resourceName">Dll embed path</param>
    /// <param name="targetPath">Dll extract distination</param>
    public void ExtractEmbeddedDLL(string resourceName, string targetPath)
    {
        var targetDir = Path.GetDirectoryName(targetPath);
        if (!string.IsNullOrEmpty(targetDir) && !Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);

        using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
        {
            using (FileStream fileStream = new FileStream(targetPath, FileMode.Create))
            {
                resourceStream.CopyTo(fileStream);
            }
        }
    }

    /// <summary>
    /// 設置解析組件路徑的事件處理常式
    /// </summary>
    public void EnableEmbededManifestDll() => AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;

    /// <summary>
    /// Assembly 解析行為
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
    {
        Assembly assembly = Assembly.GetCallingAssembly();

        string project = Assembly.GetEntryAssembly().GetName().Name;
        string manifestItem = $"{project}.{new AssemblyName(args.Name).Name}.dll";
        using (Stream stream = assembly.GetManifestResourceStream(manifestItem))
        {
            if (stream == null) return null;

            byte[] assemblyRawBytes = new byte[stream.Length];
            stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);
            return Assembly.Load(assemblyRawBytes);
        }
    }
}