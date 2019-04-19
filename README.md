# Fable.RegexProvider

Simple F# Regex Type Provider compatible with Fable.

## Usage

**IMPORTANT**: Requires fable-compiler **2.2.0** or higher.

> You can find a usage sample in the `test` directory of this repository.

- First, install the package `Fable.RegexProvider` from Nuget.
- Then create a regex with `Fable.RegexProvider.SafeRegex.Create<"^hello world!?$">()`. If the pattern is not valid, you will get a compile-time error.

Please contribute to make the type provider a great tool for all Fable users!