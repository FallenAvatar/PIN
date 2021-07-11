using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Core.Extensions;

namespace Core.Data {
	public class DatabaseConnectorFactory {
		private static Dictionary<string, string> map = new Dictionary<string, string> {
				{ "MySql", "Core.Data.MySql" }
			};

		public static void RegisterPlatform( string dbTypeName, string assemblyName ) {
			map.Add( dbTypeName, assemblyName );
		}

		private static Dictionary<string, Type> typeMap = new Dictionary<string, Type>();
		public static IDatabaseConnector Get( IDBConfig config ) {
			if( !typeMap.ContainsKey( config.DatabaseType ) ) {
				if( !map.ContainsKey( config.DatabaseType ) )
					throw new InvalidOperationException( $"Could not find an appropriate DatabaseConnector for database type {config.DatabaseType}" );

				Assembly a;
				try {
					a = AppDomain.CurrentDomain.Load( File.ReadAllBytes( Path.GetFullPath( map[config.DatabaseType] + ".dll" ) ) );
					//a = Assembly.LoadFile( Path.GetFullPath( map[Environment.OSVersion.Platform] + ".dll" ) );
				} catch( Exception inner ) {
					throw new InvalidProgramException( $"Could not load the required library, {map[config.DatabaseType] + ".dll"}, for the database type {config.DatabaseType}.", inner );
				}

				var connType = (from t in a.FindTypesByBaseClass<IDatabaseConnector>()
								where config.DatabaseType.ToLower() == t.GetAttribute<DatabaseConnectorAttribute>()?.DBType?.ToLower()
								select t).FirstOrDefault();

				if( connType == null )
					throw new InvalidOperationException( $"Could not find an appropriate DatabaseConnector Type for {config.DatabaseType} in assembly {map[config.DatabaseType] + ".dll"}" );

				typeMap[config.DatabaseType] = connType;
			}

			return (IDatabaseConnector)Activator.CreateInstance( typeMap[config.DatabaseType], new object?[] { config } );
		}
	}
}
