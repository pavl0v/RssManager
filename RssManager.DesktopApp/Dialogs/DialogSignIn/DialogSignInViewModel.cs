#define MYDEBUG

using RssManager.DesktopApp.Dialogs.DialogFacade;
using RssManager.Interfaces.DTO;
using RssManager.Objects.BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace RssManager.DesktopApp.Dialogs.DialogSignIn
{
    class DialogSignInViewModel : DialogViewModelBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region FIELDS 

        private Models.SignInModel model = null;
        private RelayCommandAsync signInCommand = null;
        private RelayCommand keyDownCommand = null;
        private RelayCommand closeCommand = null;
        private string username = string.Empty;
        private string password = string.Empty;
        private bool isErrorLabelVisible = false;

        #endregion

        #region PROPERTIES

        public RelayCommandAsync SignInCommand
        {
            get { return this.signInCommand; }
            set { this.signInCommand = value; }
        }

        public RelayCommand KeyDownCommand
        {
            get { return this.keyDownCommand; }
            set { this.keyDownCommand = value; }
        }

        public RelayCommand CloseCommand
        {
            get { return this.closeCommand; }
            set { this.closeCommand = value; }
        }

        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

        public string Password
        {
            get { return this.password; }
            set { this.password = value; }
        }

        public bool IsErrorLabelVisible
        {
            get { return this.isErrorLabelVisible; }
            set
            {
                if (this.isErrorLabelVisible != value)
                {
                    this.isErrorLabelVisible = value;
                    this.OnPropertyChanged("IsErrorLabelVisible");
                }
            }
        }

        #endregion

        #region CONSTRUCTORS

        public DialogSignInViewModel(string message)
            : base(message)
        {
#if(MYDEBUG)
            this.username = "demo";
            this.password = "demo";
#endif
            this.model = new Models.SignInModel();
            this.keyDownCommand = new RelayCommand(OnGotFocus);
            this.closeCommand = new RelayCommand(OnClose);
            this.signInCommand = new RelayCommandAsync(new Action<object>(OnSignIn));
            this.signInCommand.Completed += SignInCommand_Completed;
        }

        #endregion

        private void SignInCommand_Completed(object sender, EventArgs e)
        {
            TaskCompleteEventArgs args = e as TaskCompleteEventArgs;
            if (args != null)
            {
                switch (args.CompleteState)
                {
                    case TaskCompleteState.Error:
                        System.Diagnostics.Debug.WriteLine("Completed with error: " + args.Message);
                        this.IsErrorLabelVisible = true;
                        break;

                    case TaskCompleteState.Completed:
                        System.Diagnostics.Debug.WriteLine("Completed successfully");
                        this.IsErrorLabelVisible = false;
                        this.CloseDialogWithResult(args.Parameter as Window, DialogResult.Ok);
                        break;
                }
                return;
            }

            System.Diagnostics.Debug.WriteLine("Completed successfully");
        }

        private void OnSignIn(object parameter)
        {
            TokenDTO t = this.model.Auth(this.username, this.password);
            Token token = Token.GetInstance();
            token.AccessToken = t.AccessToken;
            token.ExpiresIn = t.ExpiresIn;
            token.TokenType = t.TokenType;
        }

        private void OnGotFocus(object parameter)
        {
            this.IsErrorLabelVisible = false;
        }

        private void OnClose(object parameter)
        {
            Window win = parameter as Window;
            if (win != null)
                win.Close();
        }
        
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
