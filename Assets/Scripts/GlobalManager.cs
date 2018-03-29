using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager : MonoBehaviour {

	public byte storedDificulty = 0;
	public byte dificultySelected = 0;

	private bool difSet;

	GameManager _gameManager;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

	void Update () 
    {
		if(dificultySelected != 0 && !difSet)
        {
			storeSettings();
			gameObject.transform.position = new Vector3(1, 4, -27);
			SceneManager.LoadScene("GalinheiroDoEggvaldo");
        }
	}

	void storeSettings()
    {
		storedDificulty = dificultySelected;
		difSet = true;
    }
}
