using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
	public GUIStyle resultsStyle;
	public GUIStyle pipeButton;

	TextMesh _textMesh;
    GlobalManager _globalManager;

	public Texture2D[] pipeShape; // = {"\\", "2rightFromUp", "2rightFromMiddle", "-", "2leftFromMiddle", "2leftFromUp", "/"};
	public Texture2D[] pipeQuality; // = {"Ouro", "Prata", "Bronze"};
	public Texture2D chickenPNG;
	public Texture2D syringePNG;
	
    public Transform nestPrefab;
	public Transform bucketPrefab;
	public Transform chickenPrefab;
    public Transform pipeHolder;

	#region Public Int
	public int totalEggs;
	public int lostEggs;
	public int collectedEggs;
	public int remainingEggs;
	public int pipesBought;
	public int chickensNum;
	public int curLvl;

    public int shapeSelect;
	public int qualitySelect;
	public int gameResults; // -1 = GameOver / 0 = Running / 1 = LevelUp
	#endregion

	#region Public Float
	public const float lossLimitPercentage = 10; //%
	public float pauseTimer;
	public float lossPercentage;
    public float cash;
	public float[] chickenLocation;
    #endregion

	#region Public Bool
	public bool initialPause;
	public bool fortifiedEggs;
    #endregion

    #region Private Bool
	private bool newGameSet;
    #endregion

    void Awake()
    {
        _globalManager = GameObject.Find("GlobalManager").GetComponent<GlobalManager>();
		_textMesh = gameObject.transform.GetComponent<TextMesh>();
	}
	
	void Start () 
	{
		chickenLocation = new float[22]; //22 = Numero de galinhas que cabem no galinheiro
		fortifiedEggs = false;
		curLvl = _globalManager.dificultySelected;
		SetInitialGrid();
		SetBucket(); // SetBucket(int minX, int maxX)
		SetNewGame(curLvl);
		ChickenSpawner(curLvl % 3 + 2, -(curLvl * 2 +1), curLvl * 2 +3); // Deve ficar apos SetNewGame para que n seja duplicado o n de totalEggs
	}
	
	void Update()
    {
		remainingEggs = totalEggs - (collectedEggs + lostEggs);
		lossPercentage = lostEggs / (totalEggs + 0.00001f) * 100;

        CheckGameState();
        
        if (initialPause)
        {
			_textMesh.text = "Inicia em  " + ((int)pauseTimer) + "\n$ " + cash;
		} else {
			_textMesh.text = "Margem de erro atual  % " + (int)lossPercentage + "\nOvos restantes: " + remainingEggs + "\n$ " + cash;
		}
	}

	void OnGUI()
	{   
		if (gameResults == 0)
		{
		shapeSelect = GUI.SelectionGrid(new Rect (Screen.width * .25f, Screen.height * .85f, Screen.width * .5f, Screen.height * .145f), shapeSelect, pipeShape, 7, pipeButton);
		qualitySelect = GUI.SelectionGrid (new Rect (0, Screen.height *  .8f, Screen.width * .05f, Screen.height * .2f), qualitySelect, pipeQuality, 1);  // Horizontal layout

			if (GUI.Button(new Rect (Screen.width * .9f, Screen.height * .9f, Screen.width * .1f, Screen.height * .1f), chickenPNG))
			{
				if (cash >= 130 && chickenLocation[21] % 2 == 0) // Prevents buying more chickens than capacity can carry
				{
					cash -= 130; // cost for a new chicken
					ChickenSpawner(1,-(curLvl * 2 +1), curLvl * 2 +3);
				}
			}
		
			if (GUI.Button(new Rect (Screen.width * .9f, Screen.height * .79f, Screen.width * .1f, Screen.height * .1f), syringePNG))
			{
				if (cash >= 100) // Cost for it
				{
					cash -= 100;
					fortifiedEggs = true;
				} 
			}
		}

		else 
		{
			GUI.Box(new Rect(Screen.width * .1f, Screen.height * .175f, Screen.width * .75f, Screen.height * .7f), "\n\n\n\tCapacidade de producao: " + totalEggs + "\n\tOvos perdidos: "+ lostEggs +"\n\tOvos coletados: " + collectedEggs + "\n\tCalhas compradas: " + pipesBought + "\n\tNumero de galinhas: " + chickensNum, resultsStyle);
			if(gameResults == 1)
			{
				if((GUI.Button (new Rect (Screen.width * .325f, Screen.height * .65f, Screen.width * .3f, Screen.height * .13f), "\nProximo dia!", resultsStyle))) 
				{
					SetNewGame(++curLvl);
				}
			}
		}
	}

	void SetNewGame(int lvl)
	{
		SetHolder(lvl * 2 +3); //-21 - 19
		SetHolder(-(lvl * 2 +1));
		SetBucket();

		shapeSelect = 6;
		qualitySelect = 2;
		
		cash += lvl * 5;
		
		if (lvl % 3 == 0) for( byte c = 0; c < 3; c++) ChickenSpawner(1, -(lvl * 2 +1), lvl * 2 +3); //Da uma galinha a cada 3 lvls

		totalEggs = 0;
		lostEggs = 0;
		collectedEggs = 0;
		pipesBought = 0;
		if (chickenLocation[0] != 0) for(byte i = 0; chickenLocation[i] != 0; i++) totalEggs += 30; // adiciona 30 ovos pra cada galinha da nova fase

		pauseTimer = 10; //testes
		gameResults = 0; 
		initialPause = true;
	}

	#region SetBucket
	void SetBucket()
	{
		Destroy(GameObject.FindGameObjectWithTag("bucket"));

		float rand;
		if(curLvl < 5)
		{
			rand = 2;
			while(rand == 2)
			{
				rand = Random.Range(-(curLvl + 3), curLvl + 3); // tiro 
				if(rand % 2 == 0) rand = 2;
			}
			Instantiate(bucketPrefab, new Vector3( rand, -3.84813f, 0), Quaternion.Euler ( -85, -0.4399414f, 0.5f));
		}

		else
		{
			rand = 2;
			while(rand == 2)
			{
				rand = Random.Range(-curLvl, -3); // tiro 
				if(rand % 2 == 0) rand = 2;
			}
			Instantiate(bucketPrefab, new Vector3( rand, -3.84813f, 0), Quaternion.Euler ( -85, -0.4399414f, 0.5f));

			rand = 2;
			while(rand == 2)
			{
				rand = Random.Range(3, curLvl); // tiro 
				if(rand % 2 == 0) rand = 2;
			}
			Instantiate(bucketPrefab, new Vector3( rand, -3.84813f, 0), Quaternion.Euler ( -85, -0.4399414f, 0.5f));
		}
	}
	#endregion

	/*
     * Método para instanciar as galinhas em random no inicio da fase. 
     * As galinhas deixam registradas em chickenLocation sua transf.pos.x, para que 2 galinhas nao ocupem o mesmo lugar.
     */
	#region ChickenSpawner 
	void ChickenSpawner(int amount, int minX, int maxX) //-21 - 19
	{
		if (chickenLocation[21] % 2 == 1) return;
		float rand;
		byte chicIndex;
		for (int actualSpawn = 0; actualSpawn < amount; actualSpawn++) //contador para quantidade de galinhas a serem instanciadas
		{
			for (chicIndex = 0; chickenLocation[chicIndex] % 2 != 0; chicIndex++); //Procura pela menor indice nao ocupado na ChickenLocation

			rand = 2;

			while(rand == 2)
			{
				rand = Random.Range(minX, maxX); // tiro 
				for(byte i = 0; i <= chicIndex; i++) // Previne que se instancie uma galinha em uma transform ocupada por outra galinha
				{
					if(chickenLocation[i] == rand || rand % 2 == 0) rand = 2; //Se a posicao (rand, 9, 1.5) estiver ocupada ou rand for par, seta rand pra 2 e faz outro tiro
				}
			}
			chickenLocation[chicIndex] = rand; // registra que a posicao (rand, 9, 1.5) esta ocupada
			Instantiate(chickenPrefab, new Vector3(rand, 9, 1.5f), Quaternion.Euler(90, -180, 0));
			chickensNum++;
			totalEggs += 30;
		}
	}
	#endregion

    /*
     * Método atualizado todos os frames para checar o estado em que o jogo se encontra
     */
	#region CheckGameState
	void CheckGameState()
	{
		if (initialPause)
        {
            pauseTimer -= Time.deltaTime;

            if (pauseTimer < 0)
			{
                initialPause = false;
			}
        }

		else if (remainingEggs == 0) // Fim do dia
        {
			Debug.Log ("Level Up!");
			gameResults = 1;
    	}

		else if (lossPercentage > lossLimitPercentage) //Gameover
		{
			Debug.Log ("GameOver");
			gameResults = -1; 
		}
	}
	#endregion
	
	#region SetInitialGrid
	void SetInitialGrid()
	{
		switch( _globalManager.dificultySelected) //-21 - 19
		{
		case 3:
			SetHolder(9);
			SetHolder(-7);
			SetHolder(7);
			SetHolder(-5);
			SetHolder(5);
			SetHolder(-3);
			SetHolder(3);
			SetHolder(-1);
			SetHolder(1);
			break;
		case 2:
			SetHolder(7);
			SetHolder(-5);
			SetHolder(5);
			SetHolder(-3);
			SetHolder(3);
			SetHolder(-1);
			SetHolder(1);
			break;
		case 1:
			SetHolder(5);
			SetHolder(-3);
			SetHolder(3);
			SetHolder(-1);
			SetHolder(1);
			break;
		}
	}
	#endregion

	#region SetHolder
	void SetHolder(int x)
	{
		Instantiate(nestPrefab, new Vector3(x, 8.5f, 2.1f), Quaternion.Euler(90, 0, 0));
		for (int y = 1; y < 8; y+=2)
		{
			Instantiate(pipeHolder, new Vector3(x, y, 2), Quaternion.identity);
		}
	}
	#endregion
}
