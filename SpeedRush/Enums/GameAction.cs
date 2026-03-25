namespace SpeedRush.Enums;

/// <summary>
/// Represents the possible actions a player can perform during a race.
/// </summary>
public enum GameAction
{
    /// <summary>
    /// Speed up the car - increases speed but consumes more fuel.
    /// </summary>
    SpeedUp,

    /// <summary>
    /// Maintain current speed - balanced fuel consumption.
    /// </summary>
    MaintainSpeed,

    /// <summary>
    /// Pit stop - refuel the car but lose time.
    /// </summary>
    PitStop
}
