using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankInfoRetriever
{
    public class TextBoxLogger : ILogger
    {
        TextBox textBox;
        public TextBoxLogger(TextBox textBox)
        {
            this.textBox = textBox;
        }

        public void AddLogEntry(string content)
        {
            textBox.AppendText(content + "\r\n");
        }
    }
}
