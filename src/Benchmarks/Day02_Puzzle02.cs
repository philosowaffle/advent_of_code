using AdventOfCode;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;
public class Day02_Puzzle02
{
	private enum Shape : byte
	{
		Rock = 1,
		Paper = 2,
		Scissor = 3
	}

	private enum Outcome : byte
	{
		Lose = 0,
		Draw = 1,
		Win = 2
	}

	private static byte WinBonus = 6;
	private static byte DrawBonus = 3;
	private static byte LoseBonus = 0;

	private static readonly Dictionary<Shape, Shape> WinnerLoserCombinations = new Dictionary<Shape, Shape>()
	{
		// (Winnner, loser)
		{ Shape.Rock, Shape.Scissor },
		{ Shape.Paper, Shape.Rock },
		{ Shape.Scissor, Shape.Paper },
	};

	private static readonly Dictionary<Shape, Shape> LoserWinnerCombinations = new Dictionary<Shape, Shape>()
	{
		// (Loser, Winner)
		{ Shape.Scissor, Shape.Rock },
		{ Shape.Rock, Shape.Paper },
		{ Shape.Paper, Shape.Scissor },
	};

	private static Shape ConvertToShape(string character)
	{
		if (character == "A" || character == "X")
			return Shape.Rock;

		if (character == "B" || character == "Y")
			return Shape.Paper;

		return Shape.Scissor;
	}

	private static Outcome ConvertToOutcome(string character)
	{
		if (character == "X")
			return Outcome.Lose;

		if (character == "Y")
			return Outcome.Draw;

		return Outcome.Win;
	}

	private static ICollection<(Shape, Outcome)> OriginalMoves;
	private static (Shape, Outcome)[] MovesArray;

	[GlobalSetup]
	public void SetUp()
	{
		Helpers.DataDirectory = Path.Join(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "..", "..", "..", "..", "AdventOfCode", "data");
		Console.Out.WriteLine("DATA DIRECTORY: " + Helpers.DataDirectory);

		var text = Helpers.ReadTextFromFile(day: 2, puzzleNumber: 1);
		var lines = Helpers.GetCleanedLines(text);
		OriginalMoves = Helpers.BuildListOfTuples(lines, ConvertToShape, ConvertToOutcome);
		MovesArray = OriginalMoves.ToArray();
	}

	/// <summary>
	///  88.50 us - 126456B - Gen0 40.2832
	/// </summary>
	[Benchmark]
	public int OriginalSolution()
	{
		var myScore = 0;

		foreach (var move in OriginalMoves)
		{
			var otherPlayerMove = move.Item1;
			var myDesiredOutCome = move.Item2;

			if (myDesiredOutCome == Outcome.Draw)
			{
				myScore += (int)otherPlayerMove + DrawBonus;
				continue;
			}

			if (myDesiredOutCome == Outcome.Lose)
			{
				var losingMove = WinnerLoserCombinations.GetValueOrDefault(otherPlayerMove);
				myScore += (int)losingMove + LoseBonus;
				continue;
			}

			var winningMove = WinnerLoserCombinations.FirstOrDefault(k => k.Value == otherPlayerMove);
			myScore += (int)winningMove.Key + WinBonus;
		}

		if (myScore != 10238) throw new ArgumentException("Incorrect Answer!");
		return myScore;
	}

	/// <summary>
	/// 59.83 us - 40B - Gen0 0
	/// </summary>
	[Benchmark]
	public int Also_Have_LoserWinnerCombo_Available()
	{
		var myScore = 0;

		foreach (var move in OriginalMoves)
		{
			var otherPlayerMove = move.Item1;
			var myDesiredOutCome = move.Item2;

			if (myDesiredOutCome == Outcome.Draw)
			{
				myScore += (byte)otherPlayerMove + DrawBonus;
				continue;
			}

			if (myDesiredOutCome == Outcome.Lose)
			{
				var losingMove = WinnerLoserCombinations.GetValueOrDefault(otherPlayerMove);
				myScore += (byte)losingMove + LoseBonus;
				continue;
			}

			var winningMove = LoserWinnerCombinations.GetValueOrDefault(otherPlayerMove);
			myScore += (byte)winningMove + WinBonus;
		}

		if (myScore != 10238) throw new ArgumentException("Incorrect Answer!");
		return myScore;
	}

