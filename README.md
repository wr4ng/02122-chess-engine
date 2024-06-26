# Chess Engine

**Course:** 02122 Software Technology Project

**Group:** 2

**Group members:**
- Frederik Hvarregaard Andersen - s224801
- Mads Christian Wrang Nielsen - s224784
- Christoffer Dam-Hansen - s224789
- Rasmus Sarbæk Salling - s224788

# Running the application
Builds for Windows and Linux (both x86) can be found under [/Build](Build/). Unzip the `.zip` file and then run the executable found inside to run the application.

# Formatting
Before comitting, run:
```shell
dotnet format .\02122.chess-engine.sln
```
to format all files in solution according to the [.editorconfig](.editorconfig).

# Testing
Give **.NET** is installed, you can run the tests (from the root directory) using:
```shell
# Windows path
dotnet test .\Tests\Tests.csproj
# Linux path
dotnet test ./Tests/Tests.csproj
```
To exclude **Perft** tests (wich takes around 6-10 seconds):
```shell
dotnet test --filter Name!~Perft Tests/Tests.csproj # Powershell
dotnet test --filter Name\!~Perft Tests/Tests.csproj # Linux (zsh)
```
## Visual Studio Code
Install the [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) **VSCode** extension.

Open the project in **VSCode**. Press `Ctrl + Shift + P` to open the command palet. Select `.NET: Add existing project`. Navigate to and select [Tests/Tests.csproj](Tests/Tests.csproj).

Now you should be able to view and run tests under *Testing* in **VSCode**.