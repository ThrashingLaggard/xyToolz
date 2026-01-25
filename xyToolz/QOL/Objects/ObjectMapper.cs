using xyToolz.QOL;

namespace xyToolz.QOL.Objects;

/// <summary>
/// Facade for mapping objects to and from dictionaries.
/// </summary>
public sealed class ObjectMapper
{
    private readonly xyQol _qol;

    public ObjectMapper(xyQol qol)
    {
        _qol = qol;
    }

    public Dictionary<TKey, TValue> ToDictionary<TKey, TValue, T>(T obj)
        where T : class
        where TKey : class
        => _qol.GetPropertyValuesForTarget<TKey, TValue, T>(obj);

    public T FromDictionary<T, TKey, TValue>(Dictionary<TKey, TValue> values)
        where T : class
        where TKey : class
        => _qol.GetEntityFromDictionary<T, TKey, TValue>(values);
}
