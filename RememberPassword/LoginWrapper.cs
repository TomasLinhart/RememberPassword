using System;
using System.Reflection;

namespace RememberPassword
{
	public class LoginWrapper
	{
		private Login login;
		private FieldInfo usernameFieldInfo;
		private FieldInfo passwordFieldInfo;
		private FieldInfo rememberMeCheckedFieldInfo;
		private MethodInfo loginMethodInfo;

		public LoginWrapper(Login login)
		{
			this.login = login;

			rememberMeCheckedFieldInfo = login.GetType().GetField("_rememberMeChecked", BindingFlags.NonPublic | BindingFlags.Instance);
			usernameFieldInfo = login.GetType().GetField("username", BindingFlags.NonPublic | BindingFlags.Instance);
			passwordFieldInfo = login.GetType().GetField("password", BindingFlags.NonPublic | BindingFlags.Instance);
			loginMethodInfo = login.GetType().GetMethod("login", BindingFlags.NonPublic | BindingFlags.Instance); 
		}

		public bool RememberMeChecked
		{
			get {
				Object rememberMeChecked = rememberMeCheckedFieldInfo.GetValue(login);
				return rememberMeChecked != null ? (bool) rememberMeChecked : false;
			}
		}

		public string Username
		{
			get {
				Object username = usernameFieldInfo.GetValue(login);
				return username != null ? (string) username : null;
			}
		}

		public string Password
		{
			get {
				Object password = passwordFieldInfo.GetValue(login);
				return password != null ? (string) password : null;
			}

			set {
				passwordFieldInfo.SetValue(login, value);
			}
		}

		public void Login()
		{
			loginMethodInfo.Invoke(login, null);
		}
	}
}

