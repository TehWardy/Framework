using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using Voxels.Objects;

namespace System.Threading.Tasks
{
	public delegate void TaskCompleteEventHandler<T>(Task<T> task);
	public delegate void TaskCompleteEventHandler(Task task);

	public sealed class TaskFactory {

		public static TaskFactory Instance { get; private set; }

		Timer timer;
		List<BackgroundWorker> runningThreads;
		List<BackgroundWorker> idleThreads;
        List<Task> queue = new List<Task>(10000);

		static TaskFactory()
		{
			Instance = new TaskFactory();
		}

		TaskFactory()
		{
			Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;
			lock (queue) {
				runningThreads = new List<BackgroundWorker> ();
				idleThreads = new List<BackgroundWorker> ();
				var cores = SystemInfo.processorCount;

				for (int i = 0; i < cores; i++)
					idleThreads.Add (CreateThread());
			}

			// set timer to kick off any new processing once per frame.
			timer = new Timer((obj) => { ProcessNextTask(); }, null, 16, 16);
		}

		public Task StartNew(Action task)
		{
            var newTask = new Task(task);
            lock (queue) {
				queue.Add (newTask);
				queue.Sort((t1, t2) => t1.ExecutionTime.CompareTo(t2.ExecutionTime));
			}
            return newTask;
		}

        public Task<T> StartNew<T>(Func<T> task)
		{
			var newTask = new Task<T>(task);
			lock (queue) {
				queue.Add (newTask);
                queue.Sort((t1, t2) => t1.ExecutionTime.CompareTo(t2.ExecutionTime));
			}
			return newTask;
		}

		void ProcessNextTask()
		{
			lock (queue)
			{
				if(queue.Any () && idleThreads.Any())
				{
					var task = queue.FirstOrDefault(t => t.ExecutionTime < DateTime.Now);

					if (task != null)
					{
						var thread = idleThreads.First();
						idleThreads.Remove(thread);
						runningThreads.Add(thread);
						queue.Remove(task);

						Debug.Log ("Running Task");
						thread.RunWorkerAsync(task);
					}
				}
			}
		}

		void ClearQueue()
		{
			lock (queue)
				queue.Clear();
		}

		BackgroundWorker CreateThread()
		{
			var thread = new BackgroundWorker();

			thread.DoWork += (object sender, DoWorkEventArgs e) =>
			{
				Task t = e.Argument as Task;
				t.Execute();
			};

			thread.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) =>
			{
				lock (queue)
				{
					runningThreads.Remove(thread);
					idleThreads.Add(thread);
				}
			};

			return thread;
		}
	}
}
