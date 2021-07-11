//using System;
using System.Collections.Generic;

using Shared.Udp;
using Shared.Udp.Attributes;

namespace MyGameServer.Packets.GSS.Character.ObserverView {
	[GSSMessage( Enums.GSS.Controllers.Character_ObserverView, (byte)Enums.GSS.Character.Events.ViewKeyFrame )]
	public class KeyFrame {
		[Field]
		public uint UnkUInt1;
		[Field]
		public string DisplayName;
		[Field]
		public string UniqueName;
		[Field]
		public byte Gender;
		[Field]
		public byte Race;
		[Field]
		public uint CharInfoID;
		[Field]
		public uint HeadMain;
		[Field]
		public uint Eyes;
		[Field]
		public byte UnkByte1;
		[Field]
		public byte IsNPC;
		[Field]
		public byte IsStaff;
		[Field]
		public uint CharTypeID;
		[Field]
		public uint VoiceSet;
		[Field]
		public ushort TitleID;
		[Field]
		public uint NameLocaliztionID;
		[Field]
		[LengthPrefixed( typeof( byte ) )]
		public IList<uint> HeadAccessories = new List<uint>();
		[Field]
		public uint VehicleLoadout;
		[Field]
		public uint GliderLoadout;
		[Field]
		public Visuals CharacterVisuals = new Visuals();
		[Field]
		public string ArmyName;
		[Field]
		public uint KeyFrameTime_0;
		[Field]
		public byte EffectsFlag;

		[Field]
		public ushort UnkShort1;
		[Field]
		public uint KeyFrameTime_1;
		[Field]
		public uint UnkUInt2;
		[Field]
		public byte CharacterState;
		[Field]
		public uint KeyFrameTime_2;
		[Field]
		public byte FactionMode;
		[Field]
		public byte FactionID;
		[Field]
		[Length( 20 )]
		public IList<byte> Unk1 = new List<byte>();
		[Field]
		public byte HealthPercent;
		[Field]
		public uint MaxHealth;
		[Field]
		public uint KeyFrameTime_4;
		[Field]
		[Padding( 21 )]
		public ulong ArmyID;
		[Field]
		[Length( 37 )]
		public IList<byte> Unk2 = new List<byte>();

		public class Palette {
			[Field]
			public byte Type;
			[Field]
			public uint ID;
		}

		public class Pattern {
			[Field]
			public uint ID;
			[Field]
			[Length( 4 )]
			public IList<Half> Transform = new List<Half>();
			[Field]
			public byte Usage;
		}

		public class Visuals {
			[Field]
			[LengthPrefixed( typeof( byte ) )]
			public IList<Decal> Decals = new List<Decal>();

			[Field]
			[LengthPrefixed( typeof( byte ) )]
			public IList<DecalGradient> DecalGradients = new List<DecalGradient>();

			[Field]
			[LengthPrefixed( typeof( byte ) )]
			public IList<uint> Colors = new List<uint>();

			[Field]
			[LengthPrefixed( typeof( byte ) )]
			public IList<Palette> Palettes = new List<Palette>();

			[Field]
			[LengthPrefixed( typeof( byte ) )]
			public IList<Pattern> Patterns = new List<Pattern>();

			[Field]
			[LengthPrefixed( typeof( byte ) )]
			public IList<uint> OrnamentGroups = new List<uint>();

			// Below are likely Morph Weights, Overlays and Pattern Gradients?

			[Field]
			[LengthPrefixed( typeof( byte ) )]
			public IList<uint> ItemsUnk1 = new List<uint>();

			[Field]
			[LengthPrefixed( typeof( byte ) )]
			public IList<uint> ItemsUnk2 = new List<uint>();

			[Field]
			[LengthPrefixed( typeof( byte ) )]
			public IList<uint> ItemsUnk3 = new List<uint>();
		}

		public class Decal {
			[Field]
			public uint ID;
			[Field]
			public uint Color;
			[Field]
			[Length( 12 )]
			public IList<Half> Transform = new List<Half>();
			[Field]
			public byte Usage;
		}

		public class DecalGradient {
			[Field]
			public byte Unk;
		}
	}
}
