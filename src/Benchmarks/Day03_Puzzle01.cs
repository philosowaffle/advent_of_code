using AdventOfCode;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;
public class Day03_Puzzle01
{
	private static readonly Dictionary<char, byte> ItemPriority = new Dictionary<char, byte>() { };
	private static readonly byte[] ItemPriorityArray = new byte[123];
	private static string[] Lines;

	[GlobalSetup]
	public void SetUp()
	{
		Helpers.DataDirectory = Path.Join(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "..", "..", "..", "..", "AdventOfCode", "data");
		Console.Out.WriteLine("DATA DIRECTORY: " + Helpers.DataDirectory);

		var text = Helpers.ReadTextFromFile(day: 3);
		Lines = Helpers.GetCleanedLines(text);

		var alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		byte counter = 1;
		foreach (var c in alphabet.ToArray())
		{
			ItemPriority.Add(c, counter);
			counter++;
		}

		// build array
		alphabet = "abcdefghijklmnopqrstuvwxyz";
		counter = 1;
		var upper = alphabet.ToUpper().ToCharArray();
		foreach (var c in upper.ToArray())
		{
			ItemPriorityArray[c] = counter;
			counter++;
		}

		var lower = alphabet.ToLower().ToCharArray();
		foreach (var c in lower.ToArray())
		{
			ItemPriorityArray[c] = counter;
			counter++;
		}
	}

	/// <summary>
	///  183.7 us - 202.73 KB - Gen0  66.1621
	/// </summary>
	[Benchmark]
	public int OriginalSolution()
	{
		var priorityTotal = 0;

		foreach (var line in Lines)
		{
			var span = line.AsSpan();
			var totalItemsInRucksack = span.Length;
			var itemsPerCompartment = totalItemsInRucksack / 2;

			var itemsInCompartment1 = span.Slice(0, itemsPerCompartment).ToArray();
			var itemsInCompartment2 = span.Slice(itemsPerCompartment, itemsPerCompartment).ToArray();

			var itemsFoundInBothCompartments = itemsInCompartment1.Intersect(itemsInCompartment2);

			foreach (var item in itemsFoundInBothCompartments)
			{
				priorityTotal += ItemPriority.GetValueOrDefault(item);
			}
		}

		if (priorityTotal != 7811) throw new ArgumentException($"Incorrect Answer: {priorityTotal}");
		return priorityTotal;
	}

	/// <summary>
	///  183.5 us - 202.73 KB - Gen0  66.1621
	/// </summary>
	[Benchmark]
	public int ConvertFirstLoopTo_ForLoop()
	{
		var priorityTotal = 0;

		for (var index = 0; index < Lines.Length; index++)
		{
			var line = Lines[index];
			var span = line.AsSpan();
			var totalItemsInRucksack = span.Length;
			var itemsPerCompartment = totalItemsInRucksack / 2;

			var itemsInCompartment1 = span.Slice(0, itemsPerCompartment).ToArray();
			var itemsInCompartment2 = span.Slice(itemsPerCompartment, itemsPerCompartment).ToArray();

			var itemsFoundInBothCompartments = itemsInCompartment1.Intersect(itemsInCompartment2);

			foreach (var item in itemsFoundInBothCompartments)
				priorityTotal += ItemPriority.GetValueOrDefault(item);
		}

		if (priorityTotal != 7811) throw new ArgumentException($"Incorrect Answer: {priorityTotal}");
		return priorityTotal;
	}

	/// <summary>
	///  465.5 us - 539.11 KB - Gen0  175.7813
	/// </summary>
	[Benchmark]
	public int ConvertSecondLoopTo_ForLoop()
	{
		var priorityTotal = 0;

		for (var index = 0; index < Lines.Length; index++)
		{
			var line = Lines[index];
			var span = line.AsSpan();
			var totalItemsInRucksack = span.Length;
			var itemsPerCompartment = totalItemsInRucksack / 2;

			var itemsInCompartment1 = span.Slice(0, itemsPerCompartment).ToArray();
			var itemsInCompartment2 = span.Slice(itemsPerCompartment, itemsPerCompartment).ToArray();

			var itemsFoundInBothCompartments = itemsInCompartment1.Intersect(itemsInCompartment2);

			// Iterating the IEnumberable to get Count seems to be expensive, perhaps also using ElementAt
			for (var index2 = 0; index2 < itemsFoundInBothCompartments.Count(); index2++)
			{
				var item = itemsFoundInBothCompartments.ElementAt(index2);
				priorityTotal += ItemPriority.GetValueOrDefault(item);
			}
		}

		if (priorityTotal != 7811) throw new ArgumentException($"Incorrect Answer: {priorityTotal}");
		return priorityTotal;
	}

