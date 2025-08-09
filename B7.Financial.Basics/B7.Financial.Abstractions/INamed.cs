namespace B7.Financial.Abstractions;

/// <summary>
/// A named instance.
/// </summary>
public interface INamed
{
    /// <summary>
    /// The unique name of the instance. <br/>
    /// </summary>
    public string Name { get; }
}

/// <summary>
/// A named instance.
/// <para/>
/// This simple interface is used to define objects that can be identified by a unique name. <br/>
/// The name contains enough information to be able to recreate the instance.
/// </summary>
public interface INamed<out T> : INamed
{
    /// <summary>
    /// Factory method.
    /// </summary>
    /// <param name="name">The name of the instance</param>
    /// <returns>Instance of <see cref="T"/></returns>
    static abstract T Of(string name);
}