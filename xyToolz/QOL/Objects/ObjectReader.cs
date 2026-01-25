using System.Reflection;
using xyToolz.QOL;

namespace xyToolz.QOL.Objects;

/// <summary>
/// Facade for inspecting object properties.
/// </summary>
public sealed class ObjectReader
{
    private readonly xyQol _qol;

    public ObjectReader(xyQol qol)
    {
        _qol = qol;
    }

    public PropertyInfo[] PropertiesOf<T>(T obj)
        => _qol.GetPropertyInfosForTarget(obj);
}
