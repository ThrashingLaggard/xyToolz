
# xyDataProtectionHelper

Ver-& Entschlüsseln von Daten per Windows DPAPI.

--

## 🔒 Sicherheit & Einschränkungen

- Kein statischer Schlüssel – es wird ein Passwort mit Salt über `xyHashHelper.BuildKeyFromPassword(...)` abgeleitet.
- IV wird zur Entschlüsselung im Klartext mitgegeben (Standardverfahren).
- Die Verantwortung für sicheren Umgang mit Passwörtern und Salts liegt beim Aufrufer.

---

## 📘 Methodenübersicht

### EncryptAsync<T>
```csharp
Task<byte[]?> EncryptAsync<T>(T obj, string password, byte[] salt)
```
Serialisiert ein beliebiges Objekt zu JSON, verschlüsselt es und gibt das Ergebnis als [IV][CipherText] zurück.

### DecryptAsync<T>
```csharp
Task<T?> DecryptAsync<T>(byte[] encryptedData, string password, byte[] salt)
```
Entschlüsselt die zuvor verschlüsselten Daten und deserialisiert sie zu einem Objekt vom Typ T.

### ProtectAsync
```csharp
Task<byte[]> ProtectAsync(string plainText, byte[] key)
```
Niedrigere Ebene: Verschlüsselt einen String mit gegebenem symmetrischem Schlüssel.

### UnprotectAsync
```csharp
Task<string> UnprotectAsync(byte[] cipherData, byte[] key)
```
Entschlüsselt [IV][CipherText] zu einem UTF8-String mit dem bereitgestellten AES-Schlüssel.

---

## 💡 Beispiel: Verwendung mit Objekt und Passwort

```csharp
var myObject = new MySettings { Username = "admin", Password = "secret" };
string password = "SuperSecurePass123!";
byte[] salt = Encoding.UTF8.GetBytes("12345678");

byte[] encrypted = await xyDataProtectionHelper.EncryptAsync(myObject, password, salt);
var restored = await xyDataProtectionHelper.DecryptAsync<MySettings>(encrypted, password, salt);
```

---

## 📁 Abhängigkeiten

- [`System.Security.Cryptography`](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography)
- [`xyHashHelper`](#)
- [`xyLog`](#)
- [`xyJson`](#)

---

## ✅ Geeignet für

- Verschlüsselte Konfigurationsdateien
- Temporäre sensible Nutzerdaten
- Sichere Objektübertragung im Speicher

---

## 🔍 Hinweise

- Falls du Base64 für Speicherung benötigst: `Convert.ToBase64String(...)`
- Nutze für große Dateien eher Datei-basierte Streams statt MemoryStreams
