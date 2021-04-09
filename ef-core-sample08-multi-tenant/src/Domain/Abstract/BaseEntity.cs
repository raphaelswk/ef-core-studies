namespace src.Domain.Abstract
{
    public class BaseEntity
    {
        public int Id { get; set; }
        
        // MULTI-TENANT: TABLE FIELD STRATEGY
        public string TenantId { get; set; }
    }
}
