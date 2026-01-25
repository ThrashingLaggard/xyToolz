using xyToolz.QOL;

namespace xyToolz.QOL.Collections;

/// <summary>
/// Facade for numeric enumerable helpers.
/// </summary>
public static class Fill
{
    public static IEnumerable<int> FillTheList(int limit)
        => xyColQol.FillTheList(limit);

    public static IEnumerable<int> FillEvenList(int limit)
        => xyColQol.FillEvenList(limit);

    public static IEnumerable<int> FillOddList(int limit)
        => xyColQol.FillOddList(limit);
}
