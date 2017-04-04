using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestWebApiConvertBinFile.Model;
using TestWebApiConvertBinFile.WorkBD;
using TestWebApiConvertBinFile.WorkBin;

namespace TestWebApiConvertBinFile.Converting
{
    public static class ConvertingFile
    {
        public static void ConvertFileSQL(string PathFile="")
        {
         
           Task t = ConvertFileSQL_Async(PathFile);
           t.Wait();
        }

        private static async Task ConvertFileSQL_Async(string PathFile = "")
        {
            bool result = await ConvertFileSQL_Task(PathFile);

            Console.WriteLine("Converting to SQL ok");
            //"Задача синхронизации завершена";
        }

        private static Task<bool> ConvertFileSQL_Task(string PathFile = "")
        {
            return Task.Run(() =>
            {
                try
                {
                    HeaderTradeRecord fileBinRead  = WorkBinFiles.ReadBinFile(PathFile);
                    if (fileBinRead.trades.Count>0)
                    {
                        WorkingBD.InsertDataBDSQL(PathFile, fileBinRead);
                    }

                    return true;
                }
                catch (Exception x)
                {
                    Console.WriteLine("Error ConvertFileCSV_Task: " + x.Message);

                }
                return false;
            });
          
        }

        public static void ConvertFileCSV(string PathFile = "")
        {
            Task t = ConvertFileCSV_Async(PathFile);
            t.Wait();
        }

        private static async Task ConvertFileCSV_Async(string PathFile = "")
        {
            bool result = await ConvertFileCSV_Task(PathFile);

            Console.WriteLine("Converting to CSV ok");
        }

        private static Task<bool> ConvertFileCSV_Task(string PathFile = "")
        {
            return Task.Run(() =>
            {
                try
                {
                    HeaderTradeRecord fileBinRead = WorkBinFiles.ReadBinFile(PathFile);
                    if (fileBinRead.trades.Count>0)
                    {
                        WorkingBD.InsertDataCSV(PathFile, fileBinRead);
                    }

                    return true;
                }
                catch (Exception x)
                {
                    Console.WriteLine("Error ConvertFileCSV_Task: " + x.Message);
                }
                return false;
            });
        }


       
    }
}
