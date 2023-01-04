using System;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    
    private List<IPlayerFunctionController> controllers = new List<IPlayerFunctionController>();
    
    private KeybindController keybindController;

    private void Awake()
    {
        keybindController = GetComponent<KeybindController>();
    }

    private void Start()
    {
        foreach (IPlayerFunctionController controller in controllers)
        {
            controller.SetKeybinds(keybindController);
        }
    }

    private void Update()
    {
        
    }

    public void SendMsg(string msg)
    {
        foreach (IPlayerFunctionController controller in controllers)
        {
            controller.ReceiveMessage(msg);
        }
    }


    public void AddFunction(IPlayerFunctionController playerFunctionController)
    {
        controllers.Add(playerFunctionController);
    }
}