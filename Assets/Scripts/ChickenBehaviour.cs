using UnityEngine;
using System.Collections;

public class ChickenBehaviour : MonoBehaviour {

	private float timer;
	private float movementStartTime;
	
	public byte eggsBroken; // numero de ovos dessa galinha que nao chegaram ao balde
	public byte eggs2spawn;
	public byte spawnTimer;

	public int mood;
	private int curIndex;

	private bool moving;
	private bool showProfile = false;
	private bool chickenSet = false;

	GameManager _gameManager;

	public GUIStyle transparent;
	public Texture2D bar;
	public Texture2D profile;

	public Rigidbody eggPrefab;

	void Awake() {
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	void Start()
	{
		mood = (int)Random.Range(8, 10);
		curIndex = -1; // ainda nao setado. Nao mudar!
		spawnTimer = (byte)Random.Range(3, 10);
		timer = 0;
		eggsBroken = 0;
		eggs2spawn = 30;
		moving = false;
	}

	void Update () 
	{	
		if(!chickenSet && _gameManager.gameResults == 1)
		{
			if ( eggsBroken / 30 < 0.1f ) //se na fase anterior mais q 90% dos ovos dessa galinha foram coletados..
			{
				mood++;
			}
			eggsBroken = 0;
			mood+=2; // Recompensa para as galinhas que passaram para a prox fase
			timer = 0;
			eggs2spawn = 30;
			chickenSet = true;
		}

		if (!_gameManager.initialPause && _gameManager.gameResults == 0) //Se nao estiver no initialPause: ou bota, ou se desloca, ou decrementa seu tempo ate o prox ovo
		{
			chickenSet = false;
			timer += Time.deltaTime;

			if(moving)
			{
				CheckCurrentLocation();
			}

			else if ( UnityEngine.Random.Range(0, mood * 1000) == 1) // random para movimento
			{
				Move();
			}

			else if (timer >= spawnTimer && eggs2spawn > 0) 
			{
				EggSpawn();
			}

			else if (mood < 1) //Adeus mundo cruel e chato
			{
				_gameManager.totalEggs -= eggs2spawn;
				_gameManager.chickenLocation[curIndex] = 0;
				_gameManager.chickensNum--;
				Destroy(gameObject);
			}
		}
	}

	//Função que instancia o prefab dos ovos na posição da galinha, decrementa seus ovos a serem botados e decrementa o total de ovos restantes no _Manager
	#region EggSpawn
	void EggSpawn()
	{
		timer = 0;
		eggs2spawn--;
		Instantiate(eggPrefab, transform.position, Quaternion.identity);
		//egg.AddForce(0, -1 * _gameManager.gravity, 0);
	}
	#endregion

	/*
     * Método para mover as galinhas durante o jogo. 
     * As galinhas deixam registradas em chickenLocation sua transf.pos.x, para que 2 galinhas nao ocupem o mesmo lugar.
     */
	#region Move
	void Move()
	{
		float rand = 2;

		if(curIndex == -1)
		{
			for(int o = 0; o < _gameManager.chickensNum; o++) 
			{
				if (_gameManager.chickenLocation[o] == (float)gameObject.transform.position.x) // procura pela propria X na array da localizacao das galinhas
				{
					curIndex = o;
				}
			}
		} 

		while(rand == 2)
		{
			rand = Random.Range(-(_gameManager.curLvl * 2 +1), _gameManager.curLvl * 2 +3); // tiro
			for(int i = 0; i < _gameManager.curLvl * 5; i++)  // Previne que defina uma transform ocupada por outra galinha
			{																 
				if(rand == _gameManager.chickenLocation[i] || rand % 2 == 0 || rand == _gameManager.chickenLocation[curIndex])
				{
					rand = 2; //Se a posicao (rand, 9, 1.5) estiver ocupada ou rand for par, seta rand pra 2 e faz outro tiro
				}
			} 
		} 
		_gameManager.chickenLocation[curIndex] = rand; // Atualiza o novo X da galinha na array e seta o X em que a galinha deve ir
		movementStartTime = Time.time;
		moving = true; // seta a bool moving para que ela possa andar usando o Update
		//gameObject.renderer.material.color = Color.red;
	}
	#endregion

	#region CheckCurrentLocation
	void CheckCurrentLocation()
	{
		if(transform.position.x != _gameManager.chickenLocation[curIndex])
		{
			transform.position = Vector3.Lerp(transform.position, new Vector3(_gameManager.chickenLocation[curIndex], 9, 1.5f), (Time.time - movementStartTime) / 10); //movement
		}
		
		else {
			moving = false;
			gameObject.GetComponent<Renderer>().material.color = Color.white;
		}
	}
	#endregion

	#region ProfileHandler
	void OnGUI()
	{
		if (showProfile)
		{
			GUI.Box(new Rect(Screen.width * .38f, Screen.height * .19f, Screen.width * .23f, Screen.height * .15f), profile, transparent); //Profile
			GUI.Box(new Rect(Screen.width * (0.49f + mood * 0.01f), Screen.height * .22f, Screen.width * .08f, Screen.height * .02f), bar, transparent);// mood Bar
			GUI.Box(new Rect(Screen.width * (0.49f + (30 - eggs2spawn) * 0.003f), Screen.height * .26f, Screen.width * .08f, Screen.height * .02f), bar, transparent);// eggs Bar
		}

		//max -> 0.59 ---- Mood 10*0.01
		//min -> 0.49 ---- Eggs 30*0.003

		//GUI.Box(new Rect(Screen.width * .59f, Screen.height * .22f, Screen.width * .08f, Screen.height * .02f), bar, transparent);// mood Bar
		//GUI.Box(new Rect(Screen.width * .49f, Screen.height * .26f, Screen.width * .08f, Screen.height * .02f), bar, transparent);// eggs Bar
	}

	void OnMouseEnter() 
	{
		showProfile = true;
	}

	void OnMouseExit()
	{
		showProfile = false;
	}
	#endregion
}
