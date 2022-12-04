using FluentAssertions;

namespace AdventOfCode;
public class Day04_CampCleanup
{
	[Test]
	public void Puzzle01()
	{
		var text = Helpers.ReadTextFromFile(day: 4);
		var lines = Helpers.GetCleanedLines(text);

		var answer = 0;

		for (var index = 0; index < lines.Length; index++)
		{
			var line = lines[index];

			var elfs = line.Split(',');
			var elf1Assignment = elfs[0];
			var elf2Assignment = elfs[1];

			var elf1Boundaries = GetSectionBoundaries(elf1Assignment);
			var elf2Boundaries = GetSectionBoundaries(elf2Assignment);

			if (elf1Boundaries.Lower > elf2Boundaries.Upper
				|| elf1Boundaries.Upper < elf2Boundaries.Lower)
				continue;

			var elf1Sections = GetSectionsInBoundary(elf1Boundaries.Lower, elf1Boundaries.Upper);
			var elf2Sections = GetSectionsInBoundary(elf2Boundaries.Lower, elf2Boundaries.Upper);

			var numberOfMatchingSections = elf1Sections.Intersect(elf2Sections).Count();

			if (numberOfMatchingSections == elf1Sections.Length
				|| numberOfMatchingSections == elf2Sections.Length)
			{
				answer++;
				continue;
			}
		}

		TestContext.Out.WriteLine($"Answer: {answer}");
		answer.Should().Be(515);
	}

	[Test]
	public void Puzzle02()
	{
		var text = Helpers.ReadTextFromFile(day: 4);
		var lines = Helpers.GetCleanedLines(text);

		var answer = 0;

		for (var index = 0; index < lines.Length; index++)
		{
			var line = lines[index];

			var elfs = line.Split(',');
			var elf1Assignment = elfs[0];
			var elf2Assignment = elfs[1];

			var elf1Boundaries = GetSectionBoundaries(elf1Assignment);
			var elf2Boundaries = GetSectionBoundaries(elf2Assignment);

			if (elf1Boundaries.Lower > elf2Boundaries.Upper
				|| elf1Boundaries.Upper < elf2Boundaries.Lower)
				continue;

			var elf1Sections = GetSectionsInBoundary(elf1Boundaries.Lower, elf1Boundaries.Upper);
			var elf2Sections = GetSectionsInBoundary(elf2Boundaries.Lower, elf2Boundaries.Upper);

			var numberOfMatchingSections = elf1Sections.Intersect(elf2Sections).Count();

			if (numberOfMatchingSections > 0)
			{
				answer++;
				continue;
			}
		}

		TestContext.Out.WriteLine($"Answer: {answer}");
		answer.Should().Be(883);
	}

	private (int Lower, int Upper) GetSectionBoundaries(string assignmentString)
	{
		var boundaries = assignmentString
			.Split('-')
			.Select(n => int.Parse(n))
			.ToList();

		return (boundaries[0], boundaries[1]);
	}

	private int[] GetSectionsInBoundary(int lower, int upper)
	{
		var numberOfSections = upper - lower + 2;
		var sections = new int[numberOfSections];

		var index = 0;
		var current = lower;
		while(current <= upper)
		{
			sections[index] = current;
			current++;
			index++;
		}

		return sections;
	}
}
