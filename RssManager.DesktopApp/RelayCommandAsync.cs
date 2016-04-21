using System;
using System.Threading.Tasks;

namespace RssManager.DesktopApp
{
    public class RelayCommandAsync : RelayCommand
    {
        public event EventHandler Started = null;
        public event EventHandler Completed = null;

        #region Private members

        private bool isExecuting = false;

        #endregion

        public bool IsExecuting
        {
            get { return this.isExecuting; }
        }

        public RelayCommandAsync(Action execute)
            : base(execute)
        {

        }

        public RelayCommandAsync(Action execute, Func<bool> canExecute)
            : base (execute, canExecute)
        {

        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter) && !this.isExecuting;
        }

        public override void Execute(object parameter)
        {
            try
            {
                this.isExecuting = true;
                if (this.Started != null)
                    this.Started(this, EventArgs.Empty);
                Task task = new Task(() => { this.execute(); });
                task.ContinueWith(t => 
                {
                    if (t.IsFaulted)
                    {
                        this.OnCompleted(new TaskCompleteEventArgs(TaskCompleteState.Error, t.Exception.InnerExceptions[0].Message));
                        return;
                    }
                    this.OnCompleted(new TaskCompleteEventArgs(TaskCompleteState.Completed, "OK")); 
                });
                task.Start();
            }
            catch (Exception ex)
            {
                this.OnCompleted(new TaskCompleteEventArgs(TaskCompleteState.Error, ex.Message));
            }
        }

        private void OnCompleted(EventArgs e)
        {
            this.isExecuting = false;
            if (this.Completed != null)
                this.Completed(this, e);
        }
    }
}
