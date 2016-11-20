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
using System.Threading.Tasks;

namespace libctdyn
{
    class ZoneModule : Module
    {
        int zones;

        public ZoneModule(int _zones)
        {
            zones = _zones;
        }

        public override void OutputHeaders(StreamWriter sw)
        {
            for (int i = 0; i < zones; i++)
            {
                sw.Write("z" + i.ToString() + "_count,");
                sw.Write("z" + i.ToString() + "_mean,");
                sw.Write("z" + i.ToString() + "_area,");
                sw.Write("z" + i.ToString() + "_mass,");
            }
        }

        public override void OutputSlice(StreamWriter sw, short[,] data, int z, int f_index, SubjectData pd)
        {
            List<short>[] pixels = new List<short>[zones];
            for (int i = 0; i < zones; i++)
                pixels[i] = new List<short>();


            // Do an initial pass to determine the first and last rows that contain pixels
            int first_row = -1;
            int last_row = -1;
            int count = 0;

            for (int y = 0; y < pd.dimensions[1]; y++)
            {
                for (int x = 0; x < pd.dimensions[0]; x++)
                {
                    short pixel = data[y, x];
                    if (pixel >= -1000 && pixel <= 100)
                    {
                        if (first_row == -1)
                            first_row = y;
                        last_row = y;
                        count++;
                    }
                }
            }

            var dim_height = (double)(last_row - first_row) / zones;

            if (first_row == -1)
            {
                sw.Write("0,0,0,0,");
                return;
            }

            for (int y = 0; y < pd.dimensions[1]; y++)
            {
                // Old algorithm
                int zone = (int)((y - first_row) / dim_height);
                if (zone >= zones)
                    zone = zones - 1;

                for (int x = 0; x < pd.dimensions[0]; x++)
                {
                    short pixel = data[y, x];

                    if (pixel >= -1000 && pixel <= 100)
                    {
                        //int zone = cur_pixel++ * zones / count;
                        if (zone >= zones)
                            zone = zones - 1;
                        pixels[zone].Add(pixel);
                    }
                }
            }

            int[] counts = new int[zones];
            double[] means = new double[zones];
            double[] areas = new double[zones];
            double[] masses = new double[zones];

            for (int i = 0; i < zones; i++)
            {
                counts[i] = pixels[i].Count;

                double mean = 0.0;
                foreach (var pixel in pixels[i])
                    mean += pixel;
                mean /= pixels[i].Count;
                means[i] = mean;

                areas[i] = pixels[i].Count * pd.spacing[0] * pd.spacing[1] / 100;

                masses[i] = areas[i] * pd.spacing[2] / 10 * (1.0 + mean / 1000.0);
            }

            // output
            for (int i = 0; i < zones; i++)
            {
                sw.Write(counts[i].ToString() + ",");
                sw.Write(means[i].ToString() + ",");
                sw.Write(areas[i].ToString() + ",");
                sw.Write(masses[i].ToString() + ",");
            }
        }
    }
}
