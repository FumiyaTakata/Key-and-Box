using UnityEngine;
using UnityEngine.UI;
using System;

public class RoomManagerScript: MonoBehaviour {

	private string playerName = "GuestAAA";
	private string roomName = "";

	bool flgCreatedRoom = false;
	bool flgJoinedRoom = false;
	public bool flgRamdomRoom = false;


	public Text m_uiTxtMessage;    
	public Text m_uiTxtStatus;
	public Button m_uiBtnEnter;

	const int maxMember = 4;

	public void Awake()
	{
		// マスタークライアントのsceneと同じ部屋に入室した人もロードする。
		PhotonNetwork.automaticallySyncScene = true;
		// もしまだ接続していない状態ならば
		if (PhotonNetwork.connectionStateDetailed ==ClientState.PeerCreated )
		{
			// PhotonServerSettingsの設定に従ってPhotonNetwork（マスターサーバー）に接続する。
			PhotonNetwork.ConnectUsingSettings("1.0");
		}

		// もしPhotonNetWorkに名前を登録していないならば、PhotonNetworkに名前を登録する
		if (PhotonNetwork.playerName==null)
		{
			//ランダムにプレイヤーの名前を生成
			this.playerName = "Guest" + UnityEngine.Random.Range(1, 9999);
			//Photonにプレイヤーを登録
			PhotonNetwork.playerName = this.playerName; 
		}else{
			//Photonにプレイヤーを登録
			this.playerName = PhotonNetwork.playerName;
		}
	}

	public void OnGUI(){

		GUILayout.BeginArea (new Rect (Screen.width / 2 -50 , Screen.height / 2 -50, 100, 100));

		//ルーム名をテキストフィールドから入力させる

		this.roomName = GUILayout.TextField(this.roomName);
		GUILayout.Space (10);

		//ルーム作成ボタン
		if (GUILayout.Button("Create Room", GUILayout.Width(100)))
		{
			if (flgCreatedRoom == false) {
				//ルームを作成。引数はルーム名
				RoomOptions roomOptions = new RoomOptions ();
				roomOptions.isVisible = true;
				roomOptions.isOpen = true;
			    roomOptions.maxPlayers = maxMember;			
				// 入力がなかったら
				if (this.roomName == "") {
					//ランダムにルームに入る
					PhotonNetwork.JoinRandomRoom ();
					flgRamdomRoom = true;
				} else{
					PhotonNetwork.CreateRoom (this.roomName, roomOptions, null);
				}
			} else {
				m_uiTxtMessage.text = "Room alredy created";
			}
		}
		GUILayout.Space (10);

		//ルームに入室するボタン
		if (GUILayout.Button("Join Room", GUILayout.Width(100)))
		{
			if (flgJoinedRoom == false) {
				
				// 入力がなかったら
				if (this.roomName == "") {
					//ランダムにルームに入る
					PhotonNetwork.JoinRandomRoom ();
					flgRamdomRoom = true;
				} else {
					//入室。引数はルーム名
					PhotonNetwork.JoinRoom (this.roomName);
				}
			} else {
				m_uiTxtMessage.text = "You already joined";
			}
		}
			
		GUILayout.EndArea ();
	}

	/// <summary>
	/// Raises the joined room event.
	/// </summary>
	public void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom");
		//Debug.Log("Joined !! : Room Name :" + PhotonNetwork.room.name + "-" + PhotonNetwork.room.playerCount);
		m_uiTxtMessage.text = "Joined !! ";

		string roomName = null;

		string playerNumber = PhotonNetwork.room.playerCount.ToString ();

		if (flgRamdomRoom == false) {
			roomName = PhotonNetwork.room.name.ToString ();
		} else {
			roomName = "RandomRoom";
		}
		m_uiTxtStatus.text = "Status ->" + Environment.NewLine +
			"  Room Name -> " + roomName  + Environment.NewLine +
			"  Member ->" + playerNumber;

		// Roomに参加しているプレイヤー情報を配列で取得.
		PhotonPlayer [] player = PhotonNetwork.playerList;

		// プレイヤー名とIDを表示.
		for (int i = 0; i < player.Length; i++) {
			Debug.Log((i).ToString() + " : " + player[i].name + " ID = " + player[i].ID);
		}
		flgJoinedRoom = true;
		/*
		if (PhotonNetwork.player.ID == maxMember) {
			m_uiBtnEnter.interactable = true;
		}
		*/
	}

	/// <summary>
	/// Raises the created room event.
	/// </summary>
	public void OnCreatedRoom()
	{
		Debug.Log("OnCreatedRoom");
		flgCreatedRoom = true;
	}

	/// <summary>
	/// 部屋の作成に失敗した時に呼ばれるコールバック
	/// </summary>
	public void OnPhotonCreateRoomFailed()
	{
		m_uiTxtMessage.text = "can't creat # The Rom Name has been already used";
	}


	/// <summary>
	/// Raises the photon join room failed event.
	/// </summary>
	/// <param name="codeAndMsg">Code and message.</param>
	public void OnPhotonJoinRoomFailed(object[] codeAndMsg)
	{
		if (codeAndMsg [0].ToString () == "32758") {
			m_uiTxtMessage.text = "can't join # Room has not been created ";
		} else {
			m_uiTxtMessage.text = "can't join # Room is full";
		}
	}

	/// <summary>
	/// ランダムの部屋に入る
	/// </summary>
	void OnPhotonRandomJoinFailed()
	{
		RoomOptions roomOptions = new RoomOptions ();
		roomOptions.isVisible = true;
		roomOptions.isOpen = true;
		roomOptions.maxPlayers = maxMember;			
		PhotonNetwork.CreateRoom (null, roomOptions, null);
	}


	/// <summary>
	/// 部屋に誰かが入ってきたら呼ばれるコールバック
	/// </summary>
	/// <param name="newPlayer">New player.</param>
	public void OnPhotonPlayerConnected	(PhotonPlayer newPlayer){
		// Roomに参加しているプレイヤー情報を配列で取得.
		PhotonPlayer [] player = PhotonNetwork.playerList;

		// プレイヤー名とIDを表示.
		for (int i = 0; i < player.Length; i++) {
			Debug.Log((i).ToString() + " : " + player[i].name + " ID = " + player[i].ID);
		}
				
		m_uiTxtMessage.text = "Other Player Joined !! ";

		string roomName = null;

		string playerNumber = PhotonNetwork.room.playerCount.ToString ();

		if (flgRamdomRoom == false) {
			roomName = PhotonNetwork.room.name.ToString ();
		} else {
			roomName = "RandomRoom";
		}
		m_uiTxtStatus.text = "Status ->" + Environment.NewLine +
			"  Room Name -> " + roomName  + Environment.NewLine +
			"  Member ->" + playerNumber;

		if(PhotonNetwork.room.playerCount == maxMember && PhotonNetwork.isMasterClient){
			m_uiBtnEnter.interactable = true;
		}

	}

	//接続が切断されたときにコール
	public void OnDisconnectedFromPhoton()
	{
		Debug.Log("Disconnected from Photon.");
	}
	//接続失敗時にコール
	public void OnFailedToConnectToPhoton(object parameters)
	{
		Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters + " ServerAddress: " + PhotonNetwork.networkingPeer.ServerAddress);
	}


}