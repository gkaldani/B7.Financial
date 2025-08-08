namespace B7.Financial.Abstractions;

/// <summary>
/// A named instance.
/// </summary>
public interface INamed
{
    /// <summary>
    /// The unique name of the type. <br/>
    /// </summary>
    public static abstract string Name { get; }

    /// <summary>
    /// Gets the unique name of the instance. <br/>
    /// </summary>
    /// <returns>The unique name of the instance</returns>
    public string GetName();
}