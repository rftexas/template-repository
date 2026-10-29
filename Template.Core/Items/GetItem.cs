namespace Template.Core.Items;

using Template.Domain.Items;
using FluentResults;
using Template.Persistence;
using Microsoft.EntityFrameworkCore;
using Template.Domain.Items.Specifications;
using Template.Domain;

public class GetItem
{
    public record ById : IQuery<Item>
    {
        public ItemId ItemId { get; private set; }

        public static ById From(Guid itemId) => new() { ItemId = ItemId.From(itemId) };
    }

    public record ByName(string ItemName) : IQuery<Item>;

    public class Handler(DataContext dataContext)
        : IQueryHandler<ById, Item>,
          IQueryHandler<ByName, Item>
    {
        private readonly DataContext _dataContext = dataContext;

        public async Task<Result<Item>> Handle(ById command, CancellationToken cancellationToken = default)
        {
            using var span = Observability.ActivitySource.StartActivity($"{nameof(GetItem)}.{nameof(Handle)}");
            var item = await _dataContext.Items.FirstOrDefaultAsync(new MatchId(command.ItemId).Expression, cancellationToken);
            if (item is null)
            {
                return Result.Fail<Item>("Item not found");
            }

            return Result.Ok(item);
        }

        public async Task<Result<Item>> Handle(ByName command, CancellationToken cancellationToken = default)
        {
            using var span = Observability.ActivitySource.StartActivity($"{nameof(GetItem)}.{nameof(Handle)}");
            var item = await _dataContext.Items.FirstOrDefaultAsync(new MatchItemName(command.ItemName).Expression, cancellationToken);
            if (item is null)
            {
                return Result.Fail<Item>("Item not found");
            }

            return Result.Ok(item);
        }
    }
}