using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bubbleshot.Server.Adapters.Pcl.Base
{
	public class PollingManager
	{
		private TimeSpan _interval;
		private Action _action;
		private CancellationToken _cancellationToken;
		private CancellationTokenSource _cancellationTokenSource;
		private Task _task;

		public PollingManager()
		{
			
		}

		public void Start(TimeSpan interval, Action action)
		{
			_cancellationTokenSource = new CancellationTokenSource();
			_cancellationToken = _cancellationTokenSource.Token;
			CreateTask(interval, action, _cancellationToken);
		}

		public void Stop()
		{
			_cancellationTokenSource.Cancel();
		}

		private void CreateTask(TimeSpan interval, Action action, CancellationToken cancellationToken)
		{

			Task.Factory.StartNew(
				() =>
				{
					for (;;)
					{
						if (cancellationToken.WaitHandle.WaitOne(interval))
							break;

						action();
					}
				}, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
			return;
		}
	}
}
