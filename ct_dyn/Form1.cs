using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ct_dyn
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void inputdirbtn_Click(object sender, EventArgs e)
        {
            var fp = new FolderBrowserDialog();
            fp.ShowNewFolderButton = true;
            fp.RootFolder = Environment.SpecialFolder.MyComputer;
            fp.ShowDialog();

            try
            {
                inputdir.Text = fp.SelectedPath;
            }
            catch (Exception) { }
        }

        private void outputfilebtn_Click(object sender, EventArgs e)
        {
            var fp = new SaveFileDialog();
            fp.Filter = "Comma-separated variables|*.csv";
            fp.ShowDialog();

            try
            {
                outputfile.Text = fp.FileName;
            }
            catch (Exception) { }
        }
    }
}
