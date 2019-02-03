using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using System;

public class TargetHandler : MonoBehaviour
{

    //Swap curser per target type
    public Texture2D pointer; //Normal pointer
    public Texture2D target; //Cursor for clickable objects
    public Texture2D door; //Cursor for doorways
    public Texture2D combat; //Cursor for combat actions

    enum CursorType { Pointer, Door, Target, Combat };
    CursorType cursorType;

    // Start is called before the first frame update
    void Start()
    {
        cursorType = CursorType.Pointer;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            UpdateTargetCursorIcon(hit);
            HandleUserInput();
        }
    }

    private void UpdateTargetCursorIcon(RaycastHit hit)
    {
        if (hit.collider.gameObject.tag == "Door" && hit.distance < 3)
        {
            cursorType = CursorType.Door;
            Target targetCursor = FindObjectOfType<Target>();
            targetCursor.GetComponent<RawImage>().texture = door;
            targetCursor.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
        }
        else
        {
            cursorType = CursorType.Pointer;
            Target targetCursor = FindObjectOfType<Target>();
            targetCursor.GetComponent<RectTransform>().sizeDelta = new Vector2(5, 5);
            targetCursor.GetComponent<RawImage>().texture = pointer;
        }
    }

    private void HandleUserInput()
    {
        if (CrossPlatformInputManager.GetButtonDown("Interact")) {
            if(cursorType == CursorType.Door)
            {
                SceneManager.LoadScene("Shuli's House");
            }
        }
    }
}
