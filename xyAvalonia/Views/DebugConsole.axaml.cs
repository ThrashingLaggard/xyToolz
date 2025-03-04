using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using xyToolz;
using xyToolz.Helper.Logging;

namespace xyAvalonia;

public partial class DebugConsole : Window
{
      public event Action<String> LogMessageReceived;

      public void OnMessageReceived ( String message , [CallerMemberName] String? callerName = null )
      {
            LogMessageReceived?.Invoke (( message ));
      }
      public DebugConsole ( )
      {
            xyLog.LogMessageSent +=  LogMessageReceived ;
          
            InitializeComponent ( );
      }

      private static String FormatMsgForLogs ( String message ) => xyLogFormatter.FormatMessageForLogging (message);
      private static String FormatExMsgForLogs ( Exception exception ) => xyLogFormatter.FormatExceptionDetails (exception , Microsoft.Extensions.Logging.LogLevel.Error);

      /// <summary>
      /// Text zur GUI hinzufügen
      /// </summary>
      /// <param name="message"></param>
      public void AppendText ( String message )
      {
            Dispatcher.UIThread.InvokeAsync (( ) =>
            {
                  ConsoleOutput.Text += message + Environment.NewLine;
                  ConsoleOutput.CaretIndex = ConsoleOutput.Text.Length;
                  ScrollToEnd ( );
            });
      }

      /// <summary>
      /// Immer die neuesten Ausgaben
      /// </summary>
      private void ScrollToEnd ( )
      {
            var scrollViewer = this.FindControl<ScrollViewer> ("ScrollViewer");
            if (scrollViewer != null)
            {
                  scrollViewer.Offset = new Vector (0 , scrollViewer.Extent.Height);
            }
      }

      /// <summary>
      /// Submit yout input
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void AddButton_Click ( Object sender , RoutedEventArgs e )
      {
            string input = InputTextBox.Text!;
            if (!string.IsNullOrWhiteSpace (input))
            {
                  var formatted = xyLogFormatter.FormatMessageForLogging (input);
                  AppendText (formatted);
                  InputTextBox.Text = string.Empty; // Eingabefeld leeren
            }
      }

      /// <summary>
      /// Clear the Debug Console
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void BtnClear_Click ( Object sender , RoutedEventArgs e )
      {
            ConsoleOutput.Text = String.Empty;
            //xyQOL.Crash(9999);
      }

      private void BtnExport_Click ( Object sender , RoutedEventArgs e )
      {
            string? outputText = ConsoleOutput.Text;
            try
            {
                  using var _ = WriteIntoFile (outputText!);
            }
            catch (Exception ex)
            {
                  xyLog.ExLog (ex);
            }


      }
      private async Task WriteIntoFile ( String outputText )
      {
            // Beispiel: Speichern in eine Textdatei
            var saveFileDialog = new SaveFileDialog
            {
                  Title = "Export Console Output" ,
                  Filters = new List<FileDialogFilter>
        {
            new FileDialogFilter { Name = "Text Files", Extensions = { "txt" } },
            new FileDialogFilter { Name = "All Files", Extensions = { "*" } }
        }
            };

            var result = await saveFileDialog.ShowAsync (this);
            if (result != null)
            {
                  File.WriteAllText (result , outputText);
            }

      }

      /// <summary>
      /// Delete the placehoder when clicking on it
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void InputTextBox_GotFocus ( Object sender , RoutedEventArgs e )
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
      private void InputTextBox_LostFocus ( Object sender , RoutedEventArgs e )
      {
            if (string.IsNullOrWhiteSpace (InputTextBox.Text))
            {
                  InputTextBox.Text = "Please enter your billing info here...";
            }
      }




}
