using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace StartUpProject.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var currentDir = Directory.GetCurrentDirectory();
            foreach (var item in args)
            {
                Console.WriteLine(Path.Combine(currentDir, item));
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .LoadProjects(args);
    }

    public static class ExtensionsClass
    {
        public static IHostBuilder LoadProjects(this IHostBuilder builder, string[] args)
        {
            var assembly = Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), args[0]));
            var instance = assembly.CreateInstance("HelloWorldProject.StartUp");
            var method = instance.GetType().GetMethod("Run");
            method.Invoke(instance, new object[] { new CancellationToken() });

            return builder;
        }
    }
}
