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
    class SubjectInfo : Module
    {
        public override void OutputHeaders(StreamWriter sw)
        {
            sw.Write("subj_id,breath_id,ct_date,ct_time,is_injured,is_pc,ie_ratio,ie,pc_ie,");
        }

        public override void OutputSlice(StreamWriter sw, short[,] slice, int z, int f_index, SubjectData pd)
        {
            // parse subject data
            var subj_id = pd.SubjectIndex;
            string breath_id = pd.Name;

            // Determine breath characteristics
            SubjectData.BreathCharacteristics bc = libctdyn.GetBreathCharacteristics(pd);

            breath_id += (char)('a' + f_index / bc.fpb);

            sw.Write(subj_id.ToString() + ",");
            sw.Write(breath_id + ",");

            // split date time
            var at = pd.acquisition_datetime;
            if (pd.AcquisitionTime.HasValue)
            {
                sw.Write(pd.AcquisitionTime.Value.ToShortDateString() + "," +
                    pd.AcquisitionTime.Value.ToLongTimeString() + ",");
            }
            else
                sw.Write(",,");

            if (bc == null)
                sw.Write(",,,,,");
            else
                sw.Write(bc.is_injured.ToString() + "," +
                    bc.is_pc.ToString() + "," +
                    ((double)bc.i / (double)bc.e).ToString() + "," +
                    bc.i.ToString() + "_" + bc.e.ToString() + "," +
                    bc.is_pc.ToString() + "_" + bc.i.ToString() + "_" + bc.e.ToString() + ",");
        }
    }
}
