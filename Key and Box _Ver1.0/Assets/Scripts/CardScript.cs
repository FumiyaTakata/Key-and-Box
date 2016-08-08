using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
	public GameObject card;

	int count = 0;
	public bool flgFinish= false;

	// Use this for initialization
	void Start ()
	{
		var sr = card.GetComponent<SpriteRenderer>();
		Color randomColor = new Color( Random.value, Random.value, Random.value, 256);
		sr.color = randomColor;
	}
		
	// Update is called once per frame
	void Update ()
	{
		

			if (Input.GetMouseButtonDown (0)) {
				Vector3 aTapPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				Collider2D aCollider2d = Physics2D.OverlapPoint (aTapPoint);

				if (aCollider2d) {
					 {
						GameObject obj = aCollider2d.transform.gameObject;

						obj.transform.position = new Vector3 (Camera.main.transform.position.x + count * 0.1f, 
							Camera.main.transform.position.y + count * 0.1f, 
							0);							
						count++;
					    this.flgFinish = true;
					}
				}

			}
				
		}
			

		
	
}

