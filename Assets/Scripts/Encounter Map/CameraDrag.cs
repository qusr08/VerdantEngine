using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraDrag : MonoBehaviour
{

    private Vector3 origin;
    private Vector3 difference;

    private Camera mainCamera;

    private bool isDragging;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrag(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            origin = GetMousePosition();
            origin.x = transform.position.x;
            origin.z = transform.position.z;
        }
        isDragging = context.started || context.performed;
    }

    private void LateUpdate()
    {
        if(!isDragging)
        {
            return;
        }

        difference = GetMousePosition() - transform.position;
        difference.x = 0;
        difference.z = 0;
        transform.position = origin - difference;

        if(transform.position.y >= 23)
        {
            transform.position = new Vector3(transform.position.x, 23, transform.position.z);
        }
        else if (transform.position.y <= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }

    }

    private Vector3 GetMousePosition()
    {
        return mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }


}
