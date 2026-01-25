using xyToolz.QOL;

namespace xyToolz.Safety;

/// <summary>
/// Facade for QOL try/catch helpers.
/// This class contains NO logic of its own.
/// </summary>
public static class Try
{
    // --------------------
    // PARAMETERLESS
    // --------------------

    public static object? Run(Func<object> method)
        => xy.TryCatch(method);

    public static Task<object> Async(Func<Task<object>> method)
        => xy.TryCatch(method);

    // --------------------
    // SINGLE PARAMETER
    // --------------------

    public static object? Run(Func<object, object> method, object arg)
        => xy.TryCatch(method, arg);

    public static Task<object> Async(Func<object, Task<object>> method, object arg)
        => xy.TryCatch(method, arg);

    // --------------------
    // TWO PARAMETERS
    // --------------------

    public static object? Run(Func<object, object, object> method, object arg1, object arg2)
        => xy.TryCatch(method, arg1, arg2);

    public static Task<object> Async(
        Func<object, object, Task<object>> method,
        object arg1,
        object arg2)
        => xy.TryCatch(method, arg1, arg2);

    // --------------------
    // PARAM ARRAY
    // --------------------

    public static object? Run(Func<object[], object> method, params object[] args)
        => xy.TryCatch(method, args);

    public static Task<object> Async(Func<object[], Task<object>> method, params object[] args)
        => xy.TryCatch(method, args);
}
