# C# Speed Rush - Racing Game Simulation

A turn-based, time-focused racing game simulation built with **Avalonia UI** and **.NET 10.0**. Players select a car with unique attributes and manage fuel, time, and speed to complete 5 laps.

## Features

### Game Mechanics
- **3 Unique Cars**: Ferrari F1 (fast, high fuel consumption), Tesla Model S (balanced), Toyota Eco (slow, efficient)
- **5-Lap Race**: Complete all 5 laps to win
- **Turn-Based Actions**: Speed Up, Maintain Speed, Pit Stop
- **Resource Management**: Manage fuel and time carefully
- **Real-Time Feedback**: Progress bars and status displays

### Technical Highlights
- **Object-Oriented Design**: Clean separation of concerns with Models, Managers, and Enums
- **Comprehensive Testing**: 12 unit tests covering core game logic
- **Exception Handling**: Robust error handling for invalid moves
- **XML Documentation**: Full documentation for all public methods and classes
- **Avalonia UI**: Cross-platform desktop application

## How to Play

1. **Select a Car**: Choose from Ferrari F1, Tesla Model S, or Toyota Eco
2. **Start the Race**: Click "Start Race" button
3. **Control Your Car**:
   - **Speed Up**: Increases speed (200% of normal), consumes 5L fuel, takes 10 seconds
   - **Maintain Speed**: Steady pace (50% of max speed), consumes 2L fuel, takes 15 seconds
   - **Pit Stop**: Refuel to maximum, takes 20 seconds
4. **Win Condition**: Complete all 5 laps before running out of fuel or time
5. **Reset**: Click "Reset Race" to start over


## How to Run

### Prerequisites
- .NET 7 SDK or later
- Avalonia UI 11.3.12 or later

### Build and Run

```bash
# Navigate to project directory
cd SpeedRush

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run

# Run tests
dotnet test
```

## Author

Fawziyyah Oke for Programming in C# module.
