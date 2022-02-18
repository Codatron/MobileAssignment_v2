using Firebase.Auth;
using System.Collections.Generic;
using UnityEngine;

public class SessionData : MonoBehaviour
{
	private static SessionData _instance;
	public static SessionData Instance { get { return _instance; } }

	public GameInfo gameInfo;
	public PlayerInfo playerInfo;
	public PlayerInGame playerInGame;
	static string userPath;

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(this.gameObject);
		}
	}

	private void Start()
	{
		FindObjectOfType<FirebaseLogin>().OnSignIn += OnSignIn;
		userPath = "users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId;

		playerInGame = new PlayerInGame();
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
			playerInGame = JsonUtility.FromJson<PlayerInGame>(json);
		}

		playerInfo ??= new PlayerInfo();
		playerInfo.activeGames ??= new List<string>();
		SaveData();

		FindObjectOfType<FirebaseLogin>()?.PlayerDataLoaded();
	}

	public static void SaveData()
	{
		userPath = "users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId;
		SaveManager.Instance.SaveData(userPath, JsonUtility.ToJson(SessionData.Instance.playerInfo));    // argument in JsonUtility.ToJson was playerInGame
	}
}
