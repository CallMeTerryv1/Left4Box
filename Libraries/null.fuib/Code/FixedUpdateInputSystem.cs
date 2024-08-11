using System.Collections.Generic;

namespace Sandbox;

/// <summary>
/// How to use the system: 
/// <code>
/// public sealed class ExampleComponent : Component
/// {
///		// Reference to the system.
///		private FixedUpdateInputSystem _fixedInput;
///		
///		protected override void Start()
///		{
///			// Get the reference like this:
///			_fixedInput = Scene.GetSystem&lt;FixedUpdateInputSystem&gt;();
///			
///			base.OnStart();
///		}
///		
///		protected override void OnFixedUpdate()
///		{
///			// Query for input like usual.
///			if( _fixedInput.Pressed("jump") )
///			{
///				Log.Info("Jumped");
///			}
///			
///			base.OnFixedUpdate();
///		}
/// }
/// </code>
/// </summary>
public sealed class FixedUpdateInputSystem : GameObjectSystem
{
	private struct FixedUpdateInputBuffer
	{
		private class State
		{
			public bool Held;
			public bool Pressed;
			public bool Released;
		}

		private Dictionary<string, State> _actionStates;

		public FixedUpdateInputBuffer()
		{
			_actionStates = new Dictionary<string, State>();

			foreach ( var b in Input.GetActions() )
			{
				_actionStates[b.Name.ToLowerInvariant()] = new State();
			}
		}

		/// <summary>
		/// Call from a <see cref="Component.OnUpdate"/> method
		/// to update the states of the actions.
		/// </summary>
		public void OnUpdate()
		{
			foreach ( var (name, state) in _actionStates )
			{
				if ( Input.Down( name ) )
					_actionStates[name].Held = true;

				if ( Input.Pressed( name ) )
					_actionStates[name].Pressed = true;

				if ( Input.Released( name ) )
					_actionStates[name].Released = true;
			}
		}

		/// <summary>
		/// Call from a <see cref="Component.OnFixedUpdate"/>
		/// method to get the <see cref="State.Held"/> state of this action.
		/// </summary>
		/// <param name="action">The action name (case insensitive).</param>
		/// <returns></returns>
		/// 
		public bool Held( string action )
		{
			return _actionStates[action.ToLowerInvariant()].Held;
		}

		/// <summary>
		/// Call from a <see cref="Component.OnFixedUpdate"/>
		/// method to get the <see cref="State.Pressed"/> state of this action.
		/// </summary>
		/// <param name="action">The action name (case insensitive).</param>
		/// <returns></returns>
		public bool Pressed( string action )
		{
			return _actionStates[action.ToLowerInvariant()].Pressed;
		}

		/// <summary>
		/// Call from a <see cref="Component.OnFixedUpdate"/>
		/// method to get the <see cref="State.Pressed"/> state of this action.
		/// </summary>
		/// <param name="action">The action name (case insensitive).</param>
		/// <returns></returns>
		public bool Released( string action )
		{
			return _actionStates[action.ToLowerInvariant()].Released;
		}

		/// <summary>
		/// Call at the end of your <see cref="Component.OnFixedUpdate"/> method
		/// to clear the state of the struct and reset.
		/// </summary>
		public void Clear()
		{
			foreach ( var actionName in _actionStates.Keys )
			{
				_actionStates[actionName].Held = false;
				_actionStates[actionName].Pressed = false;
			}
		}
	}

	private FixedUpdateInputBuffer _buffer;

	public FixedUpdateInputSystem( Scene scene ) : base( scene )
	{
		_buffer = new();
		Listen( Stage.StartUpdate, int.MinValue, OnStartUpdate, "FUIB.OnStartUpdate" );
		Listen( Stage.FinishFixedUpdate, int.MaxValue, OnFinishFixedUpdate, "FUIB.OnFinishFixedUpdate" );
	}

	private void OnStartUpdate()
	{
		_buffer.OnUpdate();
	}

	private void OnFinishFixedUpdate()
	{
		_buffer.Clear();
	}

	/// <summary>
	/// Is the action currently held down?
	/// </summary>
	/// <param name="action">The action name (case insensitive).</param>
	/// <returns></returns>
	/// 
	public bool Held( string action ) => _buffer.Held( action );

	/// <summary>
	/// Was the action pressed?
	/// </summary>
	/// <param name="action">The action name (case insensitive).</param>
	/// <returns></returns>
	public bool Pressed( string action ) => _buffer.Pressed( action );

	/// <summary>
	/// Was the action released?
	/// </summary>
	/// <param name="action">The action name (case insensitive).</param>
	/// <returns></returns>
	public bool Released( string action ) => _buffer.Released( action );
}
