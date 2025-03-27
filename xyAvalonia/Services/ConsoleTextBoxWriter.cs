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
                        UpdateCaretAndScroll();
                    }
            );
        }

        public override async Task WriteAsync(string value)
        {
            await Dispatcher.UIThread.InvokeAsync
            (
                () =>
                {
                    _output.Text += value + Environment.NewLine;
                    UpdateCaretAndScroll();
                }
            );
        }

        private void UpdateCaretAndScroll()
        {
            _output.CaretIndex = _output.Text.Length;
            var lineIndex = _output.Text.Length - 1;
            _output.ScrollToLine(lineIndex);
        }

        public override Encoding Encoding => Encoding.UTF8;
    }
}
