using System;
using UnityEngine;

public class SocialSimulator : Singleton<SocialSimulator>, ISocialProvider
{
	public GameObject gameCenterManager;

	public bool Authenticated => true;

	private void Awake()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void Authenticate()
	{
	}

	public void CloseViews()
	{
	}

	public void LoadAchievements()
	{
	}

	public void LoadLeaderboardScores()
	{
	}

	public void LoadScoreForLeaderboard(string leaderboardId)
	{
	}

	public void ReportAchievementProgress(string achievementId, double progress)
	{
	}

	public void ReportLeaderboardScore(string leaderboardId, long score, Action<bool> handler)
	{
	}

	public void ResetAchievementData()
	{
	}

	public void ShowAchievementsView()
	{
	}

	public void ShowLeaderboardsView()
	{
	}

	public void SyncAllAchievementsNow()
	{
	}

	public bool ViewsActive()
	{
		return false;
	}
}
