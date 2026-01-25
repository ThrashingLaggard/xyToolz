using xyToolz.QOL;

namespace xyToolz.Objects;

/// <summary>
/// Facade for rendering object-related data.
/// </summary>
public static class ObjectFormatter
{
    public static string ToString<TKey, TValue, T>(Dictionary<TKey, TValue> values)
        where TKey : class
        where T : class
        => xyQol.PropertiesToString<TKey, TValue, T>(values);
}
