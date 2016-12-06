using UnityEngine;

public class EventRelay 
{
	// Score changed
	public delegate void TurnStarted(AbstractPlayer player);
	public static event TurnStarted OnTurnStarted;
	
	public static void FireOnTurnStarted(AbstractPlayer player)
	{
		if(OnTurnStarted != null) 
			OnTurnStarted(player);
	}

	/// <summary>
	/// Remove event subscribers from some events (events that are designed to work with singletons/multyscene objects do not remove their subscribers)
	/// </summary>
	public static void ResetEvents()
	{
		OnTurnStarted = null;
	}
}
