namespace ct_dyn
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.inputdir = new System.Windows.Forms.TextBox();
            this.inputdirbtn = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.outputfile = new System.Windows.Forms.TextBox();
            this.outputfilebtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.cb_zones = new System.Windows.Forms.CheckBox();
            this.cb_thresholds = new System.Windows.Forms.CheckBox();
            this.btn_reset = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tb_frame = new System.Windows.Forms.TrackBar();
            this.tb_w = new System.Windows.Forms.TrackBar();
            this.tb_l = new System.Windows.Forms.TrackBar();
            this.tb_thresh = new System.Windows.Forms.TrackBar();
            this.dxImageBox1 = new DXImageBox.DXImageBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tb_frame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_w)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_l)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_thresh)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.81896F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.18104F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1488F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(2112, 1488);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.listView1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.button1, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(792, 1482);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.inputdir, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.inputdirbtn, 2, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(786, 53);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Input Directory:";
            // 
            // inputdir
            // 
            this.inputdir.Dock = System.Windows.Forms.DockStyle.Top;
            this.inputdir.Location = new System.Drawing.Point(182, 3);
            this.inputdir.Name = "inputdir";
            this.inputdir.Size = new System.Drawing.Size(520, 35);
            this.inputdir.TabIndex = 1;
            // 
            // inputdirbtn
            // 
            this.inputdirbtn.AutoSize = true;
            this.inputdirbtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.inputdirbtn.Location = new System.Drawing.Point(708, 3);
            this.inputdirbtn.Name = "inputdirbtn";
            this.inputdirbtn.Size = new System.Drawing.Size(75, 47);
            this.inputdirbtn.TabIndex = 2;
            this.inputdirbtn.Text = "...";
            this.inputdirbtn.UseVisualStyleBackColor = true;
            this.inputdirbtn.Click += new System.EventHandler(this.inputdirbtn_Click);
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(3, 62);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(786, 1321);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.outputfile, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.outputfilebtn, 2, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 1389);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(786, 45);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 29);
            this.label2.TabIndex = 0;
            this.label2.Text = "Output File:";
            // 
            // outputfile
            // 
            this.outputfile.Dock = System.Windows.Forms.DockStyle.Top;
            this.outputfile.Location = new System.Drawing.Point(146, 3);
            this.outputfile.Name = "outputfile";
            this.outputfile.Size = new System.Drawing.Size(556, 35);
            this.outputfile.TabIndex = 1;
            // 
            // outputfilebtn
            // 
            this.outputfilebtn.AutoSize = true;
            this.outputfilebtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.outputfilebtn.Location = new System.Drawing.Point(708, 3);
            this.outputfilebtn.Name = "outputfilebtn";
            this.outputfilebtn.Size = new System.Drawing.Size(75, 39);
            this.outputfilebtn.TabIndex = 2;
            this.outputfilebtn.Text = "...";
            this.outputfilebtn.UseVisualStyleBackColor = true;
            this.outputfilebtn.Click += new System.EventHandler(this.outputfilebtn_Click);
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(3, 1440);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(786, 39);
            this.button1.TabIndex = 3;
            this.button1.Text = "Do Analysis";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.tb_thresh, 0, 5);
            this.tableLayoutPanel5.Controls.Add(this.tb_l, 0, 4);
            this.tableLayoutPanel5.Controls.Add(this.tb_w, 0, 3);
            this.tableLayoutPanel5.Controls.Add(this.tb_frame, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.dxImageBox1, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(801, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 6;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1308, 1482);
            this.tableLayoutPanel5.TabIndex = 1;
            this.tableLayoutPanel5.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel5_Paint);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.AutoSize = true;
            this.tableLayoutPanel6.ColumnCount = 3;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanel6.Controls.Add(this.btn_reset, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.cb_thresholds, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.cb_zones, 2, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(1294, 45);
            this.tableLayoutPanel6.TabIndex = 3;
            // 
            // cb_zones
            // 
            this.cb_zones.AutoSize = true;
            this.cb_zones.Dock = System.Windows.Forms.DockStyle.Top;
            this.cb_zones.Location = new System.Drawing.Point(857, 3);
            this.cb_zones.Name = "cb_zones";
            this.cb_zones.Size = new System.Drawing.Size(434, 33);
            this.cb_zones.TabIndex = 2;
            this.cb_zones.Text = "Show AP Zones";
            this.cb_zones.UseVisualStyleBackColor = true;
            this.cb_zones.CheckedChanged += new System.EventHandler(this.cb_zones_CheckedChanged);
            // 
            // cb_thresholds
            // 
            this.cb_thresholds.AutoSize = true;
            this.cb_thresholds.Dock = System.Windows.Forms.DockStyle.Top;
            this.cb_thresholds.Location = new System.Drawing.Point(430, 3);
            this.cb_thresholds.Name = "cb_thresholds";
            this.cb_thresholds.Size = new System.Drawing.Size(421, 33);
            this.cb_thresholds.TabIndex = 1;
            this.cb_thresholds.Text = "Show Thresholds";
            this.cb_thresholds.UseVisualStyleBackColor = true;
            this.cb_thresholds.CheckedChanged += new System.EventHandler(this.cb_thresholds_CheckedChanged);
            // 
            // btn_reset
            // 
            this.btn_reset.AutoSize = true;
            this.btn_reset.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_reset.Location = new System.Drawing.Point(3, 3);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(421, 39);
            this.btn_reset.TabIndex = 3;
            this.btn_reset.Text = "Reset Window/Level";
            this.btn_reset.UseVisualStyleBackColor = true;
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.tableLayoutPanel6);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 537);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1302, 180);
            this.flowLayoutPanel1.TabIndex = 1;
            this.flowLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel1_Paint);
            // 
            // tb_frame
            // 
            this.tb_frame.AutoSize = false;
            this.tb_frame.Dock = System.Windows.Forms.DockStyle.Top;
            this.tb_frame.Location = new System.Drawing.Point(3, 1071);
            this.tb_frame.Name = "tb_frame";
            this.tb_frame.Size = new System.Drawing.Size(1302, 87);
            this.tb_frame.TabIndex = 5;
            this.tb_frame.Scroll += new System.EventHandler(this.tb_frame_Scroll);
            // 
            // tb_w
            // 
            this.tb_w.AutoSize = false;
            this.tb_w.Dock = System.Windows.Forms.DockStyle.Top;
            this.tb_w.Location = new System.Drawing.Point(3, 1164);
            this.tb_w.Maximum = 4000;
            this.tb_w.Name = "tb_w";
            this.tb_w.Size = new System.Drawing.Size(1302, 101);
            this.tb_w.TabIndex = 6;
            this.tb_w.TickFrequency = 100;
            this.tb_w.Value = 1400;
            this.tb_w.Scroll += new System.EventHandler(this.tb_w_Scroll);
            // 
            // tb_l
            // 
            this.tb_l.AutoSize = false;
            this.tb_l.Dock = System.Windows.Forms.DockStyle.Top;
            this.tb_l.Location = new System.Drawing.Point(3, 1271);
            this.tb_l.Maximum = 2000;
            this.tb_l.Minimum = -2000;
            this.tb_l.Name = "tb_l";
            this.tb_l.Size = new System.Drawing.Size(1302, 101);
            this.tb_l.TabIndex = 7;
            this.tb_l.TickFrequency = 100;
            this.tb_l.Value = -500;
            this.tb_l.Scroll += new System.EventHandler(this.tb_l_Scroll);
            // 
            // tb_thresh
            // 
            this.tb_thresh.Dock = System.Windows.Forms.DockStyle.Top;
            this.tb_thresh.Location = new System.Drawing.Point(3, 1378);
            this.tb_thresh.Maximum = 100;
            this.tb_thresh.Name = "tb_thresh";
            this.tb_thresh.Size = new System.Drawing.Size(1302, 101);
            this.tb_thresh.TabIndex = 8;
            this.tb_thresh.TickFrequency = 10;
            this.tb_thresh.Value = 60;
            this.tb_thresh.Scroll += new System.EventHandler(this.tb_thresh_Scroll_1);
            // 
            // dxImageBox1
            // 
            this.dxImageBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dxImageBox1.Frame = 0;
            this.dxImageBox1.Level = -500;
            this.dxImageBox1.Location = new System.Drawing.Point(3, 3);
            this.dxImageBox1.Name = "dxImageBox1";
            this.dxImageBox1.ShowZones = false;
            this.dxImageBox1.Size = new System.Drawing.Size(1302, 528);
            this.dxImageBox1.TabIndex = 0;
            this.dxImageBox1.Text = "dxImageBox1";
            this.dxImageBox1.Threshold = 0F;
            this.dxImageBox1.Window = 1400;
            this.dxImageBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dxImageBox1_MouseDown);
            this.dxImageBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dxImageBox1_MouseMove);
            this.dxImageBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.dxImageBox1_MouseWheel);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2112, 1488);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "CT Dynamic Analysis";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tb_frame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_w)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_l)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_thresh)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox inputdir;
        private System.Windows.Forms.Button inputdirbtn;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox outputfile;
        private System.Windows.Forms.Button outputfilebtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private DXImageBox.DXImageBox dxImageBox1;
        private System.Windows.Forms.TrackBar tb_thresh;
        private System.Windows.Forms.TrackBar tb_l;
        private System.Windows.Forms.TrackBar tb_w;
        private System.Windows.Forms.TrackBar tb_frame;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Button btn_reset;
        private System.Windows.Forms.CheckBox cb_thresholds;
        private System.Windows.Forms.CheckBox cb_zones;
    }
}

