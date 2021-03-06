﻿/* Copyright (C) 2016 by John Cronin
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:

* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.

* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

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

            tb_opacity.Text = tb_thresh.Value.ToString();
            tb_window.Text = tb_w.Value.ToString();
            tb_level.Text = tb_l.Value.ToString();
            tb_slice.Text = tb_frame.Value.ToString();
        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tb_frame_Scroll(object sender, EventArgs e)
        {
            dxImageBox1.Frame = tb_frame.Value;
            tb_slice.Text = tb_frame.Value.ToString();
            mldDisplay1.Marker = tb_frame.Value;
        }

        private void tb_w_Scroll(object sender, EventArgs e)
        {
            dxImageBox1.Window = tb_w.Value;
            tb_window.Text = tb_w.Value.ToString();
        }

        private void tb_l_Scroll(object sender, EventArgs e)
        {
            dxImageBox1.Level = tb_l.Value;
            tb_level.Text = tb_l.Value.ToString();
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
            tb_slice.Text = new_z.ToString();
            mldDisplay1.Marker = new_z;
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

                tb_window.Text = new_w.ToString();
                tb_level.Text = new_l.ToString();
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

            dxImageBox1.Window = tb_w.Value;
            dxImageBox1.Level = tb_l.Value;

            tb_window.Text = tb_w.Value.ToString();
            tb_level.Text = tb_l.Value.ToString();
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
            tb_opacity.Text = tb_thresh.Value.ToString();
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
            catch(Exception e)
            {
                listView1.Items.Clear();
            }
        }

        private void load_datafile(string v)
        {
            System.IO.StreamReader sr = null;

            try
            {
                data_file = new System.IO.FileInfo(v);

                var fs = new System.IO.FileStream(v, System.IO.FileMode.Open, System.IO.FileAccess.Read,
                    System.IO.FileShare.None);

                sr = new System.IO.StreamReader(fs);

                while (!sr.EndOfStream)
                {
                    int subj_idx = int.Parse(sr.ReadLine());
                    int ser_idx = int.Parse(sr.ReadLine());
                    int oth_idx = int.Parse(sr.ReadLine());

                    int i = int.Parse(sr.ReadLine());
                    int e = int.Parse(sr.ReadLine());
                    int f_adjust = int.Parse(sr.ReadLine());
                    int is_pc = int.Parse(sr.ReadLine());
                    int is_injured = int.Parse(sr.ReadLine());
                    int fpb = int.Parse(sr.ReadLine());
                    int tinterval = int.Parse(sr.ReadLine());
                    bool is_checked = bool.Parse(sr.ReadLine());

                    foreach (MyListViewItem lvi in listView1.Items)
                    {
                        if (lvi.sd.SubjectIndex == subj_idx &&
                            lvi.sd.SeriesIndex == ser_idx &&
                            lvi.sd.OtherIndex == oth_idx)
                        {
                            lvi.sd.bc.i = i;
                            lvi.sd.bc.e = e;
                            lvi.sd.bc.f_adjust = f_adjust;
                            lvi.sd.bc.is_pc = is_pc;
                            lvi.sd.bc.is_injured = is_injured;
                            lvi.sd.bc.fpb = fpb;
                            lvi.sd.bc.time_interval = tinterval;
                            lvi.Checked = is_checked;
                        }
                    }
                }
            }
            catch(System.IO.FileNotFoundException)
            {
                return;
            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                if(sr != null)
                    sr.Close();
            }
        }

        private void persist_datafile()
        {
            if (listView1.Items.Count == 0)
                return;

            System.IO.StreamWriter sw = null;
            try
            {
                if (data_file != null)
                {
                    var fs = new System.IO.FileStream(data_file.FullName, System.IO.FileMode.Create,
                        System.IO.FileAccess.Write, System.IO.FileShare.Read);
                    sw = new System.IO.StreamWriter(fs);

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
                        sw.WriteLine(lvi.Checked.ToString());
                    }
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                if (sw != null)
                    sw.Close();
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
                    tb_slice.Text = "0";

                    tb_w.Value = dxImageBox1.Window;
                    tb_window.Text = tb_w.Value.ToString();

                    tb_l.Value = dxImageBox1.Level;
                    tb_level.Text = tb_l.Value.ToString();

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
                    cb_flip.Checked = false;

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

        private void tb_slice_TextChanged(object sender, EventArgs e)
        {
            int val;
            if (int.TryParse(tb_slice.Text, out val))
            {
                if (val >= tb_frame.Minimum && val <= tb_frame.Maximum)
                {
                    tb_frame.Value = val;
                    dxImageBox1.Frame = val;
                    mldDisplay1.Marker = val;
                }
            }
        }

        private void tb_window_TextChanged(object sender, EventArgs e)
        {
            int val;
            if (int.TryParse(tb_window.Text, out val))
            {
                if (val >= tb_w.Minimum && val <= tb_w.Maximum)
                {
                    tb_w.Value = val;
                    dxImageBox1.Window = val;
                }
            }
        }

        private void tb_level_TextChanged(object sender, EventArgs e)
        {
            int val;
            if (int.TryParse(tb_level.Text, out val))
            {
                if (val >= tb_l.Minimum && val <= tb_l.Maximum)
                {
                    tb_l.Value = val;
                    dxImageBox1.Level = val;
                }
            }
        }

        private void tb_opacity_TextChanged(object sender, EventArgs e)
        {
            int val;
            if(int.TryParse(tb_opacity.Text, out val))
            {
                if(val >= tb_thresh.Minimum && val <= tb_thresh.Maximum)
                {
                    tb_thresh.Value = val;
                    thresh_changed();
                }
            }
        }

        private void button2_Click(object sender, EventArgs eargs)
        {
            var fp = new SaveFileDialog();
            fp.Filter = "Portable Network Graphic|*.png";
            fp.ShowDialog();

            string fname;

            try
            {
                fname = fp.FileName;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
                return;
            }

            var ss = dxImageBox1.GetScreenshot();

            if(ss == null)
            {
                MessageBox.Show("Unable to get screenshot", "Error");
                return;
            }

            Bitmap bmp = new Bitmap(ss.GetLength(1), ss.GetLength(0),
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            var bd = bmp.LockBits(new Rectangle(0, 0, ss.GetLength(1), ss.GetLength(0)),
                System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            for(int y = 0; y < ss.GetLength(0); y++)
            {
                unsafe
                {
                    uint* destPtr = (uint*)(bd.Scan0 + y * bd.Stride);
                    fixed(uint *srcPtr = &ss[y, 0])
                    {
                        for (int x = 0; x < ss.GetLength(1); x++)
                        {
                            var val = srcPtr[x];

                            // flip blue and red (ABGR->ARGB)
                            uint a = val >> 24;
                            uint b = (val >> 16) & 0xffU;
                            uint g = (val >> 8) & 0xffU;
                            uint r = val & 0xffU;
                            uint newval = (a << 24) | (r << 16) | (g << 8) | b;

                            destPtr[x] = newval;
                        }
                    }
                }
            }

            bmp.UnlockBits(bd);

            bmp.Save(fname);
        }

        private void btn_toggleall_Click(object sender, EventArgs e)
        {
            // decide if all checked
            bool set_val = false;
            foreach(MyListViewItem lvi in listView1.Items)
            {
                if (!lvi.Checked)
                    set_val = true;
            }

            foreach(MyListViewItem lvi in listView1.Items)
            {
                lvi.Checked = set_val;
            }
        }

        private void cb_flip_CheckedChanged(object sender, EventArgs e)
        {
            dxImageBox1.Flip = cb_flip.Checked;
        }
    }
}
