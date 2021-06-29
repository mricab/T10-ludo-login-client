using System;
using GameClient;

using AppKit;
using Foundation;

namespace LudoClient
{
    public partial class ViewController : NSViewController
    {
        private GClient GClient;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
            GClient = new GClient(9999);
            GClient.Start();
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }

        partial void OnClickLoginButton(Foundation.NSObject sender)
        {
            LoginButton.Enabled = true;
            string[] credentials = new string[] { UsernameField.StringValue, PasswordField.StringValue };
            GClient.Send("login", credentials);
        }
    }
}