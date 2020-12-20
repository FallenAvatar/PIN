using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Shared.Common.Extensions;

using MyGameServer.Packets;

using Serilog;

namespace MyGameServer.Controllers {
	public abstract class Base {
		protected ILogger Log { get { return Program.Logger; } }
		public Enums.GSS.Controllers ControllerID { get; private set; }

		protected Base() {
			try {
				ControllerID = GetType().GetAttribute<ControllerIDAttribute>().ControllerID;
			} catch {
				throw new MissingMemberException(GetType().FullName, "ControllerIDAttribute" );
			}
		}

		public abstract Task Init( INetworkPlayer player, IShard shard );

		public void HandlePacket( INetworkPlayer player, IShard shard, ulong EntityID, byte MsgID, GamePacket packet) {
			var method = ReflectionUtils.FindMethodsByAttribute<MessageIDAttribute>(this).Where(( mi ) => mi.GetAttribute<MessageIDAttribute>().MsgID == MsgID).FirstOrDefault();

			if( method == null ) {
				Log.Error("---> Unrecognized MsgID for GSS Packet; Controller = {0} Entity = 0x{1:X8} MsgID = {2}!", ControllerID, EntityID, MsgID);
				Log.Warning(">  {0}", BitConverter.ToString(packet.PacketData.ToArray()).Replace("-", " "));
				return;
			}

			// if method is async (and actually async, not just awaitable)
			if( method.ReturnType != null && (method.ReturnType == typeof( Task ) || method.ReturnType.GetMethod( nameof( Task.GetAwaiter ) ) != null) && method.GetCustomAttribute<AsyncStateMachineAttribute>() != null )
				_ = method.Invoke( this, new object[] { player, shard, EntityID, packet } );
			else
				_ = Task.Run(() => method.Invoke( this, new object[] { player, shard, EntityID, packet } ) );
		}
	}
}
