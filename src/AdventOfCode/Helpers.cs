namespace AdventOfCode;

public static class Helpers
{
	public static string DataDirectory;

	static Helpers()
	{
		DataDirectory = Path.GetFullPath(Path.Join(Directory.GetCurrentDirectory(), "..", "..", "..", "data"));
		Console.Out.WriteLine("INIT: " + DataDirectory);
	}

	public static async Task<string> ReadTextFromFileAsync(byte day, byte puzzleNumber = 1)
	{
		var path = Path.Join(DataDirectory, $"day{day}_puzzle{puzzleNumber}.txt");
		using (var reader = new StreamReader(path))
		{
			var content = await reader.ReadToEndAsync();
			return content;
		}
	}

	public static string ReadTextFromFile(byte day, byte puzzleNumber = 1)
	{
		var path = Path.Join(DataDirectory, $"day{day}_puzzle{puzzleNumber}.txt");
		using (var reader = new StreamReader(path))
		{
			var content = reader.ReadToEnd();
			return content;
		}
	}

	public static string[] GetCleanedLines(this string input)
	{
		return input
				.Trim()
				.Replace("\r", String.Empty)
				.Split("\n", StringSplitOptions.TrimEntries);
	}

	public static ICollection<(T1, T2)> BuildListOfTuples<T1, T2>(string[] lines, Func<string, T1> mapperFuncT1, Func<string, T2> mapperFuncT2, char splitChar = ' ')
	{
		var list = new List<(T1, T2)>();
		
		foreach(var line in lines)
		{
			var parts = line.Split(splitChar);

			list.Add((mapperFuncT1(parts[0]), mapperFuncT2(parts[1])));
		}

		return list;
	}
}
