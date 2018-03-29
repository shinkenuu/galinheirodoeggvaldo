using UnityEngine;
using System.Collections;

public class ScoreUpdate : MonoBehaviour {

    GameManager _gameManager;

	void Start () 
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	void OnCollisionEnter(UnityEngine.Collision other)
    {
        if (other.gameObject.tag == "egg")
        {
			_gameManager.collectedEggs++;
			_gameManager.cash += 0.5f;
            Destroy(other.gameObject);
        }
    }
}   

