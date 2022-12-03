using FluentAssertions;

namespace AdventOfCode;
public class Day03_RucksackReorganization
{
	private static readonly Dictionary<char, byte> ItemPriority = new Dictionary<char, byte>() { };

	[OneTimeSetUp]
	public void SetUp()
	{
		var alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		byte counter = 1;
		foreach (var c  in alphabet.ToArray())
		{
			ItemPriority.Add(c, counter);
			counter++;
		}
	}

	[Test]
	public void Puzzle01()
	{
		var text = Helpers.ReadTextFromFile(day: 3, puzzleNumber: 1);
		var lines = Helpers.GetCleanedLines(text);

		var priorityTotal = 0;

		foreach (var line in lines)
		{
			var span = line.AsSpan();
			var totalItemsInRucksack = span.Length;
			var itemsPerCompartment = totalItemsInRucksack / 2;

			var itemsInCompartment1 = span.Slice(0, itemsPerCompartment).ToArray();
			var itemsInCompartment2 = span.Slice(itemsPerCompartment, itemsPerCompartment).ToArray();

			var itemsFoundInBothCompartments = itemsInCompartment1.Intersect(itemsInCompartment2);
			
			foreach(var item in itemsFoundInBothCompartments)
			{
				priorityTotal += ItemPriority.GetValueOrDefault(item);
			}
		}

		TestContext.Out.WriteLine($"Answer: {priorityTotal}");
		priorityTotal.Should().Be(7811);
	}

	[Test]
	public void Puzzle02()
	{
		var text = Helpers.ReadTextFromFile(day: 3, puzzleNumber: 1);
		var lines = Helpers.GetCleanedLines(text);

		var priorityTotal = 0;

		for (var index = 0; index < lines.Length - 1; index += 3)
		{
			var elf1Rucksack = lines[index].ToCharArray();
			var elf2Rucksack = lines[index + 1].ToCharArray();
			var elf3Rucksack = lines[index + 2].ToCharArray();

			var itemsFoundInAll3Rucksacks = elf1Rucksack
											.Intersect(elf2Rucksack)
											.Intersect(elf3Rucksack);

			foreach (var item in itemsFoundInAll3Rucksacks)
			{
				priorityTotal += ItemPriority.GetValueOrDefault(item);
			}
		}

		TestContext.Out.WriteLine($"Answer: {priorityTotal}");
		priorityTotal.Should().Be(2639);
	}
}
