using UnityEngine;
using System.Collections;

public class SetPipe : MonoBehaviour {

	GameManager _gameManager;
	Transform pipeSet; // previne que se instancie um outro pipe nessa mesma pipeHolder enquanto houver um pipe nela
	
	public Transform[] pipe;

	void Start () 
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(0) && pipeSet == null) // se for null significa que nao ha nenhum pipe instanciado por esse pipeHolder
		{
			switch (_gameManager.qualitySelect)
			{
			case 0:  // Golden Pipes
				if (_gameManager.cash >= 5) { //Tem cash suficiente?
					_gameManager.cash -= 5; // Cost for it
					_gameManager.pipesBought++;
					pipeSet = Instantiate(pipe[_gameManager.shapeSelect], SetPipePos(), SetPipeRot()) as UnityEngine.Transform; // define que esta ocupado
				}
				break;
			case 1: // Silver Pipes
				if (_gameManager.cash >= 3) { //Tem cash suficiente?
					_gameManager.cash -= 3; // Cost for it
					_gameManager.pipesBought++;
					pipeSet = Instantiate(pipe[_gameManager.shapeSelect], SetPipePos(), SetPipeRot()) as UnityEngine.Transform;  // define que esta ocupado
				}
				break;
			case 2: //Bronze  Pipes
				if (_gameManager.cash >= 1) { //Tem cash suficiente?
					_gameManager.cash -= 1; // Cost for it
					_gameManager.pipesBought++;
					pipeSet = Instantiate(pipe[_gameManager.shapeSelect], SetPipePos(), SetPipeRot()) as UnityEngine.Transform; // define que esta ocupado
				}
				break;
			}
		}
	}

	Vector3 SetPipePos() // "\", "2rightFromUp", "2rightFromMiddle", "-", "2leftFromMiddle", "2leftFromUp", "/"
	{
		switch(_gameManager.shapeSelect)
		{
		case 0:
		case 6:
			return new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 1.5f); 
		case 3:
			return new Vector3(gameObject.transform.position.x, gameObject.transform.position.y -1, 1.5f);
		case 1:
		case 5:
			return new Vector3(gameObject.transform.position.x, gameObject.transform.position.y +0.5f, 1.5f); 
		case 2:
		case 4:
			return new Vector3(gameObject.transform.position.x, gameObject.transform.position.y -0.240288f, 1.5f); 
		default:
			return gameObject.transform.position;
		}
	}

	Quaternion SetPipeRot()
	{
		switch(_gameManager.shapeSelect)
		{
		case 0:
			return Quaternion.Euler (0, 0, -45);
		case 1:
		case 2:
			return Quaternion.Euler (0, 0,  340);
		case 4:
		case 5:
			return Quaternion.Euler (0, 0,  20);
		case 6:
			return Quaternion.Euler (0, 0, 45);
		default:
			return Quaternion.identity;
		}
	}
}
