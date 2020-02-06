using las_cs.src;
using System;

namespace las_cs
{
    class Program
    {
        static void Main(string[] args)
        {
			string path = @"C:\Users\khelechy\Documents\GitHub\las-cs\test.las";
			string test = "#ignore me\nDon't ignore me\n#   \nEHllo # don't ignore";
			LasCs LasEngine = new LasCs(path);
			string result = LasEngine.other();
			//Console.WriteLine(LasCs.removeComment(path));

			//foreach (string s in result)
			//{
			//	Console.WriteLine(s);
			//}
			Console.WriteLine(result);

		}
    }
}
