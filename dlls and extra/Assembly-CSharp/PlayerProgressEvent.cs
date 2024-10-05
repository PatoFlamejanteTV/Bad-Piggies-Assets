public class PlayerProgressEvent : EventManager.Event
{
	public int level;

	public int experience;

	public int experienceToNextLevel;

	public int pendingExperience;

	public PlayerProgressEvent(int level, int experience, int experienceToNextLevel, int pendingExperience)
	{
		this.level = level;
		this.experience = experience;
		this.experienceToNextLevel = experienceToNextLevel;
		this.pendingExperience = pendingExperience;
	}
}
