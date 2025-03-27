using Avalonia.Controls;
using Avalonia.Threading;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using xyToolz.Helper.Logging;

namespace xyAvalonia.Services
{
    public class ConsoleTextBoxWriter : TextWriter
    {
        private readonly TextBox _output;

        public ConsoleTextBoxWriter(TextBox output)
        {
            _output = output;
        }

        public override void Write(string value)
        {
            Dispatcher.UIThread.Post
            (
                () =>
                    {
                        _output.Text += value + Environment.NewLine;
                        _output.CaretIndex = _output.Text.Length;
                        _output.ScrollToLine(_output.CaretIndex);
                    }
            );
        }






        private static String FormatMsgForLogs(String message) => xyLogFormatter.FormatMessageForLogging(message);
        private static String FormatExMsgForLogs(Exception exception) => xyLogFormatter.FormatExceptionDetails(exception, Microsoft.Extensions.Logging.LogLevel.Error);


        public override Encoding Encoding => Encoding.UTF8;
    }
}
