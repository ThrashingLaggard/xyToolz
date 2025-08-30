# Projektstruktur

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
  │   ├─xy.md
  │   ├─xyColQol.md
  │   ├─xyConversion.md
  │   ├─xyDataProtector.md
  │   ├─xyDirectoryHelper.md
  │   ├─xyFiles.md
  │   ├─xyJson.md
  │   ├─xyPdf.md
  │   ├─xyQol.md
  │   ├─xyRsa.md
  │   └─xyXml.md
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
  │   ├─xyToolz.Helper/
  │   ├─xyHasher.md
  │   └─xyPath.md
  │   ├─xyToolz.Helper.Interfaces/
  │   ├─IxyDataProtector.md
  │   ├─IxyFiles.md
  │   └─IxyJson.md
  │   ├─xyToolz.Helper.Logging/
  │   ├─xyLog.md
  │   ├─xyLogFormatter.md
  │   └─xyStaticMsgFactory.md
  │   ├─xyToolz.List/
  │   └─xyList_T_.md
  │   ├─xyToolz.Logging.Helper/
  │   ├─xyLoggerManager.md
  │   └─xyMessageFactory.md
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
  │   └─xyToolz.Logging.Models/
  │   ├─xyDefaultLogEntry.md
  │   └─xyExceptionEntry.md
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
  │ └─readme.md
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
  │ │ ├─xyExceptionEntry.cs
  │ │ └─xyXml.cs
  │ │ └─Static Logging Stuff/
  │ │ ├─xyLog.cs
  │ │ ├─xyLogArchiver.cs
  │ │ ├─xyLogFormatter.cs
  │ │ ├─xyLogTargets.cs
  │ │ └─xyStaticMsgFactory.cs
  │ ├─READMEs/
  │ ├─README_xyLog.md
  │ ├─README_xyRsa.md
  │ ├─xyDataProtectionHelper_README.md
  │ ├─xyFiles_Documentation.md
  │ └─xyJson_Documentation.md
  │ └─xUnit Test-Interfaces/
  │ ├─IxyDataProtector.cs
  │ ├─IxyFiles.cs
  │ └─IxyJson.cs
  ├─GlobalSuppressions.cs
  ├─PdfSharp_License.txt
  ├─xy.cs
  ├─xyCard.cs
  ├─xyColQol.cs
  ├─xyConversion.cs
  ├─xyDataProtector.cs
  ├─xyDirectoryHelper.cs
  ├─xyFiles.cs
  ├─xyHasher.cs
  ├─xyJson.cs
  ├─xyPath.cs
  ├─xyPdf.cs
  ├─xyQol.cs
  ├─xyRsa.cs
  ├─xyToolz.csproj
  └─xyWebDriver.cs
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
├─ReadMe.md
├─SECURITY.md
└─xyToolz.sln
