namespace AdventOfCode;

/// <summary>
///
/// </summary>
public class Day02_
{
	private static readonly string Input = "";

	[Test]
	public void Puzzle_01()
	{
		var lines = Input.GetCleanedLines();

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
	}

	[Test]
	public void Puzzle_02()
	{
		var lines = Input.GetCleanedLines();

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
	}
}