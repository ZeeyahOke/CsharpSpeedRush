using System;

namespace SpeedRush.Models;

/// <summary>
/// Represents a racing car with specific attributes and capabilities.
/// Each car has unique speed, fuel consumption, and fuel capacity characteristics.
/// </summary>
public class Car
{
    /// <summary>
    /// Gets the unique identifier for the car.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Gets the name of the car.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the maximum speed of the car in km/h.
    /// </summary>
    public int MaxSpeed { get; private set; }

    /// <summary>
    /// Gets the fuel consumption rate (liters per km).
    /// Higher values mean more fuel is consumed per kilometer.
    /// </summary>
    public double FuelConsumptionRate { get; private set; }

    /// <summary>
    /// Gets the maximum fuel capacity in liters.
    /// </summary>
    public double MaxFuelCapacity { get; private set; }

    /// <summary>
    /// Gets or sets the current fuel level in liters.
    /// </summary>
    public double CurrentFuel { get; set; }

    /// <summary>
    /// Gets or sets the current speed of the car in km/h.
    /// </summary>
    public int CurrentSpeed { get; set; }

    /// <summary>
    /// Gets or sets the distance covered in the current lap (in percentage, 0-100).
    /// </summary>
    public double LapProgress { get; set; }

    /// <summary>
    /// Gets or sets the current lap number (1-5).
    /// </summary>
    public int CurrentLap { get; set; }

    /// <summary>
    /// Gets or sets the total time elapsed in seconds.
    /// </summary>
    public double TimeElapsed { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the car is currently racing.
    /// </summary>
    public bool IsRacing { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the car has finished the race.
    /// </summary>
    public bool HasFinished { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Car"/> class.
    /// </summary>
    /// <param name="id">The unique identifier for the car.</param>
    /// <param name="name">The name of the car.</param>
    /// <param name="maxSpeed">The maximum speed in km/h.</param>
    /// <param name="fuelConsumptionRate">The fuel consumption rate (liters per km).</param>
    /// <param name="maxFuelCapacity">The maximum fuel capacity in liters.</param>
    /// <exception cref="ArgumentException">Thrown when any parameter is invalid.</exception>
    public Car(int id, string name, int maxSpeed, double fuelConsumptionRate, double maxFuelCapacity)
    {
        if (id <= 0)
            throw new ArgumentException("Car ID must be positive.", nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Car name cannot be empty.", nameof(name));

        if (maxSpeed <= 0)
            throw new ArgumentException("Max speed must be positive.", nameof(maxSpeed));

        if (fuelConsumptionRate <= 0)
            throw new ArgumentException("Fuel consumption rate must be positive.", nameof(fuelConsumptionRate));

        if (maxFuelCapacity <= 0)
            throw new ArgumentException("Max fuel capacity must be positive.", nameof(maxFuelCapacity));

        Id = id;
        Name = name;
        MaxSpeed = maxSpeed;
        FuelConsumptionRate = fuelConsumptionRate;
        MaxFuelCapacity = maxFuelCapacity;
        CurrentFuel = maxFuelCapacity;
        CurrentSpeed = 0;
        LapProgress = 0;
        CurrentLap = 1;
        TimeElapsed = 0;
        IsRacing = false;
        HasFinished = false;
    }

    /// <summary>
    /// Increases the car's speed, consuming additional fuel.
    /// </summary>
    /// <param name="fuelCost">The additional fuel cost for speeding up.</param>
    /// <returns>True if speed was increased successfully; otherwise, false.</returns>
    public bool SpeedUp(double fuelCost)
    {
        if (CurrentFuel < fuelCost)
            return false;

        CurrentSpeed = MaxSpeed;
        CurrentFuel -= fuelCost;
        return true;
    }

    /// <summary>
    /// Maintains the current speed of the car.
    /// </summary>
    /// <param name="fuelCost">The fuel cost for maintaining speed.</param>
    /// <returns>True if speed was maintained successfully; otherwise, false.</returns>
    public bool MaintainSpeed(double fuelCost)
    {
        if (CurrentFuel < fuelCost)
            return false;

        CurrentSpeed = MaxSpeed / 2;
        CurrentFuel -= fuelCost;
        return true;
    }

    /// <summary>
    /// Refuels the car to its maximum capacity.
    /// </summary>
    /// <returns>The amount of fuel added.</returns>
    public double Refuel()
    {
        double fuelAdded = MaxFuelCapacity - CurrentFuel;
        CurrentFuel = MaxFuelCapacity;
        CurrentSpeed = 0;
        return fuelAdded;
    }

    /// <summary>
    /// Checks if the car has enough fuel to continue racing.
    /// </summary>
    /// <returns>True if the car has fuel; otherwise, false.</returns>
    public bool HasFuel()
    {
        return CurrentFuel > 0;
    }

    /// <summary>
    /// Resets the car to its initial state for a new race.
    /// </summary>
    public void Reset()
    {
        CurrentFuel = MaxFuelCapacity;
        CurrentSpeed = 0;
        LapProgress = 0;
        CurrentLap = 1;
        TimeElapsed = 0;
        IsRacing = false;
        HasFinished = false;
    }

    /// <summary>
    /// Returns a string representation of the car's current state.
    /// </summary>
    /// <returns>A string containing the car's name and current status.</returns>
    public override string ToString()
    {
        return $"{Name} - Speed: {CurrentSpeed} km/h, Fuel: {CurrentFuel:F2}L, Lap: {CurrentLap}/5";
    }
}
