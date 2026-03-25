namespace SpeedRush.Enums;

/// <summary>
/// Enum that specifies the reason why a race ended.
/// This allows us to distinguish between winning and losing conditions.
/// </summary>
public enum RaceEndReason
{
    /// <summary>
    /// Player successfully completed all 5 laps.
    /// </summary>
    Completed = 0,

    /// <summary>
    /// Player ran out of fuel before completing the race.
    /// </summary>
    OutOfFuel = 1,

    /// <summary>
    /// Player ran out of time before completing the race.
    /// </summary>
    TimeExpired = 2
}
