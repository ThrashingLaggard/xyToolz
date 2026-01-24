namespace xyToolz.Fonts;
#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using PdfSharpCore.Fonts;
using xyToolz.Helper.Logging;
using xyToolz.StaticLogging;


/// <summary>
/// PdfSharpCore font resolver that auto-detects embedded font resources (EmbeddedResource).
/// It scans the assembly for .ttf/.otf resources and picks:
///   - SansRegular  : first match from a "sans" candidate list (Inter, Roboto, OpenSans, Noto Sans, DejaVu Sans, Source Sans, Montserrat, Lato, Arial, Helvetica, ...),
///   - SansBold     : a matching bold face for the chosen sans (if available), otherwise falls back to regular,
///   - MonoRegular  : first match from a "mono" candidate list (Cascadia, FiraMono, DejaVu Sans Mono, Noto Sans Mono, Courier, Consolas, Source Code, Menlo, ...).
/// Use the family names below with XFont:
///   FamilySans = "XY Sans", FamilyMono = "XY Mono".
/// </summary>
public sealed partial class AutoResourceFontResolver : IFontResolver
{
    /// <summary> Add useful information here </summary>
    public string? Description { get; set; }

    private readonly Assembly _asm;

    /// <summary> Sans family name </summary>
    public const string FamilySans = "XY Sans";
    /// <summary> Mono family name </summary>
    public const string FamilyMono = "XY Mono";

    private const string FaceSansRegular = "XY_SANS_REG";
    private const string FaceSansBold = "XY_SANS_BOLD";
    private const string FaceMonoRegular = "XY_MONO_REG";
    private readonly string? _resSansReg;
    private readonly string? _resSansBold;
    private readonly string? _resMonoReg;

    /// <summary> Name of the sans regular resource  </summary>
    public string? SansRegularResourceName => _resSansReg;
    /// <summary> Name of the sans bold resource  </summary>
    public string? SansBoldResourceName => _resSansBold;
    /// <summary> Name of the mono regular resource  </summary>
    public string? MonoRegularResourceName => _resMonoReg;

    // buffers for resource names
    private byte[]? _bufSansReg, _bufSansBold, _bufMonoReg;

    /// <summary>
    /// Get the value from the FamilySans property as default font name 
    /// </summary>
    public string DefaultFontName => FamilySans;

    /// <summary>
    /// Basic constructor wirh lots of work in it... need to source something out
    /// </summary>
    public AutoResourceFontResolver()
    {
        _asm = typeof(AutoResourceFontResolver).Assembly;
        var names = _asm.GetManifestResourceNames();

        if (names.Length == 0)
        {
            // Fallback: try the entry assembly (CLI)
            var entry = Assembly.GetEntryAssembly();
            if (entry != null)
            {
                names = entry.GetManifestResourceNames();
                _asm = entry;
            }
        }

        foreach (var n in typeof(AutoResourceFontResolver).Assembly.GetManifestResourceNames().Take(10))
            System.Diagnostics.Debug.WriteLine("RES: " + n);

        // Consider only .ttf/.otf
        string[] fontRes = [.. names.Where(n => n.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase) || 
                                                                  n.EndsWith(".otf", StringComparison.OrdinalIgnoreCase))];

        // Heuristics for picking sans/mono/bold faces
        Regex sansRegex = SansRegex();
        Regex monoRegex = MonoRegex();
        Regex boldRegex = BoldRegex();

        // Pick a sans regular
        _resSansReg = fontRes.FirstOrDefault(n => sansRegex.IsMatch(n))
                   ?? fontRes.FirstOrDefault(); // fallback: any font if no sans candidate is found

        // Try to find a matching bold for the chosen sans (same stem + bold marker)
        if (_resSansReg != null)
        {
            string stem = GetFontStem(_resSansReg);
            _resSansBold = fontRes.FirstOrDefault(n => n != _resSansReg &&
                GetFontStem(n) == stem && boldRegex.IsMatch(n))
                ?? fontRes.FirstOrDefault(n => boldRegex.IsMatch(n)); // generic bold as a fallback
        }

        // Pick a mono regular
        // _resMonoReg = fontRes.FirstOrDefault(n => monoRegex.IsMatch(n));

        // Pick a mono regular (Comic bevorzugen)
        _resMonoReg = fontRes.Where(n => monoRegex.IsMatch(n)).OrderByDescending(n => MatchComicRegex().IsMatch(n) ? 
            2 : MatchMonospaceRegex().IsMatch(n) ? 1 : 0).FirstOrDefault();



        // Optional diagnostics: set XYDOCGEN_LOG_FONTS=1 to log selected resources
        if (Environment.GetEnvironmentVariable("XYDOCGEN_LOG_FONTS") == "1")
        {
            Console.WriteLine("[AutoResourceFontResolver] Embedded fonts:");
            foreach (var r in fontRes) Console.WriteLine("  - " + r);
            Console.WriteLine($"Chosen SansReg : {_resSansReg ?? "(none)"}");
            Console.WriteLine($"Chosen SansBold: {_resSansBold ?? "(none)"}");
            Console.WriteLine($"Chosen MonoReg : {_resMonoReg ?? "(none)"}");
        }

