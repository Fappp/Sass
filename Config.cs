using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace Sass
{
	public class ConfigException : System.Exception
	{
		public ConfigException ( string a_message ) : base ( a_message ) {}
	}

	class Config
	{
		// Singleton shit.
		private static Config m_instance = null;
		public static Config Instance
		{
			get { return m_instance; }
		}

		// Members.
		Dictionary<string, object> m_tokens;
		
		// Opan doooor.
		private string GenPrefix ( Stack<string> a_objStack )
		{
			if ( a_objStack.Count == 0 )
			{
				return "";
			}

			StringBuilder sb = new StringBuilder ();
			foreach ( string obj in a_objStack )
			{
				sb.Append ( obj );
				sb.Append ( '.' );
			}
			return sb.ToString ();
		}

		private void _Open ( string a_path )
		{
			JsonTextReader reader = null;
			try
			{
				reader = new JsonTextReader ( new StreamReader ( a_path ) );
			}
			catch ( IOException e )
			{
				throw new ConfigException ( $"IO error: {e.Message}" );
			}

			m_tokens = new Dictionary<string, object> ();
			string token = null;
			string objPrefix = "";
			Stack<string> objStack = new Stack<string> ();

			while ( reader.Read () )
			{
				switch ( reader.TokenType )
				{
				case ( JsonToken.StartObject ):
					if ( reader.Value != null )
					{
						objStack.Push ( reader.Value as string );
						objPrefix = GenPrefix ( objStack );
					}
					break;

				case ( JsonToken.EndObject ):
					if ( objStack.Count > 0 )
					{
						objStack.Pop ();
						objPrefix = GenPrefix ( objStack );
					}
					break;

				case ( JsonToken.PropertyName ):
					token = reader.Value as string;
					break;

				case ( JsonToken.String ):
				case ( JsonToken.Integer ):
				case ( JsonToken.Boolean ):
				case ( JsonToken.Date ):
				case ( JsonToken.Bytes ):
				case ( JsonToken.Float ):
				case ( JsonToken.Null ):
				case ( JsonToken.Undefined ):
					if ( token == null )
					{
						throw new ConfigException ( "Malformed config file." );
					}

					m_tokens.Add ( objPrefix + token, reader.Value );
					token = null;
					break;
				}
			}
		}

		public static void Open ( string a_path )
		{
			m_instance = new Config ();
			m_instance._Open ( a_path );
		}

		public T Get<T> ( string a_token, T a_default = default(T) )
		{
			try
			{
				return (T)m_tokens[a_token];
			}
			catch ( System.Exception )
			{
				return a_default;
			}
		}
	}
}
