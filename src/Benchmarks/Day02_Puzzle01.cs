using AdventOfCode;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;
public class Day02_Puzzle01
{	
	private enum Shape : byte
	{
		Rock = 1,
		Paper = 2,
		Scissor = 3
	}

	private static byte WinBonus = 6;
	private static byte DrawBonus = 3;
	private static byte LoseBonus = 0;

	private static readonly Dictionary<Shape, Shape> WinnerLoserCombinations = new Dictionary<Shape, Shape>()
	{
		// (Winnner, looser)
		{ Shape.Rock, Shape.Scissor },
		{ Shape.Paper, Shape.Rock },
		{ Shape.Scissor, Shape.Paper },
	};

	private static Shape ConvertToShape(string character)
	{
		if (character == "A" || character == "X")
			return Shape.Rock;

		if (character == "B" || character == "Y")
			return Shape.Paper;

		return Shape.Scissor;
	}

	private static ICollection<(Shape, Shape)> OriginalMoves;
	private static (Shape, Shape)[] MovesArray;

	[GlobalSetup]
	public void SetUp()
	{
		Helpers.DataDirectory = Path.Join(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "..", "..", "..", "..", "AdventOfCode", "data");
		Console.Out.WriteLine("MY DIR: " + Helpers.DataDirectory);

		var text = Helpers.ReadTextFromFile(day: 2, puzzleNumber: 1);
		var lines = Helpers.GetCleanedLines(text);
		OriginalMoves = Helpers.BuildListOfTuples(lines, ConvertToShape, ConvertToShape);
		MovesArray = OriginalMoves.ToArray();
	}

	/// <summary>
	/// 51.52 us - 40B - Gen0 0
	/// </summary>
	[Benchmark]
	public int OriginalSolution()
	{
		var myScore = 0;

		foreach (var move in OriginalMoves)
		{
			var otherPlayerMove = move.Item1;
			var myMove = move.Item2;

			if (myMove == otherPlayerMove)
			{
				myScore += (int)myMove + DrawBonus;
				continue;
			}

			var losingMove = WinnerLoserCombinations.GetValueOrDefault(otherPlayerMove);

			if (myMove == losingMove)
			{
				myScore += (int)myMove + LoseBonus;
				continue;
			}

			myScore += (int)myMove + WinBonus;
		}

		if (myScore != 8890) throw new ArgumentException("Incorrect Answer!");

		return myScore;
	}

	/// <summary>
	/// 51.41 us - 40B - Gen0 0
	/// </summary>
	[Benchmark]
	public int UseByteCast()
	{
		var myScore = 0;

		foreach (var move in OriginalMoves)
		{
			var otherPlayerMove = move.Item1;
			var myMove = move.Item2;

			if (myMove == otherPlayerMove)
			{
				myScore += (byte)myMove + DrawBonus;
				continue;
			}

			var losingMove = WinnerLoserCombinations.GetValueOrDefault(otherPlayerMove);

			if (myMove == losingMove)
			{
				myScore += (byte)myMove + LoseBonus;
				continue;
			}

			myScore += (byte)myMove + WinBonus;
		}

		if (myScore != 8890) throw new ArgumentException("Incorrect Answer!");

		return myScore;
	}

	/// <summary>
	/// 14.41 us - 0B - Gen0 0
	/// </summary>
	[Benchmark]
	public int LearningsFromPuzzle2()
	{
		var myScore = 0;

		for (var index = 0; index < MovesArray.Length; index++)
		{
			var move = MovesArray[index];
			var otherPlayerMove = move.Item1;
			var myMove = move.Item2;

			if (myMove == otherPlayerMove)
			{
				myScore += (byte)myMove + DrawBonus;
				continue;
			}

			var losingMove = WinnerLoserCombinations[otherPlayerMove];

			if (myMove == losingMove)
			{
				myScore += (byte)myMove + LoseBonus;
				continue;
			}

			myScore += (byte)myMove + WinBonus;
		}

		if (myScore != 8890) throw new ArgumentException($"Incorrect Answer: {myScore}");
		return myScore;
	}
}
