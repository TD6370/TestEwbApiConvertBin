using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace TestClientWebApi.Model
{
    class Work
    {
        public static void SetRichTextBoxText(RichTextBox rtb = null, string Text = "")
        {
            try
            {
                var document = new FlowDocument();
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(Text);
                document.Blocks.Add(paragraph);
                rtb.Document = document;
            }
            catch (Exception x)
            {
                Console.Write("Error SetRichTextBoxText: " + x.Message);
            }
        }
    }
}
