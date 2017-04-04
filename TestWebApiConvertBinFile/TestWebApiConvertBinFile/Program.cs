using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWebApiConvertBinFile.Api;
using TestWebApiConvertBinFile.Model;
using TestWebApiConvertBinFile.WorkBD;
using TestWebApiConvertBinFile.WorkBin;

namespace TestWebApiConvertBinFile
{
    class Program
    {
        static ServiceApi serv;

        static void Main(string[] args)
        {
            try
            {
                StartServer();
                if (serv != null)
                {
                    serv.Stop();
                }
            }
            catch (Exception x)
            {
                Console.WriteLine("Error main: " + x.Message);
                WorkingBD.SaveLog("Error main: " + x.Message);
                Console.ReadLine();
            }
        }

        private static void StartServer()
        {
            try
            {
                serv = new ServiceApi();
                serv.Start();
                Console.ReadLine();
            }
            catch (Exception x)
            {
                Console.WriteLine("Error StartServer: " + x.Message);
                WorkingBD.SaveLog("Error StartServer: " + x.Message);
                Console.ReadLine();
            }

        }
        
    }
}
