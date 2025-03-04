using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace xyToolz.Helper.Logging
{
    internal class xyLogArchiver
    {
            private readonly long _maxFileSize;

            public xyLogArchiver(long maxFileSize)
            {
                  _maxFileSize = maxFileSize;
            }

            public string FormatForArchive(string filePath)
            {
                  xyLog.Log("Formatting for archive:");
                  string archivePath = $"{filePath}_{DateTime.Now}.log";
                  xyLog.Log(archivePath);
                  return archivePath;
            }

            public void MoveLogToArchiveFileIfTooBig(string filepath_)
            {
                  if (File.Exists(filepath_) && new FileInfo(filepath_).Length > 0)
                  {
                        string newPath = FormatForArchive(filepath_);
                        try
                        {
                              File.Move(filepath_, newPath);
                              xyLog.Log("Moving the log to archive was successfull");
                        }
                        catch (IOException ioEx)
                        {
                              xyLog.ExLog(ioEx, LogLevel.Error);
                        }
                  }
            }

            public async Task MoveLogToArchiveFileIfTooBigAsync(string filepath_)
            {
                  if (File.Exists(filepath_) && new FileInfo(filepath_).Length > 0)
                  {
                        string newPath = "";
                        await Task.Run(() => newPath = FormatForArchive(filepath_));
                        try
                        {
                              await Task.Run(() => File.Move(filepath_, newPath));
                              xyLog.Log("Moving the log to archive was successfull");

                        }
                        catch (IOException ioEx)
                        {
                              xyLog.ExLog(ioEx);
                        }
                        catch (Exception Ex)
                        {
                              xyLog.ExLog(Ex);
                        }
                  }
            }
      }
}
