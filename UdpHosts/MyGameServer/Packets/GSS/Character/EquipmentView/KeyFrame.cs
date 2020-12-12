using System;
using System.Collections.Generic;
using System.Text;

using Shared.Udp;

namespace MyGameServer.Packets.GSS.Character.EquipmentView {
    [GSSMessage( Enums.GSS.Controllers.Character_EquipmentView, (byte)Enums.GSS.Character.Events.KeyFrame )]
    public class KeyFrame {
		[Field]
		[Padding(1)]
		public uint ChassisLoadout;
		[Field]
		[Length(3)]
		public IList<byte> UnkBytes = new List<byte>();
		[Field]
		[LengthPrefixed(typeof(byte))]
		public IList<GearItem> Gear = new List<GearItem>();
		[Field]
		public Visuals ChassisVisuals = new Visuals();
		[Field]
		public uint BackpackLoadout;
		[Field]
		[Length(3)]
		public IList<byte> UnkBytes2 = new List<byte>();
		[Field]
		[LengthPrefixed(typeof(byte))]
		public IList<Ability> Abilities = new List<Ability>();
		[Field]
		[Padding(9)]
		public uint PrimaryWeaponID;
		[Field]
		[Length(3)]
		public IList<byte> UnkBytes3 = new List<byte>();
		[Field]
		[LengthPrefixed(typeof(byte))]
		public IList<WeaponModule> PrimaryWeaponModules = new List<WeaponModule>();
		[Field]
		public Visuals PrimaryWeaponVisuals = new Visuals();
		[Field]
		[Padding(8)]
		public uint SecondaryWeaponID;
		[Field]
		[Length(3)]
		public IList<byte> UnkBytes4 = new List<byte>();
		[Field]
		[LengthPrefixed(typeof(byte))]
		public IList<WeaponModule> SecondaryWeaponModules = new List<WeaponModule>();
		[Field]
		public Visuals SecondaryWeaponVisuals = new Visuals();
		[Field]
		public byte Level;
		[Field]
		public byte EffectiveLevel;
		[Field]
		public ushort UnkUShort = 0x63;
		[Field]
		[LengthPrefixed(typeof(ushort))]
		public IList<StatValue> ItemStatValues = new List<StatValue>();
		
		[Field]
		[Padding(4)]
		[LengthPrefixed(typeof(ushort))]
		public IList<StatValue> Weapon1StatValues = new List<StatValue>();

		[Field]
		[Padding(4)]
		[LengthPrefixed(typeof(ushort))]
		public IList<StatValue> Weapon2StatValues = new List<StatValue>();

		[Field]
		[Padding(4)]
		[LengthPrefixed(typeof(ushort))]
		public IList<StatValue> AttribCats1 = new List<StatValue>();

		[Field]
		[LengthPrefixed(typeof(ushort))]
		public IList<StatValue> AttribCats2 = new List<StatValue>();

		[Field]
		public uint PVPRank;
		[Field]
		public uint EliteRank;


		public class Palette {
			[Field]
			public byte Type;
			[Field]
			public uint ID;
		}

		public class StatValue {
			[Field]
			public ushort StatID;
			[Field]
			public float Value;
		}

		public class Pattern {
			[Field]
			public uint ID;
			[Field]
			[Length(4)]
			public IList<Half> Transform = new List<Half>();
			[Field]
			public byte Usage;
		}

		public class Visuals {
			[Field]
			[LengthPrefixed(typeof(byte))]
			public IList<Decal> Decals = new List<Decal>();

			[Field]
			[LengthPrefixed(typeof(byte))]
			public IList<DecalGradient> DecalGradients = new List<DecalGradient>();

			[Field]
			[LengthPrefixed(typeof(byte))]
			public IList<uint> Colors = new List<uint>();

			[Field]
			[LengthPrefixed(typeof(byte))]
			public IList<Palette> Palettes = new List<Palette>();

			[Field]
			[LengthPrefixed(typeof(byte))]
			public IList<Pattern> Patterns = new List<Pattern>();

			[Field]
			[LengthPrefixed(typeof(byte))]
			public IList<uint> OrnamentGroups = new List<uint>();

			// Below are likely Morph Weights, Overlays and Pattern Gradients?

			[Field]
			[LengthPrefixed(typeof(byte))]
			public IList<uint> ItemsUnk1 = new List<uint>();

			[Field]
			[LengthPrefixed(typeof(byte))]
			public IList<uint> ItemsUnk2 = new List<uint>();

			[Field]
			[LengthPrefixed(typeof(byte))]
			public IList<uint> ItemsUnk3 = new List<uint>();
		}

		public class GearItem {
			[Field]
			public uint ID;
			[Field]
			[Length(3)]
			public IList<byte> Unk = new List<byte>();
		}

		public class Ability {
			[Field]
			public uint ID;
			[Field]
			public byte Slot;
			[Field]
			[LengthPrefixed(typeof(byte))]
			public IList<AbilityModule> AbilityModules = new List<AbilityModule>();
			[Field]
			public byte UnkByte1;
		}

		public class AbilityModule {
			[Field]
			public uint Unk;
			[Field]
			public uint ID;
		}

		public class Decal {
			[Field]
			public uint ID;
			[Field]
			public uint Color;
			[Field]
			[Length(12)]
			public IList<Half> Transform = new List<Half>();
			[Field]
			public byte Usage;
		}

		public class DecalGradient {
			[Field]
			public byte Unk;
		}

		public class WeaponModule {
			[Field]
			public uint ID;
			[Field]
			[Length(3)]
			public IList<byte> Unk = new List<byte>();
		}
	}
}
