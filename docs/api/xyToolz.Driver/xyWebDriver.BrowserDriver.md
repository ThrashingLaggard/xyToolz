# class xyWebDriver.BrowserDriver

Namespace: `xyToolz.Driver`  
Visibility: `public`  
Base/Interfaces:`IDisposable`  
Source: `xyToolz\Driver\xyWebDriver.cs`

## Description:

(No XML‑Summary )

## Constructors

- `BrowserDriver(int timeoutSeconds = 10)` — `public`
  
  (No XML‑Summary )

## Methods

- `Task CheckCheckBox(By by)` — `public async`
  
  /// Check
            ///
- `Task EnterText(By by, string text)` — `public async`
  
  /// Input text
            ///
- `Task ExecuteScript(string script, params object[] args)` — `public static async`
  
  /// Execute Script
            ///
- `Task NavigateTo(string url)` — `public async`
  
  (No XML‑Summary )
- `Task SelectDropdownOption(By by, string target, int selector)` — `public async`
  
  /// Fix
            ///
- `Task TakeScreenshot(string path)` — `public async`
  
  /// Screenshot
            ///
- `Task UncheckCheckbox(By by)` — `public async`
  
  /// Uncheck
            ///
- `Task<bool> Click(By by)` — `public async`
  
  /// Click
            ///
- `Task<IWebElement> FindElement(By by)` — `public async`
  
  (No XML‑Summary )
- `Task<ReadOnlyCollection<IWebElement>> FindElements(By by)` — `public async`
  
  (No XML‑Summary )
- `Task<T> ExecuteScript<T>(string script, params object[] args)` — `public static async`
  
  /// Execute script but generic
            ///
- `void Dispose()` — `public`
  
  /// Dispose
            ///

## Fields

- `IWebDriver? _driver` — `private static`
  
  (No XML‑Summary )
- `WebDriverWait _wait` — `private readonly`
  
  (No XML‑Summary )

