using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TestWebApiConvertBinFile.Model;
using TestWebApiConvertBinFile.WorkBD;

namespace TestWebApiConvertBinFile.WorkBin
{
    public static class WorkBinFiles
    {

        public static void CreateBinFile(string path = @"C:\Files\file.dat")
        {
            try
            {
                if (!File.Exists(path))
                {
                    //string dir = Path.GetDirectoryName(path);
                    //if (Directory.Exists(dir) == false)
                    //{
                    //    try
                    //    {
                    //        Directory.CreateDirectory(dir);
                    //    }
                    //    catch (Exception xd)
                    //    {
                    //        Console.WriteLine("Error CreateBinFile.CreateDirectory: " + xd.Message);
                    //    }
                    //}

                    CreateBinFileStruct objCreate = new CreateBinFileStruct();
                    HeaderTradeRecord fileBin = objCreate.GetBinFile();

                    using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
                    {
                        writer.Write(fileBin.version);
                        writer.Write(fileBin.type);
                        foreach (TradeRecord t in fileBin.trades)
                        {
                            writer.Write(t.id);
                            writer.Write(t.account);
                            writer.Write(t.volume);
                            writer.Write(t.comment);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error CreateBinFile: " + e.Message);
                WorkingBD.SaveLog("Error CreateBinFile: " + e.Message);
            }
        }

        public static HeaderTradeRecord ReadBinFile(string path = @"C:\Files\file.dat")
        {
            HeaderTradeRecord fileBinRead = new HeaderTradeRecord();
            fileBinRead.trades = new List<TradeRecord>();

            try
            {
                CreateBinFile(path);

                if (File.Exists(path))
                {
                    using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
                    {
                        while (reader.PeekChar() > -1)
                        {
                            if (fileBinRead.trades.Count == 0)
                            {
                                fileBinRead.version = reader.ReadInt32();
                                fileBinRead.type = reader.ReadString();
                            }
                            int id = reader.ReadInt32();
                            int account = reader.ReadInt32();
                            double volume = reader.ReadDouble();
                            string comment = reader.ReadString();
                            fileBinRead.trades.Add(new TradeRecord
                            {
                                id = id,
                                account = account,
                                comment = comment,
                                volume = volume
                            });
                        }
                    }
                    Console.WriteLine("fileBinRead: " + fileBinRead);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error ReadBinFile: " + e.Message);
                WorkingBD.SaveLog("Error ReadBinFile: " + e.Message);
            }
            return fileBinRead;
        }


        public static bool DeleteFile(string Path)
        {
            try
            {
                //Response.ContentType = ContentType;
                //Response.AppendHeader("Content-Disposition",                                "attachment; filename=myFile.txt");
                //Response.WriteFile(Server.MapPath("~/uploads/myFile.txt"));
                //Response.Flush();
                File.Delete(Path);
                //Response.End();

                WorkingBD.RemoveFileList(null, Path);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error DeleteFile: " + e.Message);
                WorkingBD.SaveLog("Error DeleteFile: " + e.Message);
            }
            return false;
        }
        
    }
}
