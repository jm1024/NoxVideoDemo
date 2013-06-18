using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VideoDemo
{
    public partial class HistoryList : UserControl
    {

        public List<Dictionary<string,string>> historyList = new List<Dictionary<string,string>>();

        public HistoryList()
        {
            InitializeComponent();
        }


    }
}
