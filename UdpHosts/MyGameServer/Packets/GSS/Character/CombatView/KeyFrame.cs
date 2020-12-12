using System;
using System.Collections.Generic;
using System.Text;

using Shared.Udp;

namespace MyGameServer.Packets.GSS.Character.CombatView {
    [GSSMessage( Enums.GSS.Controllers.Character_CombatView, (byte)Enums.GSS.Character.Events.KeyFrame )]
    public class KeyFrame {
		[Field]
		public ushort BitFieldBytes;
		[Field]
		public uint Unk1;
		[Field]
		public IList<ushort> EffectsKeyTimes = new List<ushort>();
		[Field]
		[Length(8)]
		public IList<uint> Unk2 = new List<uint>();
		[Field]
		public IList<EffectData> EffectsData;
		[Field]
		public byte FireMode;
		[Field]
		public uint FireModeGameTime;
		[Field]
		public byte InScope;
		[Field]
		public uint InScopeGameTime;
		[Field]
		public ushort ShadowField0x42;
		[Field]
		public ushort ShadowField0x43;
		[Field]
		public ushort ShadowField0x44;
		[Field]
		public ushort ShadowField0x45;
		[Field]
		public byte SelectedWeaponIdx;
		[Field]
		public byte SelectedWeaponUnk1;
		[Field]
		public byte SelectedWeaponUnk2;
		[Field]
		public uint SelectedWeaponGameTime;
		[Field]
		public uint ShadowField0x47GameTime;
		[Field]
		public uint ShadowField0x48GameTime;
		[Field]
		public uint FireBurstGameTime;
		[Field]
		public uint FireEndGameTime;
		[Field]
		public uint FireCancelGameTime;
		[Field]
		public uint ReloadWeaponGameTime;
		[Field]
		public uint UnkGameTime1;
		[Field]
		public uint Unk3;
		[Field]
		public uint UnkGameTime2;
		[Field]
		public uint UnkGameTime3;
		[Field]
		public uint UnkGameTime4;
		[Field]
		public uint UnkGameTime5;
		[Field]
		public uint UnkGameTime6;
		[Field]
		public uint Unk4;
		[Field]
		public uint UnkGameTime7;
		[Field]
		[Length(30)]
		public IList<byte> Unk5 = new List<byte>();

		public class EffectData {
			[Field]
			public uint StatusID;
			[Field]
			public byte Flag1;
			[Field]
			public ulong InitiatorID;
			[Field]
			public uint StartTime;
			[Field]
			[ExistsPrefix(typeof(byte), 1)]
			public IList<byte> Data = new List<byte>();
		}
	}
}
