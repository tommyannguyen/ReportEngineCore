using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reporting;
using Reporting.HtmlEngine;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Test.Console
{
    class Program
    {
        static ILoggerFactory _logFactory;
        static INodeServices _nodeServices;
        static string OutPutDirectory => AppDomain.CurrentDomain.BaseDirectory;
        static void Main(string[] args)
        {

            var services = new ServiceCollection();
            services.AddNodeServices();
            var serviceProvider = services
                      .AddLogging(configure => configure.AddConsole())
                      .BuildServiceProvider();
            _logFactory = serviceProvider.GetService<ILoggerFactory>();
            _nodeServices = serviceProvider.GetService<INodeServices>();

            Task.Run(async () => await ExportHtmlAsync("ten ten ten"," x x x")).Wait();

           
            System.Console.ReadLine();
        }

        private static async Task ExportHtmlAsync(string htmlPath, string pdfPath)
        {
            Process cmd = new Process();

            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;

            cmd.Start();

            cmd.StandardInput.WriteLine("cd private_module");
            cmd.StandardInput.WriteLine($"node index.js {htmlPath} {pdfPath}");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            System.Console.WriteLine(cmd.StandardOutput.ReadToEnd());

            //var result = await _nodeServices.InvokeAsync<long>("private_module/index.js", "run");
        }
    }
}
