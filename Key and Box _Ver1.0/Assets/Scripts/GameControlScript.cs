using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameControlScript : Photon.MonoBehaviour {

	public List<GameObject> cardList = new List<GameObject>();
	public GameObject card;

	public Text m_uiTxtTurnNotification;

	private int[] turnLotation = {1,2,3,4};
	bool clockwise = true;
	public int currentTurn = 1;

	private PhotonView myPV;

	private Rigidbody myRigid;

	public int turnStatus = 0;
	public int turnStatusFinish = 1;
	public bool myTurn = false;


	// Use this for initialization
	void Start () {
		//photonViewを取得し、自分のオブジェクトでなければスクリプト停止

		// 手札
		var position = new Vector3 (0, 0, 0);
		var firstPosition = new Vector3 (0, 0, 0);

		myPV = PhotonView.Get(this);

		int i = 0;

		var sr = card.GetComponent<SpriteRenderer>();
		var width = sr.bounds.size.x;
		var height = sr.bounds.size.y;

		if (PhotonNetwork.player.ID == 1) {
			firstPosition = new Vector3 (-2, -4, 0);
		} else if (PhotonNetwork.player.ID == 2) {
			firstPosition = new Vector3 (-2, 4, 0);
		} else if (PhotonNetwork.player.ID == 3) {
			firstPosition = new Vector3 (5, -2, 0);
		} else if (PhotonNetwork.player.ID == 4){
			firstPosition = new Vector3 (-5, -2, 0);
		}
			
		for (i = 4; i >= 0; i--) {			

			Quaternion q = new Quaternion();
			if ((PhotonNetwork.player.ID == 1) || (PhotonNetwork.player.ID == 2)) {
				position.x = firstPosition.x + i * width + i * 0.1f;
				position.y = firstPosition.y;
				q= Quaternion.identity;
			} else {
				position.x = firstPosition.x;
				position.y = firstPosition.y + i * width+ i * 0.1f;
				q = Quaternion.Euler (0, 0, 90);
			}
				
			card = PhotonNetwork.Instantiate ("Card", position,q, 0);
			//  自分が生成したCardを移動可能にする
			card.GetComponent<CardScript>().enabled = true;
			cardList.Add (card);
        }
			

	}
		
	// Update is called once per frame
	void Update () {
		//m_uiTxtTurnNotification.text = PhotonNetwork.connectionStateDetailed.ToString();

		m_uiTxtTurnNotification.text = "Turn = " + currentTurn;	

		if (currentTurn == PhotonNetwork.player.ID) {
			myTurn = true;

			if (turnStatus == 0) {
				foreach (GameObject clearcard in cardList) {
					clearcard.GetComponent<CardScript> ().enabled = true;
					clearcard.GetComponent<CardScript> ().flgFinish = false;
				}
				turnStatus = 1;
			}
			foreach (GameObject finishCard in cardList) {
				if (finishCard.GetComponent<CardScript> ().flgFinish == true) {
					myTurn = false;
				}
			}
			if (myTurn == false && turnStatus == 1) {
				foreach (GameObject clearcard in cardList) {
					clearcard.GetComponent<CardScript> ().enabled = false;
				}
				if (clockwise == true) {
					currentTurn++;
					if (currentTurn == 5) {
						currentTurn = 1;
					}
				} else {

					currentTurn--;
					if (currentTurn == 0) {
						currentTurn = 4;
					}
				}
				myPV.RPC("sendFinishMessage", PhotonTargets.All, currentTurn);

			}


		}

	}
	[PunRPC]
	void sendFinishMessage(int turn){
		currentTurn = turn;
		turnStatus = 0;
	}
					
}




