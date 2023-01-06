using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    
    private List<IPlayerFunctionController> controllers = new List<IPlayerFunctionController>();
    
    private KeybindController keybindController;

    private Bounds characterBounds;
    private LayerMask collisionMask;

    private void Awake()
    {
        keybindController = GetComponent<KeybindController>();
    }

    private void Start()
    {

        SortControllers();
        
        foreach (IPlayerFunctionController controller in controllers)
        {
            controller.SetKeybinds(keybindController);
        }
    }

    private void SortControllers()
    {
        controllers = controllers.OrderBy(controller => controller.GetPriority()).ToList();
    }

    private void Update()
    {
        
        foreach (IPlayerFunctionController controller in controllers)
        {
            controller.Tick();
        }
    }

    public void SendMsg(string msg)
    {
        Debug.Log("Player sent: " + msg);
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