# Project structure 

└─xyToolz/
  ├─.github/
  │ └─workflows/
  │ ├─ci-xyToolz - Backup.yml
  │ └─ci-xyToolz.yml
  ├─docs/
  │ └─api/
  │   ├─_global_/
  │   └─Program.md
  │   ├─xyToolz/
  │   └─xyConversion.md
  │   ├─xyToolz_Exec.Properties/
  │   └─Resources.md
  │   ├─xyToolz.Database.Basix/
  │   ├─xyBaseModel.md
  │   ├─xyBasicModel.md
  │   └─xyDBContext.md
  │   ├─xyToolz.Database.Interfaces/
  │   ├─IBaseCrud_T_.md
  │   ├─IBaseCrudExtensions_T_.md
  │   ├─IEfCoreCrudExtensions_T_.md
  │   └─IExtendedCrud_T_.md
  │   ├─xyToolz.Database.Repos/
  │   └─xyCrudRepo_T_.md
  │   ├─xyToolz.Database.Services/
  │   └─xyCrudService_T_.md
  │   ├─xyToolz.Driver/
  │   ├─xyWebDriver.BrowserDriver.md
  │   └─xyWebDriver.md
  │   ├─xyToolz.Filesystem/
  │   ├─xyDirectoryHelper.md
  │   ├─xyFiles.md
  │   └─xyPath.md
  │   ├─xyToolz.Helper.Interfaces/
  │   ├─IxyDataProtector.md
  │   ├─IxyFiles.md
  │   └─IxyJson.md
  │   ├─xyToolz.Helper.Logging/
  │   ├─xyLog.md
  │   ├─xyLogArchiver.md
  │   ├─xyLogFormatter.md
  │   └─xyLogTargets.md
  │   ├─xyToolz.List/
  │   ├─xyList_T_.md
  │   └─xyListWrapper.md
  │   ├─xyToolz.Logging.Helper/
  │   ├─xyLoggerManager.md
  │   ├─xyMessageFactory.md
  │   └─xyStaticMsgFactory.md
  │   ├─xyToolz.Logging.Helper.Formatters/
  │   ├─xyDefaultExceptionEntryFormatter.md
  │   ├─xyDefaultExceptionFormatter.md
  │   ├─xyDefaultLogEntryFormatter_T_.md
  │   ├─xyDefaultLogFormatter.md
  │   └─xyMessageFormatter.md
  │   ├─xyToolz.Logging.Interfaces/
  │   ├─IExceptionEntityFormatter.md
  │   ├─IExceptionFormatter.md
  │   ├─ILogging.md
  │   ├─IMessageEntityFormatter_T_.md
  │   └─IMessageFormatter.md
  │   ├─xyToolz.Logging.Loggers/
  │   ├─xyAsyncLogger_T_.md
  │   ├─xyConsoleLogger_T_.md
  │   └─xyILoggerAdapter_T_.md
  │   ├─xyToolz.Logging.Models/
  │   ├─xyDefaultLogEntry.md
  │   └─xyExceptionEntry.md
  │   ├─xyToolz.PDF/
  │   ├─xyCard.md
  │   └─xyPdf.md
  │   ├─xyToolz.QOL/
  │   ├─xy.md
  │   ├─xyColQol.md
  │   └─xyQol.md
  │   ├─xyToolz.Security/
  │   ├─xyDataProtector.md
  │   ├─xyHasher.md
  │   └─xyRsa.md
  │   └─xyToolz.Serialization/
  │   ├─xyJson.md
  │   └─xyXml.md
  │ └─INDEX.md
  ├─source/
  ├─xyToolz/
  │ ├─Database/
  │ │ ├─Controllers/
  │ │ └─xyCrudController.cs
  │ │ ├─Interfaces/
  │ │ ├─IBaseCrud.cs
  │ │ ├─IBaseCrudExtensions.cs
  │ │ ├─IEfCoreCrudExtensions.cs
  │ │ └─IExtendedCrud.cs
  │ │ ├─Repos/
  │ │ └─xyCrudRepo.cs
  │ │ └─Services/
  │ │ └─xyCrudService.cs
  │ ├─xyBaseModel.cs
  │ └─xyDBContext.cs
  │ ├─Docs/
  │ ├─PdfSharp_License.txt
  │ ├─readme.md
  │ ├─ShellCommands.txt
  │ └─TODO.txt
  │ ├─Driver/
  │ └─xyWebDriver.cs
  │ ├─Filesystem/
  │ ├─xyDirectoryHelper.cs
  │ ├─xyFiles.cs
  │ └─xyPath.cs
  │ ├─List/
  │ ├─xyList.cs
  │ └─xyListWrapper.cs
  │ ├─Logging/
  │ │ ├─Helper/
  │ │ │ └─Formatters/
  │ │ │ ├─xyDefaultExceptionEntryFormatter.cs
  │ │ │ ├─xyDefaultExceptionFormatter.cs
  │ │ │ ├─xyDefaultLogEntryFormatter.cs
  │ │ │ ├─xyDefaultLogFormatter.cs
  │ │ │ └─xyMessageFormatter.cs
  │ │ ├─xyLoggerManager.cs
  │ │ └─xyMessageFactory.cs
  │ │ ├─Interfaces/
  │ │ ├─IExceptionEntityFormatter.cs
  │ │ ├─IExceptionFormatter.cs
  │ │ ├─ILogging.cs
  │ │ ├─IMessageEntityFormatter.cs
  │ │ └─IMessageFormatter.cs
  │ │ ├─Loggers/
  │ │ ├─xyAsyncLogger.cs
  │ │ ├─xyConsoleLogger.cs
  │ │ └─xyILoggerAdapter.cs
  │ │ ├─Models/
  │ │ ├─xyDefaultLogEntry.cs
  │ │ └─xyExceptionEntry.cs
  │ │ └─Static Logging Stuff/
  │ │ ├─xyLog.cs
  │ │ ├─xyLogArchiver.cs
  │ │ ├─xyLogFormatter.cs
  │ │ ├─xyLogTargets.cs
  │ │ └─xyStaticMsgFactory.cs
  │ ├─PDF/
  │ ├─xyCard.cs
  │ └─xyPdf.cs
  │ ├─QOL/
  │ ├─xy.cs
  │ ├─xyColQol.cs
  │ └─xyQol.cs
  │ ├─READMEs/
  │ ├─README_xyLog.md
  │ ├─README_xyRsa.md
  │ ├─xyDataProtectionHelper_README.md
  │ ├─xyFiles_Documentation.md
  │ └─xyJson_Documentation.md
  │ ├─Security/
  │ ├─xyDataProtector.cs
  │ ├─xyHasher.cs
  │ └─xyRsa.cs
  │ ├─Serialization/
  │ ├─xyJson.cs
  │ └─xyXml.cs
  │ └─xUnit Test-Interfaces/
  │ ├─IxyDataProtector.cs
  │ ├─IxyFiles.cs
  │ └─IxyJson.cs
  ├─GlobalSuppressions.cs
  ├─xyConversion.cs
  └─xyToolz.csproj
  └─xyToolz_Exec/
    └─Properties/
    ├─Resources.Designer.cs
    └─Resources.resx
  ├─Program.cs
  └─xyToolz_Exec.csproj
├─.dockerignore
├─.editorconfig
├─.gitattributes
├─.gitignore
├─CHANGELOG.md
├─ci-template-xyProjects.yml
├─CONTRIBUTIONS.md
├─generate-workflow.sh
├─LICENSE.txt
├─SECURITY.md
└─xyToolz.sln
