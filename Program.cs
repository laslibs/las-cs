using System;
using System.Linq;

namespace las_cs_new_features
{
    class Program
    {
       static ILas lasParser;
        
        static void Main(string[] args)
        {
            // Path to LAS file
            string filePath = @"./test.las";
            
            // Initialize larse parser
            lasParser = new Las(filePath);

            var data = lasParser.DataStripped();
            foreach ( var item in data.literals.ToList())
            {  
                foreach (var i in item.ToList())
                    Console.Write($" {i} ");
                Console.WriteLine(" ");
            }
		}
   }
}