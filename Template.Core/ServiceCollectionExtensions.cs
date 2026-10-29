using Microsoft.Extensions.DependencyInjection;
using Template.Core.Items;
using Template.Domain.Items;

namespace Template.Core;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddTemplateServices()
        {
            services.AddScoped<IQueryBus, QueryBus>();
            services.AddScoped<ICommandBus, CommandBus>();

            services.AddScoped<IQueryHandler<GetItem.ById, Item>, GetItem.Handler>();
            services.AddScoped<IQueryHandler<GetItem.ByName, Item>, GetItem.Handler>();
            services.AddScoped<ICommandHandler<AddItem.Command, Item>, AddItem.Handler>();


            return services;
        }
    }
}