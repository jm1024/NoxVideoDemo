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
    public partial class NameDisplayList : UserControl
    {

        private List<int> lineTopPositionList = new List<int>();

        private const int REF_MAX_LINE_HEIGHT = 110;
        private const int REF_MARGIN = 50;


        public NameDisplayList()
        {
            InitializeComponent();
        }

        private void NameDisplayList_Load(object sender, EventArgs e)
        {
            
        }

        private void calculateLines()
        {
            lineTopPositionList.Clear();
            int screenHeight = this.Height;
            int totalLines = (screenHeight - REF_MARGIN) / REF_MAX_LINE_HEIGHT;
            for (int i = 1; i <= totalLines; i++)
            {
                lineTopPositionList.Add(i * REF_MAX_LINE_HEIGHT);
            }
        }
    }

    public class DisplayName
    {
        public string name;
        public int x;
        public int y;
        public float weight = 0;
        public bool found = false;
        public bool isNew = true;
    }
}
