
# xyToolz Utility Library

xyToolz is a modular and highly reusable utility library designed for .NET applications. It provides a wide range of helper functions and classes to streamline everyday development tasks, improve code readability, maintainability, and enhance overall productivity.

## Features and Classes Overview

### General Utilities
- xy: Centralized helper methods used across the xyToolz library.
- xyColQol: Quality-of-life enhancements for collections.
- xyConversion: Utility methods for conversion between number systems.

### File and Directory Handling
- xyFiles: Simplifies file operations like reading, writing, and managing files.
- xyDirUtils: Streamlines directory-related operations and management.
- xyPath: Helps manage file and directory paths.

### Logging
- xyLog: Core logging class supporting structured and async logging.
- xyLogFormatter: Provides formatting capabilities for log messages.
- xyLogTargets: Defines various logging targets (file, console, etc.).
- xyLogArchiver: Manages archival and rotation of log files.

### Security and Cryptography
- xyHasher: Provides hashing methods, ideal for password hashing and data verification.
- xyRsa: Facilitates RSA encryption and decryption.
- xyDataProtector: Uses Windows DPAPI for encryption and secure data handling.

### JSON Handling
- xyJson: JSON serialization and deserialization helper.

### PDF Handling
- xyPdf: Tools for creating and manipulating PDF documents.

### Web Automation
- xyWebDriver: Simplifies automation tasks using Selenium WebDriver.




## Installation

Install via NuGet package manager:

dotnet add package xyToolz




## Usage Example

Here's a quick example of using xyLog to log messages:


using xyToolz;

xyLog.Log("Application started.");

try
{
    // Code that might throw an exception
}
catch (Exception ex)
{
    xyLog.ExLog(ex);
}


## Contributing

Feel free to contribute by submitting issues and pull requests. Ensure your code follows the existing style and conventions else I will rewrite it.

## License

xyToolz is licensed under the GPL-3.0. See the LICENSE file for more details.

