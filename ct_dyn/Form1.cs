using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ct_dyn
{
    public partial class Form1 : Form
    {
        System.IO.FileInfo data_file = null;

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
            inputdir.Text = Properties.Settings.Default.InputPath;
            outputfile.Text = Properties.Settings.Default.OutputPath;
            dir_changed();
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
            if (e.Button == MouseButtons.Left)
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
            persist_datafile();

            var dir = inputdir.Text;

            try
            {
                var di = new System.IO.DirectoryInfo(dir);
                if (di.Exists)
                {
                    var files = libctdyn.libctdyn.GetFilesInDir(di.FullName);

                    listView1.Items.Clear();

                    foreach (var file in files)
                    {
                        var sd = libctdyn.libctdyn.LoadSubjectData(file);

                        var lvi = new MyListViewItem();
                        lvi.fi = file;
                        lvi.sd = sd;
                        lvi.Text = sd.subject_name;

                        lvi.SubItems.Add(sd.series_name);

                        if (sd.AcquisitionTime.HasValue)
                        {
                            lvi.SubItems.Add(sd.AcquisitionTime.Value.ToShortDateString());
                            lvi.SubItems.Add(sd.AcquisitionTime.Value.ToLongTimeString());
                        }
                        else
                        {
                            lvi.SubItems.Add("");
                            lvi.SubItems.Add("");
                        }

                        lvi.Checked = true;

                        listView1.Items.Add(lvi);
                    }

                    load_datafile(di.FullName + "\\ctdyn.settings");
                }
            }
            catch(Exception)
            {
                listView1.Items.Clear();
            }
        }

        private void load_datafile(string v)
        {
            data_file = new System.IO.FileInfo(v);

            var sr = data_file.OpenText();

            while(!sr.EndOfStream)
            {
                try
                {
                    int subj_idx = int.Parse(sr.ReadLine());
                    int ser_idx = int.Parse(sr.ReadLine());
                    int oth_idx = int.Parse(sr.ReadLine());

                    int i = int.Parse(sr.ReadLine());
                    int e = int.Parse(sr.ReadLine());
                    int f_adjust = int.Parse(sr.ReadLine());
                    bool is_pc = bool.Parse(sr.ReadLine());
                    bool is_injured = bool.Parse(sr.ReadLine());
                    int fpb = int.Parse(sr.ReadLine());
                    int tinterval = int.Parse(sr.ReadLine());

                    foreach(MyListViewItem lvi in listView1.Items)
                    {
                        if(lvi.sd.SubjectIndex == subj_idx &&
                            lvi.sd.SeriesIndex == ser_idx &&
                            lvi.sd.OtherIndex == oth_idx)
                        {
                            lvi.sd.bc.i = i;
                            lvi.sd.bc.e = e;
                            lvi.sd.bc.f_adjust = f_adjust;
                            lvi.sd.bc.is_pc = is_pc ? 1 : 0;
                            lvi.sd.bc.is_injured = is_injured ? 1 : 0;
                            lvi.sd.bc.fpb = fpb;
                            lvi.sd.bc.time_interval = tinterval;
                        }
                    }
                }
                catch(Exception)
                {
                    return;
                }
            }
        }

        private void persist_datafile()
        {
            try
            {
                if (data_file != null)
                {
                    var fs = data_file.OpenWrite();
                    var sw = new System.IO.StreamWriter(fs);

                    foreach (MyListViewItem lvi in listView1.Items)
                    {
                        sw.WriteLine(lvi.sd.SubjectIndex.ToString());
                        sw.WriteLine(lvi.sd.SeriesIndex.ToString());
                        sw.WriteLine(lvi.sd.OtherIndex.ToString());
                        sw.WriteLine(lvi.sd.bc.i.ToString());
                        sw.WriteLine(lvi.sd.bc.e.ToString());
                        sw.WriteLine(lvi.sd.bc.f_adjust.ToString());
                        sw.WriteLine(lvi.sd.bc.is_pc.ToString());
                        sw.WriteLine(lvi.sd.bc.is_injured.ToString());
                        sw.WriteLine(lvi.sd.bc.fpb.ToString());
                        sw.WriteLine(lvi.sd.bc.time_interval.ToString());
                    }

                    sw.Close();
                }
            }
            catch (Exception) { }
        }

        private void inputdir_TextChanged(object sender, EventArgs e)
        {
            dir_changed();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var si = listView1.SelectedItems;
            if (si.Count == 0)
            {
                dxImageBox1.ImageData = null;
                mldDisplay1.ImageData = null;
                lab_name.Text = "NONE";
            }
            else
            {
                var sia = si[0] as MyListViewItem;

                if (sia == null || sia.fi == null)
                {
                    dxImageBox1.ImageData = null;
                    mldDisplay1.ImageData = null;
                    lab_name.Text = "NONE";
                }
                else
                {
                    libctdyn.SubjectData sd;
                    var id = libctdyn.libctdyn.LoadImageData(sia.fi, out sd);
                    dxImageBox1.ImageData = id;
                    dxImageBox1.Frame = 0;
                    tb_frame.Value = 0;
                    tb_frame.Maximum = sd.dimensions[2] - 1;
                    tb_w.Value = dxImageBox1.Window;
                    tb_l.Value = dxImageBox1.Level;

                    if (sia.sd.bc.i == 1 && sia.sd.bc.e == 2)
                        cb_ie.SelectedIndex = 0;
                    else if (sia.sd.bc.i == 2 && sia.sd.bc.e == 1)
                        cb_ie.SelectedIndex = 1;
                    else if (sia.sd.bc.i == 1 && sia.sd.bc.e == 4)
                        cb_ie.SelectedIndex = 2;
                    else if (sia.sd.bc.i == 4 && sia.sd.bc.e == 1)
                        cb_ie.SelectedIndex = 3;
                    else
                        cb_ie.SelectedIndex = -1;

                    tb_fa.Text = sia.sd.bc.f_adjust.ToString();

                    cb_pc.Checked = (sia.sd.bc.is_pc == 0) ? false : true;
                    cb_injured.Checked = (sia.sd.bc.is_injured == 0) ? false : true;

                    tb_fpb.Text = sia.sd.bc.fpb.ToString();
                    tb_tinterval.Text = sia.sd.bc.time_interval.ToString();

                    lab_name.Text = sd.Name;

                    mldDisplay1.ImageData = id;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<System.IO.FileInfo> fis = new List<System.IO.FileInfo>();
            foreach (MyListViewItem lvi in listView1.CheckedItems)
            {
                fis.Add(lvi.fi);
                libctdyn.libctdyn.SetBreathCharacteristics(lvi.sd, lvi.sd.bc);
            }

            ProgressBar pb = new ProgressBar(fis, outputfile.Text);
            pb.ShowDialog();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tb_fa_TextChanged(object sender, EventArgs e)
        {
            int val;
            if (int.TryParse(tb_fa.Text, out val))
            {
                mldDisplay1.FrameAdjust = val;

                if (listView1.SelectedItems.Count > 0)
                    ((MyListViewItem)listView1.SelectedItems[0]).sd.bc.f_adjust = val;
            }
        }

        private void cb_ie_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void cb_ie_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cb_ie.SelectedIndex)
            {
                case 0:
                    mldDisplay1.I = 1;
                    mldDisplay1.E = 2;
                    break;
                case 1:
                    mldDisplay1.I = 2;
                    mldDisplay1.E = 1;
                    break;
                case 2:
                    mldDisplay1.I = 1;
                    mldDisplay1.E = 4;
                    break;
                case 3:
                    mldDisplay1.I = 4;
                    mldDisplay1.E = 1;
                    break;
            }

            if (listView1.SelectedItems.Count > 0)
            {
                ((MyListViewItem)listView1.SelectedItems[0]).sd.bc.i = mldDisplay1.I;
                ((MyListViewItem)listView1.SelectedItems[0]).sd.bc.e = mldDisplay1.E;
            }
        }

        private void cb_pc_CheckedChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
                ((MyListViewItem)listView1.SelectedItems[0]).sd.bc.is_pc = cb_pc.Checked ? 1 : 0;
        }

        private void cb_injured_CheckedChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
                ((MyListViewItem)listView1.SelectedItems[0]).sd.bc.is_injured = cb_injured.Checked ? 1 : 0;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            persist_datafile();
            Properties.Settings.Default.InputPath = inputdir.Text;
            Properties.Settings.Default.OutputPath = outputfile.Text;
            Properties.Settings.Default.Save();
        }

        private void dxImageBox1_Resize(object sender, EventArgs e)
        {
            dxImageBox1.Invalidate();
        }

        private void tb_fpb_TextChanged(object sender, EventArgs e)
        {
            int val;
            if (int.TryParse(tb_fpb.Text, out val))
            {
                mldDisplay1.FramesPerBreath = val;

                if (listView1.SelectedItems.Count > 0)
                    ((MyListViewItem)listView1.SelectedItems[0]).sd.bc.fpb = val;
            }
        }

        private void tb_tinterval_TextChanged(object sender, EventArgs e)
        {
            int val;
            if (int.TryParse(tb_tinterval.Text, out val))
            {
                if (listView1.SelectedItems.Count > 0)
                    ((MyListViewItem)listView1.SelectedItems[0]).sd.bc.time_interval = val;
            }
        }
    }
}
