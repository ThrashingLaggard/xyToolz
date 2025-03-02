using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using System;
using xyToolz;
using xyToolz.Helper;

namespace xyAvalonia;

public partial class DebugConsole : Window
{
    public DebugConsole()
    {
        InitializeComponent();
    }

    private static string FormatMsgForLogs(string message) => xyLogFormatter.FormatMessageForLogging( message);
    private static string FormatExMsgForLogs(Exception exception) => xyLogFormatter.FormatExceptionDetails(exception,Microsoft.Extensions.Logging.LogLevel.Error);

    public void AppendText(string message)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            ConsoleOutput.Text += message + Environment.NewLine;
            ConsoleOutput.CaretIndex = ConsoleOutput.Text.Length;
            ScrollToEnd();
        });
    }

    private void ScrollToEnd()
    {
        var scrollViewer = this.FindControl<ScrollViewer>("ScrollViewer");
        if (scrollViewer != null)
        {
            scrollViewer.Offset = new Vector(0, scrollViewer.Extent.Height);
        }
    }

    private void BtnClear_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ConsoleOutput.Text = string.Empty; 
    }

}
