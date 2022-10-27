# cmdline illustrations

This is a collection of illustrative use cases for command-line parsing and console applications, showing a varying degree of complexity and feature usage. The overall idea with these illustrations is to show how the API shape might allow incremental introduction to concepts as an application's complexity grows--but the developer never needs to be introduced to concepts they aren't using.

## Scenarios
### cmdline-1

Perhaps the simplest scenario: a single string argument. The CommandLineParser would provide the following functionality:

1. Automatically generated help
	- Showed when `--help` or aliases are supplied

2. Ensuring the expected parameter is supplied
	- The developer defines the argument but does not need to validate that it was supplied (it's non-nullable)
	- The parser is given the `args` value that is in scope in the top-level statements template
	- If the parser determines that parsing failed, help will be displayed and a non-zero exit code will be set

The application needs a means for presenting errors to the console and returning a non-zero exit code easily. This is illustrated now with `throw new CommandLineException`, but right now nothing is set up to be able to handle that gracefully. Another mechanism could be used.

### cmdline-2

This scenario illustrates a couple of additional features/behaviors.

1. The `CommandLineParser` accepts an optional description of the command. This would be included in the help display.
2. The app uses `AddArgument<int>()` to add a non-nullable/required int argument.
3. `AddArgument` accepts an optional name for the argument. That name would be included in the help display.
4. Since the month argument is non-nullable, its `Value` is also non-nullable.

### cmdline-3

This scenario combines the concepts of arguments and options.

1. It has an Enum-based option and a numeric argument, both of which are non-nullable/required
2. The option supports both a `--` long name and a `-` single-character alias

### cmdline-4

This scenario demonstrates a couple additional concepts:

1. Boolean options, each of which having at least a name and a short name
2. Name and short name aliases, simply by having arrays of names and shortnames
	- We'd allow _configuration_ of which name is the primary name (via a property)
	- But we'd also recognize the _convention_ that the first element in the array is the primary name by default
3. This program's "command" uses async code, and the top-level statements template still supports this easily

## Notes

These illustrations were crafted up with deliberate ignorance of today's System.CommandLine APIs. This approach aimed to have the perspective of which concepts were needed as opposed to which APIs could be used to get close to what was conceptualized.

### Pain Points

- I wanted to be able to use `parser.AddArgument<string>()`, but `String` does not implement `IParsable<T>`
	- This led to a string-specific `Argument` and a generic `Argument<T> where T : IParsable<T>`
	- I mentioned this to Tanner; I'm going to get his input on this; maybe `String` could implement `IParsable<T>` in .NET 8
- I also wanted to be able to use `parser.AddOption<ItemType>()` in cmdline-3
	- Enum types also don't implement `IParsable<T>`
	- This led to an Enum-specific `EnumOption<T>` and `AddEnumOption<T>`

### Alternate Designs

I considered a few alternatives to `parser.Parse(args)` that I dismissed.

- We could pass `args` into the `CommandLineParser` constructor
	- This will break down later when we introduce root commands and sub commands, because we're going to end up with nested `CommandLineParser` instances (I think)
	- This would also make unit testing less straightforward; it's nice being able to construct a parser and configure it without having to supply the args
- We could make `parser.Parse()` grab `System.Environment.CommandLine` if `args` isn't passed in
	- This would hide the magical scoping of `args` that trips people up today, but I think it's better to just showcase `args` as available
	- It would actually be trading one form of magic for another--"how does it get the args!?"
	- Having a dependency on `System.Environment.CommandLine` would again hurt unit testing (for ourselves too) and it brings in another dependency that can otherwise be unnecessary
- If we _did_ pass `args` into the `CommandLineParser` constructor, then we could cleverly call `Parse()` automatically whenever an Argument/Option's `Value` is accessed
	- I've implemented those types of "automatically call a function for me when something is accessed" designs before, and they always end up causing frustration later
	- Every time the Argument/Option concept is extended, those extenders need to know that they must ensure `Parse()` is called
	- It introduces a circular dependency too; the Argument/Option instances need to reach back into their parent
	- Over time, the cleverness loses its luster and users want to be able to turn it off and be responsible for invoking the method themselves

### dotnet watch

The `dotnet watch` command isn't helping with argument parsing--it inconsistently invokes the console application, sometimes with `run` as the first argument the console application receives. This should be fixed to ensure our parser doesn't need to accommodate for that bug, and app developers definitely shouldn't accommodate for it.

1. `dotnet watch`: (1) "run"
2. `dotnet watch run`: (1) "run"
3. `dotnet watch run Hello`: (1) "Hello"
4. `dotnet run`: (0)
5. `dotnet run Hello`: (1) "Hello"
6. `dotnet run -- Hello`: (1) "Hello"
7. `dotnet watch run -- Hello`: (1) "Hello"
8. `dotnet watch -- Hello`: Error from dotnet.exe

