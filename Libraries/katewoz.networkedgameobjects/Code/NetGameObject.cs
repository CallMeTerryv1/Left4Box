using Sandbox;
using System;


/// <summary>
/// This is a component - in your library!
/// </summary>
public struct NetGameObject
{
	private Guid Id { get; set; }

	public GameObject Object { 
		get { return Game.ActiveScene.Directory.FindByGuid( new() ); } 
		set { Id = value.Id; }
	}

	public NetGameObject(GameObject gameObject)
	{
		Id = gameObject.Id;
	}
}
