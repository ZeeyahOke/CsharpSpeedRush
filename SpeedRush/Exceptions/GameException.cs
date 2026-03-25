using System;

namespace SpeedRush.Exceptions;

/// <summary>
/// Represents an exception that occurs during game execution.
/// </summary>
public class GameException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GameException"/> class.
    /// </summary>
    public GameException() : base("An error occurred during the game.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GameException"/> class with a specified message.
    /// </summary>
    public GameException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GameException"/> class with a specified message and inner exception.
    /// </summary>
    public GameException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
