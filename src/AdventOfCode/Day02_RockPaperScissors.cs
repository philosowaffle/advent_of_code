using FluentAssertions;

namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2022/day/2
/// </summary>
public class Day02_RockPaperScissors
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

	private static Outcome ConvertToOutcome(string character)
	{
		if (character == "X")
			return Outcome.Lose;

		if (character == "Y")
			return Outcome.Draw;

		return Outcome.Win;
	}

	[Test]
	public async Task Puzzle_01()
	{
		var myScore = 0;

		var text = await Helpers.ReadTextFromFileAsync(day: 2, puzzleNumber: 1);
		var lines = Helpers.GetCleanedLines(text);
		var moves = Helpers.BuildListOfTuples<Shape, Shape>(lines, ConvertToShape, ConvertToShape);

		foreach (var move in moves)
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

		TestContext.Out.WriteLine($"Answer: {myScore}");
		myScore.Should().Be(8890);
	}

	[Test]
	public async Task Puzzle_02()
	{
		var myScore = 0;

		var text = await Helpers.ReadTextFromFileAsync(day: 2, puzzleNumber: 2);
		var lines = Helpers.GetCleanedLines(text);
		var moves = Helpers.BuildListOfTuples(lines, ConvertToShape, ConvertToOutcome);

		foreach (var move in moves)
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

		TestContext.Out.WriteLine($"Answer: {myScore}");
		myScore.Should().Be(10238);
	}

}