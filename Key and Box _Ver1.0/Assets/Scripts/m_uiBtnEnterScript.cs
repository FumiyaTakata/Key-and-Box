using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
public class m_uiBtnEnterScript : MonoBehaviour {

	public Button m_uiBtnEnter;

	// Use this for initialization
	void Start () {
		m_uiBtnEnter.interactable = false;
	}
	
	public void OnClick() {
		// room を閉じる
		PhotonNetwork.room.open = false; 
		// 同じタイミングでシーンが切り替わるようにする
		PhotonNetwork.automaticallySyncScene = true;
		PhotonNetwork.LoadLevel ("GameScene");
	}
}
