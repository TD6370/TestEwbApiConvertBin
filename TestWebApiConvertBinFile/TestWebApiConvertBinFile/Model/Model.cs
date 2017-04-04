using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TestWebApiConvertBinFile.Model
{
    [Serializable()]
    public class DataRep
    {
        public string Event { get; set; }
        public DateTime Date { get; set; }
    }


    public static class SelDate{
        static SelDate()
        {
            SelectDownloadFile = "";
        }
        //public static string SelectDownloadFile = "";
        public static string SelectDownloadFile{set; get;}
    }

    public class ConvFile
    {
        public string Path { set; get; }
        public string TypeF { set; get; }
    }

    //Имеется набор бинарных файлов. Каждый имеет следующую структуру:

    //<header>
    //<trade1>
    //<trade2>
    //<trade3>
    //...     
    //<tradeN>
    //где <header> описывается такой структурой:

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Header
    {
        public int version;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string type;
    }
    //а <trade> такой:

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TradeRecord
    {
        public int id;
        public int account;
        public double volume;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]    
        public string comment;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HeaderTradeRecord
    {
        public int version;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string type;

        public List<TradeRecord> trades;
    }

    public class CreateBinFileStruct
    {
        HeaderTradeRecord htr;
        public CreateBinFileStruct()
        {
            htr = new HeaderTradeRecord() 
            {
                version = 10, 
                type = "typeHeader",
                trades = new List<TradeRecord>()
                {
                    new   TradeRecord{ id=0, account =0, volume=0.1, comment="comment 1" },
                    new   TradeRecord{ id=1, account =1, volume=0.2, comment="comment 2" },
                    new   TradeRecord{ id=2, account =2, volume=0.3, comment="comment 3" },
                    new   TradeRecord{ id=3, account =3, volume=0.4, comment="comment 4" }
                }
            };
        }
        public HeaderTradeRecord GetBinFile()
        {
            return this.htr;
        }
    }
}
