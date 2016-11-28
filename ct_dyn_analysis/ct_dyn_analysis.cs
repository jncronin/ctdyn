/* Copyright (C) 2016 by John Cronin
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
using System.IO;
using System.Linq;
using System.Text;
using Mono.Options;

namespace ct_dyn_analysis
{
    class ct_dyn_analysis
    {
        static void Main(string[] args)
        {
            // Generate output file
            string ofname = null;
            string file_dir = null;
            bool show_help = false;

            var p = new OptionSet()
            {
                {
                    "o|output-file=", "output file name",
                    v => ofname = v
                },
                {
                    "d|file-dir=", "input file directory",
                    v => file_dir = v
                },
                {
                    "h|help", "show this message and exit",
                    v => show_help = v != null
                }
            };

            p.Parse(args);
            if(show_help || file_dir == null || ofname == null)
            {
                ShowHelp(p);
                return;
            }

            libctdyn.libctdyn.DoAnalysis(libctdyn.libctdyn.GetFilesInDir(file_dir), ofname, ConsoleRP);
        }

        private static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: ct_dyn_analysis [options] -o output_file -d input_file_dir");
            Console.WriteLine();
            Console.WriteLine("Options: ");
            p.WriteOptionDescriptions(Console.Out);
        }

        static void ConsoleRP(int n, int total)
        {
            Console.WriteLine((n * 100 / total).ToString() + "%");
        }

        private static void GenerateBitmap(short[,] slice, int[] dimensions, int z)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(dimensions[0], dimensions[1], System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            /*bmp.Set

            var bd = bmp.LockBits(new System.Drawing.Rectangle(0, 0, dimensions[0], dimensions[1]), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int bytes = bd.Stride * bd.Height;
            IntPtr ptr = bd.Scan0;
            byte[] rgbValues = new byte[bytes];
            System.Runtime*/

            int level = 0;
            int window = 200;
            int low = level - window / 2;
            int high = level + window / 2;

            for(int y = 0; y < dimensions[1]; y++)
            {
                for(int x = 0; x < dimensions[0]; x++)
                {
                    var pixel = slice[y, x];

                    double intensity;
                    if (pixel < low)
                        intensity = 0;
                    else if (pixel > high)
                        intensity = 255;
                    else
                        intensity = ((double)pixel - low) * 255.0 / window;

                    bmp.SetPixel(x, y, System.Drawing.Color.FromArgb((int)intensity, (int)intensity, (int)intensity));
                }
            }

            bmp.Save(@"c:\tmp\" + z.ToString() + ".bmp");
        }
    }
}
