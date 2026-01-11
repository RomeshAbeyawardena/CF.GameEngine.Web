using System;
using System.Collections.Generic;
using System.Linq;
public class Program
{
	public static void Main()
	{
		var t = new List<byte>();
		var random = Random.Shared;
		while(t.Count() < 255)
		{
			var v= t.LastOrDefault((byte)random.Next(1,255));
			
			var ld = (v < 50) ? 1 : -5;
			
			var delta = random.Next(ld, 5);
			
			var n = v + delta;
			if (n > 255)
			{
				t.Add(255);
			}
			else if(n < 0)
			{
				t.Add(1);
			}
			else {
				t.Add(Convert.ToByte(n));
			}
		}
		
		Console.WriteLine("{0:x2}",  string.Join('-', t.Select(x => x.ToString("x2"))));
	}
}
