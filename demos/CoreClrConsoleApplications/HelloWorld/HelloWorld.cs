using System;

class Program {
	static void Main(string[] args)
	{
		Console.WriteLine("Hello World!");
		foreach (var arg in args)
		{
			Console.Write("Hello ");
			Console.Write(arg);
			Console.WriteLine("!");
		}
		Console.WriteLine("Press ENTER to exit ...");
		Console.ReadLine();
	}
} 