using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config
{
	public class Giveaway
	{
		RegistryKey mainKey;
		RegistryKey giveawayKey;

		public Giveaway()
		{
			// Mainkey
			mainKey = Registry.CurrentUser.OpenSubKey("BlackBoxBot", true);
			if (mainKey == null)
			{
				mainKey = Registry.CurrentUser.CreateSubKey("BlackBoxBot", RegistryKeyPermissionCheck.ReadWriteSubTree);
			}

			// Giveaway 
			giveawayKey = mainKey.OpenSubKey("Giveaway", true);
			if (giveawayKey == null)
			{
				giveawayKey = mainKey.CreateSubKey("Giveaway", RegistryKeyPermissionCheck.ReadWriteSubTree);

				// Command Entry
				giveawayKey.SetValue("Command", "");
				giveawayKey.SetValue("Active", "false");
			}
		}

		public void SaveSetting(string RegKey, object value)
		{
			giveawayKey.SetValue(RegKey, value);
		}

		public string LoadSetting(string RegKey)
		{
			return (string)giveawayKey.GetValue(RegKey);
		}
	}
}
