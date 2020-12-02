using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;

using MyGameServer.Entities;
using MyGameServer.GameObjects;

namespace MyGameServer.Systems {
	public class Aptitude : ISystem {
		public SystemType SystemType { get { return SystemType.Aptitude; } }
		
		protected IList<BaseEffect> ActiveEffectsData;
		protected object[] DeployableCalldownRequests;
		protected object[] NamedVarHack;
		protected object[] ActivationCooldown;

		public Aptitude() {
			ActiveEffectsData = new List<BaseEffect>();
        }

		// "stepEffects"
		public void Tick( double deltaTime, ulong currTime, CancellationToken ct ) {
			foreach( var e in ActiveEffectsData ) {
				if( ct.IsCancellationRequested )
					break;

				stepEffect( e, deltaTime, currTime, ct );
            }
		}



		public void ApplyEffect( uint effectId, IEntity self, IEntity initiator, object sourceChainContext, object pass) {

        }

		public void ActivateAbility( uint abilityId, IEntity entity, IEntity initiator, ulong gametime, ulong keytime, Vector3 init_pos, IEntity[] targets, IEntity interaction_target_entity, object namedvar ) {

		}

		public void RegisterDeployableCalldownRequest( IEntity requestedBy, uint deployableTypeId, Vector3 pos, Quaternion rot ) {

		}

		public void LocalProximityAbilitySuccess( IEntity initiator, object unk1, uint commandId, IEntity[] targets, ulong gametime, ulong keytime ) {

		}

		public void OnEntityRemoved( IEntity e ) {
			ActiveEffectsData = ActiveEffectsData.Where( be => be.Owner != e ).ToList();
		}



		protected void stepEffect( object effect, double deltaTime, ulong currTime, CancellationToken ct ) {

		}
	}
}
