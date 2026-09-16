namespace Cintrix.Commerce.Api.Domain.Entities
{
    public class ItemVariant
    {
        public Guid ItemVariantId { get; private set; }
        public Guid ItemId { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset? UpdatedAt { get; private set; }
        private ItemVariant() { }
        public ItemVariant(Guid itemId)
        {
            ItemVariantId = Guid.CreateVersion7();
            ItemId = itemId;
            CreatedAt = DateTimeOffset.UtcNow;
        }
    }
}
