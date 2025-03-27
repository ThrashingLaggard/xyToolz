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

public partial class DebugConsole : Window
{

    private TextWriter _consoleWriter;

    public event Action<String> LogMessageReceived;
    public void OnMessageReceived(String message, [CallerMemberName] String? callerName = null)
    {
        LogMessageReceived?.Invoke((message));
    }



    public DebugConsole()
    {
        InitializeComponent();
        xyLog.LogMessageSent += LogMessageReceived;
        _consoleWriter = new ConsoleTextBoxWriter(ConsoleOutput);
        Console.SetOut(_consoleWriter);
        Console.SetError(_consoleWriter);

    }
     
    

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


    public async Task ReadProgramOutput(string programPath)
    {
        
        Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = programPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = true,
                CreateNoWindow = true,
                Verb = "runas"
            }
        };

        if( (MainWindow)this.Owner is MainWindow mainWindow)
        {
            mainWindow.Hide();
        }

        process.OutputDataReceived += async (sender, e) => await _consoleWriter.WriteAsync(FormatMsgForLogs(e.Data!));
        process.ErrorDataReceived += async (sender, e) => 
         {
             if (!string.IsNullOrEmpty(e.Data))
             {
                 Exception exception = new Exception(e.Data);
                 if (FormatExMsgForLogs(exception) is   string exContent)
                 {
                    await _consoleWriter.WriteAsync(exContent);
                     
                 }
             }
         };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.WaitForExitAsync();
        
        //mainWindow.Show();
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
