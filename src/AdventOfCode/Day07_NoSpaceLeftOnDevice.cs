using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;
public class Day07_NoSpaceLeftOnDevice
{
	[Test]
	public void Puzzle01()
	{
		var text = Helpers.ReadTextFromFile(day: 7);
		var lines = Helpers.GetCleanedLines(text);

		var terminalCommand = "$";
		var cd = "cd";
		var rootName = "/";
		var ls = "ls";
		var back = "..";
		var dir = "dir";

		var root = new ElfFile()
		{
			Name = "/",
			IsDir = true,
		};


		var currentDir = root;
		foreach (var line in lines)
		{
			var span = line.AsSpan();
			if (span.Slice(0,1).ToString() == terminalCommand)
			{
				var command = span.Slice(2, 2).ToString();

				if (command == cd)
				{
					var target = span.Slice(5).ToString();

					if (target == rootName)
					{
						currentDir = root;
						continue;
					}

					if (target == back)
					{
						currentDir = currentDir.Parent;
						continue;
					}

					currentDir = currentDir.Children[target.ToString()];
					continue;
				}

				if (command == ls)
					continue;

			}

			var prefix = span.Slice(0, 3).ToString();
			if (prefix == dir)
			{
				var target = span.Slice(4).ToString();
				if (!currentDir.Children.ContainsKey(target))
					currentDir.Children.Add(target, new ElfFile()
					{
						IsDir = true,
						Name = target,
						Parent = currentDir
					});
			} else
			{
				var spaceIndex = span.IndexOf(' ');
				var fileSize = int.Parse(span.Slice(0, spaceIndex));
				var target = span.Slice(spaceIndex + 1).ToString();
				if (!currentDir.Children.ContainsKey(target))
					currentDir.Children.Add(target, new ElfFile()
					{
						IsDir = false,
						Name = target,
						Size = fileSize,
						Parent = currentDir
					});
			}
		}

		var answer = 0;

		// get rollup dir sizes
		answer = root.Children.Select(kvp => CalculateSize(kvp.Value, 100000)).Sum();
		

		TestContext.Out.WriteLine($"Answer: {answer}");
		//answer.Should().Be(1134);
	}

	public static int CalculateSize(ElfFile file, int maxSize)
	{
		if (!file.IsDir)
			return file.Size;

		file.Size = file.Children.Select(kvp => CalculateSize(kvp.Value, maxSize)).Sum();

		if (file.Size <= maxSize)
			return file.Size;

		return 0;
	}

	//public static int Filter(ElfFile file, int maxSize)
	//{
	//	if (!file.IsDir)
	//		return 0;

	//	var size = 0;
	//	if (file.Size <= maxSize)
	//		size = file.Size;

	//	size += file.Children.Select(kvp => Filter(kvp.Value, maxSize)).Sum();
	//	return file.Size;
	//}

	public class ElfFile
	{
		public string Name { get; set; }
		public int Size { get; set; }
		public bool IsDir { get; set; }

		
		public ElfFile Parent { get; set; }
		public Dictionary<string, ElfFile> Children { get; set; }

		public ElfFile()
		{
			Children = new Dictionary<string, ElfFile>();
		}
	}
}
