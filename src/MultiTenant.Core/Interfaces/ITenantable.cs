namespace MultiTenant.Core.Interfaces
{
    public interface ITenantable
    {
        public string TenantId { get; set; }
    }
}
