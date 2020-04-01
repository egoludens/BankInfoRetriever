using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankInfoRetriever
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonRetrieve_Click(object sender, EventArgs e)
        {
            tbLog.Clear();
            (new BICProcessor(new TextBoxLogger(tbLog))).Process(DateTime.Now.Date);
        }
    }
}
