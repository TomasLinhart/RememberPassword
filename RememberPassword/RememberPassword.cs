using System;

using ScrollsModLoader.Interfaces;
using UnityEngine;
using Mono.Cecil;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace RememberPassword
{
	public class RememberPassword : BaseMod
	{
		private byte[] KEY = { 0x41, 0x65, 0x70, 0x6a, 0x4b, 0x63, 
			0x6a, 0x45, 0x6a, 0x44, 0x6f, 0x74, 0x56, 0x32, 0x6b, 0x41, 0x64, 0x73, 0x38, 0x6d 
		};
		private byte[] IV = { 0x46, 0x58, 0x34, 0x75, 0x45, 0x73, 
			0x70, 0x65, 0x69, 0x6f, 0x47, 0x69, 0x4b, 0x75, 0x71, 0x45, 0x33, 0x65, 0x50, 0x52
		};
		private const string PASSWORD_FILENAME = "/password.txt";

		public RememberPassword()
		{
		}

		public static string GetName()
		{
			return "RememberPassword";
		}

		public static int GetVersion()
		{
			return 1;
		}

		public static MethodDefinition[] GetHooks(TypeDefinitionCollection scrollsTypes, int version)
		{
			try
			{
				return new MethodDefinition[] {
					scrollsTypes["Login"].Methods.GetMethod("loadSettings")[0],
					scrollsTypes["Login"].Methods.GetMethod("saveSettings")[0],
				};
			} 
			catch
			{
				return new MethodDefinition[] {};
			}
		}

		public override bool BeforeInvoke(InvocationInfo info, out object returnValue)
		{
			returnValue = null;
			return false;
		}

		public override void AfterInvoke(InvocationInfo info, ref object returnValue)
		{
			var filename = Application.persistentDataPath + PASSWORD_FILENAME;

			if (info.targetMethod.Equals("loadSettings")) {
				LoginWrapper login = new LoginWrapper((Login) info.target);

				if (login.RememberMeChecked) {
					var fileContent = FileUtils.ReadFileContent(filename);
					if (!string.IsNullOrEmpty(fileContent)) {
						var bytes = fileContent.HexToBytes();
						login.Password = AesCryptoServiceHelper.DecryptStringFromBytes(bytes, KEY, IV);
						//login.Login(); // connection fails for some unknown reason
					}
				}
			}

			if (info.targetMethod.Equals("saveSettings")) {
				LoginWrapper login = new LoginWrapper((Login) info.target);

				if (login.RememberMeChecked) {
					if (!string.IsNullOrEmpty(login.Password)) {
						var bytes = AesCryptoServiceHelper.EncryptStringToBytes(login.Password, KEY, IV);
						FileUtils.WriteFileContent(filename, String.Concat(Array.ConvertAll(bytes, x => x.ToString("X2"))));
					}
				}
			}

			return;
		}

	}
}

