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
        System.Drawing.Pen p;

        int[] points = null;
        int max_pt = -1000;
        int min_pt = 0;
        int i = 1;
        int e = 2;

        int fa = 0;

        public MLDDisplay()
        {
            bground = new System.Drawing.SolidBrush(System.Drawing.Color.White);
            fground = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            f = new System.Drawing.Font("Arial", 14.0f);
            p = new System.Drawing.Pen(fground, 1.0f);
        }

        public int I { get { return i; } set { i = value; Invalidate(); } }
        public int E { get { return e; } set { e = value; Invalidate(); } }
        public int FrameAdjust { get { return fa; } set { fa = value; Invalidate(); } }

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
                }

                int ie_line1 = ClientSize.Width * i / (i + e) / 2;
                int ie_line2 = ie_line1 + ClientSize.Width / 2;
                eargs.Graphics.DrawLine(p, ie_line1, 0, ie_line1, ClientSize.Height);
                eargs.Graphics.DrawLine(p, ie_line2, 0, ie_line2, ClientSize.Height);

                //eargs.Graphics.DrawString(i.ToString() + ":" + e.ToString(), f, fground, 10.0f, 10.0f);

            }
        }
    }
}
