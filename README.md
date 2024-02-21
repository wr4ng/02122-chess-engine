# Chess Engine

**Course:** 02122 Software Technology Project

**Group:** 2

**Group members:**
- Frederik Hvarregaard Andersen - s224801
- Mads Christian Wrang Nielsen - s224784
- Christoffer Dam-Hansen - s224789
- Rasmus Sarbæk Salling - s224788

# Formatting
Before comitting, run:
```shell
dotnet format .\02122.chess-engine.sln
```
to format all files in solution according to the [.editorconfig](.editorconfig).

# Testing
## Visual Studio Code
Install the [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) **VSCode** extension.

Open the project in **VSCode**. Press `Ctrl + Shift + P` to open the command palet. Select `.NET: Add existing project`. Navigate to and select [Tests/Tests.csproj](Tests/Tests.csproj).

Now you should be able to view and run tests under *Testing* in **VSCode**.

Can also run tests (from the root directory) using:
```shell
dotnet test .\Tests\Tests.csproj
```