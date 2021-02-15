using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Menu : MonoBehaviour
{


    public virtual void Open(bool closeOther=true)
    {
        if (closeOther && !gameObject.activeSelf)
        {
            InterfaceController.CloseMenus();
        }
        gameObject.SetActive(!gameObject.activeSelf);
    }
    public virtual bool IsOpen()
    {
        return gameObject.activeSelf;
    }
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
  
}
