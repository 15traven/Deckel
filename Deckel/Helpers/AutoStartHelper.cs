using Microsoft.Win32.TaskScheduler;
using Task = Microsoft.Win32.TaskScheduler.Task;

namespace Deckel.Helpers
{
    public static class AutoStartHelper
    {
        private const string TaskFolderName = "Deckel";
        private static readonly string TaskName = $"Autorun for {UserName}";
        private static readonly string ExecutablePath = Environment.ProcessPath;
        private static readonly string UserName = Environment.UserName;
        private static readonly string UserDomainName = $"{Environment.UserDomainName}\\{UserName}";

        private static readonly TaskService _taskService = new();

        public static void CreateAutoStartTask()
        {
            // ------------------------------------------------------------
            // Get the Deckel task folder. Creates it if it doesn't exist.
            TaskFolder taskFolder = _taskService.GetFolder(TaskFolderName);
            taskFolder ??= _taskService.RootFolder.CreateFolder(TaskFolderName);

            // If the task exists, just enable it.
            if (taskFolder.GetTasks().Exists(TaskName))
                return;

            // ------------------------------------------------------------
            // Create the task
            TaskDefinition taskDefinition = _taskService.NewTask();
            taskDefinition.RegistrationInfo.Author = UserDomainName;

            // Create the settings for the task
            taskDefinition.Settings.StartWhenAvailable = false;
            taskDefinition.Settings.StopIfGoingOnBatteries = false;
            taskDefinition.Settings.ExecutionTimeLimit = TimeSpan.Zero;
            taskDefinition.Settings.DisallowStartIfOnBatteries = false;
            taskDefinition.Settings.Priority = System.Diagnostics.ProcessPriorityClass.Normal;

            // ------------------------------------------------------------
            // Add the logon trigger to the task
            LogonTrigger logonTrigger = new()
            {
                Id = "Trigger1",
                UserId = UserDomainName,
                Delay = TimeSpan.FromSeconds(3)
            };
            taskDefinition.Triggers.Add(logonTrigger);

            // ------------------------------------------------------------
            // Add an action to the task.
            ExecAction execAction = new()
            { 
                Path = ExecutablePath
            };
            taskDefinition.Actions.Add(execAction);

            // ------------------------------------------------------------
            // Create the principal for the task.
            taskDefinition.Principal.Id = "Principal1";
            taskDefinition.Principal.UserId = UserDomainName;
            taskDefinition.Principal.LogonType = TaskLogonType.InteractiveToken;
            taskDefinition.Principal.RunLevel = TaskRunLevel.LUA;

            // ------------------------------------------------------------
            // Save the task in the Deckel folder.
            taskFolder.RegisterTaskDefinition(TaskName, taskDefinition);
        }

        public static bool IsAutoStartTaskEnabled()
        {
            TaskFolder taskFolder = _taskService.GetFolder(TaskFolderName);
            if (!taskFolder.GetTasks().Exists(TaskName))
                return false;

            return taskFolder.Tasks[TaskName].Enabled;
        }

        public static void ToggleAutoStartTask()
        {
            TaskFolder taskFolder = _taskService.GetFolder(TaskFolderName);
            if (!taskFolder.GetTasks().Exists(TaskName))
                return;

            bool isEnabled = taskFolder.Tasks[TaskName].Enabled;
            taskFolder.Tasks[TaskName].Enabled = !isEnabled;
        }
    }
}
