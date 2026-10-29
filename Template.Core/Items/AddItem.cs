namespace Template.Core.Items;

using Template.Domain.Items;
using FluentResults;
using Template.Persistence;
using Template.Domain;
using Template.Domain.Items.Specifications;
using Microsoft.EntityFrameworkCore;

public class AddItem
{
    public record Command(string ItemName) : ICommand<Item>;

    public class Handler(DataContext dataContext)
        : ICommandHandler<Command, Item>
    {
        private readonly DataContext _dataContext = dataContext;

        public async Task<Result<Item>> Handle(Command command, CancellationToken cancellationToken = default)
        {
            using var span = Observability.ActivitySource.StartActivity($"{nameof(AddItem)}.{nameof(Handle)}");
            if (string.IsNullOrWhiteSpace(command.ItemName))
            {
                return Result.Fail<Item>("Item name cannot be empty.");
            }
            try
            {
                if (await _dataContext.Items.AnyAsync(new MatchItemName(command.ItemName).Expression, cancellationToken))
                {
                    return Result.Fail<Item>("Item with the same name already exists.");
                }

                var item = Item.Create(command.ItemName);
                _dataContext.Items.Add(item);
                await _dataContext.SaveChangesAsync(cancellationToken);
                span?.SetTag("item.itemId", item.ItemId);

                return Result.Ok(item);
            }
            catch (Exception ex)
            {
                return Result.Fail<Item>(ex.Message);
            }
        }
    }
}