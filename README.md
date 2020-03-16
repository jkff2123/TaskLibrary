# TaskManager
 Task handing library
# Release Note
 ## 1.1.0.0
  1. Added CancelTask function. It is available only when you add task with CancellationTokenSource.
  2. Added AddTask function with Task and CancellationTokenSource.
  3. Bugfix.
 ## 1.0.1.1
  1. Changed namespace to 'JK.TaskLibrary'.
# How to use
 1. Single tone pattern is used. Get instance from TaskManager.Instance.
 2. Add event function in the event handler OnFinish
  ex) taskmanager.Onfinish += event;
 3. Make a Task instance and Add it to the Taskmanager instance. The task starts if it is not begun when it is added in the Taskmanager.
  ex) taskmanager.AddTask(task);
 4. When the task is finished the event function is triggered.
 5. Wait for a task and wait for all tasks are supported.
