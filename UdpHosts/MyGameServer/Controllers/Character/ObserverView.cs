using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Shared.Common.Extensions;

namespace MyGameServer.Controllers.Character {
    [ControllerID( Enums.GSS.Controllers.Character_ObserverView )]
    public class ObserverView : Base {
        public async override Task Init( INetworkPlayer player, IShard shard ) {
            // TODO: Implement

        }

        public static Packets.GSS.Character.ObserverView.KeyFrame BuildKeyframe(Entities.Character c) {
            var cd = c.CharData;
            var cv = cd.CharVisuals;
            var cl = cd.Loadout;

            var ret = new Packets.GSS.Character.ObserverView.KeyFrame() {
                UnkUInt1 = 0xFFFFFFFEU,
                DisplayName = cd.Name,
                UniqueName = cd.Name,
                Gender = (byte)cd.Gender,
                Race = (byte)cd.Race,
                CharInfoID = cd.CharInfoID,
                HeadMain = cv.HeadMain,
                Eyes = cv.Eyes,
                UnkByte1 = 0xAA,
                IsNPC = (byte)(cd.IsNPC ? 0x01 : 0x00),
                IsStaff = (byte)((cd.IsDev ? 0x01 : 0x00) << 0 + (cd.IsMentor ? 0x01 : 0x00) << 1 + (cd.IsRanger ? 0x01 : 0x00) << 2 + (cd.IsPublisher ? 0x01 : 0x00) << 3),
                CharTypeID = cv.CharTypeID,
                VoiceSet = cd.VoiceSet,
                TitleID = cd.TitleID,
                NameLocaliztionID = cd.NameLocalizationID,
                VehicleLoadout = cl.VehicleID,
                GliderLoadout = cl.GliderID,
                ArmyName = cd.Army.Name,
                KeyFrameTime_0 = IShard.CurrentShard.CurrentTime,
                EffectsFlag = 0,
                UnkShort1 = 0x0000,
                KeyFrameTime_1 = IShard.CurrentShard.CurrentTime,
                UnkUInt2 = 0,
                CharacterState = 6,
                KeyFrameTime_2 = IShard.CurrentShard.CurrentTime,
                FactionMode = cd.Faction.Mode,
                FactionID = cd.FactionID,
                HealthPercent = 100,
                MaxHealth = cd.MaxHealth,
                KeyFrameTime_4 = IShard.CurrentShard.CurrentTime,
                ArmyID = cd.ArmyGUID
            };

            ret.HeadAccessories.AddAll( cv.HeadAccessories );

            // TODO: Copy Visuals
            //ret.CharacterVisuals.

            ret.Unk1.AddAll( new byte[] {
                0x32, 0x00, 0x09, 0x8e, 0xdc,
                0xff, 0xff, 0x09, 0x00,
                0x00, 0x32,
                0x00, 0xf2, 0x00, 0x20,
                0x00, 0x00, 0xf2, 0x00, 0x00
            } );

            return ret;
        }
    }
}
