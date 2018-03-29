using UnityEngine;
using System.Collections;

public class PipeBehaviour : MonoBehaviour {

	GameManager _gameManager;

	public Material[] materials;
	/* gold
	 * silver
	 * bronze
	 */

	private bool qualitySet = false;
	private bool glowYellow = false;
	private bool glowRed = false;
	
	private float timer;

	private byte quality; //number of eggs that it can manage to pass through without breaking
	private byte poorQuality;

	void Awake()
	{
		_gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

	void FixedUpdate()
	{
		if(!qualitySet) SetQuality(); //Check if this pipe quality has been set. If not, set it getting the currently selected qualitySelect right in the Instantiation time
		else
		{
			CheckQuality();

			if (glowYellow)
			{
				timer += Time.deltaTime;
				GlowYellow();
			}

			else if (glowRed) 
			{
				timer += Time.deltaTime;
				GlowRed();
			}
		}
	}

	void OnCollisionExit(UnityEngine.Collision other) 
	{
		if (other.gameObject.tag == "egg")
		{
			quality--;
		}
	}

	void OnMouseOver() {
		if (Input.GetMouseButtonDown(1)) { // Se clicado com o direito, destroi o pipe, possibilitando o player de colocar um outro pipe no mesmo local
			if (quality > poorQuality) _gameManager.cash += 0.5f; // Se o cano ainda n estiver quebrado, reembolsa $0.5
			Destroy(gameObject);
		}
	}

	#region SetQuality()
	void SetQuality()
	{
		timer = 0;
		switch(_gameManager.qualitySelect) {
				case 0: 
					quality = 30; // Golden pipe currently selected in the moment of the instantion
					poorQuality = 15;
					GetComponent<Renderer>().material = materials[0];//texture change
					qualitySet = true;
					break;
				case 1: 
					quality = 20;// Silver pipe currently selected in the moment of the instantion
					poorQuality = 10;
					GetComponent<Renderer>().material = materials[1];//texture change
					qualitySet = true;
					break;
				case 2: 
					quality = 10;// Bronze pipe currently selected in the moment of the instantion
					poorQuality = 5;
					GetComponent<Renderer>().material = materials[2];//texture change
					qualitySet = true;
					break;
			}
	}
	#endregion

	#region CheckQuality
	void CheckQuality()
	{
		if ( quality == poorQuality )
		{
			glowYellow = true;
		}

		else if (quality < 3 && quality > 0)
		{
			glowYellow = false;
			timer = 0;
			glowRed = true;
		}

		else if (quality == 0) {
			Destroy(gameObject);
		}
	}
	#endregion

	#region GlowYellow
	void GlowYellow()
	{
		if(timer < 2) 
		{
			GetComponent<Renderer>().material.color = Color.yellow;
		}

		else if(timer < 4)
		{
			GetComponent<Renderer>().material.color = Color.white;
		}

		else timer = 0;
	}
	#endregion

	#region GlowRed
	void GlowRed()
	{
		if(timer < 2) 
		{
			GetComponent<Renderer>().material.color = Color.red;
		}
		
		else if(timer < 3)
		{
			GetComponent<Renderer>().material.color = Color.white;
		}

		else timer = 0;
	}
	#endregion
}
