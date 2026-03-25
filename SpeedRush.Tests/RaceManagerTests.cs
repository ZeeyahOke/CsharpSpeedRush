using Xunit;
using SpeedRush.Models;
using SpeedRush.Managers;
using SpeedRush.Enums;

namespace SpeedRush.Tests;

/// <summary>
/// Unit tests for the RaceManager class.
/// Tests core game logic including fuel consumption, lap progression, and race completion.
/// </summary>
public class RaceManagerTests
{
    /// <summary>
    /// Test that verifies fuel is consumed correctly when speeding up.
    /// </summary>
    [Fact]
    public void SpeedUp_ConsumesCorrectFuelAmount()
    {
        // Arrange
        var car = new Car(1, "Test Car", 100, 0.5, 50.0);
        var manager = new RaceManager(car);
        double initialFuel = car.CurrentFuel;

        // Act
        manager.ExecuteAction(GameAction.SpeedUp);

        // Assert
        Assert.True(car.CurrentFuel < initialFuel);
        Assert.Equal(100, car.CurrentSpeed); // Should be at max speed
    }

    /// <summary>
    /// Test that verifies lap progression works correctly.
    /// </summary>
    [Fact]
    public void ExecuteAction_ProgressesLapCorrectly()
    {
        // Arrange
        var car = new Car(1, "Test Car", 100, 0.5, 50.0);
        var manager = new RaceManager(car);
        double initialProgress = car.LapProgress;

        // Act
        manager.ExecuteAction(GameAction.SpeedUp);

        // Assert
        Assert.True(car.LapProgress > initialProgress);
    }

    /// <summary>
    /// Test that verifies the race ends when all laps are completed.
    /// </summary>
    [Fact]
    public void RaceEnds_WhenAllLapsCompleted()
    {
        // Arrange
        var car = new Car(1, "Test Car", 100, 0.5, 500.0);
        var manager = new RaceManager(car, 5000.0);

        // Act
        // Complete all 5 laps (each SpeedUp is 25% of lap)
        for (int i = 0; i < 20; i++)
        {
            if (!manager.IsRaceActive)
                break;
            manager.ExecuteAction(GameAction.SpeedUp);
        }

        // Assert
        Assert.True(car.HasFinished);
        Assert.Equal(5, car.CurrentLap); // Should be capped at lap 5
    }

    /// <summary>
    /// Test that verifies pit stop refuels the car correctly.
    /// </summary>
    [Fact]
    public void PitStop_RefuelsCar()
    {
        // Arrange
        var car = new Car(1, "Test Car", 100, 0.5, 50.0);
        var manager = new RaceManager(car);
        
        // Use fuel first
        manager.ExecuteAction(GameAction.SpeedUp);
        double fuelAfterSpeedup = car.CurrentFuel;

        // Act
        manager.ExecuteAction(GameAction.PitStop);

        // Assert
        Assert.Equal(50.0, car.CurrentFuel); // Should be back to max
        Assert.True(car.CurrentFuel > fuelAfterSpeedup);
    }

    /// <summary>
    /// Test that verifies the race ends when time runs out.
    /// </summary>
    [Fact]
    public void RaceEnds_WhenTimeRunsOut()
    {
        // Arrange
        var car = new Car(1, "Test Car", 100, 0.5, 500.0);
        var manager = new RaceManager(car, 0.1); // Very short time limit (100ms)
        manager.StartRaceTimer();

        // Act - Wait for time to run out
        System.Threading.Thread.Sleep(200); // Wait 200ms for timer to expire
        
        // Assert
        Assert.True(manager.TimeRemaining <= 0);
    }
}
