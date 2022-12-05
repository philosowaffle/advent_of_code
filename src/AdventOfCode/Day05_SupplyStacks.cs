using FluentAssertions;

namespace AdventOfCode;
public class Day05_SupplyStacks
{
	private static readonly Dictionary<byte, Stack<char>> Stacks = new Dictionary<byte, Stack<char>>();
	private static string[] Instructions;

	[SetUp]
	public void Setup()
	{
		var text = Helpers.ReadTextFromFile(day: 5);
		var lines = Helpers.GetCleanedLines(text);

		var temp = new Dictionary<byte, List<char>>();

		var index = 0;

		// parse columns
		for (index = 0; index < lines.Length; index++)
		{
			var line = lines[index];
			var span = line.AsSpan();

			if (!span.Contains('['))
				break;

			byte column = 1;
			byte i2 = 0;
			while (i2 + 3 <= span.Length)
			{
				var col = span.Slice(i2, 3);

				if (!Stacks.ContainsKey(column))
					temp.TryAdd(column, new List<char>());

				if (col.Trim().IsEmpty)
				{
					i2 += 4;
					column++;
					continue;
				}

				temp[column].Add(span[i2 + 1]);
				i2 += 4;
				column++;
			}
		}

		foreach (var kvp in temp)
		{
			kvp.Value.Reverse();
			Stacks.TryAdd(kvp.Key, new Stack<char>(kvp.Value));
		}

		// grab Instructions
		index += 2;
		Instructions = lines.AsSpan().Slice(index).ToArray();
	}

	[Test]
	public void Puzzle01()
	{
		for (var index = 0; index < Instructions.Length; index++)
		{
			var instruction = Instructions[index];

			var splitOnSpace = instruction.Split(' ');
			var numberToMove = byte.Parse(splitOnSpace[1]);
			var fromColumnNumber = byte.Parse(splitOnSpace[3]);
			var toColumnNumber = byte.Parse(splitOnSpace[5]);

			while (numberToMove > 0)
			{
				Stacks[toColumnNumber].Push(Stacks[fromColumnNumber].Pop());
				numberToMove--;
			}
		}

		var answer = string.Concat(Stacks.Select(kvp => kvp.Value.Pop()));

		TestContext.Out.WriteLine($"Answer: {answer}");
		answer.Should().Be("SBPQRSCDF");
	}

	[Test]
	public void Puzzle02()
	{
		for (var index = 0; index < Instructions.Length; index++)
		{
			var instruction = Instructions[index];

			var splitOnSpace = instruction.Split(' ');
			var numberToMove = byte.Parse(splitOnSpace[1]);
			var fromColumnNumber = byte.Parse(splitOnSpace[3]);
			var toColumnNumber = byte.Parse(splitOnSpace[5]);

			var popped = new Stack<char>();
			var numberToPop = numberToMove;
			while (numberToPop > 0)
			{
				popped.Push(Stacks[fromColumnNumber].Pop());
				numberToPop--;
			}

			while (numberToMove > 0)
			{
				Stacks[toColumnNumber].Push(popped.Pop());
				numberToMove--;
			}
		}

		var answer = string.Concat(Stacks.Select(kvp => kvp.Value.Pop()));

		TestContext.Out.WriteLine($"Answer: {answer}");
		answer.Should().Be("RGLVRCQSB");
	}
}
