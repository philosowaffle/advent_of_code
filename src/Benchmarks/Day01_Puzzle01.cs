using AdventOfCode;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

public class Day01_Puzzle01
{
	private static string[] Lines;

	[GlobalSetup]
	public void SetUp()
	{
		Helpers.DataDirectory = Path.Join(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "..", "..", "..", "..", "AdventOfCode", "data");
		Console.Out.WriteLine("MY DIR: " + Helpers.DataDirectory);

		var text = Helpers.ReadTextFromFile(day: 1, puzzleNumber: 1);
		Lines = Helpers.GetCleanedLines(text);
	}

	/// <summary>
	/// 29.65 us - 0B - Gen0 0
	/// </summary>
	[Benchmark]
	public int OriginalSolution()
	{
		var maxCalories = 0;
		var currentElfCalorieCount = 0;

		foreach (var line in Lines)
		{
			if (int.TryParse(line, out var calorie))
			{
				currentElfCalorieCount += calorie;
			}
			else
			{
				if (currentElfCalorieCount > maxCalories)
					maxCalories = currentElfCalorieCount;

				currentElfCalorieCount = 0;
			}
		}

		if (maxCalories != 70720) throw new ArgumentException("Incorrect Answer!");

		return maxCalories;
	}

	/// <summary>
	/// 29.49 us - 0B - Gen0 0
	/// </summary>
	[Benchmark]
	public int OldSchoolForLoop()
	{
		var maxCalories = 0;
		var currentElfCalorieCount = 0;

		for (var index = 0; index < Lines.Length; index++)
		{
			var line = Lines[index];

			if (int.TryParse(line, out var calorie))
			{
				currentElfCalorieCount += calorie;
			}
			else
			{
				if (currentElfCalorieCount > maxCalories)
					maxCalories = currentElfCalorieCount;

				currentElfCalorieCount = 0;
			}
		}

		if (maxCalories != 70720) throw new ArgumentException("Incorrect Answer!");

		return maxCalories;
	}
}
