# 🖥️ xyAvalonia – Konsolenintegration für Avalonia GUIs

`xyAvalonia` ist ein Teilmodul von `xyToolz` und stellt spezifische Helferklassen für Avalonia-Projekte bereit.  
Derzeit liegt der Fokus auf einer erweiterten Ausgabelogik für Textausgaben in grafischen Benutzeroberflächen.

---

## 📦 Enthaltene Klassen

| Typ   | Name                  | Beschreibung |
|:------|:----------------------|:-------------|
| class | **ConsoleTextBoxWriter** | Leitet Konsolenausgabe (Console.Write) in eine Avalonia-TextBox um. Nützlich für Debug-Ausgaben in GUI-Anwendungen. |

---

## 🛠️ Beispielhafte Verwendung

```csharp
var writer = new ConsoleTextBoxWriter(myTextBox);
Console.SetOut(writer);
Console.WriteLine("Dies erscheint nun in der GUI!");
```

---

## 📁 Projektstruktur (xyAvalonia)

```plaintext
xyAvalonia/
└── Services/
    └── ConsoleTextBoxWriter.cs
```

---

## 📚 Abhängigkeiten

- Avalonia UI Framework
- Keine weiteren externen Bibliotheken
- Ausschließlich eigene Hilfsklassen (🛡️ NurMeineKlassen-Modus)

---

# ✅ Hinweis
Dieses Modul erweitert GUI-Projekte um einfache Konsolenausgaben innerhalb der Benutzeroberfläche und ist vollständig anpassbar.
