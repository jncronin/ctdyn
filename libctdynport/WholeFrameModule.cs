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
    class WholeFrameModule : Module
    {
        public override void OutputHeaders(StreamWriter sw)
        {
            sw.Write("total_count,");
            sw.Write("total_mean,");
            sw.Write("total_area,");
            sw.Write("total_mass,");
        }

        public override void OutputSlice(StreamWriter sw, short[,] slice, int z, int f_index, SubjectData pd)
        {
            int count = 0;
            double mean = 0.0;
            double area;
            double mass;

            for (int y = 0; y < pd.dimensions[1]; y++)
            {
                for (int x = 0; x < pd.dimensions[0]; x++)
                {
                    short pixel = slice[y, x];
                    if (pixel <= -1001 || pixel > 100)
                        continue;

                    count++;
                    mean += pixel;
                }
            }

            mean /= count;
            area = count * pd.spacing[0] * pd.spacing[1] / 100;
            mass = area * pd.spacing[2] / 10 * (1.0 + mean / 1000.0);

            sw.Write(count.ToString() + ",");
            sw.Write(mean.ToString() + ",");
            sw.Write(area.ToString() + ",");
            sw.Write(mass.ToString() + ",");
        }
    }
}
