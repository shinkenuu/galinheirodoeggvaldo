using UnityEngine;
using System.Collections;

public class EggBehaviour : MonoBehaviour 
{
	GameManager _gameManager;
	ChickenBehaviour _chicken;

	private float time2spoil;
	private byte fort;

	void Awake() 
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
   
	void Start()
	{
		SetEgg();
	}

	void Update()
	{
		time2spoil -= Time.deltaTime;
		if (time2spoil < 0) // Previne que o ovo fique estatico por muito tempo.
		{
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter(UnityEngine.Collision other)
	{
		switch(other.gameObject.tag)
		{
		case "egg":
			if (fort == 0)
			{
				_chicken.eggsBroken++;
				_gameManager.lostEggs++;
				Destroy(other.gameObject);
				Destroy(gameObject);
			} else fort--;
			break;

		case "bucket":
			_gameManager.collectedEggs++;
			_gameManager.cash += 0.5f;
			Destroy(gameObject);
			break;

		case "ground":
			_chicken.eggsBroken++;
			_gameManager.lostEggs++;
			Destroy(gameObject);
			break;

		case "chicken":
			_chicken = other.gameObject.GetComponent<ChickenBehaviour>();
			break;
		}
	}

	void SetEgg()
	{
		time2spoil = 15;

		if (_gameManager.fortifiedEggs) fort = 1;
		else fort = 0;
	}
}