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
    public partial class ProgressBar : Form
    {
        public ProgressBar()
        {
            InitializeComponent();
        }

        class WorkerArgs
        {
            public List<System.IO.FileInfo> fis;
            public string ofile;
        }

        WorkerArgs wa;

        public ProgressBar(List<System.IO.FileInfo> fis, string output_name) : this()
        {
            wa = new WorkerArgs { fis = fis, ofile = output_name };
        }

        private void ProgressBar_Load(object sender, EventArgs e)
        {
            progressBar1.Maximum = 100;
            backgroundWorker1.RunWorkerAsync(wa);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            WorkerArgs wargs = e.Argument as WorkerArgs;

            libctdyn.libctdyn.DoAnalysis(wargs.fis, wargs.ofile, rp);
        }

        void rp(int n, int total)
        {
            backgroundWorker1.ReportProgress(n * 100 / total);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }
    }
}
