using Firebase.Auth;
using System.Collections.Generic;
using UnityEngine;

public class SessionData : MonoBehaviour
{
	private static SessionData _instance;
	public static SessionData Instance { get { return _instance; } }

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

		if (FirebaseAuth.DefaultInstance.CurrentUser.UserId == null)
			return;
		else
			userPath = "users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId;

		playerInGame = new PlayerInGame();
		//playerInGame.name = "Error, Im not in this game? SessionData cs.37";

		foreach (var player in SaveManager.Instance.gameInfo.players)
        {
            if (player.userID == FirebaseAuth.DefaultInstance.CurrentUser.UserId)
            {
				playerInGame = player;
				Debug.Log("our ID:" + playerInGame.userID);
			}
        }
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
		SavePlayerInfoData();

		FindObjectOfType<FirebaseLogin>()?.PlayerDataLoaded();
	}

	public static void SavePlayerInfoData()
	{
		SessionData.Instance.playerInGame.userID = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        userPath = "users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId;
		SaveManager.Instance.SaveData(userPath, JsonUtility.ToJson(SessionData.Instance.playerInfo));   
	}

	public static void SavePlayerInGameData()
	{
		SessionData.Instance.playerInGame.userID = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

		Debug.Log(SessionData.Instance.playerInGame.playerNumber);
	

		SaveManager.Instance.gameInfo.players[SessionData.Instance.playerInGame.playerNumber] = SessionData.Instance.playerInGame;

        string gamePath = "games/" + SaveManager.Instance.gameInfo.gameID;
		SaveManager.Instance.SaveData(gamePath, JsonUtility.ToJson(SaveManager.Instance.gameInfo));
	}
}
