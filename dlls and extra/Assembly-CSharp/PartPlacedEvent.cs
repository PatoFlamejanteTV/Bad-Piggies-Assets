using UnityEngine;

public struct PartPlacedEvent : EventManager.Event
{
	public BasePart.PartType partType;

	public BasePart.PartTier partTier;

	public Vector3 position;

	public PartPlacedEvent(BasePart.PartType partType, BasePart.PartTier partTier, Vector3 position)
	{
		this.partType = partType;
		this.partTier = partTier;
		this.position = position;
	}
}
