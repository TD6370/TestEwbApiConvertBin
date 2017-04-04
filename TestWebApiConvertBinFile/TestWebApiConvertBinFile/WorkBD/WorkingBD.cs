using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using TestWebApiConvertBinFile.Model;

namespace TestWebApiConvertBinFile.WorkBD
{
   

    public static class WorkingBD
    {
        public static string  baseNameLastSQL="";

        public static void CreateBDSQL(string baseName = "ConvertBinFile.db3")
        {
            try
            {
                if (!File.Exists(baseName))
                {
                    SQLiteConnection.CreateFile(baseName);
                    
                    using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", baseName)))
                    {
                        connection.ConnectionString = "Data Source = " + baseName;
                        connection.Open();

                        using (SQLiteCommand command = new SQLiteCommand(connection))
                        {
                           
                            command.CommandText = @"CREATE TABLE [Header] (
                                [version] int NOT NULL,
                                [type] char(16) NOT NULL
                                );";
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();

                            command.CommandText = @"CREATE TABLE [TradeRecord] (
                                [id] int NOT NULL,
                                [account] int NOT NULL,
                                [volume] REAL NOT NULL,
                                [comment] char(64) NOT NULL
                                );";
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception x)
            {
                Console.WriteLine("Error CreateBD SQL: " + x.Message);
                WorkingBD.SaveLog("Error CreateBD SQL: " + x.Message);
            }
        }

        public static void InsertDataBDSQL(string baseName = "ConvertBinFile.db3", HeaderTradeRecord fileBinRead = new HeaderTradeRecord())
        {
            try
            {
                string fName = Path.GetFileNameWithoutExtension(baseName);
                
                string path = Path.GetDirectoryName(baseName);
                baseName = path + "\\" + fName + ".db3";

                WorkingBD.CreateBDSQL(baseName);
                baseNameLastSQL = baseName;
              
                using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", baseName)))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText = @"DELETE FROM [Header];";
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();

                        command.CommandText = @"DELETE FROM [TradeRecord];";
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();

                        command.CommandText = @"INSERT INTO [Header] ([version], [type]) VALUES (" + fileBinRead.version + ", '" + fileBinRead.type + "');";
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();

                        foreach (Model.TradeRecord t in fileBinRead.trades)
                        {
                            command.CommandText = @"INSERT INTO [TradeRecord] ([id], [account], [volume], [comment]) VALUES ("
                                + t.id + ", " + t.account + ", '" + t.volume + "', '" + t.comment + "');";
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();
                        }

                    }
                }

                SavePathFileConvert(baseName, "sql");

            }
            catch (Exception x)
            {
                Console.WriteLine("Error InsertDataBD SQL: " + x.Message);
                WorkingBD.SaveLog("Error InsertDataBD SQL: " + x.Message);
            }
        }

