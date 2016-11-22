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

            dir_changed();
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tb_frame_Scroll(object sender, EventArgs e)
        {
            dxImageBox1.Frame = tb_frame.Value;
        }

        private void tb_w_Scroll(object sender, EventArgs e)
        {
            dxImageBox1.Window = tb_w.Value;
        }

        private void tb_l_Scroll(object sender, EventArgs e)
        {
            dxImageBox1.Level = tb_l.Value;
        }

        int mouse_x;
        int mouse_y;

        private void dxImageBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            int diff_z = e.Delta / SystemInformation.MouseWheelScrollDelta;

            int new_z = tb_frame.Value + diff_z;

            if (new_z > tb_frame.Maximum)
                new_z = tb_frame.Maximum;
            if (new_z < tb_frame.Minimum)
                new_z = tb_frame.Minimum;

            tb_frame.Value = new_z;
            dxImageBox1.Frame = new_z;
        }

        private void dxImageBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                int diff_x = e.X - mouse_x;
                int diff_y = e.Y - mouse_y;

                mouse_x = e.X;
                mouse_y = e.Y;

                int new_w = tb_w.Value + diff_x * 3;
                int new_l = tb_l.Value + diff_y * 3;

                if (new_w > tb_w.Maximum)
                    new_w = tb_w.Maximum;
                if (new_w < tb_w.Minimum)
                    new_w = tb_w.Minimum;

                if (new_l > tb_l.Maximum)
                    new_l = tb_l.Maximum;
                if (new_l < tb_l.Minimum)
                    new_l = tb_l.Minimum;

                tb_w.Value = new_w;
                tb_l.Value = new_l;

                dxImageBox1.Window = new_w;
                dxImageBox1.Level = new_l;  
            }
        }

        private void dxImageBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_x = e.X;
            mouse_y = e.Y;
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            tb_w.Value = 1400;
            tb_l.Value = -500;

            dxImageBox1.Window = 1400;
            dxImageBox1.Level = -500;
        }

        private void cb_thresholds_CheckedChanged(object sender, EventArgs e)
        {
            thresh_changed();
        }

        private void thresh_changed()
        {
            if (cb_thresholds.Checked)
            {
                dxImageBox1.Threshold = (float)tb_thresh.Value / (float)tb_thresh.Maximum;
            }
            else
                dxImageBox1.Threshold = 0.0f;
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tb_thresh_Scroll_1(object sender, EventArgs e)
        {
            thresh_changed();
        }

        private void cb_zones_CheckedChanged(object sender, EventArgs e)
        {
            dxImageBox1.ShowZones = cb_zones.Checked;
        }

        class MyListViewItem : ListViewItem
        {
            public System.IO.FileInfo fi;
            public libctdyn.SubjectData sd;
        }

        void dir_changed()
        {
            var dir = inputdir.Text;
            var di = new System.IO.DirectoryInfo(dir);
            if(di.Exists)
            {
                var files = libctdyn.libctdyn.GetFilesInDir(di.FullName);

                listView1.Items.Clear();

                foreach(var file in files)
                {
                    var sd = libctdyn.libctdyn.LoadSubjectData(file);

                    var lvi = new MyListViewItem();
                    lvi.fi = file;
                    lvi.sd = sd;
                    lvi.Text = sd.pig_name;

                    lvi.SubItems.Add(sd.series_name);
                    lvi.SubItems.Add(sd.acquisition_datetime.Substring(0, 8));
                    lvi.SubItems.Add(sd.acquisition_datetime.Substring(8, 6));

                    lvi.Checked = true;

                    listView1.Items.Add(lvi);
                }
            }
        }

        private void inputdir_TextChanged(object sender, EventArgs e)
        {
            dir_changed();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var si = listView1.SelectedItems;
            if (si.Count == 0)
                dxImageBox1.ImageData = null;
            else
            {
                var sia = si[0] as MyListViewItem;

                if (sia == null || sia.fi == null)
                    dxImageBox1.ImageData = null;
                else
                {
                    libctdyn.SubjectData sd;
                    dxImageBox1.ImageData = libctdyn.libctdyn.LoadImageData(sia.fi, out sd);
                    dxImageBox1.Frame = 0;
                    tb_frame.Value = 0;
                    tb_frame.Maximum = sd.dimensions[2] - 1;
                    tb_w.Value = dxImageBox1.Window;
                    tb_l.Value = dxImageBox1.Level;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<System.IO.FileInfo> fis = new List<System.IO.FileInfo>();
            foreach (MyListViewItem lvi in listView1.CheckedItems)
                fis.Add(lvi.fi);

            ProgressBar pb = new ProgressBar(fis, outputfile.Text);
            pb.ShowDialog();
        }
    }
}
