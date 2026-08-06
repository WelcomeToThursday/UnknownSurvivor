using JetBrains.Annotations;
using SPTarkov.Server.Core.Models.Spt.Mod;
using Range = SemanticVersioning.Range;
using Version = SemanticVersioning.Version;

namespace UnknownSurvivor;

[UsedImplicitly]
public class ModMetadata : IModMetadata
{
    public string ModGuid { get; init; } = "com.dsnyder.unknownsurvivor";
    public string Name { get; init; } = "Unknown Survivor";
    public string Author { get; init; } = "Dsnyder";
    public List<string>? Contributors { get; init; }
    public Version Version { get; init; } = new(typeof(ModMetadata).Assembly.GetName().Version!.ToString(3));
    public Range SptVersion { get; init; } = new("~4.1.0");
    public bool HasPrepatcher { get; init; } = false;
    public List<string>? Incompatibilities { get; init; }

    public Dictionary<string, Range>? ModDependencies { get; init; } = new()
    {
        { "com.wtt.commonlib", new Range("~3.0.0") }
    };
    public string? Url { get; init; } = "https://github.com/WelcomeToThursday/UnknownSurvivor";
    public string License { get; init; } = "MIT";
}