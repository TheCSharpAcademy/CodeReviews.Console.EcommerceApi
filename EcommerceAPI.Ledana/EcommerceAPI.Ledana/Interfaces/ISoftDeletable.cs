namespace EcommerceAPI.Ledana.Interfaces
{
    public interface ISoftDeletable
    {
        public bool IsDeleted { get; set; }
        DateTime? DeletedOnUtc { get; set; }
    }
}
