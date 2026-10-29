using Vogen;

namespace Template.Domain.Items;

public class Item
{
    public ItemId ItemId { get; }
    public string ItemName { get; private set; }

    private Item(ItemId itemId, string itemName)
    {
        ItemId = itemId;
        ItemName = itemName;
    }

    public static Item Create(string itemName)
    {
        return new Item(ItemId.New(), itemName);
    }
}

[ValueObject<Guid>(Conversions.EfCoreValueConverter | Conversions.SystemTextJson)]
public readonly partial record struct ItemId
{
    public static ItemId New() => From(Guid.NewGuid());
    private static Validation Validate(Guid input)
    {
        return input == Guid.Empty ? Validation.Invalid("ItemId cannot be empty") : Validation.Ok;
    }
}