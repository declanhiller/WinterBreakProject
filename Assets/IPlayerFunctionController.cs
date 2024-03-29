using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public interface IPlayerFunctionController
{

    public void Tick();
    
    public void SetKeybinds(KeybindController keybindController);
    
    public void ReceiveMessage(string msg);

    public int GetPriority();

}