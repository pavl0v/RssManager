using RssManager.DesktopApp.Dialogs.DialogFacade;
using RssManager.Interfaces.DTO;
using RssManager.Objects.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RssManager.DesktopApp.ViewModels
{
    public class SignInViewModel
    {
        #region FIELDS

        private IDialogFacade dialogFacade = null;
        private Models.SignInModel model = null;
        private RelayCommandAsync cmdOnSignIn = null;
        private string username = string.Empty;
        private string password = string.Empty;

        #endregion

        #region PROPERTIES

        public RelayCommandAsync CmdOnSignIn
        {
            get { return this.cmdOnSignIn; }
            set { this.cmdOnSignIn = value; }
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

        #endregion

        #region CONSTRUCTOR

        public SignInViewModel(IDialogFacade dialogFacade)
        {
            this.dialogFacade = dialogFacade;
            this.model = new Models.SignInModel();
            this.cmdOnSignIn = new RelayCommandAsync(new Action<object>(OnSignIn));
            this.cmdOnSignIn.Completed += cmdOnSignIn_Completed;
        }

        #endregion

        private void cmdOnSignIn_Completed(object sender, EventArgs e)
        {
            TaskCompleteEventArgs args = e as TaskCompleteEventArgs;
            if (args != null)
            {
                switch (args.CompleteState)
                {
                    case TaskCompleteState.Error:
                        System.Diagnostics.Debug.WriteLine("Completed with error: " + args.Message);
                        this.dialogFacade.ShowDialogYesNo(
                            args.Message, 
                            new DialogWindowProperties {
                                Owner = args.Parameter as System.Windows.Window,
                                ResizeMode = System.Windows.ResizeMode.NoResize,
                                Title = "Authentication"
                            });
                        break;
                    case TaskCompleteState.Completed:
                        System.Diagnostics.Debug.WriteLine("Completed successfully");
                        this.dialogFacade.ShowDialogYesNo(
                            "Authentication completed successfully",
                            new DialogWindowProperties
                            {
                                Owner = args.Parameter as System.Windows.Window,
                                ResizeMode = System.Windows.ResizeMode.NoResize,
                                Title = "Authentication"
                            });
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
    }
}
