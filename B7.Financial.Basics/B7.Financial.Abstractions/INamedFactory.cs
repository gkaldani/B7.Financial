namespace B7.Financial.Abstractions;

/// <summary>
/// This interface defines a factory for creating named instances of type T.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface INamedFactory<out T> where T : INamed
{
    /// <summary>
    /// Retrieves an instance of type T by its name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public T Of(string name);
}