using Firebase.Auth;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
	public static PlayerInfo playerInfo;
	public GameInfo gameInfo;
	static string userPath;

	private void Start()
	{
		FindObjectOfType<FirebaseLogin>().OnSignIn += OnSignIn;
		userPath = "users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId;

		playerInfo = new PlayerInfo();
	}

	void OnSignIn()
	{
		userPath = "users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId;
		SaveManager.Instance.LoadData(userPath, OnLoadData);
	}

	void OnLoadData(string json)
	{
		if (json != null)
		{
			playerInfo = JsonUtility.FromJson<PlayerInfo>(json);
		}

		playerInfo ??= new PlayerInfo();
		playerInfo.activeGames ??= new List<string>();
		SaveData();


		FindObjectOfType<FirebaseLogin>()?.PlayerDataLoaded();
	}

	public static void SaveData()
	{
		userPath = "users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId;
		SaveManager.Instance.SaveData(userPath, JsonUtility.ToJson(playerInfo));    // argument in JsonUtility.ToJson was playerInGame
	}
}
