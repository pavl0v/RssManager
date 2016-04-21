using System;

namespace RssManager.DesktopApp
{
    public class TaskCompleteEventArgs : EventArgs
    {
        private string message = string.Empty;
        private TaskCompleteState completeState = TaskCompleteState.Completed;

        public string Message
        {
            get { return this.message; }
        }

        public TaskCompleteState CompleteState
        {
            get { return this.completeState; }
        }

        public TaskCompleteEventArgs(TaskCompleteState completeState, string message)
        {
            this.message = message;
            this.completeState = completeState;
        }
    }
}
