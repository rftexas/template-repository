using System.Diagnostics;

namespace Template.Domain;

public static class Observability
{
    public static ActivitySource ActivitySource { get; } = new ActivitySource("Template");
}
