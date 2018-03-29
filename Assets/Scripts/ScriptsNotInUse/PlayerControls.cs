using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour
{
    public float speed = 1;

    private Transform _myTransform;
    private CharacterController _controller;

    public void Awake()
    {
        _myTransform = transform;
        _controller = GetComponent<CharacterController>();
    }

	void Update()
    {
        Move();   
    }

    public void Move()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
            _controller.SimpleMove(_myTransform.TransformDirection(Vector3.right) * Input.GetAxis("Horizontal") * speed);
    }
}