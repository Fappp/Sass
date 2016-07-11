using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sass.Modules
{
	class PingPong : IModule
	{
		void IModule.Install ( ModuleManager.Module a_handle )
		{
			a_handle.MessageReceived = async ( s, e ) =>
			{
				if ( e.Message.RawText.ToLower () == "ping" )
				{
					await e.Channel.SendMessage ( $"{e.User.Mention} Pong." );
				}
			};
		}
	}
}
