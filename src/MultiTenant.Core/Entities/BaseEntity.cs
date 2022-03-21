namespace MultiTenant.Core.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; }
    }
}
