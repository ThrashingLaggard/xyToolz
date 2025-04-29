# 📘 xyLog – Custom Logging Utility

`xyLog` ist eine zentrale Logging-Komponente zur Verwaltung und Ausgabe von Nachrichten, Fehlern und strukturierten Daten in C#-Anwendungen. Sie unterstützt synchrone und asynchrone Protokollierung, Ereignisbindung und Exception-Formatierung.

---

## ✅ Features

- ✍️ **Synchronous Logging**  
  Direkte Ausgabe von Log-Texten und Exceptions in Konsole und Logfile.

- 🔄 **Asynchronous Logging**  
  Nicht-blockierende Log-Ausgaben mit `Task`.

- 📢 **Event-basiertes Logging**  
  Ermöglicht Weiterverarbeitung via `LogMessageSent` und `ExLogMessageSent`.

- 📑 **Exception-Darstellung als Klartext und JSON**  
  Ideal für Debugging oder maschinenlesbare Protokolle.

- 🧠 **Metadaten**  
  Automatische Anreicherung mit Timestamps, LogLevel, Aufrufername (via `CallerMemberName`).

---

## 🧩 Methodenübersicht

| Methode            | Beschreibung                                                        |
|--------------------|----------------------------------------------------------------------|
| `Log(...)`         | Normale Nachricht synchron loggen                                    |
| `AsxLog(...)`      | Normale Nachricht asynchron loggen                                   |
| `ExLog(...)`       | Exception synchron loggen                                            |
| `AsxExLog(...)`    | Exception asynchron loggen                                           |
| `JsonExLog(...)`   | Exception als JSON (synchron)                                        |
| `JsonAsxExLog(...)`| Exception als JSON (asynchron)                                      |

---

## 💡 Beispielverwendung

```csharp
try
{
    throw new InvalidOperationException("Something went wrong!");
}
catch (Exception ex)
{
    xyLog.ExLog(ex);
}
```

Oder asynchron:

```csharp
await xyLog.AsxLog("Application started.");
await xyLog.AsxExLog(new Exception("Async failure"));
```

---

## 🧵 Thread-Safety

- ✅ Log-Dateien sind **thread-safe** durch interne Sperre (`lock`).
- Ereignisse sind **null-safe** aufrufbar mit dem `?.Invoke` Pattern.

---

## ⚠️ Limitierungen

- Kein dynamisches Konfigurationssystem für Log-Level oder Targets.
- Kein Caching, kein Streaming, kein Rolling-File-Konzept (nur manuelles Archiving).
- Keine `ILogger`-Schnittstelle implementiert (nicht standardkonform im .NET-Kontext).

---

## ⚙️ Konfiguration

| Eigenschaft       | Typ      | Beschreibung                        |
|------------------|----------|-------------------------------------|
| `_logFilePath`   | `string` | Pfad für Standard-Logdateien        |
| `_exLogFilePath` | `string` | Pfad für Exception-Logdateien       |
| `_maxLogFileSize`| `long`   | Grenzwert für Archivierung (10 MB)  |
| `_archiver`      | `xyLogArchiver` | Verantwortlich für Archivierung |

---

## 🚀 Performance

- Gut geeignet für mittelgroße Apps mit **geringer bis mittlerer Log-Frequenz**.
- Für Echtzeit- oder Hochfrequenzsysteme wäre ein Buffered Writer oder BackgroundQueue ratsam.

---

## 🧪 Erweiterbar mit

- `xyLogFormatter`: Für Exception-Formatierung und JSON-Ausgabe
- `xyLogTargets`: Enum für spätere Routing-Mechanismen (optional)
- `xyLogArchiver`: Automatische Archivierung von großen Logdateien

---

## 🔗 Abhängigkeiten

- .NET Standard (kein NuGet nötig)
- `Microsoft.Extensions.Logging` (für `LogLevel`)
- Eigene Hilfs-Klassen: `xyLogFormatter`, `xyLogArchiver`, etc.

---

## 📂 Speicherorte

Standardmäßig werden Logs im Unterordner `logs/` erzeugt:

```
logs/
├── app.log         (Standard-Logs)
└── exceptions.log  (Fehler & Exceptions)
```

---

## 📦 ToDo / Ideen für später

- [ ] Integration mit Microsoft.Extensions.Logging
- [ ] Konfiguration via JSON oder AppSettings
- [ ] Log-Rotationsmechanismus
- [ ] JSON-File-Output oder DB-Stores