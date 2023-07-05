using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace WebView
{
    public partial class MainForm : Form
    {
        //private delegate void showWebHandler(string url);
        delegate Task showWebHandler(string url);
        //private const string url = "https://term.ptt.cc/";
        //private const string title = "PTT";
        private const string url = "http://localhost/test/test.html";
        private const string title = "TEST";
        public MainForm()
        {
            InitializeComponent();
            this.Text = title;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (wv.InvokeRequired)
            {
                this.Invoke(new showWebHandler(showWeb), new object[] { url });
            }
            else
            {
                showWeb(url);
            }
        }

        private async Task showWeb(string url)
        {
            try
            {
                // 重設網頁暫存檔案位置
                var asm = new AssemblyHelper();
                var webView2DataPath = Path.Combine(asm.AppDataPath, $"webview2");
                Directory.CreateDirectory(webView2DataPath);
                var webView2Environment = await CoreWebView2Environment.CreateAsync(null, webView2DataPath);

                await wv.EnsureCoreWebView2Async(webView2Environment);
                wv.CoreWebView2.SetVirtualHostNameToFolderMapping("appdata", webView2DataPath, CoreWebView2HostResourceAccessKind.Allow);
                wv.CoreWebView2.AddHostObjectToScript("example", new CustomWebView2HostObject());
                wv.Source = new Uri(url);
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                throw;
            }
        }
        [ClassInterface(ClassInterfaceType.AutoDual)]
        [ComVisible(true)]
        public class CustomWebView2HostObject
        {
            public string Test(int num1, int num2, string message)
            {
                var b = Uri.UnescapeDataString(message);
                message = HttpUtility.UrlDecode(message, Encoding.UTF8);
                MessageBox.Show($"C# 收到參數 num1={num1}，num2={num2}，message={message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return "計算結果：" + (num1 + num2);
            }
        }
    }
}
