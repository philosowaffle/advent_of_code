// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;

Console.WriteLine("Advent of Code Benchmarks!");

var config = DefaultConfig.Instance
			.AddDiagnoser(MemoryDiagnoser.Default)
			.AddDiagnoser(ThreadingDiagnoser.Default);

BenchmarkSwitcher
	.FromAssembly(typeof(Program).Assembly)
	.Run(args, config);