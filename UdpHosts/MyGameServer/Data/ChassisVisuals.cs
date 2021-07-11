﻿//using System;
using System.Collections.Generic;
using System.Text;

using Shared.Common.Extensions;
using Shared.Udp;

namespace MyGameServer.Data {
	public class ChassisVisuals : CommonVisuals {
		public ChassisVisuals( ) {
			Decals.Add( new Decal {
				ID = 10000,
				Usage = 0xff,
				Color = 0xffffffff,
				Transform = new Half[] {
				(Half)0.052460, (Half)0.019623, (Half)0, (Half)0.007484,
				(Half)(-0.020020), (Half)(-0.051758), (Half)0.018127, (Half)(-0.048492),
				(Half)0.021362, (Half)0.108154, (Half)(-0.105469), (Half)1.495117,
			}
			} );

			Colors.Add( 0x10000020u );
			Colors.Add( 0xc8000000u );
			Colors.Add( 0xfe206281u );
			Colors.Add( 0xc8000000u );
			Colors.Add( 0xc8000000u );
			Colors.Add( 0xf5621861u );
			Colors.Add( 0xf5621861u );

			Palettes.Add( new Palette { ID = 85163, Type = Enums.Visuals.PaletteType.FullBody } );

			Patterns.Add( new Pattern {
				ID = 10022,
				Usage = 0x0,
				Transform = new Half[] {
					0, Half.ToHalf((ushort)16384u),
					0, 0
				}
			} );
		}
	}
}
