using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

namespace WebView
{
    internal static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                var asm = new AssemblyHelper();
                // Load an extracted DLL dynamically
                asm.EnableEmbededManifestDll();

                var loaderDllFolderPath = Path.Combine(asm.AppDataPath, "runtimes\\win-x64\\native");
                var dll = Path.Combine(loaderDllFolderPath, "WebView2Loader.dll");
                var loaderDllEmbedPath = $"{asm.Name}.runtimes.win_x64.native.WebView2Loader.dll";
                asm.ExtractEmbeddedDLL(loaderDllEmbedPath, dll);

                // 將需注入 DLL 的邏輯抽離 Main 才能跑
                run(loaderDllFolderPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private static void run(string loaderDllFolderPath)
        {
            CoreWebView2Environment.SetLoaderDllFolderPath(loaderDllFolderPath);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
