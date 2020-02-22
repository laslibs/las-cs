using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;
using las_cs.src.models;

namespace las_cs.src {
    public class LasCs {
        public string path;
		public string blobString;

        public LasCs (string path) {

            this.path = path;
			blobString = File.ReadAllText(path);
		}

		public struct multipleValue
		{
			public double convertedValue;
			public string stringValue;

		}

		public multipleValue convertToValue(string s)
		{
			multipleValue values = new multipleValue();
			dynamic value;
			if(double.TryParse(s, out double val))
			{
				values.convertedValue = val;
			}
			else
			{
				values.stringValue = s;
			}
			return values;
		}

		public string column(string column) {
			var hds = this.header();
			var sB = this.data();
			var index = hds.ToList().FindIndex(item => item.ToLower() == column.ToLower());
			if(index < 0)
			{
				Console.WriteLine("No Column found");
			}

			return (string)sB.Select(c => c[index]);
        }

		/**
		* Returns the number of rows in a .las file
		* @returns number
		* @memberof Las
		*/
		public int rowCount()
		{
			var l = this.data();
			return l.Length;
		}

		/**
		* Returns the number of colunms in a .las file
		* @returns number
		* @memberof Las
		*/
		public int columnCount()
		{
			var l = this.header();
			return l.Length;
		}


		/**
		* Returns a two-dimensional array of data in the log
		* @returns {(Promise<Array<Array<string | number>>>)}
		* @memberof Las
		*/
		public string[] data()
		{
			var s = this.blobString;
			var hds = this.header();
			var totalheadersLength = hds.Length;

			var sB = Regex.Split((string)s, @"~A(?:\w*\s*)*\n")[1]
				.Trim()
				.Split(@"\s+")
				.Select(m => convertToValue(m.Trim()));

			if(sB.Count() < 0)
			{
				Console.WriteLine("No data/~A section in the file");
			}


			string[] arr = new string[2] { "g", "f" };
			return arr;
			
		}

		public string other()
		{
			var s = this.blobString;
			var som = Regex.Split((string)s, @"~O(?:\w*\s*)*\n\s*i")[1];
			var str = " ";
			if (som != null)
			{
				var some =
					som.Split("~")[0].Replace(@"/\n\s*/g", " ").Trim();
				str = removeComment(some);
			}
			if (str.Length <= 0)
			{
				return " ";
			}
			return str;
		}

		public (double, bool) metadata()
		{
			var sB = Regex.Split(this.blobString, @"~V(?:\w*\s*)*\n\s*")[1].Split("~")[0];
			var sw = removeComment(sB);
			var refined = sw.Split("\n").Select(m => Regex.Split(m, @"\s{2,}|\s*:").ToList()
			.GetRange(0, 2))
			.Where(f => f != null).ToList();
			var res = refined.Select(r => r[1]);
			var wrap = res.ToList()[1].ToLower() == "yes" ? true : false;

			object[] arr = new object[2] {double.Parse(res.ToList()[0]), wrap};

			if(arr.Length < 0)
			{
				Console.WriteLine("Couldn't get metadata");
			}

			return (double.Parse(res.ToList()[0]), wrap);
		}

		public bool wrap()
		{
			retrun this.metadata().Item2;
		}
	    
	    	public double version()
		{
			retrun this.metadata().Item1;
		}

		public string[] header()
		{
			var s = this.blobString;
			var sth = Regex.Split((string)s, @"~C(?:\w*\s*)*\n\s*")[1].Split("~")[0];
			var uncommentSth = removeComment(sth).Trim();
			if(uncommentSth.Length < 0)
			{
				//TODO: Throw Error instead of printing to the console
				Console.WriteLine("There is no header in the file");
			}
			return uncommentSth.Split("\n").Select(m => Regex.Split(m.Trim(), @"\s+|[.]")[0]).ToArray();
		}

		public static string removeComment(string str)
		{
			return string.Join("\n",
				str.Trim()
					.Split("\n")
					.Select(val => val.TrimStart())
					.Where(f => (f[0] != '#')));
		}
	    	
	    	// TODO: reimplement with Array.Copy
    		public static T[][] chunk<T>(T[] arr, int size)
      		{	
        		int i = 0;
        		return arr.GroupBy(s => i++ / size).Select(g => g.ToArray()).ToArray();
	    	}


    }
}
