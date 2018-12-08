namespace StoreHelper.Dal.Core.Interfaces.Entity
{
    /// <summary>
    /// Represents the identifiable entity
    /// </summary>
    /// <typeparam name="TKey">Identifier type</typeparam>
    public interface IKeyable <TKey>
    { 
        /// <summary>
      /// Gets object's id
      /// </summary>
        TKey Id { get; }
}
}
