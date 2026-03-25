using Xunit;
using SpeedRush.Models;

namespace SpeedRush.Tests;

/// <summary>
/// Unit tests for the Car model class.
/// Tests car initialization, fuel management, and state transitions.
/// </summary>
public class CarTests
{
    /// <summary>
    /// Test that verifies a car is initialized with correct attributes.
    /// </summary>
    [Fact]
    public void Car_InitializesWithCorrectAttributes()
    {
        // Arrange & Act
        var car = new Car(1, "Ferrari", 200, 0.8, 60.0);

        // Assert
        Assert.Equal(1, car.Id);
        Assert.Equal("Ferrari", car.Name);
        Assert.Equal(200, car.MaxSpeed);
        Assert.Equal(0.8, car.FuelConsumptionRate);
        Assert.Equal(60.0, car.MaxFuelCapacity);
        Assert.Equal(60.0, car.CurrentFuel);
        Assert.False(car.HasFinished);
    }

    /// <summary>
    /// Test that verifies invalid car parameters throw exceptions.
    /// </summary>
    [Fact]
    public void Car_ThrowsException_WithInvalidParameters()
    {
        // Assert
        Assert.Throws<ArgumentException>(() => new Car(0, "Car", 100, 0.5, 50.0)); // Invalid ID
        Assert.Throws<ArgumentException>(() => new Car(1, "", 100, 0.5, 50.0)); // Empty name
        Assert.Throws<ArgumentException>(() => new Car(1, "Car", -100, 0.5, 50.0)); // Negative speed
    }

    /// <summary>
    /// Test that verifies the car can speed up and consume fuel.
    /// </summary>
    [Fact]
    public void SpeedUp_IncreasesSpeedAndConsumeFuel()
    {
        // Arrange
        var car = new Car(1, "Test Car", 100, 0.5, 50.0);
        double initialFuel = car.CurrentFuel;

        // Act
        bool result = car.SpeedUp(5.0);

        // Assert
        Assert.True(result);
        Assert.Equal(100, car.CurrentSpeed);
        Assert.Equal(initialFuel - 5.0, car.CurrentFuel);
    }

    /// <summary>
    /// Test that verifies the car cannot speed up without enough fuel.
    /// </summary>
    [Fact]
    public void SpeedUp_FailsWithInsufficientFuel()
    {
        // Arrange
        var car = new Car(1, "Test Car", 100, 0.5, 50.0);
        car.CurrentFuel = 2.0; // Not enough for 5.0 fuel cost

        // Act
        bool result = car.SpeedUp(5.0);

        // Assert
        Assert.False(result);
        Assert.Equal(2.0, car.CurrentFuel); // Fuel unchanged
    }

    /// <summary>
    /// Test that verifies the car can refuel to maximum capacity.
    /// </summary>
    [Fact]
    public void Refuel_RestoresFuelToMaxCapacity()
    {
        // Arrange
        var car = new Car(1, "Test Car", 100, 0.5, 50.0);
        car.CurrentFuel = 10.0;

        // Act
        double fuelAdded = car.Refuel();

        // Assert
        Assert.Equal(50.0, car.CurrentFuel);
        Assert.Equal(40.0, fuelAdded); // 50 - 10 = 40
    }

    /// <summary>
    /// Test that verifies the car resets correctly.
    /// </summary>
    [Fact]
    public void Reset_RestoresCarToInitialState()
    {
        // Arrange
        var car = new Car(1, "Test Car", 100, 0.5, 50.0);
        car.CurrentFuel = 10.0;
        car.CurrentSpeed = 50;
        car.CurrentLap = 3;
        car.HasFinished = true;

        // Act
        car.Reset();

        // Assert
        Assert.Equal(50.0, car.CurrentFuel);
        Assert.Equal(0, car.CurrentSpeed);
        Assert.Equal(1, car.CurrentLap);
        Assert.False(car.HasFinished);
    }
}
