using FluentAssertions;

namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2022/day/1
/// 
/// Brute Force for the win!
/// </summary>
public class Day01_CalorieCounter
{
	[Test]
	public void Puzzle_01()
	{
		var text = Helpers.ReadTextFromFile(day: 1, puzzleNumber: 1);
		var lines = Helpers.GetCleanedLines(text);

		var maxCalories = 0;
		var currentElfCalorieCount = 0;

		foreach (var line in lines)
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

		TestContext.Out.WriteLine($"Answer: {maxCalories}");
		maxCalories.Should().Be(70720);
	}

	[Test]
	public void Puzzle_02()
	{
		var text = Helpers.ReadTextFromFile(day: 1, puzzleNumber: 1);
		var lines = Helpers.GetCleanedLines(text);

		var currentElfCalorieCount = 0;
		var calories = new List<int>();

		foreach (var line in lines)
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

		TestContext.Out.WriteLine($"Answer: {answer}");
		answer.Should().Be(207148);
	}
}