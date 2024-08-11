using System.Linq;
using Sandbox;
using Sandbox.Network;
using Sandbox.UI;

namespace GeneralGame;


public class NetworkManager : Component, Component.INetworkListener
{
	[Property] public PrefabScene PlayerPrefab { get; set; }

	protected override void OnStart()
	{
		if ( !GameNetworkSystem.IsActive )
		{
			GameNetworkSystem.CreateLobby();
		}
		
		base.OnStart();
	}

	void INetworkListener.OnActive( Connection connection )
	{
		var player = PlayerPrefab.Clone();
		player.BreakFromPrefab();
		player.NetworkSpawn( connection );

		// Initialize HUD for the player
		var screenPanel = new ScreenPanel();
		screenPanel.Style.Dirty(); // Mark the style as dirty to ensure it's updated
		var hud = new Hud();
		screenPanel.AddChild( hud );

		// Attach the screen panel to the player's view or HUD manager
		var playerView = player.Components.Get<ScreenPanel>(); // Ensure player has a ScreenPanel component
		if ( playerView != null )
		{
			playerView.AddChild( screenPanel );
		}

		HudInstance = hud; // Save reference to the HUD instance
	}

	public static Hud HudInstance { get; private set; } // Add this to hold the HUD instance
}