	/// <summary>
	/// 49.97 us - 40B - Gen0 0
	/// </summary>
	[Benchmark]
	public int Dictionary_DontUse_GetValueOrDefault()
	{
		var myScore = 0;

		foreach (var move in OriginalMoves)
		{
			var otherPlayerMove = move.Item1;
			var myDesiredOutCome = move.Item2;

			if (myDesiredOutCome == Outcome.Draw)
			{
				myScore += (byte)otherPlayerMove + DrawBonus;
				continue;
			}

			if (myDesiredOutCome == Outcome.Lose)
			{
				var losingMove = WinnerLoserCombinations[otherPlayerMove];
				myScore += (byte)losingMove + LoseBonus;
				continue;
			}

			var winningMove = LoserWinnerCombinations[otherPlayerMove];
			myScore += (byte)winningMove + WinBonus;
		}

		if (myScore != 10238) throw new ArgumentException("Incorrect Answer!");
		return myScore;
	}

	/// <summary>
	/// 50.65 us - 40B - Gen0 0
	/// </summary>
	[Benchmark]
	public int SpecialDataStructure()
	{
		var myScore = 0;

		foreach (var move in OriginalMoves)
		{
			var otherPlayerMove = move.Item1;
			var myDesiredOutCome = move.Item2;

			if (myDesiredOutCome == Outcome.Draw)
			{
				myScore += (byte)otherPlayerMove + DrawBonus;
				continue;
			}

			if (myDesiredOutCome == Outcome.Lose)
			{
				var losingMove = ShapeRules[otherPlayerMove].WinsAgainst;
				myScore += (byte)losingMove + LoseBonus;
				continue;
			}

			var winningMove = ShapeRules[otherPlayerMove].LosesAgainst;
			myScore += (byte)winningMove + WinBonus;
		}

		if (myScore != 10238) throw new ArgumentException("Incorrect Answer!");
		return myScore;
	}

	/// <summary>
	/// 17.91 us - 0B - Gen0 0
	/// </summary>
	[Benchmark]
	public int OldSchoolForLoop()
	{
		var myScore = 0;

		for (var index = 0; index < MovesArray.Length; index++)
		{
			var move = MovesArray[index];

			var otherPlayerMove = move.Item1;
			var myDesiredOutCome = move.Item2;

			if (myDesiredOutCome == Outcome.Draw)
			{
				myScore += (byte)otherPlayerMove + DrawBonus;
				continue;
			}

			if (myDesiredOutCome == Outcome.Lose)
			{
				var losingMove = ShapeRules[otherPlayerMove].WinsAgainst;
				myScore += (byte)losingMove + LoseBonus;
				continue;
			}

			var winningMove = ShapeRules[otherPlayerMove].LosesAgainst;
			myScore += (byte)winningMove + WinBonus;
		}

		if (myScore != 10238) throw new ArgumentException($"Incorrect Answer: {myScore}");
		return myScore;
	}

	private struct Rules
	{
		public Shape WinsAgainst { get; set; }
		public Shape LosesAgainst { get; set; }
	}

	private static readonly Dictionary<Shape, Rules> ShapeRules = new Dictionary<Shape, Rules>()
	{
		{ Shape.Rock, new Rules() { WinsAgainst = Shape.Scissor, LosesAgainst = Shape.Paper } },
		{ Shape.Paper, new Rules() { WinsAgainst = Shape.Rock, LosesAgainst = Shape.Scissor } },
		{ Shape.Scissor, new Rules() { WinsAgainst = Shape.Paper, LosesAgainst = Shape.Rock } },
	};
}
