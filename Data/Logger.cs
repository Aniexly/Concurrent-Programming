using System.Collections.Concurrent;
using System.Text;

namespace Data
{
    public class Logger : ILogger
    {
        private readonly string _filePath;
        private readonly CancellationToken _cancellationToken;
        private readonly BlockingCollection<string> _logsQueue = new BlockingCollection<string>();
        private readonly Lock _fileLock = new Lock();

        public Logger(string filePath, CancellationToken cancellationToken)
        {
            _filePath = filePath;
            _cancellationToken = cancellationToken;
            Task.Run(WriteLoop, CancellationToken.None);
        }

        private void WriteLoop()
        {
            while (!_logsQueue.IsCompleted)
            {
                try
                {
                    string log = _logsQueue.Take(_cancellationToken);
                    AppendFile(log);
                }
                catch (OperationCanceledException)
                {
                    CompleteAndDrainQueue();
                    break;
                }
                catch (InvalidOperationException)
                {
                    break;
                }
                catch (IOException)
                {
                }
                catch (UnauthorizedAccessException)
                {
                }
            }
        }

        private void AppendFile(string log)
        {
            lock (_fileLock)
            {
                File.AppendAllText(_filePath, log + Environment.NewLine, Encoding.ASCII);
            }
        }

        private void CompleteAndDrainQueue()
        {
            _logsQueue.CompleteAdding();
            while (_logsQueue.TryTake(out string? log))
            {
                try
                {
                    AppendFile(log);
                }
                catch (IOException)
                {
                }
                catch (UnauthorizedAccessException)
                {
                }
            }
        }

        public async Task LogBallEventAsync(IBall ball, string eventType)
        {
            await Task.Run(() => LogBallEvent(ball, eventType), _cancellationToken);
        }

        private void LogBallEvent(IBall ball, string eventType)
        {
            string logEntry = $"{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ss.fff} " +
                              $"({Thread.CurrentThread.ManagedThreadId}) " +
                              $"Event={eventType}; " +
                              $"Ball={ball}";
            try
            {
                _logsQueue.Add(logEntry, _cancellationToken);
            }
            catch (OperationCanceledException)
            {
                _logsQueue.CompleteAdding();
            }
            catch (InvalidOperationException)
            {
            }
        }
    }
}