	/// <summary>
	///  115.8 us - 113.37 KB - Gen0  36.9873
	/// </summary>
	[Benchmark]
	public int ImproveMemoryUsage()
	{
		var priorityTotal = 0;

		for (var index = 0; index < Lines.Length; index++)
		{
			var line = Lines[index];
			var span = line.AsSpan();
			var totalItemsInRucksack = span.Length;
			var itemsPerCompartment = totalItemsInRucksack / 2;

			var itemsInCompartment1 = span.Slice(0, itemsPerCompartment);
			var itemsInCompartment2 = span.Slice(itemsPerCompartment);

			var itemsFoundInBothCompartments = new char[itemsPerCompartment];
			for (var i = 0; i < itemsPerCompartment; i++)
			{
				var item = itemsInCompartment1[i];
				if (itemsInCompartment2.Contains(item))
					itemsFoundInBothCompartments[i] = item;
			}

			var distinctItems = itemsFoundInBothCompartments.Distinct();
			foreach (var item in distinctItems)
				priorityTotal += ItemPriority.GetValueOrDefault(item);
		}

		if (priorityTotal != 7811) throw new ArgumentException($"Incorrect Answer: {priorityTotal}");
		return priorityTotal;
	}

	/// <summary>
	///  121.0 us - 129.77 KB - Gen0 42.3584
	/// </summary>
	[Benchmark]
	public int StopUsing_GetValueOrDefault()
	{
		var priorityTotal = 0;

		for (var index = 0; index < Lines.Length; index++)
		{
			var line = Lines[index];
			var span = line.AsSpan();
			var totalItemsInRucksack = span.Length;
			var itemsPerCompartment = totalItemsInRucksack / 2;

			var itemsInCompartment1 = span.Slice(0, itemsPerCompartment);
			var itemsInCompartment2 = span.Slice(itemsPerCompartment, itemsPerCompartment);

			var itemsFoundInBothCompartments = new char[itemsPerCompartment];
			for (var i = 0; i < itemsPerCompartment; i++)
			{
				var item = itemsInCompartment1[i];
				if (itemsInCompartment2.Contains(item))
					itemsFoundInBothCompartments[i] = item;
			}

			var distinctItems = itemsFoundInBothCompartments
									.Distinct()
									.Where(i => i != default);
			foreach (var item in distinctItems)
				priorityTotal += ItemPriority[item];
		}

		if (priorityTotal != 7811) throw new ArgumentException($"Incorrect Answer: {priorityTotal}");
		return priorityTotal;
	}

	/// <summary>
	///  242.4 us - 253.8 KB - 82.7637
	/// </summary>
	[Benchmark]
	public int LinqOnly()
	{
		var priorityTotal = Lines
							.Select(line =>
							{
								var half = line.Length / 2;
								var itemsInCompartment1 = line.Substring(0, half);
								var itemsInCompartment2 = line.Substring(half);

								return itemsInCompartment1
										.Intersect(itemsInCompartment2)
										.Sum(i => ItemPriority[i]);
							}).Sum();

		if (priorityTotal != 7811) throw new ArgumentException($"Incorrect Answer: {priorityTotal}");
		return priorityTotal;
	}

	// Not sure yet why this impl isn't yielding the correct answer
	//[Benchmark]
	//public int OptimizeIterations()
	//{
	//	var priorityTotal = 0;

	//	for (var index = 0; index < Lines.Length; index++)
	//	{
	//		var line = Lines[index];
	//		var span = line.AsSpan();
	//		var totalItemsInRucksack = span.Length;
	//		var itemsPerCompartment = totalItemsInRucksack / 2;

	//		var itemsInCompartment1 = span.Slice(0, itemsPerCompartment);
	//		var itemsInCompartment2 = span.Slice(itemsPerCompartment);

	//		var itemPriorityCopy = new byte[ItemPriorityArray.Length];
	//		ItemPriorityArray.CopyTo(itemPriorityCopy, 0);
	//		for (var i = 0; i < itemsPerCompartment; i++)
	//		{
	//			var character = itemsInCompartment1[i];
	//			if (itemsInCompartment2.Contains(character))
	//			{
	//				priorityTotal += itemPriorityCopy[character];
	//				itemPriorityCopy[character] = 0;
	//			}
	//		}
	//	}

	//	if (priorityTotal != 7811) throw new ArgumentException($"Incorrect Answer: {priorityTotal}");
	//	return priorityTotal;
	//}
}
