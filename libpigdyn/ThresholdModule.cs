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
    class Threshold
    {
        public string Name;
        public int LowerBound;
        public int UpperBound;
    }

    class ThresholdModule : Module
    {
        Threshold[] thresholds;

        public ThresholdModule(Threshold[] _thresholds)
        {
            thresholds = _thresholds;
        }

        public override void OutputHeaders(StreamWriter sw)
        {
            for (int i = 0; i < thresholds.Length; i++)
            {
                var t = thresholds[i].Name;
                sw.Write(t + "_count,");
                sw.Write(t + "_mean,");
                sw.Write(t + "_area,");
                sw.Write(t + "_mass,");
            }
        }

        public override void OutputSlice(StreamWriter sw, short[,] data, int z, int f_index, SubjectData pd)
        {
            List<short>[] pixels = new List<short>[thresholds.Length];
            for (int i = 0; i < thresholds.Length; i++)
                pixels[i] = new List<short>();

            for (int y = 0; y < pd.dimensions[1]; y++)
            {
                for (int x = 0; x < pd.dimensions[0]; x++)
                {
                    short pixel = data[y, x];

                    for (int i = 0; i < thresholds.Length; i++)
                    {
                        var t = thresholds[i];
                        if (pixel >= t.LowerBound && pixel <= t.UpperBound)
                            pixels[i].Add(pixel);
                    }
                }
            }

            int[] counts = new int[thresholds.Length];
            double[] means = new double[thresholds.Length];
            double[] areas = new double[thresholds.Length];
            double[] masses = new double[thresholds.Length];

            for (int i = 0; i < thresholds.Length; i++)
            {
                counts[i] = pixels[i].Count;

                double mean = 0.0;
                foreach (var pixel in pixels[i])
                    mean += pixel;

                // Avoid NaNs
                if(pixels[i].Count != 0)
                    mean /= pixels[i].Count;

                means[i] = mean;

                areas[i] = pixels[i].Count * pd.spacing[0] * pd.spacing[1] / 100;

                masses[i] = areas[i] * pd.spacing[2] / 10 * (1.0 + mean / 1000.0);
            }

            // output
            for (int i = 0; i < thresholds.Length; i++)
            {
                sw.Write(counts[i].ToString() + ",");
                sw.Write(means[i].ToString() + ",");
                sw.Write(areas[i].ToString() + ",");
                sw.Write(masses[i].ToString() + ",");
            }
        }
    }
}
