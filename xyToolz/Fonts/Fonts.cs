namespace xyToolz.Fonts;
#nullable enable

using System.Reflection;
using PdfSharpCore.Fonts;
using System.Text.RegularExpressions;

/// <summary>
/// Facade for AutoResourceFontResolver.
/// 100% pass-through. No logic. No state. No interpretation.
/// </summary>
public static class Fonts
{
    // ---------- construction ----------

    public static AutoResourceFontResolver Create()
        => new AutoResourceFontResolver();

    // ---------- constants ----------

    public static string FamilySans
        => AutoResourceFontResolver.FamilySans;

    public static string FamilyMono
        => AutoResourceFontResolver.FamilyMono;

    // ---------- instance methods ----------

    public static FontResolverInfo ResolveTypeface(
        AutoResourceFontResolver resolver,
        string familyName,
        bool isBold,
        bool isItalic)
        => resolver.ResolveTypeface(familyName, isBold, isItalic);

    public static byte[] GetFont(
        AutoResourceFontResolver resolver,
        string faceName)
        => resolver.GetFont(faceName);

    // ---------- static methods ----------

    public static byte[] GetBytesFromAssemblyManifest(
        Assembly asm,
        string manifestName)
        => AutoResourceFontResolver.GetBytesFromAssemblyManifest(asm, manifestName);

    public static string GetFontStem(string fontFileName)
        => AutoResourceFontResolver.GetFontStem(fontFileName);

    // ---------- regex passthrough ----------

    public static Regex SansRegex()
        => AutoResourceFontResolver.SansRegex();

    public static Regex BoldRegex()
        => AutoResourceFontResolver.BoldRegex();

    public static Regex MatchComicRegex()
        => AutoResourceFontResolver.MatchComicRegex();

    public static Regex MatchMonospaceRegex()
        => AutoResourceFontResolver.MatchMonospaceRegex();
}
