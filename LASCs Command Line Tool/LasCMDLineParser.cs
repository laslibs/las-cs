using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace las_cs_new_features
{
     public class LasCMDLineError : Exception
        {
            public LasCMDLineError(string message):base(message){ }
        }
    public static class LasCMDLine
    {
       
        public static Tuple<string,string> ParseCommand(string command)
        {
            string option = "";
            string argument = "";

            // trim command
            command = command.Trim().ToLower();
            
           try
           {
                var suboption = command.Contains("--")? command.Substring(command.IndexOf("--")).Trim()
                                            : command.Contains("-")? command.Substring(command.IndexOf("-")).Trim()
                                                                    : "";
                for(int i = 0; i < suboption.Length; i++)
                {
                    if(suboption[i].ToString() == "--" || suboption[i].ToString() == "-")
                        continue;
                    if(string.IsNullOrWhiteSpace(suboption[i].ToString()))
                        break;
                    option += suboption[i];
                }
                option = option.Trim();
            argument = command.Contains("--")? Regex.Split(command,@"--\w+")[1].Trim() 
                                             : command.Contains("-")? Regex.Split(command,@"-\w+")[1].Trim()
                                                                      : " ";
           }
           catch{throw new LasCMDLineError("Invalid Command");}
            return Tuple.Create(option,argument);
        }
    
         public static void LogParams(this ILas lasParser,string method, string arg)
        {
            try{
                var logs = lasParser.LogParams();
                foreach (var item in logs)
                {
                    var log = item.Value;
                    Console.WriteLine($"{item.Key}  {log.Unit}  {log.Value}  {log.Description}");
                }    
            }
            catch(PropertyError e){Console.WriteLine(e.message);}
        }

        public static void CurveParams(this ILas lasParser,string method, string arg)
        {
             try{
                var logs = lasParser.CurveParams();
                foreach (var item in logs)
                {
                    var log = item.Value;
                    Console.WriteLine($"{item.Key}  {log.Unit}  {log.Value}  {log.Description}");
                }    
            }
            catch(PropertyError e){Console.WriteLine(e.message);}
        }

        public static void WellParams(this ILas lasParser,string method, string arg)
        {
             try{
                var logs = lasParser.WellParams();
                foreach (var item in logs)
                {
                    var log = item.Value;
                    Console.WriteLine($"{item.Key}  {log.Unit}  {log.Value}  {log.Description}");
                }    
            }
            catch(PropertyError e){Console.WriteLine(e.message);}
        }


        public static void HeaderAndDescr(this ILas lasParser,string method, string arg)
        {
           try{
                var hdDisc = lasParser.HeaderAndDescr();
                foreach(var key in hdDisc.Keys)
                Console.WriteLine($"{key} {hdDisc.GetValueOrDefault(key)}");
           }
           catch(Exception e){Console.WriteLine(e.Message);}
        }

        public static void Other(this ILas lasParser,string method, string arg)
        {
            Console.WriteLine(lasParser.Other());
        }

        public static void Wrap(this ILas lasParser,string method, string arg)
        {
            Console.WriteLine(lasParser.Wrap());
        }

        public static void Version(this ILas lasParser,string method, string arg)
        {
            Console.WriteLine(lasParser.Version());
        }

        public static void DataStripped(this ILas lasParser,string method, string arg)
        {
           try
           {
               var datastr = lasParser.DataStripped();
               foreach(var item in datastr.literals.ToList())
                {
                    foreach(var val in item.ToList())
                    Console.Write($" {val} ");
                    Console.WriteLine("\n");
                }
           }
           catch(LasError e){Console.WriteLine(e.Message);}
        }

        public static void Data(this ILas lasParser,string method, string arg)
        {
           try
           {
               var datastr = lasParser.Data();
               foreach(var item in datastr.literals.ToList())
                {
                    foreach(var val in item.ToList())
                    Console.Write($" {val} ");
                    Console.WriteLine("\n");
                }
           }
           catch(LasError e){Console.WriteLine(e.Message);}
        }

        public static void ColumnCount(this ILas lasParser,string method, string arg)
        {
            Console.WriteLine(lasParser.ColumnCount());
        }

        public static void RowCount(this ILas lasParser,string method, string arg)
        {
            Console.WriteLine(lasParser.RowCount());
        }

        public static void ToCsvStripped(this ILas lasParser,string method, string arg)
        {
            try
            {
                lasParser.ToCsvStripped("TestLasStripped",arg);
            }
            catch(CsvError e){Console.WriteLine(e.message);}
        }

       public static void ToCsv(this ILas lasParser,string method, string arg)
        {
            try
            {
                lasParser.ToCsv("TestLasfile",arg);
            }
            catch(CsvError e){Console.WriteLine(e.message);}
        }

        public static void ColumnStripped(this ILas lasParser,string method, string arg)
        {
           try
           {
            var colstripped = lasParser.ColumnStripped(arg);
            foreach(var col in colstripped.literals.ToList())
                Console.WriteLine(col);
           }       
            catch(ColumnError e){Console.WriteLine(e.message);}    
        }

        public static void Column(this ILas lasParser,string method, string arg)
        {
           try
           {
            var cols = lasParser.ColumnStripped(arg);
            foreach(var col in cols.literals.ToList())
                Console.WriteLine(col);
           }
           catch(ColumnError e){Console.WriteLine(e.message);}           
        }

        public static void Help()
        {
            var help = File.ReadAllLines(@"./help.help");
            foreach(var line in help.ToList())
                Console.WriteLine(line);
        }

        public static ILas SetParser(this ILas lasParser,out string filePath)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("*********Welcome To LASCs Parser Command Line********");
            Console.ResetColor();
            
            while(true)
            {
                Console.Write("Enter Absolute Path to .LAS File :: ");
                var filepath = Console.ReadLine();
                if(File.Exists(filepath))
                {
                    filePath = filepath;
                    try{
                        lasParser = new Las(filePath);
                        return lasParser;
                    }
                    catch(LasError e)
                    {Console.WriteLine(e.Message);continue;}
                }

                Console.Error.WriteLine("File doesn't Exists");
            }
        }
    }
    
   
        

}