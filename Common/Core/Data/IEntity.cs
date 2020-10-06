namespace Common.Core.Data
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}