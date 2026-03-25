using Avalonia.Controls;
using SpeedRush.Models;
using SpeedRush.Managers;
using SpeedRush.Enums;

namespace SpeedRush;

/// <summary>
/// Dialog window that displays race completion statistics.
/// </summary>
public partial class GameEndDialog : Window
{
    public GameEndDialog()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Initializes the dialog with race statistics based on how the race ended.
    /// </summary>
    public void SetRaceStats(Car car, RaceManager manager, RaceEndReason reason)
    {
        // Set status message based on race end reason
        switch (reason)
        {
            case RaceEndReason.Completed:
                StatusMessage.Text = "Congratulations! You completed the race!";
                StatusMessage.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.LimeGreen);
                break;

            case RaceEndReason.OutOfFuel:
                StatusMessage.Text = "Out of Fuel! Race Ended.";
                StatusMessage.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.OrangeRed);
                break;

            case RaceEndReason.TimeExpired:
                StatusMessage.Text = "Time's Up! Race Ended.";
                StatusMessage.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.OrangeRed);
                break;

            default:
                StatusMessage.Text = "Race Ended!";
                StatusMessage.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Yellow);
                break;
        }

        // Set statistics
        LapsLabel.Text = $"{car.CurrentLap}/5";
        ProgressLabel.Text = $"{(car.LapProgress * 100):F0}%";
        FuelUsedLabel.Text = $"{manager.GetFuelUsed():F1}L";
        TimeSpentLabel.Text = $"{manager.GetTimeSpent():F0}s";
    }

    /// <summary>
    /// Handles the OK button click.
    /// </summary>
    private void OnOkClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }
}
