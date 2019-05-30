namespace TinyECS.Interfaces
{
    /// <summary>
    /// class IComponent
    /// 
    /// The class describes a functionality of a component
    /// that can store the data
    /// </summary>

    public interface IComponent
    {
    }


    /// <summary>
    /// class IComponentIterator
    /// 
    /// The interface describes a functionality of an iterator that enumerates all components
    /// of particular entity
    /// </summary>

    public interface IComponentIterator
    {
        /// <summary>
        /// The method returns component's value which the iterator points to
        /// </summary>
        /// <typeparam name="T">A specific type to which current component will be casted</typeparam>
        /// <returns>The method returns component's value which the iterator points to</returns>

        T Get<T>() where T: struct, IComponent;

        /// <summary>
        /// The method returns a reference to IComponent which the iterator points to
        /// </summary>
        /// <returns>The method returns a reference to IComponent which the iterator points to</returns>

        IComponent Get();

        /// <summary>
        /// The method moves iterator to next available component if the latter exists
        /// </summary>
        /// <returns>The method returns true if there is a component at next position, false in other cases</returns>

        bool MoveNext();
    }
}
