using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace LeastFrequencyNumInFile
{
    class Program
    {
        string outputs = "";
        string directoryPath;
        public Program()
        {
            this.directoryPath = Directory.GetCurrentDirectory();
        }

        public void SetFolder(string path)
        {
            this.directoryPath = path;
        }


        private bool FindSRCLocation()
        {
            // loop untill find the parent folder that contains SRC


            while (!Directory.Exists(directoryPath + "\\src"))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                if (directoryInfo.Parent != null)
                {
                    try
                    {
                        this.directoryPath = Directory.GetParent(this.directoryPath).FullName;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Can't find the parent directory");
                    }
                }
                else
                {
                    return false;
                }
            }
            directoryPath += "\\src";
            return true;

        }

        private void FindLeastFrequentNumberInFolder()
        {
            string[] files = Directory.GetFiles(this.directoryPath);

            for (int j = 0; j < files.Length; j++)
            {
                string line;
                List<int> nums = new();
                FileInfo info = new(files[j]);
                StreamReader reader = new(info.FullName);

                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        nums.Add(Int32.Parse(line));
                    }

                    catch (Exception)
                    {
                        Console.WriteLine("can't parse this '{line}' into a string");
                    }
                }

                reader.Close();

                // sort the list;
                nums.Sort();

                int count = 1, minCount = nums.Count + 1, minNum = int.MinValue;

                for (int i = 1; i < nums.Count; i++)
                {
                    if (nums[i] == nums[i - 1])
                    {
                        count++;
                    }
                    else
                    {
                        if (count < minCount)
                        {
                            minCount = count;
                            minNum = nums[i - 1];
                        }
                        count = 1;
                    }
                }

                if (count < minCount)
                {
                    minCount = count;
                    minNum = nums[nums.Count - 1];
                }

                outputs += string.Format("{0}: File: {1}, Number: {2}, Repeated: {3} times\n", j + 1, info.Name, minNum, minCount);
            }

        }

        public string GetLeastFrequentNumberInFolder()
        {
            outputs = "";

            if (FindSRCLocation())
            {
                FindLeastFrequentNumberInFolder();
            }

            return outputs;
        }


        static void Main(string[] args)
        {
            Console.Write(new Program().GetLeastFrequentNumberInFolder());
        }
    }
}
