# AdventOfCode2023

![_7e9218fd-64bd-46d5-be33-2ba6c0b0307f](https://github.com/nicolabc/AdventOfCode2023/assets/31611304/309f09b3-f65c-4f35-8639-639198eb1fbc)

## About the Project

Advent of Code is an annual event that consists of a series of small programming puzzles, one for each day of December leading up to Christmas.
This repository contains my solutions to the Advent of Code 2023 puzzles, written in C#.

## Getting Started

To get started with this project, you will need to have the following installed on your machine:

-   `.NET 8`

In your terminal, navigate to `\AdventOfCode2023` (console project) and run

```sh
dotnet build
```

then navigate to `AdventOfCode2023\bin\Debug\net8.0` and run

```sh
dotnet .\AdventOfCode2023.dll
```

To run the solution of a different day, please change Day class in `IocInstaller.cs`

```csharp
AddSingleton<IAdventSolution, Day1>()
```

## License

This project is licensed under the MIT License. See the LICENSE file for details.
