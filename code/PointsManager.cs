using System;
using System.Collections.Generic;
using Sandbox;

namespace GeneralGame
{
	public sealed class PointsManager : Component
	{
		private Dictionary<Guid, PlayerObject> playerDict = new Dictionary<Guid, PlayerObject>();

		protected override void OnUpdate()
		{
			// Your existing update logic
		}

		public void AddPlayer( Guid playerId, PlayerObject player )
		{
			if ( !playerDict.ContainsKey( playerId ) )
			{
				playerDict.Add( playerId, player );
			}
		}

		public void RemovePlayer( Guid playerId )
		{
			if ( playerDict.ContainsKey( playerId ) )
			{
				playerDict.Remove( playerId );
			}
		}

		public PlayerObject GetPlayer( Guid playerId )
		{
			return playerDict.TryGetValue( playerId, out var player ) ? player : null;
		}
	}
}