        public static TradeRecord SelectDataBDSQL(int id = 0)
        {
            TradeRecord treadRec = new TradeRecord();
           
            string baseName = baseNameLastSQL;
            if (baseName.Length != 0)
            {
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", baseName)))
                    {
                        connection.Open();
                        using (SQLiteCommand command = new SQLiteCommand(connection))
                        {
                           
                            command.CommandText = @"SELECT [id], [account], [volume], [comment] FROM [TradeRecord] WHERE [id]=" + id + ";";
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();

                            
                            SQLiteDataReader reader = command.ExecuteReader();
                            foreach (DbDataRecord record in reader)
                            {
                              
                                treadRec.id = Int32.Parse(record["id"].ToString());
                                treadRec.account = Int32.Parse(record["account"].ToString());
                                
                                treadRec.volume = (Double)record["volume"];
                                treadRec.comment = record["comment"].ToString();
                            }
                        }
                    }
                }
                catch (Exception x)
                {
                    Console.WriteLine("Error InsertDataBD SQL: " + x.Message);
                    WorkingBD.SaveLog("Error InsertDataBD SQL: " + x.Message);
                }
            }else
            {
                Console.WriteLine("File is null");
            }
            return treadRec;
        }

        public static void InsertDataCSV(string baseName = "ConvertBinFile.csv", HeaderTradeRecord fileBinRead = new HeaderTradeRecord())
        {
            try
            {
                string fName = Path.GetFileNameWithoutExtension(baseName);
               
                string path = Path.GetDirectoryName(baseName);
                baseName = path + "\\" + fName + ".csv";

                StreamWriter file = new StreamWriter(baseName, false, System.Text.Encoding.GetEncoding(1251));
                file.WriteLine("id, account, volume, comment");
                
                foreach (Model.TradeRecord t in fileBinRead.trades)
                {
                    file.WriteLine(t.id + ", " + t.account + ", '" + t.volume + "', '" + t.comment);
                    
                }

                file.Close();

                SavePathFileConvert(baseName, "csv");

            }
            catch (Exception x)
            {
                Console.WriteLine("Error InsertDataCSV SQL: " + x.Message);
                WorkingBD.SaveLog("Error InsertDataCSV SQL: " + x.Message);
            }
        }

        public static void SavePathFileConvert(string PathFile = "", string typeFile="")
        {
            SelDate.SelectDownloadFile = PathFile;

            List<ConvFile> listFC =  LoadSerializXML("CreatesFileConvert.xml");
            if (listFC == null)
            {
                listFC = new List<ConvFile>();
            }
            else 
            {
               
                if(IsExistFile(listFC, PathFile)==true) return;
            }

            listFC.Add(new ConvFile { Path = PathFile, TypeF = typeFile });
            SaveSerializXML("CreatesFileConvert.xml", listFC);
        }

        public static bool IsExistFile(List<ConvFile> listFC, string PathFile = "")
        {
            if ( listFC==null) listFC = LoadSerializXML("CreatesFileConvert.xml");
            int Ind = listFC.FindIndex(p => p.Path == PathFile);
            if (Ind != -1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static void RemoveFileList(List<ConvFile> listFC, string PathFile = "")
        {
            if (listFC == null) listFC = LoadSerializXML("CreatesFileConvert.xml");
            int Ind = listFC.FindIndex(p => p.Path == PathFile);
            if (Ind != -1)
            {
                listFC.RemoveAt(Ind);
                SaveSerializXML("CreatesFileConvert.xml", listFC);
            }
        }



        public static string GetPathFileName(List<ConvFile> listFC, string FileName = "")
        {
            if (listFC == null) listFC = LoadSerializXML("CreatesFileConvert.xml");
            int Ind = listFC.FindIndex(p => p.Path.IndexOf(FileName)!=-1);
            if (Ind != -1)
            {
                return listFC[Ind].Path;
            }
            else
            {
                return "";
            }

        }


        public static void SaveSerializXML(string path, List<ConvFile> obj)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                XmlSerializer serialiser = new XmlSerializer(typeof(List<ConvFile>));
                TextWriter FileStream = new StreamWriter(path, true, System.Text.Encoding.GetEncoding("Windows-1251"));
                serialiser.Serialize(FileStream, obj);
                FileStream.Close();

            }
            catch (Exception x)
            {
                Console.WriteLine("Error SaveSerializXML: " + x.Message);
                WorkingBD.SaveLog("Error SaveSerializXML: " + x.Message);
            }
        }
        
       
        public static List<ConvFile> LoadSerializXML(string FileName = "")
        {
            try
            {
                List<ConvFile> obj = null;
                if (File.Exists(FileName))
                {
                    Stream TestFileStream = File.OpenRead(FileName);
                    XmlSerializer deserializer = new XmlSerializer(typeof(List<ConvFile>));
                    obj = (List<ConvFile>)deserializer.Deserialize(TestFileStream);
                    TestFileStream.Close();
                }
                return obj;
            }
            catch (Exception x)
            {
                Console.WriteLine("Error LoadSerializXML: " + x.Message);
                WorkingBD.SaveLog("Error LoadSerializXML: " + x.Message);
            }
            return null;
        }


        //Запись логов в файл
        public static void SaveLog(string info = "")
        {
            try
            {
                if (info.Length != 0)
                {
                    string StrDate = String.Format("{0:H:mm:ss}", DateTime.Now);
                    string PatchGate = "";
                    PatchGate = "Log.xml";

                    DataRep obj = new DataRep();
                    obj.Date = DateTime.Parse(StrDate);
                    obj.Event = info;

                    SaveDataRepLinqXML(PatchGate, obj);
                }

            }
            catch (Exception x)
            {
                Console.Write(x.Message);

            }

        }

        public static void SaveDataRepLinqXML(string FileName, DataRep obj)
        {
            try
            {
                if (FileName.Length == 0)
                    FileName = "Log.xml";

                if (File.Exists(FileName))
                {
                    XDocument doc = XDocument.Load(FileName);
                    XElement track = new XElement("DataRep",
                       new XElement("Event", obj.Event),
                       new XElement("Date", obj.Date));

                    doc.Root.Add(track);
                    doc.Save(FileName);
                }
                else
                {
                    FileName = "Log.xml";

                    List<DataRep> objS = new List<DataRep>();
                    objS.Add(new DataRep { Date = obj.Date, Event = obj.Event });
                    SaveDataRepSerializXML(FileName, objS);
                }
            }
            catch (Exception x)
            {
                Console.Write("SaveDataRepLinqXML: " + x.Message);
            }
        }

        public static void SaveDataRepSerializXML(string path, List<DataRep> obj)
        {
            try
            {
                if (path.Length == 0) path = "Log.xml";
                if (File.Exists(path))
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch (Exception xd)
                    {
                        Console.Write(xd.Message);
                    }
                }
                XmlSerializer serialiser = new XmlSerializer(typeof(List<DataRep>));
                TextWriter FileStream = new StreamWriter(path, true, System.Text.Encoding.GetEncoding("Windows-1251"));
                serialiser.Serialize(FileStream, obj);
                FileStream.Close();
            }
            catch (Exception x)
            {
                Console.Write("SaveDataRepSerializXML: " + x.Message);
            }
        }
    }
}
