using System;
using SpeedRush.Models;
using SpeedRush.Enums;

namespace SpeedRush.Managers;

/// <summary>
/// Manages the overall race logic, including lap progression, fuel consumption, and race completion.
/// Implements automatic countdown timer that decreases over time.
/// </summary>
public class RaceManager
{
    private const int TotalLaps = 5;
    private const double LapDistance = 100.0;
    private const double TimePerLap = 60.0;
    private const double SpeedUpFuelCost = 5.0;
    private const double MaintainSpeedFuelCost = 2.0;

    private readonly Car _car;
    private double _totalTimeRemaining;
    private DateTime _raceStartTime;
    private bool _raceStarted = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="RaceManager"/> class.
    /// </summary>
    public RaceManager(Car car, double timeLimit = 180.0)
    {
        _car = car ?? throw new ArgumentNullException(nameof(car));
        if (timeLimit <= 0)
            throw new ArgumentException("Time limit must be positive.", nameof(timeLimit));

        _totalTimeRemaining = timeLimit;
    }

    /// <summary>
    /// Gets the current car in the race.
    /// </summary>
    public Car CurrentCar => _car;

    /// <summary>
    /// Gets the remaining time in the race (automatic countdown).
    /// </summary>
    public double TimeRemaining
    {
        get
        {
            if (!_raceStarted)
                return _totalTimeRemaining;

            double elapsed = (DateTime.UtcNow - _raceStartTime).TotalSeconds;
            double remaining = _totalTimeRemaining - elapsed;
            return Math.Max(0, remaining);
        }
    }

    /// <summary>
    /// Gets a value indicating whether the race is active.
    /// </summary>
    public bool IsRaceActive => !_car.HasFinished && TimeRemaining > 0 && _car.HasFuel();

    /// <summary>
    /// Starts the race timer.
    /// </summary>
    public void StartRaceTimer()
    {
        _raceStartTime = DateTime.UtcNow;
        _raceStarted = true;
    }

    /// <summary>
    /// Executes a game action.
    /// </summary>
    public bool ExecuteAction(GameAction action)
    {
        if (!IsRaceActive)
            return false;

        return action switch
        {
            GameAction.SpeedUp => PerformSpeedUp(),
            GameAction.MaintainSpeed => PerformMaintainSpeed(),
            GameAction.PitStop => PerformPitStop(),
            _ => false
        };
    }

    private bool PerformSpeedUp()
    {
        if (!_car.SpeedUp(SpeedUpFuelCost))
            return false;

        ProgressLap(0.25);
        return true;
    }

    private bool PerformMaintainSpeed()
    {
        if (!_car.MaintainSpeed(MaintainSpeedFuelCost))
            return false;

        ProgressLap(0.15);
        return true;
    }

    private bool PerformPitStop()
    {
        _car.Refuel();
        return true;
    }

    private void ProgressLap(double percentage)
    {
        // Don't progress if race is already finished
        if (_car.HasFinished)
            return;

        _car.LapProgress += percentage;

        if (_car.LapProgress >= 1.0)
        {
            _car.CurrentLap++;

            // End race when lap 5 is completed
            if (_car.CurrentLap >= TotalLaps)
            {
                _car.CurrentLap = TotalLaps; // Cap at 5
                _car.LapProgress = 1.0; // Set to 100% to show completion
                _car.HasFinished = true;
            }
            else
            {
                _car.LapProgress = 0; // Reset for next lap
            }
        }
    }

    /// <summary>
    /// Gets the race status as a formatted string.
    /// </summary>
    public string GetRaceStatus()
    {
        return $"Lap: {_car.CurrentLap}/5 | " +
               $"Progress: {(_car.LapProgress * 100):F0}% | " +
               $"Fuel: {_car.CurrentFuel:F1}L | " +
               $"Time: {TimeRemaining:F0}s | " +
               $"Speed: {_car.CurrentSpeed} km/h";
    }

    /// <summary>
    /// Resets the race to its initial state.
    /// </summary>
    public void ResetRace()
    {
        _car.Reset();
        _totalTimeRemaining = 180.0;
        _raceStarted = false;
    }

    /// <summary>
    /// Gets total fuel used during the race.
    /// </summary>
    public double GetFuelUsed()
    {
        return _car.MaxFuelCapacity - _car.CurrentFuel;
    }

    /// <summary>
    /// Gets total time spent in the race.
    /// </summary>
    public double GetTimeSpent()
    {
        if (!_raceStarted)
            return 0;
        return (DateTime.UtcNow - _raceStartTime).TotalSeconds;
    }
}
