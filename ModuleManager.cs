using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace Sass
{
	public class ModuleManager
	{
		public DiscordClient Client { get; private set; }

		public class Module
		{
			public Module ( IModule a_class )
			{
				m_mod = a_class;
				m_mod.Install ( this );
			}

			private IModule m_mod;
			public Action<object, MessageEventArgs> MessageReceived;
		}

		private List<Module> m_modules = new List<Module> ();

		public ModuleManager ( DiscordClient a_client )
		{
			Client = a_client;
			if ( Client == null )
			{
				throw new ArgumentNullException ();
			}

			Client.MessageReceived += ( s, e ) =>
			{
				// Ignore our own messages.
				if ( e.User.Id == e.Server.CurrentUser.Id )
				{
					return;
				}

				foreach ( Module mod in m_modules )
				{
					if ( mod.MessageReceived != null )
					{
						mod.MessageReceived.Invoke ( s, e );
					}
				}
			};
		}

		public void AddModule<T> () where T : class, IModule, new ()
		{
			m_modules.Add ( new Module ( new T () ) );
		}
	}
}
