
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenuController : Menu
{
    public static List<Image> pictures;
    private Button left;
    private Button right;
    private Button put;
    private Button exit;
    private Button delete;
    private UnityEngine.UI.Image picture;
    private Text descriptionPic;
    private Text namePic;
    private RectTransform areaPic;
    int currentPic;
    private void Awake()
    {

        left = transform.GetComponentsInChildren<Transform>().Where(x => x.name.ToLower() == "Left".ToLower()).FirstOrDefault().GetComponent<Button>();
        right = transform.GetComponentsInChildren<Transform>().Where(x => x.name.ToLower() == "Right".ToLower()).FirstOrDefault().GetComponent<Button>();
        put = transform.GetComponentsInChildren<Transform>().Where(x => x.name.ToLower() == "Put".ToLower()).FirstOrDefault().GetComponent<Button>();
        exit = transform.GetComponentsInChildren<Transform>().Where(x => x.name.ToLower() == "Exit".ToLower()).FirstOrDefault().GetComponent<Button>();
        delete = transform.GetComponentsInChildren<Transform>().Where(x => x.name.ToLower() == "Delete".ToLower()).FirstOrDefault().GetComponent<Button>();
        areaPic = transform.GetComponentsInChildren<Transform>().Where(x => x.name.ToLower() == "PictureArea".ToLower()).FirstOrDefault().GetComponent<RectTransform>();
        picture = transform.GetComponentsInChildren<Transform>().Where(x => x.name.ToLower() == "Picture".ToLower()).FirstOrDefault().GetComponent<UnityEngine.UI.Image>();
        namePic = transform.GetComponentsInChildren<Transform>().Where(x => x.name.ToLower() == "Name".ToLower()).FirstOrDefault().GetComponent<Text>();
        descriptionPic = transform.GetComponentsInChildren<Transform>().Where(x => x.name.ToLower() == "Description".ToLower()).FirstOrDefault().GetComponent<Text>();
        //left.gameObject.SetActive(false);
        // right.gameObject.SetActive(false);
        left.onClick.AddListener(ButtonLeft);
        right.onClick.AddListener(ButtonRight);
        put.onClick.AddListener(PutButton);
        exit.onClick.AddListener(ExitButton);
        delete.onClick.AddListener(DeleteButton);


    }
    public void ExitButton()
    {
        InterfaceController.OpenMenu("GameMenu");
    }
    public void PutButton()
    {
        if (pictures.Count > 0)
        {

            GameMenuController.SetPic(pictures[currentPic]);
        }
        InterfaceController.OpenMenu("GameMenu");
    }
    public void DeleteButton()
    {
        if (pictures.Count == 0)
        {
            return;
        }
        Downloader.DeleteImg(pictures[currentPic]);
        pictures.RemoveAt(currentPic);
        TapButton(false);
    }
    public override void Open(bool closeOther = true)
    {
        if (pictures == null)
        {
            pictures = new List<Image>();
        }
        SetPictures(PictureFrame.allImages);
        base.Open(closeOther);
        currentPic = 0;
        SetPicture();
    }
    public static void SetPictures(List<Image> pics)
    {

        pictures = pics.Where(x => x.FrameNumber < 0).ToList();
    }
    private void ButtonRight()
    {
        TapButton(true);
    }
    private void ButtonLeft()
    {
        TapButton(false);
    }
    private void TapButton(bool right)
    {
        if (right)
        {

            if (currentPic == pictures.Count - 1)
            {
                currentPic = 0;
            }
            else
            {
                currentPic++;
            }
        }
        else
        {
            if (currentPic == 0)
            {
                currentPic = pictures.Count - 1;
            }
            else
            {
                currentPic--;
            }
        }
        SetPicture();

    }
    private void SetPicture()
    {
        var tex = new Texture2D(1024, 1024);

        if (pictures.Count == 0)
        {
            tex = Texture2D.redTexture;

            namePic.text = "Картин нет";

            descriptionPic.text = "";
        }
        else
        {

            var img = pictures[currentPic];

            namePic.text = "\"" + img.Name + "\"";
            descriptionPic.text = img.Description;
            tex.LoadImage(img.PictureBytes);
        }
        tex.Apply();
        float w = tex.width;
        float h = tex.height;
        float coef = 1;
        Sprite pic = Sprite.Create(tex, new Rect(new Vector2(0, 0), new Vector2(tex.width, tex.height)), new Vector2(0.5f, 0.5f));
        picture.sprite = pic;
        picture.SetNativeSize();
        var picSize = picture.rectTransform.sizeDelta;
        if (picSize.x > areaPic.rect.width)
        {
            coef *= areaPic.rect.width / picSize.x;
            picSize *= coef;
        }
        coef = 1;
        if (picSize.y > areaPic.rect.height)
        {
            coef *= areaPic.rect.height / picSize.y;
            picSize *= coef;
        }
        picture.rectTransform.sizeDelta = picSize;

    }

}
