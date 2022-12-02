namespace AdventOfCode;

internal static class Helpers
{
	public static string[] GetCleanedLines(this string input)
	{
		return input.Replace("\r", String.Empty)
					 .Split("\n", StringSplitOptions.TrimEntries);
	}
}
