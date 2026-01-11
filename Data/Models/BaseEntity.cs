namespace ECommerceApp.RyanW84.Data.Models;

public abstract class BaseEntity
{
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    // Soft delete method
    public void SoftDelete()
    {
        IsDeleted = true;
        DeletedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    // Restore from soft delete
    public void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
