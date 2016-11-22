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
    public class libctdyn
    {
        public static ICollection<FileInfo> GetFilesInDir(string file_dir)
        {
            // Get all available analysis files
            var DirInfo = new System.IO.DirectoryInfo(file_dir);
            var files = DirInfo.GetFiles("analysis-*.bin");
            return files;
        }

        public delegate void ReportProgess(int n, int total);

        public static SubjectData LoadSubjectData(FileInfo file)
        {
            BinaryReader br = new BinaryReader(file.OpenRead());
            SubjectData pd = new SubjectData();

            for (int i = 0; i < 3; i++)
                pd.spacing[i] = br.ReadDouble();

            for (int i = 0; i < 3; i++)
                pd.dimensions[i] = br.ReadInt32();

            pd.source_name = ReadString(br);
            pd.acquisition_datetime = ReadString(br);
            pd.series_name = ReadString(br);
            pd.pig_name = ReadString(br);

            return pd;
        }

        public static short[,,] LoadImageData(FileInfo file, out SubjectData sd)
        {
            BinaryReader br = new BinaryReader(file.OpenRead());
            SubjectData pd = new SubjectData();

            for (int i = 0; i < 3; i++)
                pd.spacing[i] = br.ReadDouble();

            for (int i = 0; i < 3; i++)
                pd.dimensions[i] = br.ReadInt32();

            pd.source_name = ReadString(br);
            pd.acquisition_datetime = ReadString(br);
            pd.series_name = ReadString(br);
            pd.pig_name = ReadString(br);

            long data_start = br.BaseStream.Position;

            short[,,] ret = new short[pd.dimensions[2], pd.dimensions[1], pd.dimensions[0]];
            for (int z = 0; z < pd.dimensions[2]; z++)
            {
                for (int y = 0; y < pd.dimensions[1]; y++)
                {
                    for (int x = 0; x < pd.dimensions[0]; x++)
                        ret[z, y, x] = br.ReadInt16();
                }
            }

            sd = pd;
            return ret;
        }

        public static int DoAnalysis(ICollection<FileInfo> files, string ofname,
            ReportProgess rp = null)
        {
            StreamWriter sw = new StreamWriter(ofname);

            // Modules to use
            List<Module> modules = new List<Module>();
            modules.Add(new FrameModule(20));
            modules.Add(new TimeModule(20, 250));
            modules.Add(new SubjectInfo(20));
            modules.Add(new WholeFrameModule());
            modules.Add(new ThresholdModule(new Threshold[]
            {
                new Threshold { Name = "Atelectasis", LowerBound = -100, UpperBound = 100 },
                new Threshold { Name = "Poorly Aerated", LowerBound = -500, UpperBound = -101 },
                new Threshold { Name = "Normally Aerated", LowerBound = -900, UpperBound = -501 },
                new Threshold { Name = "Overdistended", LowerBound = -1000, UpperBound = -901 }
            }));
            modules.Add(new ZoneModule(6));

            // Write headers
            foreach (var mod in modules)
                mod.OutputHeaders(sw);
            sw.WriteLine();

            int count = 0;
            foreach (var file in files)
            {
                // Load header info
                //FileStream fs = new FileStream(fname, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(file.OpenRead());
                SubjectData pd = new SubjectData();

                for (int i = 0; i < 3; i++)
                    pd.spacing[i] = br.ReadDouble();

                for (int i = 0; i < 3; i++)
                    pd.dimensions[i] = br.ReadInt32();

                pd.source_name = ReadString(br);
                pd.acquisition_datetime = ReadString(br);
                pd.series_name = ReadString(br);
                pd.pig_name = ReadString(br);

                long data_start = br.BaseStream.Position;

                // Build list of actual frame indices
                List<int> f_indices = new List<int>();
                int f_adjust = 0;
                var pig_match = SubjectInfo.pig.Match(pd.pig_name);
                var dyn_match = SubjectInfo.dyn.Match(pd.source_name);
                string breath_id = "PIG" + pig_match.Groups[1].Value + "S" + pd.series_name + "Dyn" + dyn_match.Groups[1].Value;
                SubjectInfo.BreathCharacteristics bc;
                if (SubjectInfo.chars.TryGetValue(breath_id, out bc))
                    f_adjust = bc.f_adjust;

                for (int z = 0; z < pd.dimensions[2]; z++)
                {
                    int act_z = z + f_adjust;
                    act_z += pd.dimensions[2];
                    act_z %= pd.dimensions[2];
                    f_indices.Add(act_z);
                }

                // Analyze slice by slice
                for (int f_index = 0; f_index < pd.dimensions[2]; f_index++)
                {
                    try
                    {
                        int z = f_indices[f_index];

                        var file_idx = br.BaseStream.Position;
                        short[,] slice = new short[pd.dimensions[1], pd.dimensions[0]];

                        br.BaseStream.Seek(data_start + 2 * pd.dimensions[0] * pd.dimensions[1] * z, SeekOrigin.Begin);
                        for (int y = 0; y < pd.dimensions[1]; y++)
                        {
                            for (int x = 0; x < pd.dimensions[0]; x++)
                                slice[y, x] = br.ReadInt16();
                        }

                        foreach (var mod in modules)
                            mod.OutputSlice(sw, slice, z, f_index, pd);

                        sw.WriteLine();
                    }
                    catch (Exception)
                    {
                        sw.WriteLine();
                    }
                }

                br.Close();

                count++;

                if (rp != null)
                    rp(count, files.Count);
            }

            sw.Close();
            return count;
        }

        static string ReadString(BinaryReader br)
        {
            try
            {
                var slen = br.ReadInt16();
                StringBuilder sb = new StringBuilder();
                for (var i = 0; i < slen; i++)
                    sb.Append((char)br.ReadByte());
                return sb.ToString();
            }
            catch(Exception)
            {
                return "";
            }
        }
    }
}
