using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour {

	GlobalManager _globalManager;

	void Awake()
	{
		_globalManager = GameObject.FindGameObjectWithTag("GlobalManager").GetComponent<GlobalManager>();
	}

	void OnMouseEnter()
	{
		GetComponent<Renderer>().material.color = Color.red;
	}

	void OnMouseExit()
	{
		GetComponent<Renderer>().material.color = Color.white;
	}

	void OnMouseOver () {
		if (Input.GetMouseButtonDown(0))
		{
			Destroy(_globalManager.gameObject);
			SceneManager.LoadScene("GalinheiroDoEggvaldo");
		}
	}
}
