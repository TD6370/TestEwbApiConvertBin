using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TestWebApiConvertBinFile.Model;
using TestWebApiConvertBinFile.WorkBD;

namespace TestWebApiConvertBinFile.Api
{
    public class DownloadController : ApiController
    {
       

        [HttpGet]
        public HttpResponseMessage GetDownload()
        {
            string Path = SelDate.SelectDownloadFile;
            if (SelDate.SelectDownloadFile.Length==0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Не отправлен запрос на имя скачиваемого файла!");
            }

            if (WorkingBD.IsExistFile(null, Path))
            {
                try
                {
                    string FileName = System.IO.Path.GetFileName(Path);
                    HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                    var stream = new FileStream(Path, FileMode.Open, FileAccess.Read);
                    result.Content = new StreamContent(stream);
                    result.Content.Headers.ContentDisposition =  new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = FileName
                    };
                    result.Content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");
                    return result;
                }
                catch (Exception x)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, x.Message);
                }

            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public string PostDownload([FromBody]ConvFile data)
        {
            string Path = "";
            if (data != null)
            {
                Path = data.Path;
                SelDate.SelectDownloadFile = data.Path;
            }
            return "Ссылка для скачивания файла " + Path + " |" + "http://localhost:8080/api/Download/";
        }
       
    }
}