        if (_resSansReg == null)
            throw new FileNotFoundException("No embedded fonts found. Add .ttf/.otf under Resources/Fonts and mark them as <EmbeddedResource>.");
    }

    /// <summary>
    /// Define the typeface to be used
    /// </summary>
    /// <param name="familyName"></param>
    /// <param name="isBold"></param>
    /// <param name="isItalic"></param>
    /// <returns></returns>
    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        var fam = (familyName ?? "").Trim().ToLowerInvariant();

        // Mono family? (+Comic & Monospace)
        if (fam.Contains("comic") || fam.Contains("monospace") ||
            fam.Contains("mono") || fam.Contains("cascadia") ||
            fam.Contains("consolas") || fam.Contains("courier") || fam.Contains("code")) 
            return new FontResolverInfo(FaceMonoRegular);

        // Sans family
        if (isBold && _resSansBold != null)
            return new FontResolverInfo(FaceSansBold);

        return new FontResolverInfo(FaceSansRegular);
    }

    /// <summary>
    /// Get font by name
    /// </summary>
    /// <param name="faceName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public byte[] GetFont(string faceName)
    {
  
        return faceName switch
        {
            FaceSansRegular => _bufSansReg ??= GetBytesFromAssemblyManifest(_asm, _resSansReg!),
            FaceSansBold => _bufSansBold ??= GetBytesFromAssemblyManifest(_asm, _resSansBold ?? _resSansReg!),
            FaceMonoRegular => _bufMonoReg ??= GetBytesFromAssemblyManifest(_asm, _resMonoReg ?? _resSansReg!),
            // Comic Sans MS is missing here
            _ => throw new ArgumentException($"Unknown face name: {faceName}", nameof(faceName))
        };
    }

    /// <summary>
    /// Read the data from the target manifest resource in this assembly 
    /// </summary>
    /// <param name="asm"></param>
    /// <param name="manifestName"></param>
    /// <returns>byte[] filled with manifest data</returns>
    /// <exception cref="FileNotFoundException"></exception>
    public static byte[] GetBytesFromAssemblyManifest(Assembly asm, string manifestName)
    {
        // Read the target resource from this assembly
        using Stream DataStreamFromManifestResource = asm.GetManifestResourceStream(manifestName)?? 
            throw new FileNotFoundException($"Embedded font not found: {manifestName} \nCheck <EmbeddedResource> items and the project's default namespace.");
        
        // Copy the data into a MemoryStream 
        using MemoryStream ms_RessourceStream = new ();         DataStreamFromManifestResource.CopyTo(ms_RessourceStream);

        // Convert and return it
        byte[]  bytesFromResourceStream =ms_RessourceStream.ToArray();      
        return bytesFromResourceStream;
    }

    /// <summary>
    ///  Extracts a font's family stem from a filename
    ///  Removes style tokens like Regular/Bold/Italic/Medium/etc. 
    /// </summary>
    /// <param name="fontFileName_"></param>
    /// <returns></returns>
    public static string GetFontStem(string fontFileName_)
    {
        string fontStem = fontFileName_;
        string styleTokensToRemove = "(regular|bold|italic|oblique|medium|semi.?bold|black|light|thin|extra|ultra)";
        string emptyStringForReplacement="";
       
        // Split the filename up by the dot and reverse the order 
        IEnumerable<string> splitUpReversedFontFileName = fontFileName_.Split('.').Reverse();

        // Get the second last word from the the filename
        if (splitUpReversedFontFileName.Skip(1).FirstOrDefault() is string stemFromFileName)
        {
            // Override the whole name with only the stem
            fontStem = stemFromFileName;
        }
        else
        {
            // Shouldn't be reached i guess
            xyLog.Log($"No second entry in the enumerable {fontFileName_}!!!    \nPlease contact supervisor and stay calm!");
        }

        // Remove style tokens, low lines & hyphens from the stem and garantuee it's in lower case:
        string afterRemovingStyleTokens = Regex.Replace(fontStem, styleTokensToRemove, emptyStringForReplacement, RegexOptions.IgnoreCase);
        string removedLowLinesAndDashes = afterRemovingStyleTokens.Replace("_", "").Replace("-", "");
        string loweredStem = removedLowLinesAndDashes.ToLowerInvariant();
              
        return loweredStem;

    }

    [GeneratedRegex("(inter|roboto|open.?sans|noto.?sans(?!.*mono)|dejavu.?sans(?!.*mono)|source.?sans|montserrat|lato|arial|helvetica|liberation.?sans)", RegexOptions.IgnoreCase, "de-DE")]
    internal static partial Regex SansRegex();
    [GeneratedRegex("(comic.?mono|comic.?sans|comic|monospace|cascadia|fira.?mono|dejavu.?sans.?mono|noto.?sans.?mono|inconsolata|source.?code|courier|consolas|menlo|mono|code)", RegexOptions.IgnoreCase, "de-DE")]
    private static partial Regex MonoRegex();
    [GeneratedRegex("(bold|semi.?bold|demi|black)", RegexOptions.IgnoreCase, "de-DE")]
    internal static partial Regex BoldRegex();
    [GeneratedRegex("comic", RegexOptions.IgnoreCase, "de-DE")]
    internal static partial Regex MatchComicRegex();
    [GeneratedRegex("monospace", RegexOptions.IgnoreCase, "de-DE")]
    internal static partial Regex MatchMonospaceRegex();
}
