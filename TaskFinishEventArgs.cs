using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaskLibrary
{
    public class TaskEventOnFinish : EventArgs
    {
        public TaskEventOnFinish(Task task)
        {
            FinishedTask = task;
        }

        public Task FinishedTask { get; private set; }
    }
}
