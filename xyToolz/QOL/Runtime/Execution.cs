using xyToolz.QOL;

namespace xyToolz.QOL.Runtime;

/// <summary>
/// Facade for process execution helpers.
/// Contains no logic of its own.
/// </summary>
public static class Execution
{
    /// <summary>
    /// Starts a process by name.
    /// </summary>
    /// <param name="processName">Process name or executable.</param>
    public async static Task StartAsync(string processName)
        => await xy.Start(processName);
}
