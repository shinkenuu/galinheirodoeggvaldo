using UnityEngine;
using System.Collections;

public class GroundCheck : MonoBehaviour 
{
    GameManager _gameManager;

	void Awake()
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	void OnCollisionEnter(UnityEngine.Collision other)
    {
        if (other.gameObject.tag == "egg")
        {
            _gameManager.lostEggs++;
            Destroy(other.gameObject);
        }

    }
}
