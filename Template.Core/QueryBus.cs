using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using Template.Domain;

namespace Template.Core;

public interface IQueryBus
{
    Task<Result<TResult>> Send<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default) where TQuery : IQuery<TResult>;
}

public interface ICommandBus
{
    Task<Result> Send<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand;
    Task<Result<TResult>> Send<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand<TResult>;
}

public class QueryBus(IServiceProvider serviceProvider) : IQueryBus
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    public async Task<Result<TResult>> Send<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default) where TQuery : IQuery<TResult>
    {
        using var span = Observability.ActivitySource.StartActivity($"{nameof(QueryBus)}.{nameof(Send)}");
        span?.SetTag("query.type", typeof(TQuery).FullName);
        var handler = _serviceProvider.GetService<IQueryHandler<TQuery, TResult>>();
        if (handler == null)
        {
            span?.SetStatus(System.Diagnostics.ActivityStatusCode.Error, $"No handler registered for query type {typeof(TQuery).FullName}");
            throw new InvalidOperationException($"No handler registered for query type {typeof(TQuery).FullName}");
        }
        var result = await handler.Handle(query, cancellationToken);
        span?.SetTag("query.result", result.IsSuccess ? "success" : "failure");
        return result;
    }
}

public class CommandBus(IServiceProvider serviceProvider) : ICommandBus
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<Result> Send<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand
    {
        using var span = Observability.ActivitySource.StartActivity($"{nameof(CommandBus)}.{nameof(Send)}");
        span?.SetTag("command.type", typeof(TCommand).FullName);
        var handler = _serviceProvider.GetService<ICommandHandler<TCommand>>();
        if (handler == null)
        {
            span?.SetStatus(System.Diagnostics.ActivityStatusCode.Error, $"No handler registered for command type {typeof(TCommand).FullName}");
            throw new InvalidOperationException($"No handler registered for command type {typeof(TCommand).FullName}");
        }
        var result = await handler.Handle(command, cancellationToken);
        span?.SetTag("command.result", result.IsSuccess ? "success" : "failure");
        return result;
    }

    public async Task<Result<TResult>> Send<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand<TResult>
    {
        using var span = Observability.ActivitySource.StartActivity($"{nameof(CommandBus)}.{nameof(Send)}");
        span?.SetTag("command.type", typeof(TCommand).FullName);
        var handler = _serviceProvider.GetService<ICommandHandler<TCommand, TResult>>();
        if (handler == null)
        {
            span?.SetStatus(System.Diagnostics.ActivityStatusCode.Error, $"No handler registered for command type {typeof(TCommand).FullName}");
            throw new InvalidOperationException($"No handler registered for command type {typeof(TCommand).FullName}");
        }
        var result = await handler.Handle(command, cancellationToken);
        span?.SetTag("command.result", result.IsSuccess ? "success" : "failure");
        return result;
    }

}

public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<Result<TResult>> Handle(TQuery query, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<Result<TResult>> Handle(TCommand command, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task<Result> Handle(TCommand command, CancellationToken cancellationToken = default);
}

public interface IQuery<TResult>;

public interface ICommand;
public interface ICommand<TResult>;