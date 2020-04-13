using System.Linq;
using System.Collections.Generic;
using System.Net.Mime;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;
using System;
using System.Threading.Tasks;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace las_cs_new_features
{
    class Program
    {
       static ILas lasParser;
        
        static async Task Main(string[] args)
        {
           CancellationTokenSource cts = new CancellationTokenSource();
           await RunAsync();
		}

        private static Task RunAsync()
        {
            string filePath ="";
            lasParser = lasParser.SetParser(out filePath);
            while(true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("LAS >");
                Console.ResetColor();
                var command = Console.ReadLine();
                string method = "";
                string arg = "";

                try{
                    Tuple<string,string> parsed = LasCMDLine.ParseCommand(command);
                    method = parsed.Item1;
                    arg = parsed.Item2;
                } 
                catch(LasCMDLineError e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                if(method == "column")lasParser.Column(method,arg);
                else if(method == "columnstrp")lasParser.ColumnStripped(method,arg);
                else if(method == "tocsv")lasParser.ToCsv(method,arg);
                else if(method == "tocsvstrp")lasParser.ToCsvStripped(method,arg);
                else if(method == "rows")lasParser.RowCount(method,arg);
                else if(method == "columns")lasParser.ColumnCount(method,arg);
                else if(method == "data")lasParser.Data(method,arg);
                else if(method == "datastrp")lasParser.DataStripped(method,arg);
                else if(method == "version")lasParser.Version(method,arg);
                else if(method == "wrap")lasParser.Wrap(method,arg);
                else if(method == "other")lasParser.Other(method,arg);
                else if(method == "headerdesc")lasParser.HeaderAndDescr(method,arg);
                else if(method == "wellparams")lasParser.WellParams(method,arg);
                else if(method == "curveparams")lasParser.CurveParams(method,arg);
                else if(method == "logparams")lasParser.LogParams(method,arg);
                else if(method == "help")LasCMDLine.Help();
                else if(method == "reset")lasParser.SetParser(out filePath);
                else {
                    Console.Error.WriteLine($"{command}\tis and invalid command \n use --help or -help to view commands");
                }
                }
            }
    }
}