using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;
using TestWebApiConvertBinFile.WorkBD;

namespace TestWebApiConvertBinFile.Api
{
    class ServiceApi
    {
        private readonly HttpSelfHostServer server;
        string baseAddress = "http://localhost:8080/";

        public ServiceApi()
        {
            try
            {
                //Test:  http://localhost:8080/api/Convert/c:\files\file.bin
                var config = new HttpSelfHostConfiguration(baseAddress);



                config.Routes.MapHttpRoute(
                                    name: "DefaultApi",
                                    routeTemplate: "api/{controller}/{id}",
                                    defaults: new { id = RouteParameter.Optional });

                Console.WriteLine("Instantiating server...");
                WorkingBD.SaveLog("Инициализация сервера...");
                server = new HttpSelfHostServer(config);

            }
            catch (Exception x)
            {
                Console.WriteLine("Error ServiceApi: " + x.Message);
                WorkingBD.SaveLog("Error ServiceApi: " + x.Message);
                Console.ReadLine();
            }
        }

        public void Start()
        {
            server.OpenAsync();
            Console.WriteLine("Server run..." + baseAddress);
            WorkingBD.SaveLog("Сервер запущен");
        }

        public void Stop()
        {
            server.CloseAsync();
            server.Dispose();
            Console.WriteLine("Server stop!" + baseAddress);
            WorkingBD.SaveLog("Сервер остановлен");
        }

    }
}
