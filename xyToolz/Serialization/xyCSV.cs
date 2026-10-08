using System.Collections;
using xyMessageFactory.Factories;

namespace xyToolz.Serialization;

/// <summary>
/// Since Csv is up to 4x faster and 3x smaller, I want to replace almost all the calls for xyJson with this.
/// Good thing I wanted to do this before I knew what it meant and how much work this will be. 
/// </summary>
public static class xyCsv
{
    private static readonly xyBaseMessageFactory Fac=new ();

    /// <summary>
    /// 
    /// </summary>
    public static IList FromCsv(string csv)
    {
        string[] csvContent = [];
        
        
        
        return csvContent;
    }

    public static string FromFile(string path)
    {
        return "";
    }
    
    public static string ToFile(this IList list, string filePath)
    {
        return ",";
        
    }
    
    public static string ToCsv(this IList list)
    {
        return ",";
        
    }


}