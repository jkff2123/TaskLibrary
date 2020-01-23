using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TaskLibrary
{
    public class TaskManager : IDisposable
    {
        private bool isRunning;
        /// <summary>
        /// Taskmanager running checker
        /// </summary>
        public bool IsRunning { get { return isRunning; } }
        /// <summary>
        /// Running task list
        /// </summary>
        private List<Task> tasks;
        /// <summary>
        /// Task finish event
        /// </summary>
        public event EventHandler<TaskEventOnFinish> OnFinish;

        private bool disposed = false;
        /// <summary>
        /// Lazy initialization
        /// </summary>
        private static readonly Lazy<TaskManager> taskManager = new Lazy<TaskManager>(() => new TaskManager());
        /// <summary>
        /// Singletone instance
        /// </summary>
        public static TaskManager Instance { get { return taskManager.Value; } }


        private TaskManager()
        {
            isRunning = false;
            tasks = new List<Task>();
        }

        /// <summary>
        /// Add task in the taskmanager.
        /// </summary>
        /// <param name="newTask">New task item</param>
        public void AddTask(Task newTask)
        {
            try
            {
                if (newTask.Status != TaskStatus.Running)
                {
                    newTask.Start();
                }

                tasks.Add(newTask);

                if (!isRunning)
                {
                    isRunning = true;
                    CheckTask();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Add task that has a result in the taskmanager.
        /// </summary>
        /// <typeparam name="T">Result type</typeparam>
        /// <param name="newTask">New task item having result</param>
        public void AddTask<T>(Task<T> newTask)
        {
            AddTask(newTask as Task);
        }

        /// <summary>
        /// Add tasks in the taskmanager.
        /// </summary>
        /// <param name="newTasks">New task array</param>
        public void AddTask(Task[] newTasks)
        {
            foreach (var e in newTasks)
            {
                AddTask(e);
            }
        }

        /// <summary>
        /// Wait for a task.
        /// </summary>
        /// <param name="task">Target task to wait</param>
        public void WaitFor(Task task)
        {
            if (!tasks.Contains(task))
            {
                return;
            }

            task.Wait();

            return;
        }

        /// <summary>
        /// Wait for a task having specific ID.
        /// </summary>
        /// <param name="id">Target task ID to wait</param>
        public bool Waitfor(int id)
        {
            var target = (from e in tasks
                         where e.Id == id
                         select e).ToArray();

            if(target.Length == 0)
            {
                return false;
            }

            target[0].Wait();

            return true;
        }

        /// <summary>
        /// Wait for all tasks in the taskmanager.
        /// </summary>
        public bool WaitForAll()
        {
            if(tasks.Count == 0)
            {
                return false;
            }

            while (isRunning)
            {
                Task.Delay(10).Wait();
            }

            return true;
        }

        /// <summary>
        /// Task finish event calling function.
        /// </summary>
        private void CallFinishEvent(Task finishedTask)
        {
            OnFinish(this, new TaskEventOnFinish(finishedTask));
        }

        /// <summary>
        /// Finished task checking function starter.
        /// </summary>
        private async void CheckTask()
        {
            await Task.Run(() => OnCheckProcess());
        }

        /// <summary>
        /// Finished task checking function.
        /// </summary>
        private void OnCheckProcess()
        {
            while (tasks.Count != 0)
            {
                var index = Task.WaitAny(tasks.ToArray());
                CallFinishEvent(tasks[index]);
                tasks.RemoveAt(index);

                Task.Delay(10).Wait();
            }

            isRunning = false;
        }

        // Dispose pattern
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool IsDisposing)
        {
            if (disposed)
            {
                return;
            }

            if (IsDisposing)
            {
                foreach(var e in tasks)
                {
                    e.Dispose();
                }
            }

            disposed = true;
        }
    }

    //public class CancelableTask<T> : Task<T>
    //{
    //    private CancellationTokenSource tokenSource;
    //    public CancellationTokenSource TokenSource { get { return tokenSource; } }

    //    public CancelableTask(Func<T> function, CancellationTokenSource cancellationTokenSource)
    //        : base(function, cancellationTokenSource.Token)
    //    {
    //        tokenSource = cancellationTokenSource;
    //    }
    //}
}
