namespace xyToolz.QOL.Experiments;

/// <summary>
/// Experimental and intentionally unstable functionality.
/// Use at your own risk.
/// </summary>
public static class Chaos
{
    /// <summary>
    /// Triggers a console beep.
    /// </summary>
    public static void Beep()
    {
        xy.Piep();
    }

    /// <summary>
    /// Executes an intentionally unsafe control-flow experiment.
    /// This method is not guaranteed to terminate.
    /// </summary>
    public static string Crash(UInt128 value)
    {
        return xy.Crash(value);
    }
}
