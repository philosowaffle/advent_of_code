using FluentAssertions;

namespace AdventOfCode;
public class Day06_TuningTrouble
{
	[Test]
	public void Puzzle01()
	{
		var text = Helpers.ReadTextFromFile(day: 6);

		var answer = 0;

		var span = text.AsSpan();

		var index = 0;
		while (index < span.Length)
		{
			var chars = span.Slice(index, 4);
			var numDistinctChars = chars.ToArray().Distinct().Count();
			if (numDistinctChars == 4)
			{
				answer = index + 4;
				break;
			}

			index++;
		}

		TestContext.Out.WriteLine($"Answer: {answer}");
		answer.Should().Be(1134);
	}

	[Test]
	public void Puzzle02()
	{
		var text = Helpers.ReadTextFromFile(day: 6);

		var answer = 0;

		var span = text.AsSpan();

		var index = 0;
		while (index < span.Length)
		{
			var chars = span.Slice(index, 14);
			var numDistinctChars = chars.ToArray().Distinct().Count();
			if (numDistinctChars == 14)
			{
				answer = index + 14;
				break;
			}

			index++;
		}

		TestContext.Out.WriteLine($"Answer: {answer}");
		answer.Should().Be(1134);
	}
}
