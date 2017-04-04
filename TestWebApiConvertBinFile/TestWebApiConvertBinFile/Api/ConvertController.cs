using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TestWebApiConvertBinFile.Converting;
using TestWebApiConvertBinFile.Model;
using TestWebApiConvertBinFile.WorkBD;
using TestWebApiConvertBinFile.WorkBin;

namespace TestWebApiConvertBinFile.Api
{
    
    public class ConvertController : ApiController
    {
        [HttpGet]
        public TradeRecord GetSelectDataBDSQL(int id)
        {
            TradeRecord result = WorkingBD.SelectDataBDSQL(id);
            Console.WriteLine("Get [TradeRecord] from id: " + id);
            WorkingBD.SaveLog("Отправка одной записи по ID: " + id);
            return result;
        }

        [HttpDelete]
        public string DeleteDataBDSQL(string id)
        {
            string NameFile = id;
            string result = "";

            string pathFD = WorkingBD.GetPathFileName(null, NameFile);

            if (pathFD.Length!=0)
            {
                if (WorkBinFiles.DeleteFile(pathFD) == true)
                {
                    result = "Файл удален : " + NameFile + " [" + DateTime.Now + "]";
                }
                else
                {
                    result = "Файл НЕ удален : " + NameFile + " [" + DateTime.Now + "]";
                }
            }else{
                result = "Файл для удаления не найден : " + NameFile + " [" + DateTime.Now + "]";
            }
            WorkingBD.SaveLog(result);
            return result ;
        }

        [HttpPost]
        public string PostConvert([FromBody]ConvFile data)
        {
            string Path = "";
            string TypeF = "SQL";

            if (data != null)
            {
                Path = data.Path;
                TypeF = data.TypeF;
            }

            if (TypeF == "SQL")
            {
                ConvertingFile.ConvertFileSQL(Path);
            }
            else
            {
                ConvertingFile.ConvertFileCSV(Path);
            }
            string result = "[" + DateTime.Now + "] Файл сконвертирован в тип " + TypeF + "  Ссылка для скачивания файла " + Path + "  ->  " + "http://localhost:8080/api/Download/";
          
            WorkingBD.SaveLog("Файл сконвертирован " + Path + " в тип " + TypeF);
            return result;
        }

        public string GetConvertNull()
        {
            return "Convert path file nothing";
        }

    }

    
}
