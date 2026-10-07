# Name Sorter

A .NET 8 console app that sorts names by last name, then given names.

## Run the application

- For testing with different inputs, add the inputs to `unsorted-names-list.txt` in the root directory or replace that file with new input file (Change the input filename / path in the run command if it is different from what is mentioned below).
- Requires the .NET 8 SDK and a `.txt` input file. From the repository root, run:

```sh
dotnet run --project name-sorter -- ./unsorted-names-list.txt 
```

Sorted names are printed to the console and saved to `sorted-names-list.txt` in the current directory.

## Accepted and ignored lines

- A valid line has two to four whitespace-separated words: one to three given names followed by a last name. The final word is treated as the last name.
- A nonblank line with only one word or more than four words is ignored and reported with its line number and reason.
- Blank lines are skipped without being reported.

The app validates the number of words only; it does not check spelling or restrict the characters in a name.

## Data flow

1. `Program.cs` checks that exactly one `.txt` path was provided and creates the application services.
2. `NameSortingService` reads the input file contents through `FileReader` and skips blank lines.
3. `NameParserService` turns valid lines into `Name` values; invalid lines are recorded with their original line number and reason.
4. `NameSortingService` sorts valid names by last name, then given names, using `ComparerService`.
5. `OutputWriter` writes the sorted names to `sorted-names-list.txt` and prints them. It also reports any ignored lines to the console.

## Build and test

```sh
dotnet build NameSorter.sln --configuration Release
dotnet test NameSorter.sln --configuration Release
```

## Project structure

- `name-sorter/Program.cs` validates arguments and starts the app.
- `name-sorter/Entities` contains the name and parsing result models.
- `name-sorter/Services` parses, compares, and sorts names.
- `name-sorter/Infrastructure` handles file input and output.
- `name-sorter/Interfaces` defines input and output contracts.
- `name-sorter.tests/Unit` contains the unit tests.

## Continuous integration

GitHub Actions runs on every push and pull request. It sets up .NET 8, restores packages, builds the solution in Release mode, and runs the tests. The workflow is in `.github/workflows/dotnet.yml`.
