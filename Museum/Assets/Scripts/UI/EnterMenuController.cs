using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EnterMenuController : Menu
{
    private static List<Admin> Admins;
    public static void SetAdmData(List<Admin> admins)
    {
        Admins = admins;
    }
    public static bool isAdmin;
    private InputField login;
    private InputField password;
    private Text error;
    private Button submit;
    // Start is called before the first frame update
    void Start()
    {
        isAdmin = false;
        login = transform.GetComponentsInChildren<Transform>().Where(x => x.name.ToLower() == "InputLogin".ToLower()).FirstOrDefault().GetComponent<InputField>();
        password = transform.GetComponentsInChildren<Transform>().Where(x => x.name.ToLower() == "InputPassword".ToLower()).FirstOrDefault().GetComponent<InputField>();
        submit = transform.GetComponentsInChildren<Transform>().Where(x => x.name.ToLower() == "EnterButton".ToLower()).FirstOrDefault().GetComponent<Button>();
        submit.onClick.AddListener(Enter);
        error = transform.GetComponentsInChildren<Transform>().Where(x => x.name.ToLower() == "ErrorText".ToLower()).FirstOrDefault().GetComponent<Text>();
        error.text = "";

    }
    public override void Open(bool closeOther = true)
    {
        base.Open(closeOther);
        if (isAdmin)
        {

            InterfaceController.OpenMenu("InventoryMenu");
        }
    }

    private void Enter()
    {

        if (Admins.Where(x => x.Login == login.text && x.Password == password.text).Any())
        {
            isAdmin = true;
            InterfaceController.OpenMenu("InventoryMenu");
        }
        else
        {
            error.text = "Неверный логин или пароль!!!";
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
