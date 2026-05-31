namespace Data
{
    public interface ILogger
    {
        public Task LogBallEventAsync(IBall ball, string eventType);
    }
}
