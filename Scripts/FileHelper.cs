using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HandFile
{
    class FileHelper
    {
        /// <summary>
        /// 读取CSV
        /// </summary>
        /// <param name="filePath">CSV路径</param>
        /// <param name="n">表示第n行是字段title,第n+1行是记录开始</param>
        /// <returns></returns>
        public static List<string[]> CsvToDataTable(string filePath, int n)
        {
            List<string[]> dt = new List<string[]>();
            try
            {
                StreamReader reader = new StreamReader(filePath, System.Text.Encoding.Default, false);
                int m = 0;
                while (!reader.EndOfStream)
                {
                    m++;
                    string str = reader.ReadLine();
                    string[] split = str.Split(',');
                    if (m == n)
                    {
                        continue;
                    }
                    else if (m >= n + 1)
                    {
                        dt.Add(split);
                    }
                }
                reader.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
               // System.Environment.Exit(-1);
            }
            return dt;
        }
    }
}
