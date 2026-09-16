
namespace Cintrix.Commerce.Api.Domain.Entity
{
    public class Item
    {
        public Guid ItemId { get; private set; }
        public string Label { get; private set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset? UpdatedAt { get; private set; }
        private Item() { }
        public Item(string label)
        {
            if (string.IsNullOrWhiteSpace(label)) throw new ArgumentException("Label cannot be empty.", nameof(label));

            ItemId = Guid.CreateVersion7();
            Label = label;
            CreatedAt = DateTimeOffset.Now;

        }
    }
    public class ItemVariant
    {
    }
    public class ItemAttribute
    {
    }
    public class AttributeValue
    {
    }
    public class ItemVariantAttribute
    {
    }
}
