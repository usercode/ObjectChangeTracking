using ObjectChangeTracking.Abstractions;

namespace ObjectChangeTracking;

/// <summary>
/// SimplePropertyChange
/// </summary>
public sealed class SimplePropertyChange : IPropertyChange
{
    public SimplePropertyChange(string name, object? oldValue, object? currentValue)
    {
        Name = name;
        OldValue = oldValue;
        CurrentValue = currentValue;
    }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Value
    /// </summary>
    public object? OldValue { get; }

    /// <summary>
    /// CurrentValue
    /// </summary>
    public object? CurrentValue { get; }
}
