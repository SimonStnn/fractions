using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Navigation;
using FractionsLibrary;
using System.Threading.Tasks;
using System.Threading;

namespace WPFFractionCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum Operation
        {
            ADD = '+',
            SUBTRACT = '-',
            MULTIPLY = '*',
            DIVIDE = '/',
            EQUALS = '=',
            RECIPROCAL = 'R',
            INVERT = 'I',
            SIMPLIFY = 'S',
            NEW = 'N',
            REMOVE = 'X',
        }

        private static readonly List<Fraction> fractions = new();
        private static readonly List<Operation> operations = new();
        private static int activeOperationIndex = -1;
        private static Fraction result = new();
        private static Operation? action;

        private const int MIN_FRACTIONS = 1;
        private const int MAX_FRACTIONS = 80;

        private Task? messageTask;
        private CancellationTokenSource? cts;

        public MainWindow()
        {
            fractions.Add(new Fraction(-5, 1));
            operations.Add(Operation.ADD);
            fractions.Add(new Fraction(3, 1));
            operations.Add(Operation.EQUALS);
            InitializeComponent();
        }

        public async void ShowMessage(string message, int delay = 3000, SolidColorBrush? brush = null)
        {
            // Cancel previous message task and create a new one
            cts?.Cancel();
            cts = new CancellationTokenSource();
            messageTask = Task.Delay(delay, cts.Token);
            try
            {
                // Set message and color
                brush ??= Brushes.Black;
                lblInfo.Foreground = brush;
                lblInfo.Content = message;

                // Wait for the delay to finish and clear the message
                await messageTask;
                messageTask = null;
                lblInfo.Content = "";
            }
            catch (TaskCanceledException)
            { }
        }

        public void AddFraction(Operation operation, Fraction fraction)
        {
            fractions.Add(fraction);
            operations.Remove(Operation.EQUALS);
            operations.Add(operation);
            operations.Add(Operation.EQUALS);
            RenderFractions();
        }

        private static Fraction CalculateResult()
        {
            result = fractions[0];
            for (int i = 1; i < fractions.Count; i++)
            {
                Operation operation = operations[i - 1];
                Fraction fraction = fractions[i];
                if (operation.Equals(Operation.EQUALS)) break;

                switch (operation)
                {
                    case Operation.ADD:
                        result = result.Add(fraction);
                        break;
                    case Operation.SUBTRACT:
                        result = result.Subtract(fraction);
                        break;
                    case Operation.MULTIPLY:
                        result = result.Multiply(fraction);
                        break;
                    case Operation.DIVIDE:
                        result = result.Divide(fraction);
                        break;
                }
            }
            result = result.Simplify();
            return result;
        }

        private void RenderFractions()
        {
            // Remove preview items
            MainStackPannel.Children.Clear();

            // Add fractions
            for (int i = 0; i < fractions.Count; i++)
            {
                Fraction fraction = fractions[i];
                bool numeratorZeroAllowed = i > 0 ? operations[i - 1] != Operation.DIVIDE : true;
                MainStackPannel.Children.Add(RenderFraction(fraction, i, numeratorZeroAllowed: numeratorZeroAllowed));

                if (operations.Count > i)
                {
                    Button operation = RenderOperation(operations[i], position: i);
                    MainStackPannel.Children.Add(operation);
                    if (i == activeOperationIndex)
                    {
                        operation.Background = Brushes.Wheat;
                    }
                }
            }
        }

        private void RenderOperations()
        {
            // Remove preview items
            OperationsStackPanel.Children.Clear();

            if (activeOperationIndex < 0)
            {
                //Add operations
                OperationsStackPanel.Children.Add(RenderOperation(Operation.RECIPROCAL, -1));
                OperationsStackPanel.Children.Add(RenderOperation(Operation.INVERT, -2));
                OperationsStackPanel.Children.Add(RenderOperation(Operation.SIMPLIFY, -3));
                OperationsStackPanel.Children.Add(RenderOperation(Operation.REMOVE, -4));
                OperationsStackPanel.Children.Add(RenderOperation(Operation.NEW, -5));
            }
            else
            {
                OperationsStackPanel.Children.Add(RenderOperation(Operation.ADD, -1));
                OperationsStackPanel.Children.Add(RenderOperation(Operation.SUBTRACT, -2));
                OperationsStackPanel.Children.Add(RenderOperation(Operation.MULTIPLY, -3));
                OperationsStackPanel.Children.Add(RenderOperation(Operation.DIVIDE, -1));
            }
        }

        private void RenderResult(bool removeLast = true)
        {
            CalculateResult();
            // Remove Last Child
            if (removeLast)
                MainStackPannel.Children.RemoveAt(MainStackPannel.Children.Count - 1);
            MainStackPannel.Children.Add(RenderFraction(result, IsEnabled: false));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Remove preview items
            OperationsStackPanel.Children.Clear();

            RenderOperations();
            RenderFractions();
            RenderResult(removeLast: false);
            ShowMessage("Welcome to Fraction Calculator", 5000, Brushes.Green);
        }

        private static readonly Regex num_regex = new("[^0-9-]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return num_regex.IsMatch(text);
        }
        private static void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowed(e.Text);
        }

        public Grid RenderFraction(Fraction fraction, int fractionIndex = -1, bool IsEnabled = true, bool numeratorZeroAllowed = true)
        {
            Grid mainGrid = new();
            Border border = new()
            {
                Padding = new Thickness(5),
            };
            // Create elements
            StackPanel stackPanel = new()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                IsHitTestVisible = IsEnabled,
            };
            TextBox textBoxNumerator = new()
            {
                Width = 50,
                Height = 50,
                FontSize = 18,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                MaxLength = 6,
                MaxLines = 1,
                BorderBrush = Brushes.LightGray,
                Text = fraction.Numerator.ToString(),
            };
            TextBox textBoxDenominator = new()
            {
                Width = 50,
                Height = 50,
                FontSize = 18,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                MaxLength = 6,
                MaxLines = 1,
                BorderBrush = Brushes.LightGray,
                Text = fraction.Denominator.ToString(),
            };
            Grid separator = new()
            {
                Height = 2,
                Margin = new Thickness(1, 8, 1, 8),
                Background = Brushes.Gray,
            };

            // Handle Preview Text Input events to block non-numeric characters
            textBoxNumerator.PreviewTextInput += TextBox_PreviewTextInput;
            textBoxDenominator.PreviewTextInput += TextBox_PreviewTextInput;

            // Handle Text Changed events
            textBoxNumerator.TextChanged += (sender, e) =>
            {
                try
                {
                    fraction.Numerator = Convert.ToInt32(textBoxNumerator.Text);
                    textBoxNumerator.BorderBrush = Brushes.LightGray;
                }
                catch (FormatException)
                {
                    fraction.Numerator = 0;
                    textBoxNumerator.BorderBrush = Brushes.Red;
                }

                if (!numeratorZeroAllowed && fraction.Numerator == 0)
                {
                    fraction.Numerator = 1;
                    textBoxNumerator.BorderBrush = Brushes.Red;
                }

                RenderResult();
            };
            textBoxDenominator.TextChanged += (sender, e) =>
            {
                try
                {
                    fraction.Denominator = Convert.ToInt32(textBoxDenominator.Text);
                    textBoxDenominator.BorderBrush = Brushes.LightGray;
                }
                catch (Exception ex) when (ex is FormatException || ex is DivideByZeroException)
                {
                    fraction.Denominator = 1;
                    textBoxDenominator.BorderBrush = Brushes.Red;
                }
                RenderResult();
            };

            // Change border if there is an action
            border.MouseEnter += (sender, e) =>
            {
                if (action != null)
                {
                    border.Background = Brushes.Wheat;
                }
            };
            border.MouseLeave += (sender, e) =>
            {
                border.Background = null;
            };

            // Handle click event
            border.MouseLeftButtonDown += (sender, e) =>
            {
                if (action == null && fractionIndex != -1 && fractionIndex < fractions.Count) return;
                switch (action)
                {
                    case Operation.RECIPROCAL:
                        fractions[fractionIndex] = fraction.Reciprocal();
                        break;
                    case Operation.INVERT:
                        fractions[fractionIndex] = fraction.Invert();
                        break;
                    case Operation.SIMPLIFY:
                        fractions[fractionIndex] = fraction.Simplify();
                        break;
                }
                action = null;
                RenderOperations();
                RenderFractions();
                RenderResult(removeLast: false);
            };

            // Add elements to main grid
            mainGrid.Children.Add(border);
            border.Child = stackPanel;
            stackPanel.Children.Add(textBoxNumerator);
            stackPanel.Children.Add(separator);
            stackPanel.Children.Add(textBoxDenominator);

            return mainGrid;
        }

        public Button RenderOperation(Operation operation, int position)
        {
            Brush activeBackground = Brushes.Wheat;
            Brush inactiveBackground = Brushes.LightGray;

            string operationText = operation switch
            {
                Operation.RECIPROCAL => "1/x",
                Operation.INVERT => "-x",
                _ => ((char)operation).ToString(),
            };

            Button button = new()
            {
                Width = 28,
                Height = 28,
                FontSize = 16,
                Padding = (((char)operation <= '/' && (char)operation >= '*') || (char)operation == '=') ?
                    new Thickness(0, -2, 0, 0) :
                    new Thickness(0),
                FontWeight = FontWeights.Bold,
                VerticalContentAlignment = VerticalAlignment.Center,
                Content = operationText,
                ToolTip = operation.ToString(),
                Background = action == operation ? activeBackground : inactiveBackground,
                // Don't register events to the equals button
                IsHitTestVisible = operation != Operation.EQUALS,
                // Disable remove button if there is only one fraction or new button if there are already 8 fractions
                IsEnabled = !(operation == Operation.REMOVE && fractions.Count < (MIN_FRACTIONS + 1))
                    && !(operation == Operation.NEW && fractions.Count >= MAX_FRACTIONS),
            };
            button.Click += (object sender, RoutedEventArgs e) =>
            {
                // Toggle background color
                foreach (Button btn in OperationsStackPanel.Children)
                {
                    btn.Background = inactiveBackground;
                }
                // If the operation is in the operationstackpanel
                if (position >= 0)
                {
                    button.Background = activeBackground;
                    activeOperationIndex = position;
                }
                else if (activeOperationIndex >= 0)
                {
                    // Replace operation at activeOperationIndex with the clicked operation
                    operations[activeOperationIndex] = operation;
                    activeOperationIndex = -1;
                    ShowMessage($"Replaced operation with: {operationText}");
                }
                else if (position < 0 && activeOperationIndex < 0)
                {
                    if (action == null && (operation == Operation.RECIPROCAL || operation == Operation.INVERT || operation == Operation.SIMPLIFY))
                    {
                        ShowMessage($"Click a fraction to {operation.ToString().ToLower()}.");
                        action = operation;
                    }
                    else
                    {
                        // Toggle action
                        action = null;
                    }
                }

                switch (operation)
                {
                    case Operation.RECIPROCAL:

                        break;
                    case Operation.INVERT:

                        break;
                    case Operation.SIMPLIFY:

                        break;
                    case Operation.NEW:
                        if (fractions.Count > MAX_FRACTIONS) break;
                        operations.Remove(Operation.EQUALS);
                        operations.Add(Operation.ADD);
                        fractions.Add(new Fraction());
                        operations.Add(Operation.EQUALS);
                        break;
                    case Operation.REMOVE:
                        if (fractions.Count < (MIN_FRACTIONS + 1)) break;
                        fractions.RemoveAt(fractions.Count - 1);
                        operations.RemoveAt(operations.Count - 2);
                        break;
                }

                RenderOperations();
                RenderFractions();
                RenderResult(removeLast: false);
            };
            return button;
        }

        private void BtnAbout_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AboutWindow aboutWindow = new();
            aboutWindow.ShowDialog();
            this.Show();
        }
    }
}