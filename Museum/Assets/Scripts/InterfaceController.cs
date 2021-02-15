using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InterfaceController : MonoBehaviour
{
    public static Dictionary<string, Transform> Menus = new Dictionary<string, Transform>();
    // Start is called before the first frame update
    private static List<string> names = new List<string>();
    void Start()
    {
        names.Add("GameMenu".ToLower());
        names.Add("InventoryMenu".ToLower());
        names.Add("EnterMenu".ToLower());
        foreach (var name in names)
        {
             Menus[name.ToLower()] = GetComponentsInChildren<Transform>(true).Where(x => x.name.ToLower() == name.ToLower()).FirstOrDefault();
        }
        Menus[names[0]].gameObject.AddComponent<GameMenuController>();
        Menus[names[1]].gameObject.AddComponent<InventoryMenuController>();
        Menus[names[2]].gameObject.AddComponent<EnterMenuController>();

        CloseMenus();

        StartCoroutine(WaitLoad());
    }
    private IEnumerator WaitLoad()
    {
        while (!Downloader.ready)
        {
            yield return null;
        }
        Menus[names[0]].GetComponent<Menu>().Open();
    }
    public static void OpenMenu(string menuName,bool closeOther=true)
    {
        Menus[menuName.ToLower()].GetComponent<Menu>().Open(closeOther);
    }
    public static void CloseMenus()
    {
        foreach (var name in names)
        {
            Menus[name].GetComponent<Menu>().Close();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            OpenMenu("GameMenu");
        }
    }
}
