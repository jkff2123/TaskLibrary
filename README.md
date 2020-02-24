# TaskLibrary
 Task handing library
# How to use
 1. Get TaskManager instance. This is single tone pattern.
 2. Add event function in the event handler OnFinish
  ex) taskmanager.Onfinish += event;
 3. Make a Task instance and Add it to the Taskmanager instance. The task starts if it is not begun when it is added in the Taskmanager.
  ex) taskmanager.AddTask(task);
 4. When the task is finished the event function is triggered.
 5. Wait for a task and wait for all tasks are supported.
