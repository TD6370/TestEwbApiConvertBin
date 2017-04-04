using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestClientWebApi.Model;
using Newtonsoft.Json;
using Microsoft.CSharp;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace TestClientWebApi
{
    

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<TradeRecord> model = null;
                TradeRecord trade = new TradeRecord();

                //var baseAddress = "http://localhost:8080/";
                var baseAddress = "http://" + tbNameHost.Text + "/";
                var client = new HttpClient { BaseAddress = new Uri(baseAddress) };

                string result = "";
                string Param = "";
                Param = tbProdId.Text;
              
                string RequestStr = "api/Convert/" + Param;
                var task = client.GetAsync(RequestStr)
                    .ContinueWith((taskwithresponse) =>
                    {
                        var response = taskwithresponse.Result;
                        var resString = response.Content.ReadAsStringAsync();
                        resString.Wait();
                        result = resString.Result;
                        try
                        {
                            //model = JsonConvert.DeserializeObject<List<TradeRecord>>(result);
                            trade = JsonConvert.DeserializeObject<TradeRecord>(result);
                            model = new List<TradeRecord>();
                            model.Add(trade);
                        }catch(Exception x)
                        {
                            Console.WriteLine("Error JsonConvert: " + x.Message + "\n" + result);
                        }
                    });
                task.Wait();

                if (model != null)
                {
                    lvResult.ItemsSource = model;
                    
                }

                tbRequest.Text = RequestStr;
                tbResult.Text = result;
                Work.SetRichTextBoxText(rtbResult, tbResult.Text);

            }catch(Exception x)
            {
                tbResult.Text = "Error: " + x.Message;
                Work.SetRichTextBoxText(rtbResult, tbResult.Text);
            }

        }

        private void ButtonPostClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string file = tbxConvFile.Text;
                ClientRequestPost("Convert", file);
            }
            catch (Exception x)
            {
                tbResult.Text = "Error: " + x.Message;
                Work.SetRichTextBoxText(rtbResult, tbResult.Text);
            }
        }

        private void ClientRequestPost(string NameController = "Convert", string Path="")
        {
            try
            {
                //var baseAddress = "http://localhost:8080/Download/";
                var baseAddress = "http://" + tbNameHost.Text + "/";
                var client = new HttpClient { BaseAddress = new Uri(baseAddress) };

                string result = "";
                string Param = "SQL";
                if(rbTypeSQL.IsChecked==true)
                {
                    Param = "SQL";
                }
                else
                {
                    Param = "CSV";
                }

                if(Path.Length==0)
                {
                    if (NameController == "Convert") Path = @"c:\Files\file.dat";
                    if (NameController == "Download") Path = @"c:\Files\file.db3";
                }

                var convFile = new ConvFile{};
                if (NameController == "Convert")
                    convFile = new ConvFile { Path = Path, TypeF = Param };
                if (NameController == "Download")
                    convFile = new ConvFile { Path = Path, TypeF = Param };

                string data = JsonConvert.SerializeObject(convFile);
                Console.Write(data);

                var content = new StringContent(data, Encoding.UTF8, "application/json");
                
                string RequestStr = "api/" + NameController + "/" ;
                var task = client.PostAsync(RequestStr, content)
                    .ContinueWith((taskwithresponse) =>
                    {
                        var response = taskwithresponse.Result;
                        var resString = response.Content.ReadAsStringAsync();
                        resString.Wait();
                        result = resString.Result;
                        
                    });
                task.Wait();

                if(NameController == "Download")
                {
                    int ind = result.IndexOf("|");
                    string LinkDownload = "";

                    if (result.IndexOf("|") != -1)
                    {
                        string[] Mas= result.Split('|');
                        LinkDownload = Mas[1];
                        //string LinkDownload = "http://localhost:8080/api/Download/";
                        linkDownload.NavigateUri= new Uri(LinkDownload);
                    }

                    LinkDownload = "http://localhost:8080/api/Download/";
                    Process.Start(new ProcessStartInfo(LinkDownload));
                    
                }

                tbRequest.Text = RequestStr;
                tbResult.Text = result;
                Work.SetRichTextBoxText(rtbResult, tbResult.Text);
            }
            catch (Exception x)
            {
                tbResult.Text = "Error: " + x.Message;
                Work.SetRichTextBoxText(rtbResult, tbResult.Text);
            }
        }

        private void ClientRequestDownload(string NameController = "Convert")
        {
            try
            {
                //"http://localhost:8080/api/Download/";
                var baseAddress = "http://" + tbNameHost.Text + "/";
                var client = new HttpClient { BaseAddress = new Uri(baseAddress) };

                string result = "";
                string RequestStr = "api/Download/";
                var task = client.GetAsync(RequestStr)
                    .ContinueWith((taskwithresponse) =>
                    {
                        var response = taskwithresponse.Result;
                        var resString = response.Content.ReadAsStringAsync();
                        resString.Wait();
                        result = resString.Result;
                    });
                task.Wait();

                tbRequest.Text = RequestStr;
                tbResult.Text = result;
                Work.SetRichTextBoxText(rtbResult, tbResult.Text);
            }
            catch (Exception x)
            {
                tbResult.Text = "Error: " + x.Message;
                Work.SetRichTextBoxText(rtbResult, tbResult.Text);
            }
        }

        private void ClientRequestDelete(string NameController = "Convert", string FileName = "file.db3")
        {
            try
            {

                //var baseAddress = "http://localhost:8080/";
                var baseAddress = "http://" + tbNameHost.Text + "/";
                var client = new HttpClient { BaseAddress = new Uri(baseAddress) };

                string result = "";
               
                string RequestStr = "api/" + NameController + "/" + FileName;
                var task = client.DeleteAsync(RequestStr)
                    .ContinueWith((taskwithresponse) =>
                    {
                        var response = taskwithresponse.Result;
                        var resString = response.Content.ReadAsStringAsync();
                        resString.Wait();
                        result = resString.Result;
                    });
                task.Wait();

                tbRequest.Text = RequestStr;
                tbResult.Text = result;
                Work.SetRichTextBoxText(rtbResult, tbResult.Text);
            }
            catch (Exception x)
            {
                tbResult.Text = "Error: " + x.Message;
                Work.SetRichTextBoxText(rtbResult, tbResult.Text);
            }
        }



       

       

        

        private void ButtonPostDownload_Click(object sender, RoutedEventArgs e)
        {
            string file = tbxDownloadFile.Text;
            ClientRequestPost("Download", file);
        }

        private void ButtonPostDownloadGet_Click(object sender, RoutedEventArgs e)
        {
            ClientRequestDownload();
        }

        private void ButtonDeleteClient_Click(object sender, RoutedEventArgs e)
        {
            string file = tbxDelFile.Text;
            ClientRequestDelete("Convert", file);
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            //LinkDownload = "http://localhost:8080/api/Download/";
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}

