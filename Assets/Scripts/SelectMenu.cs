using UnityEngine;
using System.Collections;

public class SelectMenu : MonoBehaviour 
{
	GlobalManager _global;
	TextMesh _textMesh;

	void Start()
	{
		_global = GameObject.FindGameObjectWithTag("GlobalManager").GetComponent<GlobalManager>();
		_textMesh = gameObject.transform.GetComponent<TextMesh>();
	}

	void OnMouseEnter() {
		switch(_textMesh.text)
		{
		case "Easy" :
			//renderer.material.color = Color.green;
			break;
		case "Medium" :
			//renderer.material.color = Color.yellow;
			break;
		case "Hard" :
			//renderer.material.color = Color.red;
			break;
		}
		_textMesh.fontSize = 20;
	}
	
	void OnMouseExit() {
		GetComponent<Renderer>().material.color = Color.white;
		_textMesh.fontSize = 0;
	}
	
	void OnMouseUp()
	{
		switch(_textMesh.text)
		{
		case "Easy" :
			_global.dificultySelected = 1;
			break;
		case "Medium" :
			_global.dificultySelected = 2;
			break;
		case "Hard" :
			_global.dificultySelected = 3;
			break;
		}
	}
}
