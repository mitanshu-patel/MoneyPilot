namespace MoneyPilot.Shared.Contracts
{
    public interface IMediator
    {
        Task<TResult> SendAsync<TCommand, TResult>(TCommand command) where TCommand : class;
    }
}
