using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using SpeedRush.Models;
using SpeedRush.Managers;
using SpeedRush.Enums;
using System.Collections.Generic;
using System;

namespace SpeedRush;

/// <summary>
/// The main window for the Speed Rush racing game.
/// Handles UI interactions and updates the game state.
/// </summary>
public partial class MainWindow : Window
{
    private List<Car> _cars = new();
    private RaceManager? _raceManager;
    private Car? _selectedCar;
    private DispatcherTimer? _gameTimer;
    private bool _raceActive = false;

    public MainWindow()
    {
        InitializeComponent();
        InitializeCars();
        PopulateCarComboBox();
        SetupGameTimer();
    }

    /// <summary>
    /// Initializes the available cars with their attributes.
    /// </summary>
    private void InitializeCars()
    {
        _cars.Add(new Car(1, "Ferrari F1", 200, 0.8, 60.0));
        _cars.Add(new Car(2, "Tesla Model S", 150, 0.4, 80.0));
        _cars.Add(new Car(3, "Toyota Eco", 100, 0.2, 50.0));
    }

    /// <summary>
    /// Populates the car selection combo box.
    /// </summary>
    private void PopulateCarComboBox()
    {
        CarComboBox.Items.Clear();
        foreach (var car in _cars)
        {
            CarComboBox.Items.Add($"{car.Name} (Speed: {car.MaxSpeed}, Fuel: {car.MaxFuelCapacity}L)");
        }
        CarComboBox.SelectedIndex = 0;
    }

    /// <summary>
    /// Sets up the game update timer.
    /// </summary>
    private void SetupGameTimer()
    {
        _gameTimer = new DispatcherTimer();
        _gameTimer.Interval = TimeSpan.FromMilliseconds(100); // Update every 100ms
        _gameTimer.Tick += (s, e) => UpdateGameState();
    }

    /// <summary>
    /// Updates the game state and checks for race end conditions.
    /// </summary>
    private void UpdateGameState()
    {
        if (!_raceActive || _raceManager == null || _selectedCar == null)
            return;

        // Update UI
        UpdateUI();

        // Check race end conditions
        if (_selectedCar.HasFinished && _selectedCar.CurrentLap >= 5)
        {
            EndRace(RaceEndReason.Completed); // Won
        }
        else if (_raceManager.TimeRemaining <= 0)
        {
            _selectedCar.HasFinished = true;
            EndRace(RaceEndReason.TimeExpired); // Time's up
        }
        else if (!_selectedCar.HasFuel())
        {
            _selectedCar.HasFinished = true;
            EndRace(RaceEndReason.OutOfFuel); // Out of fuel
        }
    }

    /// <summary>
    /// Ends the race and shows the result dialog.
    /// </summary>
    private void EndRace(RaceEndReason reason)
    {
        _raceActive = false;
        _gameTimer?.Stop();

        if (_raceManager != null && _selectedCar != null)
        {
            var dialog = new GameEndDialog();
            dialog.SetRaceStats(_selectedCar, _raceManager, reason);
            dialog.ShowDialog(this);
        }
    }

    /// <summary>
    /// Handles the Start Race button click event.
    /// </summary>
    private void OnStartRaceClick(object? sender, RoutedEventArgs e)
    {
        if (CarComboBox.SelectedIndex < 0)
        {
            MessageLabel.Text = "Please select a car first!";
            return;
        }

        _selectedCar = _cars[CarComboBox.SelectedIndex];
        _selectedCar.Reset();
        _raceManager = new RaceManager(_selectedCar, 180.0);
        _raceManager.StartRaceTimer();

        _raceActive = true;
        _gameTimer?.Start();

        UpdateUI();
        MessageLabel.Text = "Race started! Use the buttons to control your car.";
    }

    /// <summary>
    /// Handles the Speed Up button click event.
    /// </summary>
    private void OnSpeedUpClick(object? sender, RoutedEventArgs e)
    {
        if (_raceManager == null || !_raceManager.IsRaceActive)
        {
            MessageLabel.Text = "Race is not active!";
            return;
        }

        if (_raceManager.ExecuteAction(GameAction.SpeedUp))
        {
            UpdateUI();
            MessageLabel.Text = "Accelerating! Speed increased, fuel consumed.";
        }
        else
        {
            MessageLabel.Text = "Cannot speed up - insufficient fuel!";
        }
    }

    /// <summary>
    /// Handles the Maintain Speed button click event.
    /// </summary>
    private void OnMaintainSpeedClick(object? sender, RoutedEventArgs e)
    {
        if (_raceManager == null || !_raceManager.IsRaceActive)
        {
            MessageLabel.Text = "Race is not active!";
            return;
        }

        if (_raceManager.ExecuteAction(GameAction.MaintainSpeed))
        {
            UpdateUI();
            MessageLabel.Text = "Maintaining speed - steady pace.";
        }
        else
        {
            MessageLabel.Text = "Cannot maintain speed - insufficient fuel!";
        }
    }

    /// <summary>
    /// Handles the Pit Stop button click event.
    /// </summary>
    private void OnPitStopClick(object? sender, RoutedEventArgs e)
    {
        if (_raceManager == null || !_raceManager.IsRaceActive)
        {
            MessageLabel.Text = "Race is not active!";
            return;
        }

        _raceManager.ExecuteAction(GameAction.PitStop);
        UpdateUI();
        MessageLabel.Text = "Pit stop! Car refueled.";
    }

    /// <summary>
    /// Handles the Reset button click event.
    /// </summary>
    private void OnResetClick(object? sender, RoutedEventArgs e)
    {
        _raceActive = false;
        _gameTimer?.Stop();

        if (_raceManager != null)
        {
            _raceManager.ResetRace();
            UpdateUI();
            MessageLabel.Text = "Race reset. Select a car and start again.";
        }
    }

    /// <summary>
    /// Updates the UI with the current game state.
    /// </summary>
    private void UpdateUI()
    {
        if (_selectedCar == null || _raceManager == null)
            return;

        // Update lap display
        LapLabel.Text = $"Lap: {_selectedCar.CurrentLap}/5";
        LapProgressBar.Value = _selectedCar.LapProgress * 100;

        // Update fuel display
        FuelLabel.Text = $"Fuel: {_selectedCar.CurrentFuel:F1}L / {_selectedCar.MaxFuelCapacity:F1}L";
        FuelProgressBar.Value = (_selectedCar.CurrentFuel / _selectedCar.MaxFuelCapacity) * 100;

        // Update time display
        TimeLabel.Text = $"Time: {_raceManager.TimeRemaining:F0}s";
        TimeProgressBar.Value = (_raceManager.TimeRemaining / 180.0) * 100;

        // Update speed display
        SpeedLabel.Text = $"Speed: {_selectedCar.CurrentSpeed} km/h";

        // Update status
        StatusLabel.Text = _raceManager.GetRaceStatus();
    }
}
