namespace B7.Financial.Abstractions;

/// <summary>
/// This interface defines a factory for creating named instances of type T.
/// </summary>
/// <typeparam name="T"> The type of the named instance. </typeparam>
public interface INamedFactory<out T> where T : INamed
{
    /// <summary>
    /// Retrieves an instance of type <typeparamref name="T"/> by its name.
    /// </summary>
    /// <param name="name"> The name of the instance to retrieve. </param>
    /// <returns> An instance of type <typeparamref name="T"/>. </returns>
    public T Of(Name name);
}