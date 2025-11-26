using System.Runtime.CompilerServices;

namespace Langfuse.IntegrationTests;

/// <summary>
/// Automatically loads .env file when the test assembly is loaded.
/// </summary>
public static class EnvLoader
{
    private static bool _loaded;

    /// <summary>
    /// Ensures the .env file is loaded. Safe to call multiple times.
    /// </summary>
    [ModuleInitializer]
    public static void Initialize()
    {
        if (_loaded) return;
        _loaded = true;

        // Try to find .env file by walking up from the test assembly location
        var directory = AppContext.BaseDirectory;
        
        while (directory != null)
        {
            var envPath = Path.Combine(directory, ".env");
            if (File.Exists(envPath))
            {
                DotNetEnv.Env.Load(envPath);
                Console.WriteLine($"Loaded environment from: {envPath}");
                return;
            }

            // Also check for .env.local
            var envLocalPath = Path.Combine(directory, ".env.local");
            if (File.Exists(envLocalPath))
            {
                DotNetEnv.Env.Load(envLocalPath);
                Console.WriteLine($"Loaded environment from: {envLocalPath}");
                return;
            }

            directory = Directory.GetParent(directory)?.FullName;
        }

        Console.WriteLine("No .env file found. Using existing environment variables.");
    }
}

