using UnityEngine;

public static class PlayerProfile 
{
	public class IntGameStat
	{
		string storageKey;

		public IntGameStat(string storageKey)
		{
			this.storageKey = storageKey;
		}

		public int GetValue(int difficulty)
		{
			return PlayerPrefs.GetInt(this.storageKey + difficulty, 0);
		}

		public void IncrementValue(int difficulty)
		{
			string key = this.storageKey + difficulty;
			PlayerPrefs.SetInt(key, PlayerPrefs.GetInt(key, 0) + 1);
			PlayerPrefs.Save();
		}
	}

	const string PREFERRED_DIFFICULTY_KEY = "PreferredDifficulty";
	public static int PreferredDifficulty { get; private set; }

	public static IntGameStat Wins { get; private set; }
	public static IntGameStat Losses { get; private set; }
	public static IntGameStat Draws { get; private set; }

	static PlayerProfile()
	{
		PreferredDifficulty = PlayerPrefs.GetInt(PREFERRED_DIFFICULTY_KEY, 0);

		Wins = new IntGameStat("Wins");
		Losses = new IntGameStat("Losses");
		Draws = new IntGameStat("Draws");
	}

	static void SaveInt(string key, int value)
	{
		PlayerPrefs.SetInt(key, value);
		PlayerPrefs.Save();
	}

	public static void SetPreferredDifficulty(int value)
	{
		PreferredDifficulty = value;
		SaveInt(PREFERRED_DIFFICULTY_KEY, value);
	}
}
