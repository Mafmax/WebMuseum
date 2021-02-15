using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuController : Menu
{

    private static Image MyPic;
    private RectTransform picArea;
    private UnityEngine.UI.Image pic;
    private Text NamePicture;
    private Text DescriptionPicture;
    private Text ToolTip;
    private Camera mainCam;
    private RectTransform cursor;
    // Start is called before the first frame update
    void Awake()
    {
        mainCam = Camera.main;
        NamePicture = transform.GetComponentsInChildren<Transform>().Where(x => x.name.ToLower() == "name").FirstOrDefault().GetComponent<Text>();
        DescriptionPicture = transform.GetComponentsInChildren<Transform>().Where(x => x.name.ToLower() == "description").FirstOrDefault().GetComponent<Text>();
        ToolTip = transform.GetComponentsInChildren<Transform>().Where(x => x.name.ToLower() == "tooltip").FirstOrDefault().GetComponent<Text>();
        picArea = transform.GetComponentsInChildren<Transform>().Where(x => x.name.ToLower() == "MyPictureArea".ToLower()).FirstOrDefault().GetComponent<RectTransform>();
        pic = transform.GetComponentsInChildren<Transform>().Where(x => x.name.ToLower() == "Picture".ToLower()).FirstOrDefault().GetComponent<UnityEngine.UI.Image>();
        cursor = transform.GetComponentsInChildren<Transform>().Where(x => x.name.ToLower() == "Cursor".ToLower()).FirstOrDefault().GetComponent<RectTransform>();
        MyPic = null;
    }
    public override void Open(bool closeOther = true)
    {
        base.Open(closeOther);
        RemovePictureInfo();
        ToolTip.text = "";
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ShowPic();
    }
    public static void SetPic(Image img)
    {
        MyPic = img;
    }
    private void ShowPic()
    {
        if (MyPic == null)
        {

            pic.gameObject.SetActive(false);
            return;

        }
        else
        {
            pic.gameObject.SetActive(true);
        }

        var tex = new Texture2D(1024, 1024);
        var img = MyPic;
        tex.LoadImage(img.PictureBytes);


        tex.Apply();
        float coef = 1;
        Sprite picture = Sprite.Create(tex, new Rect(new Vector2(0, 0), new Vector2(tex.width, tex.height)), new Vector2(0.5f, 0.5f));
        pic.sprite = picture;
        pic.SetNativeSize();
        var picSize = pic.rectTransform.sizeDelta;
        if (picSize.x > picArea.rect.width)
        {
            coef *= picArea.rect.width / picSize.x;
            picSize *= coef;
        }
        coef = 1;
        if (picSize.y > picArea.rect.height)
        {
            coef *= picArea.rect.height / picSize.y;
            picSize *= coef;
        }
        pic.rectTransform.sizeDelta = picSize;
    }
    public void ShowPictureInfo(Image image)
    {
        if (image == null)
        {
            NamePicture.text = "";
            DescriptionPicture.text = "";
            return;
        }
        NamePicture.text = "\"" + image.Name + "\"";
        DescriptionPicture.text = image.Description;
    }


    public void RemovePictureInfo()
    {
        NamePicture.text = "";
        DescriptionPicture.text = "";
    }
    // Update is called once per frame
    public IEnumerator ShowPicCoroutine()
    {
        while (IsOpen())
        {

            ShowPic();
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        Ray ray = mainCam.ScreenPointToRay(cursor.position);
        var hits = Physics.RaycastAll(ray, 8);

        var standHit = hits.Where(x => x.transform.TryGetComponent(out Stand stand)).FirstOrDefault();
        var frameHit = hits.Where(x => x.transform.TryGetComponent(out PictureFrame frame)).FirstOrDefault();

        if (standHit.transform != null)
        {
          

            var stand = standHit.transform.GetComponent<Stand>();
            ToolTip.text = stand.CreateToolTip();
            if (Input.GetKey(stand.ActivateKey))
            {
                stand.Activate();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

            }
        }
        else if (frameHit.transform != null)
        {
            if (!EnterMenuController.isAdmin)
            {
                ToolTip.text = "Необходимо обладать правами администратора.";
                return;
            }
            var frame = frameHit.transform.GetComponent<PictureFrame>();
            ToolTip.text = frame.CreateTooltip(MyPic != null);
            ShowPictureInfo(frame.picture);
            if (Input.GetKeyDown(frame.AddImage))
            {
                MyPic = frame.AddPicture(MyPic);
                ShowPic();
            }
            else if (Input.GetKeyDown(frame.RemoveImage))
            {
                frame.RemovePicture();
            }

        }
        else
        {
            ToolTip.text = "";
            ShowPictureInfo(null);
        }
    }

}

