// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace LudoClient
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSButton LoginButton { get; set; }

		[Outlet]
		AppKit.NSButton LogoutButton { get; set; }

		[Outlet]
		AppKit.NSButton OnClickLogoutButton { get; set; }

		[Outlet]
		AppKit.NSSecureTextField PasswordField { get; set; }

		[Outlet]
		AppKit.NSTextField ResponseLabel { get; set; }

		[Outlet]
		AppKit.NSTextField UsernameField { get; set; }

		[Action ("OnClickLoginButton:")]
		partial void OnClickLoginButton (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (OnClickLogoutButton != null) {
				OnClickLogoutButton.Dispose ();
				OnClickLogoutButton = null;
			}

			if (LogoutButton != null) {
				LogoutButton.Dispose ();
				LogoutButton = null;
			}

			if (LoginButton != null) {
				LoginButton.Dispose ();
				LoginButton = null;
			}

			if (PasswordField != null) {
				PasswordField.Dispose ();
				PasswordField = null;
			}

			if (ResponseLabel != null) {
				ResponseLabel.Dispose ();
				ResponseLabel = null;
			}

			if (UsernameField != null) {
				UsernameField.Dispose ();
				UsernameField = null;
			}
		}
	}
}
