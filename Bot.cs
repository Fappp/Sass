using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace Sass
{
	internal class Bot
	{
		private DiscordClient m_client;

		public void Start ()
		{
			if ( m_client != null )
			{
				throw new Exception ( "Bot is already started" );
			}

			// Load the config file.
			try
			{
				Config.Open ( "config.json" );
			}
			catch ( ConfigException e )
			{
				Console.WriteLine ( $"Failed to open config.json: {e.Message}" );
				return;
            }

			// Create client.
			m_client = new DiscordClient ( ( conf ) =>
			{
				conf.LogLevel = LogSeverity.Info;
				conf.LogHandler = OnLogHandler;
			} );

			// Set event handlers.
			var modMan = new ModuleManager ( m_client );
			modMan.AddModule<Modules.PingPong> ();
			modMan.AddModule<Modules.Hi> ();

			m_client.ExecuteAndWait ( async () =>
			{
				await m_client.Connect ( Config.Instance.Get<string> ( "Token", "" ) );
				m_client.SetGame ( Config.Instance.Get<string> ( "Game" ) );
			} );
		}

		private void OnLogHandler ( object sender, LogMessageEventArgs e )
		{
			Console.WriteLine ( "DN[{0}] {1}: {2}", e.Severity, e.Source, e.Message );
			if ( e.Exception != null )
			{
				Console.WriteLine ( "Something done goofed" );
				Console.WriteLine ( e.Exception );
			}
		}

		//TODO: Reimplement these 

		/*
		private void OnErrorHandler ( object sender, CommandErrorEventArgs e )
		{
			string msg = e.Exception?.GetBaseException ().Message;

			// No exception, show generic message.
			if ( msg == null )
			{
				switch ( e.ErrorType )
				{
					case ( CommandErrorType.Exception ):
						msg = "Idk what the fuck happened, but this message should not be shown unless something really went shit side up.";
						break;
					case ( CommandErrorType.BadPermissions ):
						msg = "You don't have permission to run this ya fuggin druggo.";
						break;
					case ( CommandErrorType.BadArgCount ):
						msg = "Wrong number of arguments you silly galah.";
						break;

					default:
						msg = null;
						break;
				}

				if ( msg != null )
				{
					e.Channel.SendMessage ( msg );
				}
			}
			else
			{
				e.Channel.SendMessage ( "Something goofed behind the scenes, blame Nu's shit code." );
				m_client.Log.Error ( "Command", e.Exception );
			}
		}

		private void OnCommandExecuted ( object sender, CommandEventArgs e )
		{
			Console.WriteLine ( "{1} called by {0}", e.User.Name, e.Command.Text );
		}
		*/
	}
}
