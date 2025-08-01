﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xyToolz.Logging.Interfaces;

namespace xyToolz.Logging.Helper.Formatters
{
    public class xyExceptionFormatter : IExceptionFormatter
    {
        public string FormatExceptionDetails(Exception ex, LogLevel level, string? callerName = null)
        {
            StringBuilder sb_Builder = new();

            sb_Builder.AppendLine($"{DateTime.Now} [{callerName ?? " / "}] [{level}] Exception Details:");

            sb_Builder.AppendLine($"Type: {ex.GetType().Name}");
            sb_Builder.AppendLine($"Source: {ex.Source}");
            sb_Builder.AppendLine($"TargetSite: {ex.TargetSite}");
            sb_Builder.AppendLine($"StackTrace: {ex.StackTrace}");
            sb_Builder.AppendLine($"Message: {ex.Message}");

            if (ex.Data != null && ex.Data.Count > 0)
            {
                sb_Builder.AppendLine("Custom Data:");
                foreach (var key in ex.Data.Keys)
                {
                    sb_Builder.AppendLine($"  {key}: {ex.Data[key]}");
                }
            }

            if (ex.InnerException != null)
            {
                sb_Builder.AppendLine("Inner Exception Details:");
                sb_Builder.AppendLine(FormatExceptionDetails(ex.InnerException, level));
            }

            return sb_Builder.ToString();
        }
    }
}
