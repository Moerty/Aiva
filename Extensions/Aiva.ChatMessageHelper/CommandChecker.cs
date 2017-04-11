using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Events.Client;
using static ChatMessageHelper.CommandChecker.Extension;

namespace ChatMessageHelper
{
	public class CommandChecker
	{
		private Config.Giveaway giveawayConfig;
		private Models.ReturnModel.returnModel returnModel;

		public CommandChecker(string language)
		{
			this.giveawayConfig = new Config.Giveaway();
		}

		public Models.ReturnModel.returnModel CheckCommand(string command)
		{

			new Switch<string>(command)
					.Case(s => s == Config.Bankheist.Config["General"]["Command"].ToLower(), s =>
					{
						returnModel = Models.ReturnModel.returnModel.Bankheist;
					})
                    .Case(s => s.CompareTo(Config.Songrequest.Config["General"]["Command"]) == 0, s =>
                    {
                        returnModel = Models.ReturnModel.returnModel.Songrequest;
                    })
                    .Case(s => s.CompareTo(Config.Songrequest.Config["UserCommands"]["GetSongCommand"]) == 0, s =>
					{
						returnModel = Models.ReturnModel.returnModel.SongrequestInfo;
					})
					.Case(s => s.EndsWith(giveawayConfig.LoadSetting("Command")), s =>
					{
						returnModel = Models.ReturnModel.returnModel.Giveaway;
					})
                    .Case(s => s.CompareTo(giveawayConfig.LoadSetting("Command")) == 0, s =>
                    {
                        returnModel = Models.ReturnModel.returnModel.Giveaway;
                    })
                    .Default(s =>
					{
						returnModel = Models.ReturnModel.returnModel.NotDefinedError;
					});

			return returnModel;
		}

		public static class Extension
		{
			public class Switch<T>
			{
				public Switch(T o)
				{
					Object = o;
				}

				public T Object { get; private set; }
			}
		}
	}

	public static class SwitchExtensions
	{
		public static Switch<T> Case<T>(this Switch<T> switchObject, T type, Action<T> action)
		{
			return Case<T>(switchObject, type, action, false);
		}

		public static Switch<T> Case<T>(this Switch<T> switchObject, Func<T, bool> function, Action<T> action)
		{
			return Case(switchObject, function, action, false);
		}

		public static Switch<T> Case<T>(this Switch<T> switchObject, T type, Action<T> action,
			bool fallThrough)
		{
			return Case<T>(switchObject, x => object.Equals(x, type), action, fallThrough);
		}

		public static Switch<T> Case<T>(this Switch<T> switchObject, Func<T, bool> function,
			Action<T> action, bool fallThrough)
		{
			if (switchObject == null)
			{
				return null;
			}
			else if (function(switchObject.Object))
			{
				action(switchObject.Object);
				return fallThrough ? switchObject : null;
			}

			return switchObject;
		}

		public static void Default<T>(this Switch<T> switchObject, Action<T> action)
		{
			if (switchObject != null)
			{
				action(switchObject.Object);
			}
		}
	}
}
