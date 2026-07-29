using xyToolz.Security;

/// <summary>
/// Small CLI around <see cref="xyDataProtector"/> (xySecurity) so the release hooks can keep
/// the NuGet API key DPAPI-encrypted at rest instead of in a plaintext environment variable.
/// DPAPI's CurrentUser scope means this only decrypts under the same Windows user account
/// that ran "encrypt" - it is a local-hook-only mechanism, not usable from GitHub Actions
/// runners (which use a repository secret instead, see WORKFLOWS.md).
/// </summary>
public class Program
{
    private const string SecretBeginMarker = "===XYSECRET-BEGIN===";
    private const string SecretEndMarker = "===XYSECRET-END===";

    private static async Task<int> Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.Error.WriteLine("Usage: xyToolz_Exec encrypt <output-file>   (reads plaintext from stdin)");
            Console.Error.WriteLine("       xyToolz_Exec decrypt <input-file>    (writes plaintext between markers on stdout)");
            return 1;
        }

        string command = args[0];
        string path = args[1];

        switch (command)
        {
            case "encrypt":
                {
                    // Read from stdin, never from argv - argv ends up in shell history and
                    // process listings, which is exactly what a secret must never appear in.
                    string plaintext = (await Console.In.ReadToEndAsync()).TrimEnd('\r', '\n');
                    if (string.IsNullOrEmpty(plaintext))
                    {
                        Console.Error.WriteLine("No input on stdin.");
                        return 1;
                    }

                    bool saved = await xyDataProtector.SaveProtectedToFileAsync(plaintext, path);
                    if (!saved)
                    {
                        Console.Error.WriteLine("Encryption failed.");
                        return 1;
                    }

                    // Success goes to stdout, not stderr: PowerShell renders ANY stderr output
                    // from a native exe/dll as a red NativeCommandError, even plain success text
                    // - confirmed by an actual run that looked like a failure but wasn't one.
                    Console.WriteLine($"Encrypted value written to {path}. This file is only readable under your current Windows account.");
                    return 0;
                }

            case "decrypt":
                {
                    string? plaintext = await xyDataProtector.LoadProtectedFromFileAsync<string>(path);
                    if (plaintext is null)
                    {
                        Console.Error.WriteLine("Decryption failed - wrong user account, or the file is missing/corrupt.");
                        return 1;
                    }

                    // Wrapped in unmistakable markers so the caller's shell can extract exactly
                    // the secret line even if the logging library also writes to stdout.
                    Console.WriteLine(SecretBeginMarker);
                    Console.WriteLine(plaintext);
                    Console.WriteLine(SecretEndMarker);
                    return 0;
                }

            default:
                Console.Error.WriteLine($"Unknown command '{command}'. Use 'encrypt' or 'decrypt'.");
                return 1;
        }
    }
}
