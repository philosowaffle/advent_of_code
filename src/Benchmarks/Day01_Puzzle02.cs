using AdventOfCode;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;
public class Day01_Puzzle02
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
	///  31.43 us -  2368 B - Gen0 0.7324
	/// </summary>
	[Benchmark]
	public int OriginalSolution()
	{
		var currentElfCalorieCount = 0;
		var calories = new List<int>();

		foreach (var line in Lines)
		{
			if (int.TryParse(line, out var calorie))
			{
				currentElfCalorieCount += calorie;
			}
			else
			{
				calories.Add(currentElfCalorieCount);
				currentElfCalorieCount = 0;
			}
		}

		calories.Sort();
		var answer = calories.TakeLast(3).Sum();

		if (answer != 207148) throw new ArgumentException("Incorrect Answer!");

		return answer;
	}

	/// <summary>
	///  28.68 us - 40 B - Gen0 0
	/// </summary>
	[Benchmark]
	public int UsingArray_OptimizeMemoryUsage()
	{
		var currentElfCalorieCount = 0;
		var top3CalorieCounts = new int[] { 0, 0, 0 };

		foreach (var line in Lines)
		{
			if (int.TryParse(line, out var calorie))
			{
				currentElfCalorieCount += calorie;
			}
			else
			{
				if (currentElfCalorieCount > top3CalorieCounts[0])
				{
					top3CalorieCounts[2] = top3CalorieCounts[1];
					top3CalorieCounts[1] = top3CalorieCounts[0];
					top3CalorieCounts[0] = currentElfCalorieCount;

					currentElfCalorieCount = 0;
					continue;
				}

				if (currentElfCalorieCount > top3CalorieCounts[1])
				{
					top3CalorieCounts[2] = top3CalorieCounts[1];
					top3CalorieCounts[1] = currentElfCalorieCount;

					currentElfCalorieCount = 0;
					continue;
				}

				if (currentElfCalorieCount > top3CalorieCounts[2])
				{
					top3CalorieCounts[2] = currentElfCalorieCount;

					currentElfCalorieCount = 0;
					continue;
				}
				currentElfCalorieCount = 0;
			}
		}

		var answer = top3CalorieCounts[0] + top3CalorieCounts[1] + top3CalorieCounts[2];

		if (answer != 207148) throw new ArgumentException("Incorrect Answer!");

		return answer;
	}

	/// <summary>
	///  29.07 us -  40 B - Gen0 0
	/// </summary>
	[Benchmark]
	public int UsingSpan_OptimizeMemoryUsage()
	{
		var currentElfCalorieCount = 0;
		var top3CalorieCounts = new Span<int>(new int[] { 0, 0, 0 });

		foreach (var line in Lines)
		{
			if (int.TryParse(line, out var calorie))
			{
				currentElfCalorieCount += calorie;
			}
			else
			{
				if (currentElfCalorieCount > top3CalorieCounts[0])
				{
					top3CalorieCounts[2] = top3CalorieCounts[1];
					top3CalorieCounts[1] = top3CalorieCounts[0];
					top3CalorieCounts[0] = currentElfCalorieCount;

					currentElfCalorieCount = 0;
					continue;
				}

				if (currentElfCalorieCount > top3CalorieCounts[1])
				{
					top3CalorieCounts[2] = top3CalorieCounts[1];
					top3CalorieCounts[1] = currentElfCalorieCount;

					currentElfCalorieCount = 0;
					continue;
				}

				if (currentElfCalorieCount > top3CalorieCounts[2])
				{
					top3CalorieCounts[2] = currentElfCalorieCount;

					currentElfCalorieCount = 0;
					continue;
				}
				currentElfCalorieCount = 0;
			}
		}

		var answer = top3CalorieCounts[0] + top3CalorieCounts[1] + top3CalorieCounts[2];

		if (answer != 207148) throw new ArgumentException("Incorrect Answer!");

		return answer;
	}

	/// <summary>
	///  29.22 us - 40 B - Gen0 0
	/// </summary>
	[Benchmark]
	public int ExitEarly_OptimizeSpeed()
	{
		var currentElfCalorieCount = 0;
		var top3CalorieCounts = new int[] { 0, 0, 0 };

		foreach (var line in Lines)
		{
			if (int.TryParse(line, out var calorie))
			{
				currentElfCalorieCount += calorie;
			}
			else
			{
				if (currentElfCalorieCount <= top3CalorieCounts[2])
				{
					currentElfCalorieCount = 0;
					continue;
				}

				if (currentElfCalorieCount > top3CalorieCounts[0])
				{
					top3CalorieCounts[2] = top3CalorieCounts[1];
					top3CalorieCounts[1] = top3CalorieCounts[0];
					top3CalorieCounts[0] = currentElfCalorieCount;

					currentElfCalorieCount = 0;
					continue;
				}

				if (currentElfCalorieCount > top3CalorieCounts[1])
				{
					top3CalorieCounts[2] = top3CalorieCounts[1];
					top3CalorieCounts[1] = currentElfCalorieCount;

					currentElfCalorieCount = 0;
					continue;
				}

				if (currentElfCalorieCount > top3CalorieCounts[2])
				{
					top3CalorieCounts[2] = currentElfCalorieCount;

					currentElfCalorieCount = 0;
					continue;
				}

				currentElfCalorieCount = 0;
			}
		}

		var answer = top3CalorieCounts[0] + top3CalorieCounts[1] + top3CalorieCounts[2];

		if (answer != 207148) throw new ArgumentException("Incorrect Answer!");

		return answer;
	}

	/// <summary>
	///  29.37 us - 40B - Gen0 0
	/// </summary>
	[Benchmark]
	public int OldSchoolForLoop()
	{
		var currentElfCalorieCount = 0;
		var top3CalorieCounts = new int[] { 0, 0, 0 };

		for (var index = 0; index < Lines.Length; index++)
		{
			var line = Lines[index];

			if (int.TryParse(line, out var calorie))
			{
				currentElfCalorieCount += calorie;
			}
			else
			{
				if (currentElfCalorieCount <= top3CalorieCounts[2])
				{
					currentElfCalorieCount = 0;
					continue;
				}

				if (currentElfCalorieCount > top3CalorieCounts[0])
				{
					top3CalorieCounts[2] = top3CalorieCounts[1];
					top3CalorieCounts[1] = top3CalorieCounts[0];
					top3CalorieCounts[0] = currentElfCalorieCount;

					currentElfCalorieCount = 0;
					continue;
				}

				if (currentElfCalorieCount > top3CalorieCounts[1])
				{
					top3CalorieCounts[2] = top3CalorieCounts[1];
					top3CalorieCounts[1] = currentElfCalorieCount;

					currentElfCalorieCount = 0;
					continue;
				}

				if (currentElfCalorieCount > top3CalorieCounts[2])
				{
					top3CalorieCounts[2] = currentElfCalorieCount;

					currentElfCalorieCount = 0;
					continue;
				}

				currentElfCalorieCount = 0;
			}
		}

		var answer = top3CalorieCounts[0] + top3CalorieCounts[1] + top3CalorieCounts[2];

		if (answer != 207148) throw new ArgumentException("Incorrect Answer!");

		return answer;
	}
}
