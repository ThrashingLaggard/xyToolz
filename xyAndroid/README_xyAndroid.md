# 📱 xyAndroid – Hilfsbibliothek für Android-Apps

`xyAndroid` ist ein Modul der `xyToolz`-Bibliothek und stellt hilfreiche Klassen für Android-Entwicklung bereit.
Es unterstützt dich bei Dateioperationen, Foreground-Services und Berechtigungsmanagement.

---

## 📦 Enthaltene Klassen

| Typ   | Name                            | Beschreibung |
|:------|:--------------------------------|:-------------|
| class | **ImplementForeGroundService**  | Implementierungshilfe für Android Foreground-Services. |
| class | **xyALog**                      | Einfaches Logging-Utility für Android-Anwendungen. |
| class | **xyFilesAndroid**              | Dateioperationen auf dem Android-Dateisystem. |
| class | **xyForeGroundService**         | Steuerung und Verwaltung von Foreground-Services. |
| class | **DefaultStoragePermissionService** | Standard-Implementierung zur Verwaltung von Speicherberechtigungen. |

---

## 🛠️ Beispielhafte Verwendung

```csharp
// Logging auf Android
var logger = new xyALog();
logger.Log("App gestartet");

// Dateioperationen
var fileHelper = new xyFilesAndroid();
fileHelper.SaveText("beispiel.txt", "Hallo Welt!");

// Starten eines Foreground-Services
var service = new xyForeGroundService();
service.StartService();
```

---

## 📁 Projektstruktur (xyAndroid)

```plaintext
xyAndroid/
├── xyALog.cs
├── xyFilesAndroid.cs
├── xyForeGroundService.cs
├── StoragePermission/
│   └── DefaultStoragePermissionService.cs
```

---

## 📚 Abhängigkeiten

- Xamarin.Android oder kompatible Android-Laufzeitumgebung erforderlich.
- Keine externen Bibliotheken – rein eigenes Code-Modul.

---

# ✅ Hinweis
Dieses Modul ist Teil der `xyToolz`-Bibliothek und wurde speziell für **eigene Projekte** entwickelt.
Es wird ohne zusätzliche Abhängigkeiten betrieben (🛡️ NurMeineKlassen-Modus).
