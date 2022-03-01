using Firebase.Auth;
using Firebase.Database;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSelect : MonoBehaviour
{
	//Editor Connections
	public Transform gameListHolder;
	public GameObject gameButtonPrefab;

	//Local variables
	string userID;
	string displayName;
	PlayerInfo playerInfo;

	private void Start()
	{
		UpdateGameList();
	}

	private void UpdateGameList()
    {
        //clear/remove the old list, If we have one.
        EraseButtons();

        //create new list, load each of the users active games
        foreach (string gameID in SessionData.Instance.playerInfo.activeGames)
        {
            SaveManager.Instance.LoadData("games/" + gameID, LoadGameInfo);
        }

        //We have too few games, create a create game button
        if (SessionData.Instance.playerInfo.activeGames.Count < 5)
        {
            var newButton = Instantiate(gameButtonPrefab, gameListHolder).GetComponent<Button>();
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = "New Game";
            newButton.onClick.AddListener(() => SaveManager.Instance.LoadData("games/", JoinRandomGame));
            newButton.onClick.AddListener(UpdateGameList);
        }
    }

    private void EraseButtons()
    {
        foreach (Transform child in gameListHolder)
        {
            Destroy(child.gameObject);
        }
    }

    //Create button for the games, and add onclick events with the corresponding game info.
    public void LoadGameInfo(string json)
	{
        if (json == "" || json == null)
        {
			return;
        }

		var gameInfo = JsonUtility.FromJson<GameInfo>(json);

		var newButton = Instantiate(gameButtonPrefab, gameListHolder).GetComponent<Button>();
		newButton.GetComponentInChildren<TextMeshProUGUI>().text = gameInfo.displayGameName;

		//TODO: display more game status on each button.
        newButton.onClick.AddListener(() => SceneController.Instance.StartGame(gameInfo));
    }

	public void CreateGame()
	{
		//Create a new game and start filling out the info.
		var newGameInfo = new GameInfo();
		newGameInfo.seed = Random.Range(0, int.MaxValue);
		newGameInfo.displayGameName = SessionData.Instance.playerInfo.name + "'s game";	// This doesn't always work

		//Add the user as the first player
		newGameInfo.players = new List<PlayerInGame>();

		var newPlayerInGame = new PlayerInGame();
		newPlayerInGame.playerNumber = 0;
		newPlayerInGame.name = SessionData.Instance.playerInfo.name;
        newPlayerInGame.gridPositions = new List<Vector3>();
        newPlayerInGame.hidden = false;
        newPlayerInGame.attempts = 0;
        newPlayerInGame.totalObjectsFound = 0;
		newPlayerInGame.userID = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

		newGameInfo.players.Add(newPlayerInGame);

		//get a unique ID for the game
		string key = SaveManager.Instance.GetKey("games/");
		newGameInfo.gameID = key;

		//convert to json
		string data = JsonUtility.ToJson(newGameInfo);

		//Save our new game
		string path = "games/" + key;
		SaveManager.Instance.SaveData(path, data);

		//add the key to our active games
		GameCreated(key, newGameInfo);
	}

	public void GameCreated(string gameKey, GameInfo gameInfo)
	{
		//If we dont have any active games, create the list.
		SessionData.Instance.playerInfo.activeGames ??= new List<string>();	// This seems to not be working properly
		SessionData.Instance.playerInfo.activeGames.Add(gameKey);
		
		//save our user with our new game
		SessionData.SavePlayerInfoData();

        //Start the game
        SceneController.Instance.StartGame(gameInfo);
    }

	//We will try to join a random game, if we can't we create a new game.
	public void JoinRandomGame(List<string> data)
	{
		List<GameInfo> games = new List<GameInfo>();

		foreach (var item in data)
			games.Add(JsonUtility.FromJson<GameInfo>(item));

		foreach (var activeGame in games)
		{
			//Don't list our own games or full games.
			if (SessionData.Instance.playerInfo.activeGames.Contains(activeGame.gameID) || activeGame.players.Count > 1)
				continue;

			JoinGame(activeGame);
			return;
		}

		//No random games to join, create a new game.
		CreateGame();
	}

	public void JoinGame(GameInfo gameInfo)
	{
		Debug.Log("joining game: " + gameInfo.gameID);
		SessionData.Instance.playerInfo.activeGames.Add(gameInfo.gameID);

		//save our user with our new game
		SessionData.SavePlayerInfoData();

		//Update new game name
		gameInfo.displayGameName = gameInfo.players[0].name + " vs " + SessionData.Instance.playerInfo.name;
		
		var newPlayerInGame = new PlayerInGame();
		newPlayerInGame.playerNumber = 1;
		newPlayerInGame.gridPositions = new List<Vector3>();
		newPlayerInGame.userID = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
		newPlayerInGame.name = SessionData.Instance.playerInfo.name;
		newPlayerInGame.hidden = false;
		newPlayerInGame.attempts = 0;
		newPlayerInGame.totalObjectsFound = 0;

		gameInfo.players.Add(newPlayerInGame);

		string jsonString = JsonUtility.ToJson(gameInfo);

		//Update the game
		SaveManager.Instance.SaveData("games/" + gameInfo.gameID, jsonString);

		SceneController.Instance.StartGame(gameInfo);
	}
}
