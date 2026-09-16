namespace Cintrix.Commerce.Api.Domain.Entities
{
    public class Item
    {
        public Guid ItemId { get; private set; }
        public string Label { get; private set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset? UpdatedAt { get; private set; }
        private Item() { }
        public Item(string label, Guid? itemId = null)
        {
            if (string.IsNullOrWhiteSpace(label))
                throw new ArgumentException("Label cannot be empty.", nameof(label));
            ItemId = itemId ?? Guid.CreateVersion7();
            Label = label;
            CreatedAt = DateTimeOffset.Now;

        }

        public bool ApplyChanges(string? newLabel)
        {
            bool hasChanges = false;

            if(newLabel is not null)
            {
                string trimmedLabel = newLabel.Trim();
                if (string.IsNullOrWhiteSpace(trimmedLabel))
                {
                    throw new ArgumentException("Label cannot be empty.", nameof(trimmedLabel));
                }
                if(!string.Equals(Label, trimmedLabel, StringComparison.Ordinal))
                {
                    Label = trimmedLabel;
                    hasChanges = true;
                }
            }

            return hasChanges;
        }
    }
}
