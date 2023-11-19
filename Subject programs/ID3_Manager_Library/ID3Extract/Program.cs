using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Id3Lib;
using System.IO;
using System.Threading;
using System.Data.SqlServerCe;
using System.Diagnostics;

namespace TagExtract
{
    class Program
    {
        static void Main(string[] args)
        {
            DirScan dirScan = new DirScan();
            string[] files = dirScan.Browse(@"D:\Music");


            using (SqlCeConnection connection = new SqlCeConnection("Data Source=Repository.sdf; Persist Security Info=False; Max Database Size=2000"))
            {
                connection.Open();
                ReopositoryAdapter repositoryAdapter = new ReopositoryAdapter(connection);
                foreach (string file in files)
                {
                    repositoryAdapter.PublishFrames(file);
                    Console.Write('.');

                }
            }

        }
    }
}
