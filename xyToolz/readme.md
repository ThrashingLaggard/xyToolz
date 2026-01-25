# xyToolz

## Overview
xyToolz is a .NET utility library providing reusable helpers, abstractions,
and low-level building blocks for common application concerns.

The library is intended to be consumed by other projects and does not contain
application entry points or execution logic.

---

## Modules, Namespaces & Types

This document serves as a **complete reference inventory** of all namespaces
and declared types in the `xyToolz` project.

Detailed behavior and usage are documented in XML comments and additional
documentation files.

---

## Core

### xyToolz
General-purpose core helpers.

Types:
- xyConversion

---

## Database

### xyToolz.Database.Basix
Base models and shared database infrastructure.

Types:
- xyBaseModel
- xyBasicModel
- xyDBContext

---

### xyToolz.Database.Controllers
Controller-level abstractions for database operations.

Types:
- CrudController

---

### xyToolz.Database.Interfaces
Database-related contracts and extension interfaces.

Types:
- IBaseCrud  
- IBaseCrudExtensions  
- IEfCoreCrudExtensions  
- IExtendedCrud  

---

### xyToolz.Database.Repos
Repository implementations for database access.

Types:
- xyCrudRepo

---

### xyToolz.Database.Services
Service-layer helpers for database-related operations.

Types:
- xyCrudService

---

## Driver

### xyToolz.Driver
Browser and web driver abstractions.

Types:
- BrowserDriver
- xyWebDriver

---

## Filesystem

### xyToolz.Filesystem
Filesystem-related helpers and abstractions.

Types:
- xyFiles
- xyDirectory
- xyPath

---

## Fonts

### xyToolz.Fonts
Font and resource-related helpers.

Types:
- AutoResourceFontResolver

---

## Helper Interfaces

### xyToolz.Helper.Interfaces
Shared helper interfaces used across modules.

Types:
- IxyFiles
- IxyJson
- IxyDataProtector

---

## Logging

### xyToolz.Helper.Logging
High-level logging facade helpers.

Types:
- xyLog
- xyLogArchiver
- xyLogFormatter

---

### xyToolz.Logging.Helper
Core logging helper classes and factories.

Types:
- xyLoggerManager
- xyMessageFactory
- xyStaticMsgFactory

---

### xyToolz.Logging.Helper.Formatters
Default formatter implementations for log and exception entries.

Types:
- xyDefaultExceptionEntryFormatter
- xyDefaultExceptionFormatter
- xyDefaultLogEntryFormatter
- xyDefaultLogFormatter
- xyMessageFormatter

---

### xyToolz.Logging.Interfaces
Logging-related interfaces and formatter contracts.

Types:
- ILogging  
- IExceptionFormatter  
- IExceptionEntityFormatter  
- IMessageFormatter  
- IMessageEntityFormatter  

---

### xyToolz.Logging.Loggers
Concrete logger implementations.

Types:
- xyAsyncLogger
- xyConsoleLogger
- xyILoggerAdapter

---

### xyToolz.Logging.Models
Data models used by the logging subsystem.

Types:
- xyDefaultLogEntry
- xyExceptionEntry

---

## Lists

### xyToolz.Lists
List and collection utility helpers.

Types:
- xyList
- xyListWrapper

---

## PDF

### xyToolz.Pdf
PDF-related helpers and abstractions.

Types:
- xyPdf
- xyCard

---

## QOL (Quality of Life)

### xyToolz.QOL
Convenience and quality-of-life helpers.

Types:
- xy
- xyQol
- xyColQol

---

## Security

### xyToolz.Security
Security and cryptography-related helpers.

Types:
- xyHasher
- xyRsa
- xyDataProtector
- xySecurityDefaults

---

## Serialization

### xyToolz.Serialization
Serialization helpers.

Types:
- xyJson
- xyXml

---

## Requirements
- .NET (see project file for target framework details)
