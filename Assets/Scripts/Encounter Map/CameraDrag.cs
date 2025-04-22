using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraDrag : MonoBehaviour
{

    [SerializeField] private float scrollSpeed = .2f;

    private float yValueChange;

    private Camera mainCamera;

    private bool isDragging;

    // Start is called before the first frame update
    void Start()
    {
        yValueChange = 0;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrag(InputAction.CallbackContext context)
    {
        
        isDragging = context.started || context.performed;
        
        if(isDragging)
        {
            yValueChange = context.ReadValue<float>();

            float newY = transform.position.y + (yValueChange * scrollSpeed);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            if (transform.position.y >= 23)
            {
                transform.position = new Vector3(transform.position.x, 23, transform.position.z);
            }
            else if (transform.position.y <= 0)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
        }
    }



    //Currently unused
    private Vector3 GetMousePosition()
    {
        return mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }


}
