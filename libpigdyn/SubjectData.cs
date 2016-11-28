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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libctdyn
{
    public class SubjectData
    {
        public string source_name;
        public string acquisition_datetime;
        public string series_name;
        public string subject_name;

        public class BreathCharacteristics
        {
            public int is_pc, i = 1, e = 2, is_injured, f_adjust, fpb = 20, time_interval = 250;
        }

        public BreathCharacteristics bc = new BreathCharacteristics();

        internal static System.Text.RegularExpressions.Regex subj, dyn;

        static SubjectData()
        {
            subj = new System.Text.RegularExpressions.Regex(@"^[^0-9]*([0-9]+)");
            dyn = new System.Text.RegularExpressions.Regex(@"^[^0-9]*([0-9]+)");
        }

        public int SubjectIndex
        {
            get
            {
                try
                {
                    var subj_match = subj.Match(subject_name);
                    return int.Parse(subj_match.Groups[1].Value);
                }
                catch(Exception)
                {
                    return 0;
                }
            }
        }

        public int SeriesIndex
        {
            get
            {
                try
                {
                    return int.Parse(series_name);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public int OtherIndex
        {
            get
            {
                try
                {
                    var other_match = dyn.Match(source_name);
                    return int.Parse(other_match.Groups[1].Value);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public string Name
        {
            get
            {
                return "Subj" + SubjectIndex.ToString() + "S" + SeriesIndex.ToString() + "Dyn" + OtherIndex.ToString();
            }
        }

        public DateTime? AcquisitionTime
        {
            get
            {
                var at = acquisition_datetime;
                try
                {
                    DateTime dt = new DateTime(int.Parse(at.Substring(0, 4)), int.Parse(at.Substring(4, 2)), int.Parse(at.Substring(6, 2)),
                        int.Parse(at.Substring(8, 2)), int.Parse(at.Substring(10, 2)), int.Parse(at.Substring(12, 2)),
                        int.Parse(at.Substring(15, 3)));
                    return dt;
                }
                catch(Exception)
                {
                    return null;
                }
            }
        }

        public int[] dimensions = new int[3];
        public double[] spacing = new double[3];
    }
}
