using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Threading.Tasks;
using xyAvalonia.Services;
using xyAvalonia.Views;
using xyToolz;
using xyToolz.Helper.Logging;

namespace xyAvalonia;

// Neu hinzugefügter Delegate zur Unterstützung von zwei Parametern:
public delegate void LogMessageHandler(string message, string? callerName);
public delegate void ExLogMessageHandler(string message,  string? callerName);

public partial class DebugConsole : Window
{
    private bool IsException(string input)
    {
        if (!string.IsNullOrEmpty(input))
        {
            try
            {
                Exception exception = new Exception(input);
                return true;
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
            }
        }
            return false;
    }


    private TextWriter _consoleWriter;
    private event LogMessageHandler? LogMessageReceived;
    private event LogMessageHandler? ExLogMessageReceived;
    //public event Action<String> LogMessageReceived;
    public async void OnMessageReceived(String message, [CallerMemberName] String? callerName = null)
    {
        if (FormatMsgForLogs(message) is string content)
        {
            await _consoleWriter.WriteAsync(content);
            LogMessageReceived?.Invoke(content,callerName);
        }
    }

    public async void OnExMessageReceived(String message, [CallerMemberName] String? callerName = null)
    {
        if (!string.IsNullOrEmpty(message))
        {
            Exception exception = new Exception(message);
            if (FormatExMsgForLogs(exception) is string exContent)
            {
                await _consoleWriter.WriteAsync(message);
                ExLogMessageReceived?.Invoke(message, callerName);
            }
        }
    }

    public DebugConsole()
    {
        InitializeComponent();
        xyLog.LogMessageSent += OnMessageReceived;
        xyLog.ExLogMessageSent += OnExMessageReceived;
        _consoleWriter = new ConsoleTextBoxWriter(ConsoleOutput);
        Console.SetOut(_consoleWriter);
        Console.SetError(_consoleWriter);

    }
    public async Task WriteAsync(dynamic e)  
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            Exception exception = new(e.Data);
            if (FormatExMsgForLogs(exception) is   string exContent)
            {
                await _consoleWriter.WriteAsync(exContent);
            }
        }
    }

    public Task ReadProgramOutput(string programPath)
    {
        
        Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = programPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        if( (MainWindow)this.Owner! is MainWindow mainWindow)
        {
          mainWindow.Hide();
        }

        process.OutputDataReceived += async (sender, e) => await _consoleWriter.WriteAsync(FormatMsgForLogs(e.Data!));
        process.ErrorDataReceived += async (sender, e) => await WriteAsync(e);
        

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        return Task.CompletedTask;
        
        //mainWindow.Show();
    }

    [Obsolete]
    private async Task WriteIntoFile(String outputText)
    {
        // Beispiel: Speichern in eine Textdatei
        var saveFileDialog = new SaveFileDialog
        {
            Title = "Export Console Output",
            Filters = new List<FileDialogFilter>
        {
            new FileDialogFilter { Name = "Text Files", Extensions = { "txt" } },
            new FileDialogFilter { Name = "All Files", Extensions = { "*" } }
        }
        };

        var result = await saveFileDialog.ShowAsync(this);
        if (result != null)
        {
            File.WriteAllText(result, outputText);
        }

    }
    #region EvtHandler

    private async void Btn_StartMonitoringClick(object sender, RoutedEventArgs e)
    {
        if(txt_ProgramPath.Text is  string programPath)
        {
            await ReadProgramOutput(programPath);
        }
        else
        {
            await _consoleWriter.WriteAsync("Please enter valid program path");
        }

    }

    /// <summary>
    /// Submit yout input
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddButton_Click(Object sender, RoutedEventArgs e)
    {
        string input = InputTextBox.Text!;
        if (!string.IsNullOrWhiteSpace(input))
        {
            var formatted = xyLogFormatter.FormatMessageForLogging(input);
            _consoleWriter.Write(formatted);
            InputTextBox.Text = string.Empty; // Eingabefeld leeren
        }
    }


    /// <summary>
    /// Clear the Debug Console
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnClear_Click(Object sender, RoutedEventArgs e)
    {
        ConsoleOutput.Text = String.Empty;
        //xyQOL.Crash(9999);
    }

    [Obsolete]
    private void BtnExport_Click(Object sender, RoutedEventArgs e)
    {
        string? outputText = ConsoleOutput.Text;
        try
        {
            using var _ = WriteIntoFile(outputText!);
        }
        catch (Exception ex)
        {
            xyLog.ExLog(ex);
        }
    }



    /// <summary>
    /// Delete the placehoder when clicking on it
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void InputTextBox_GotFocus(Object sender, RoutedEventArgs e)
    {
        if (InputTextBox.Text == "Please enter your billing info here...")
        {
            InputTextBox.Text = string.Empty;
        }
    }

    /// <summary>
    /// Put the placeholder back in...
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void InputTextBox_LostFocus(Object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(InputTextBox.Text))
        {
            InputTextBox.Text = "Please enter your billing info here...";
        }
    }

    /// <summary>
    /// Delete the placehoder when clicking on it
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PathTextBox_GotFocus(Object sender, RoutedEventArgs e)
    {
        if (InputTextBox.Text == "Enter the path of the target program")
        {
            InputTextBox.Text = string.Empty;
        }
    }

    /// <summary>
    /// Put the placeholder back in...
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PathTextBox_LostFocus(Object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(InputTextBox.Text))
        {
            InputTextBox.Text = "Enter the path of the target program";
        }
    }

#endregion


    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    private static String FormatMsgForLogs(String message) => xyLogFormatter.FormatMessageForLogging(message);
    private static String FormatExMsgForLogs(Exception exception) => xyLogFormatter.FormatExceptionDetails(exception, Microsoft.Extensions.Logging.LogLevel.Error);



    /// <summary>
    /// Text zur GUI hinzufügen
    /// </summary>
    /// <param name="message"></param>
    public void AppendText(String message)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            ConsoleOutput.Text += message + Environment.NewLine;
            ConsoleOutput.CaretIndex = ConsoleOutput.Text.Length;
            ScrollToEnd();
        });
    }

    /// <summary>
    /// Immer die neuesten Ausgaben
    /// </summary>
    private void ScrollToEnd()
    {
        var scrollViewer = this.FindControl<ScrollViewer>("ScrollViewer");
        if (scrollViewer != null)
        {
            scrollViewer.Offset = new Vector(0, scrollViewer.Extent.Height);
        }
    }

}
