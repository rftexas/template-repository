using Template.Domain.Specifications;

namespace Template.Domain.Items.Specifications;

public class MatchItemName(string itemName) : Specification<Item>(
    entity => entity.ItemName == itemName);

public class MatchId(ItemId itemId) : Specification<Item>(
    entity => entity.ItemId == itemId);