using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stand : MonoBehaviour
{

    public KeyCode ActivateKey { get; set; }
    private void Awake()
    {
        ActivateKey = KeyCode.F;

    }
    public void Activate()
    {
        InterfaceController.OpenMenu("EnterMenu");
    }
    public string CreateToolTip()
    {
        return $"Открыть [{ActivateKey}]";
    }
}
