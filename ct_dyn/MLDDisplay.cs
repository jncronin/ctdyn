using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ct_dyn
{
    class MLDDisplay : Control
    {
        System.Drawing.Font f;
        System.Drawing.Brush bground, fground;
        System.Drawing.Pen p, p2, p3;

        int[] points = null;
        int max_pt = -1000;
        int min_pt = 0;
        int i = 1;
        int e = 2;
        int fpb = 20;
        int marker = 0;

        int fa = 0;

        public MLDDisplay()
        {
            bground = new System.Drawing.SolidBrush(System.Drawing.Color.White);
            fground = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            f = new System.Drawing.Font("Arial", 14.0f);
            p = new System.Drawing.Pen(fground, 1.0f);
            p2 = new System.Drawing.Pen(fground, 1.0f);
            p2.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            p3 = new System.Drawing.Pen(System.Drawing.Color.DarkBlue, 1.0f);
            p3.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
        }

        public int I { get { return i; } set { i = value; Invalidate(); } }
        public int E { get { return e; } set { e = value; Invalidate(); } }
        public int FrameAdjust { get { return fa; } set { fa = value; Invalidate(); } }
        public int FramesPerBreath { get { return fpb; } set { fpb = value; Invalidate(); } }
        public int Marker { get { return marker; } set { marker = value; Invalidate(); } }

        public short[,,] ImageData
        {
            set
            {
                if(value == null)
                {
                    points = null;
                }
                else
                {
                    int slices = value.GetLength(0);

                    points = new int[slices];
                    max_pt = -1000;
                    min_pt = 0;

                    for(int z = 0; z < slices; z++)
                    {
                        long count = 0;
                        long items = 0;
                        for(int y = 0; y < value.GetLength(1); y++)
                        {
                            for (int x = 0; x < value.GetLength(2); x++)
                            {
                                var v = value[z, y, x];

                                if (v >= -1000)
                                {
                                    count += value[z, y, x];
                                    items++;
                                }
                            }
                        }

                        if(items > 0)
                            count = count / items;

                        points[z] = (int)count;

                        if (points[z] > max_pt)
                            max_pt = points[z];
                        if (points[z] < min_pt)
                            min_pt = points[z];
                    }
                }

                Invalidate();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            this.Invalidate();
            base.OnResize(e);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            pevent.Graphics.FillRectangle(bground, pevent.ClipRectangle);
        }

        protected override void OnPaint(PaintEventArgs eargs)
        {
            if(points == null)
                eargs.Graphics.DrawString("No data", f, fground, 10.0f, 10.0f);
            else
            {
                for(int idx = 1; idx < points.Length; idx++)
                {
                    int cur_z = idx + fa;
                    while (cur_z < 0)
                        cur_z += points.Length;
                    cur_z %= points.Length;

                    int prev_z = idx - 1 + fa;
                    while (prev_z < 0)
                        prev_z += points.Length;
                    prev_z %= points.Length;
                                      

                    int prev_pt = (points[prev_z] - min_pt) * ClientSize.Height / (max_pt - min_pt);
                    int cur_pt = (points[cur_z] - min_pt) * ClientSize.Height / (max_pt - min_pt);

                    int cur_x = idx * ClientSize.Width / points.Length;
                    int prev_x = (idx - 1) * ClientSize.Width / points.Length;

                    eargs.Graphics.DrawLine(p, prev_x, prev_pt, cur_x, cur_pt);
                    eargs.Graphics.FillEllipse(fground, prev_x - 2, prev_pt - 2, 5, 5);
                    eargs.Graphics.FillEllipse(fground, cur_x - 2, cur_pt - 2, 5, 5);
                }

                for(int start_frame = 0; start_frame < points.Length; start_frame += fpb)
                {
                    int start_x = ClientSize.Width * start_frame / points.Length;
                    eargs.Graphics.DrawLine(p, start_x, 0, start_x, ClientSize.Height);

                    int ie_line = start_x + i * fpb * ClientSize.Width / (i + e) / points.Length;
                    eargs.Graphics.DrawLine(p2, ie_line, 0, ie_line, ClientSize.Height);
                }

                int marker_x = ClientSize.Width * marker / points.Length;
                eargs.Graphics.DrawLine(p3, marker_x, 0, marker_x, ClientSize.Height);
            }
        }
    }
}
