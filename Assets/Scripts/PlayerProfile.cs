using UnityEngine;

public static class PlayerProfile 
{
	const string PREFERRED_DIFFICULTY_KEY = "PreferredDifficulty";
	public static int PreferredDifficulty { get; private set; }

	const string WINS_KEY = "Wins";
	const string LOSSES_KEY = "Losses";
	const string DRAWS_KEY = "Draws";
	public static int Wins { get; private set; }
	public static int Losses { get; private set; }
	public static int Draws { get; private set; }

	static PlayerProfile()
	{
		PreferredDifficulty = PlayerPrefs.GetInt(PREFERRED_DIFFICULTY_KEY, 0);

		Wins = PlayerPrefs.GetInt(WINS_KEY, 0);
		Losses = PlayerPrefs.GetInt(LOSSES_KEY, 0);
		Draws = PlayerPrefs.GetInt(DRAWS_KEY, 0);
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

	public static void AddWin()
	{
		SaveInt(WINS_KEY, ++Wins);
	}

	public static void AddLoss()
	{
		SaveInt(LOSSES_KEY, ++Losses);
	}

	public static void AddDraw()
	{
		SaveInt(DRAWS_KEY, ++Draws);
	}
}
