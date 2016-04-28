using System;

namespace RssManager.DesktopApp
{
    public class TaskCompleteEventArgs : EventArgs
    {
        private string message = string.Empty;
        private TaskCompleteState completeState = TaskCompleteState.Completed;
        private object parameter = null;

        public string Message
        {
            get { return this.message; }
        }

        public TaskCompleteState CompleteState
        {
            get { return this.completeState; }
        }

        public object Parameter
        {
            get { return this.parameter; }
        }

        public TaskCompleteEventArgs(TaskCompleteState completeState, string message)
        {
            //this.message = message;
            //this.completeState = completeState;
            new TaskCompleteEventArgs(completeState, message, null);
        }

        public TaskCompleteEventArgs(TaskCompleteState completeState, string message, object parameter)
        {
            this.message = message;
            this.completeState = completeState;
            this.parameter = parameter;
        }
    }
}
