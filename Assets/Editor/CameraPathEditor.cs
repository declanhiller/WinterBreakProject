using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;


[EditorTool("CameraPath", typeof(CameraController))]
public class CameraPathEditor : EditorTool
{

    
    void OnEnable()
    {
        ToolManager.activeToolChanged += ActiveToolDidChange;
    }

    void OnDisable()
    {
        ToolManager.activeToolChanged -= ActiveToolDidChange;
    }

    void ActiveToolDidChange()
    {
        if (!ToolManager.IsActiveTool(this))
            return;
        
        

    }

    public override void OnToolGUI(EditorWindow window)
    {
        
    }
}
