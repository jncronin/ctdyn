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
        internal static System.Text.RegularExpressions.Regex pig, dyn;
        int fpb;

        internal class BreathCharacteristics
        {
            public int is_pc, i, e, is_injured, f_adjust;
        }

        internal static Dictionary<string, BreathCharacteristics> chars = new Dictionary<string, BreathCharacteristics>();

        public SubjectInfo(int frames_per_breath)
        {
            fpb = frames_per_breath;

        }

        static SubjectInfo()
        {
            pig = new System.Text.RegularExpressions.Regex(@"^[Pp][Ii][Gg][^0-9]*([0-9]+)");
            dyn = new System.Text.RegularExpressions.Regex(@"^Dyn vp ([0-9]+)");

            // Quick database for breath type mappings
            chars["PIG1S20Dyn1"] = new BreathCharacteristics { is_pc = 0, i = 1, e = 2 };
            chars["PIG1S21Dyn2"] = new BreathCharacteristics { is_pc = 0, i = 2, e = 1 };
            chars["PIG1S22Dyn3"] = new BreathCharacteristics { is_pc = 1, i = 1, e = 2 };
            chars["PIG1S23Dyn4"] = new BreathCharacteristics { is_pc = 1, i = 2, e = 1 };
            chars["PIG1S24Dyn5"] = new BreathCharacteristics { is_pc = 0, i = 1, e = 4, f_adjust = +1 };
            chars["PIG1S25Dyn6"] = new BreathCharacteristics { is_pc = 0, i = 4, e = 1, f_adjust = -1 };
            chars["PIG1S26Dyn7"] = new BreathCharacteristics { is_pc = 1, i = 1, e = 4 };
            chars["PIG1S27Dyn8"] = new BreathCharacteristics { is_pc = 1, i = 4, e = 1 };

            chars["PIG2S1Dyn9"] = new BreathCharacteristics { is_pc = 0, i = 1, e = 2, f_adjust = -1 };
            chars["PIG2S2Dyn10"] = new BreathCharacteristics { is_pc = 0, i = 2, e = 1 };
            chars["PIG2S3Dyn11"] = new BreathCharacteristics { is_pc = 1, i = 1, e = 2, f_adjust = -1 };
            chars["PIG2S4Dyn12"] = new BreathCharacteristics { is_pc = 1, i = 2, e = 1 };
            chars["PIG2S5Dyn13"] = new BreathCharacteristics { is_pc = 0, i = 2, e = 1, f_adjust = -2 };
            chars["PIG2S6Dyn14"] = new BreathCharacteristics { is_pc = 0, i = 1, e = 2 };
            chars["PIG2S7Dyn15"] = new BreathCharacteristics { is_pc = 1, i = 2, e = 1 };
            chars["PIG2S8Dyn16"] = new BreathCharacteristics { is_pc = 1, i = 1, e = 2 };

            chars["PIG2S56Dyn1"] = new BreathCharacteristics { is_pc = 0, i = 1, e = 4, f_adjust = -1 };
            chars["PIG2S57Dyn2"] = new BreathCharacteristics { is_pc = 0, i = 4, e = 1, f_adjust = -1 };
            chars["PIG2S58Dyn3"] = new BreathCharacteristics { is_pc = 1, i = 1, e = 4 };
            chars["PIG2S59Dyn4"] = new BreathCharacteristics { is_pc = 1, i = 4, e = 1 };
            chars["PIG2S60Dyn5"] = new BreathCharacteristics { is_pc = 0, i = 4, e = 1, f_adjust = -1 };
            chars["PIG2S61Dyn6"] = new BreathCharacteristics { is_pc = 0, i = 1, e = 4 };
            chars["PIG2S62Dyn7"] = new BreathCharacteristics { is_pc = 1, i = 4, e = 1 };
            chars["PIG2S63Dyn8"] = new BreathCharacteristics { is_pc = 1, i = 1, e = 4 };

            chars["PIG6S2Dyn1"] = new BreathCharacteristics { is_pc = 0, i = 1, e = 2, is_injured = 1 };
            chars["PIG6S3Dyn2"] = new BreathCharacteristics { is_pc = 0, i = 2, e = 1, is_injured = 1, f_adjust = -1 };
            chars["PIG6S4Dyn3"] = new BreathCharacteristics { is_pc = 1, i = 1, e = 2, is_injured = 1, f_adjust = -2 };
            chars["PIG6S5Dyn4"] = new BreathCharacteristics { is_pc = 1, i = 2, e = 1, is_injured = 1 };
            chars["PIG6S6Dyn5"] = new BreathCharacteristics { is_pc = 0, i = 2, e = 1, is_injured = 1, f_adjust = -1 };
            chars["PIG6S7Dyn6"] = new BreathCharacteristics { is_pc = 0, i = 1, e = 2, is_injured = 1 };
            chars["PIG6S8Dyn7"] = new BreathCharacteristics { is_pc = 1, i = 2, e = 1, is_injured = 1 };
            chars["PIG6S9Dyn8"] = new BreathCharacteristics { is_pc = 1, i = 1, e = 2, is_injured = 1 };

            chars["PIG6S10Dyn1"] = new BreathCharacteristics { is_pc = 0, i = 1, e = 4, is_injured = 1, f_adjust = -1 };
            chars["PIG6S11Dyn2"] = new BreathCharacteristics { is_pc = 0, i = 4, e = 1, is_injured = 1, f_adjust = +1 };
            chars["PIG6S12Dyn3"] = new BreathCharacteristics { is_pc = 1, i = 1, e = 4, is_injured = 1 };
            chars["PIG6S13Dyn4"] = new BreathCharacteristics { is_pc = 1, i = 4, e = 1, is_injured = 1 };
            chars["PIG6S14Dyn5"] = new BreathCharacteristics { is_pc = 0, i = 4, e = 1, is_injured = 1, f_adjust = -1 };
            chars["PIG6S15Dyn6"] = new BreathCharacteristics { is_pc = 0, i = 1, e = 4, is_injured = 1, f_adjust = -1 };
            chars["PIG6S16Dyn7"] = new BreathCharacteristics { is_pc = 1, i = 4, e = 1, is_injured = 1 };
            chars["PIG6S17Dyn8"] = new BreathCharacteristics { is_pc = 1, i = 1, e = 4, is_injured = 1 };

            chars["PIG7S2Dyn1"] = new BreathCharacteristics { is_pc = 0, i = 1, e = 2, is_injured = 1, f_adjust = -2 };
            chars["PIG7S3Dyn2"] = new BreathCharacteristics { is_pc = 0, i = 2, e = 1, is_injured = 1, f_adjust = -1 };
            chars["PIG7S4Dyn3"] = new BreathCharacteristics { is_pc = 1, i = 1, e = 2, is_injured = 1, f_adjust = -1 };
            chars["PIG7S5Dyn4"] = new BreathCharacteristics { is_pc = 1, i = 2, e = 1, is_injured = 1 };
            chars["PIG7S6Dyn5"] = new BreathCharacteristics { is_pc = 0, i = 2, e = 1, is_injured = 1, f_adjust = -1 };
            chars["PIG7S7Dyn6"] = new BreathCharacteristics { is_pc = 0, i = 1, e = 2, is_injured = 1, f_adjust = -1 };
            chars["PIG7S8Dyn7"] = new BreathCharacteristics { is_pc = 1, i = 2, e = 1, is_injured = 1, f_adjust = -1 };
            chars["PIG7S9Dyn8"] = new BreathCharacteristics { is_pc = 1, i = 1, e = 2, is_injured = 1 };

            chars["PIG7S10Dyn1"] = new BreathCharacteristics { is_pc = 0, i = 1, e = 4, is_injured = 1 };
            chars["PIG7S11Dyn2"] = new BreathCharacteristics { is_pc = 0, i = 4, e = 1, is_injured = 1, f_adjust = -2 };
            chars["PIG7S12Dyn3"] = new BreathCharacteristics { is_pc = 1, i = 1, e = 4, is_injured = 1 };
            chars["PIG7S13Dyn4"] = new BreathCharacteristics { is_pc = 1, i = 4, e = 1, is_injured = 1 };
            chars["PIG7S14Dyn5"] = new BreathCharacteristics { is_pc = 0, i = 4, e = 1, is_injured = 1, f_adjust = -2 };
            chars["PIG7S15Dyn6"] = new BreathCharacteristics { is_pc = 0, i = 1, e = 4, is_injured = 1 };
            chars["PIG7S16Dyn7"] = new BreathCharacteristics { is_pc = 1, i = 4, e = 1, is_injured = 1 };
            chars["PIG7S17Dyn8"] = new BreathCharacteristics { is_pc = 1, i = 1, e = 4, is_injured = 1 };

        }

        public override void OutputHeaders(StreamWriter sw)
        {
            sw.Write("pig_id,breath_id,ct_date,ct_time,is_injured,is_pc,ie_ratio,ie,pc_ie,");
        }

        public override void OutputSlice(StreamWriter sw, short[,] slice, int z, int f_index, SubjectData pd)
        {
            // parse subject data
            var pig_match = pig.Match(pd.subject_name);
            var dyn_match = dyn.Match(pd.source_name);
            string breath_id = "Subj" + pig_match.Groups[1].Value + "S" + pd.series_name + "Dyn" + dyn_match.Groups[1].Value;

            // Determine breath characteristics
            BreathCharacteristics bc = null;
            chars.TryGetValue(breath_id, out bc);

            breath_id += (char)('a' + f_index / fpb);

            sw.Write(pig_match.Groups[1].Value + ",");
            sw.Write(breath_id + ",");

            // split date time
            var at = pd.acquisition_datetime;
            if (at != null && at.Length > 0)
            {
                DateTime dt = new DateTime(int.Parse(at.Substring(0, 4)), int.Parse(at.Substring(4, 2)), int.Parse(at.Substring(6, 2)),
                    int.Parse(at.Substring(8, 2)), int.Parse(at.Substring(10, 2)), int.Parse(at.Substring(12, 2)),
                    int.Parse(at.Substring(15, 3)));

                sw.Write(dt.ToShortDateString() + "," + dt.ToLongTimeString() + ",");
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
