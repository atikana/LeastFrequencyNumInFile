using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace LeastFrequencyNumInFile
{
    class Program
    {
        private string _outputs = "";
        private string _directoryPath;
        public Program()
        {
            this._directoryPath = Directory.GetCurrentDirectory();
        }

        public void SetFolder(string path)
        {
            this._directoryPath = path;
        }

        private bool FindSRCLocation()
        {
            // loop untill find the parent folder that contains SRC
            while (!Directory.Exists(_directoryPath + "\\src"))
            {
                DirectoryInfo directoryInfo = new(this._directoryPath);
                if (directoryInfo.Parent != null)
                {
                    try
                    {
                        this._directoryPath = Directory.GetParent(this._directoryPath).FullName;
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

            this._directoryPath += "\\src";
            return true;

        }

        private void FindLeastFrequentNumberInFolder()
        {
            string[] files = Directory.GetFiles(this._directoryPath);

            for (int j = 0; j < files.Length; j++)
            {
                FileInfo info = new(files[j]);
                List<int> nums = ReadFile(info.FullName);

                //sort the list
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

                _outputs += string.Format("{0}: File: {1}, Number: {2}, Repeated: {3} times\n", j + 1, info.Name, minNum, minCount);
            }
        }

        private static List<int> ReadFile(string path)
        {
            string line;
            List<int> elements = new List<int>();
            StreamReader reader = new(path);

            while ((line = reader.ReadLine()) != null)
            {
                try
                {
                    elements.Add(Int32.Parse(line));
                }
                catch (Exception)
                {
                    Console.WriteLine("can't parse this '{line}' into a string");
                }
            }

            reader.Close();
            return elements;
        }

        public string GetLeastFrequentNumberInFolder()
        {
            _outputs = "";

            if (FindSRCLocation())
            {
                FindLeastFrequentNumberInFolder();
            }

            return _outputs;
        }

        static void Main()
        {
            Console.Write(new Program().GetLeastFrequentNumberInFolder());
        }
    }
}