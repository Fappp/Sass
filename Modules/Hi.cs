using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sass.Modules
{
	class Hi : IModule
	{
		void IModule.Install ( ModuleManager.Module a_handle )
		{
			Random rand = new Random ();
			string[] hello = new string[]
			{
				"Hi {0}.",
				"What's up {0}?",
				"G'day {0}.",
				"Greetings {0}.",
				"How are you doing {0}?",
				"The fuck's a {0}? Lol.",
				"Fuck off {0}.",
				"I'm watching you,",
				"Good afternoon, sleepyhead.",
				"Waddiyatalkinabeet?",
				"{0} you fuckin potato headed looking druggo, how's it going?"
			};

			a_handle.MessageReceived = async ( s, e ) =>
			{
				if ( e.Message.IsMentioningMe () || e.Channel.IsPrivate )
				{
					if ( e.Message.Text.Contains ( "hi" ) )
					{
						int msgIndex = rand.Next ( hello.Length );
						string msg = string.Format ( hello[msgIndex], e.User.Mention );
						await e.Channel.SendMessage ( msg );
					}
				}
			};
		}
	}
}
