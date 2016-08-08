using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; 

public class ButtonScript : MonoBehaviour {

	public void ButtonPush() {
		Debug.Log("Button Push !!");
		SceneManager.LoadScene ("JoinRoomScene");
	}
}
