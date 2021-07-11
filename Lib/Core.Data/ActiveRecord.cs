using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using Core.Extensions;
using System.Data;
using System.Threading.Tasks;

namespace Core.Data {
	public abstract class ActiveRecord : RowView {
		private Dictionary<string, PropertyInfo> keys;
		private Dictionary<string, PropertyInfo> props;
		public new object? this[string column] { get { return props[column].GetValue( this ); } set { props[column].SetValue( this, value ); } }

		public override void Load( IDataRow row ) {
			base.Load( row );

			var members = GetType().GetMembersByAttribute<ColumnAttribute>( true ).Where( m => m is PropertyInfo ).Cast<PropertyInfo>();
			keys = new Dictionary<string, PropertyInfo>();
			props = new Dictionary<string, PropertyInfo>();

			foreach( var prop in members ) {
				var colAttr = prop.GetCustomAttribute<ColumnAttribute>( true );
				var keyAttr = prop.GetCustomAttribute<PrimaryKeyAttribute>( true );

				var name = colAttr.ColumnName ?? prop.Name;
				props.Add( name, prop );
				if( keyAttr != null )
					keys.Add( name, prop );

				prop.SetValue( this, data[name] );
			}
		}

		public virtual async Task Save( ) {
			var changed = new Dictionary<string, object>();

			foreach( var (name, prop) in props ) {
				//if( keys.ContainsKey( name ) ) // Not supporting changing of keys yet, maybe?
				//continue;

				var currVal = prop.GetValue( this );
				if( data[name] == prop.GetValue( this ) )
					continue;

				changed.Add( name, currVal );
			}

			// TODO: Build SQL
			var sql = "UPDATE " + Database.DelimTable( "asdf" ) + " SET ";
			var parameters = new List<IParameter>();

			bool first = true;
			foreach( var (name, newVal) in changed ) {
				if( !first )
					sql += ", ";

				sql += $"{Database.DelimColumn( name )} = {Database.DelimParameter( name )}";
				parameters.Add( new Parameter( name, newVal ) );

				first = false;
			}

			sql += " WHERE ";

			first = true;
			foreach( var (name, prop) in keys ) {
				if( !first )
					sql += " AND ";

				sql += $"{Database.DelimColumn( name )} = {Database.DelimParameter( "k_" + name )}";
				parameters.Add( new Parameter( name, data[name] ) );

				first = false;
			}

			_ = await Database.ExecuteNonQuery( sql, parameters );
		}
	}
}
