using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pig_View
{
    class ImageBox : System.Windows.Forms.Control
    {
        public short[,,] data;
        public int slice_number;
        public int window, level, low, high;
        public System.Drawing.Brush[] brushes;
        public System.Drawing.Brush[] label_brushes;

        System.Drawing.Font f;
        System.Drawing.Brush bground, fground;

        bool do_zones = false;
        bool do_thresholds = false;

        public bool DoZones
        {
            get { return do_zones; }
            set { do_zones = value; }
        }
        public bool DoThresholds
        {
            get { return do_thresholds; }
            set { do_thresholds = value; }
        }

        public ImageBox()
        {
            Width = 512;
            Height = 512;

            window = 1400;
            level = -500;
            slice_number = 0;

            low = level - window / 2;
            high = level + window / 2;

            brushes = new System.Drawing.Brush[256];
            for (int i = 0; i < 256; i++)
                brushes[i] = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(i, i, i));
            fground = new System.Drawing.SolidBrush(System.Drawing.Color.White);
            bground = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

            label_brushes = new System.Drawing.Brush[4];
            label_brushes[0] = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(255, 1, 0));
            label_brushes[1] = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(246, 255, 4));
            label_brushes[2] = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(58, 255, 28));
            label_brushes[3] = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(71, 47, 255));
            f = new System.Drawing.Font("Arial", 14.0f);

            SetStyle(ControlStyles.FixedHeight, true);
            SetStyle(ControlStyles.FixedWidth, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            pevent.Graphics.FillRectangle(bground, pevent.ClipRectangle);
        }

        protected override void OnResize(EventArgs e)
        {
            Width = 512;
            Height = 512;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if(data == null)
            {
                e.Graphics.FillRectangle(bground, e.ClipRectangle);
                e.Graphics.DrawString("No data", f, fground, 10.0f, 10.0f);
            }
            else
            {
                for(int y = e.ClipRectangle.Top; y <= e.ClipRectangle.Bottom; y++)
                {
                    for(int x = e.ClipRectangle.Left; x <= e.ClipRectangle.Bottom; x++)
                    {
                        double intensity;

                        int act_x = (int)((double)x * 512 / ClientSize.Width);
                        int act_y = (int)((double)y * 512 / ClientSize.Height);

                        if (do_thresholds)
                        {
                            if (act_x >= 0 && act_x <= 511 && act_y >= 0 && act_y <= 511)
                            {
                                var pixel = data[slice_number, 511 - act_y, 511 - act_x];

                                if (pixel >= 0 && pixel <= 3)
                                    e.Graphics.FillRectangle(label_brushes[(int)pixel], x, y, 1, 1);
                                else if (pixel == -2)
                                    e.Graphics.FillRectangle(fground, x, y, 1, 1);
                                else
                                    e.Graphics.FillRectangle(brushes[0], x, y, 1, 1);
                            }
                        }
                        else
                        {
                            if (act_x >= 0 && act_x <= 511 && act_y >= 0 && act_y <= 511)
                            {
                                var pixel = data[slice_number, 511 - act_y, 511 - act_x];
                                if (pixel < low)
                                    intensity = 0;
                                else if (pixel > high)
                                    intensity = 255;
                                else
                                    intensity = (double)(pixel - low) * 255.0 / window;
                            }
                            else
                                intensity = 0;

                            e.Graphics.FillRectangle(brushes[(int)intensity], x, y, 1, 1);
                        }
                    }
                }
            }
            
            base.OnPaint(e);
        }
    }
}
