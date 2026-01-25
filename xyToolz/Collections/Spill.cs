using System.Collections;
using xyToolz.QOL;

namespace xyToolz.Collections;

/// <summary>
/// Facade for enumerable printing helpers.
/// </summary>
public static class Spill
{
    public static string SplitSpill(IEnumerable values)
        => xyColQol.SplitSpill(values);

    public static string SpillValues(IEnumerable values)
        => xyColQol.Spill(values);

    public static string SpillDown(IEnumerable values)
        => xyColQol.SpillDown(values);

    public static Task<string> AsxSpill(IEnumerable values)
        => xyColQol.AsxSpill(values);

    public static Task<string> AsxSpillDown(IEnumerable values)
        => xyColQol.AsxSpillDown(values);
}
