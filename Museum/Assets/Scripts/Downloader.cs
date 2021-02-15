
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Downloader : MonoBehaviour
{
    public static bool ready => picReady && admReady;
    private static bool picReady = false;
    private static bool admReady = false;
    // Start is called before the first frame update
    void Start()
    {

        var frames = GameObject.FindObjectsOfType<PictureFrame>();



        Action<List<Image>> downloadPicturesCallback = (pictures) =>
        {
            PictureFrame.allImages=pictures;
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i].FrameNumber = i;
                frames[i].AddPicture(pictures.Where(x => x.FrameNumber == frames[i].FrameNumber).FirstOrDefault());
            }
            picReady = true;
        };
        Action<List<Admin>> adminsCallback = (admins) =>
         {
             EnterMenuController.SetAdmData(admins);
             admReady = true;
         };
        StartCoroutine(DownloadPictures(downloadPicturesCallback));
        StartCoroutine(DownloadAdminData(adminsCallback));


    }

    public static void DeleteImg(Image image)
    {
        var downloader = FindObjectOfType<Downloader>();
        downloader.DeleteImage(image);
    }
    public static void ChangeImg(Image image,int newFrame)
    {
        var downloader = FindObjectOfType<Downloader>();
        downloader.ChangeImage(image,newFrame);

    }
    public void ChangeImage(Image img, int newFrame)
    {
        StartCoroutine(ChangeImageCoroutine(img,newFrame));
    }
    private IEnumerator ChangeImageCoroutine(Image img, int newFrame)
    {
        var request = UnityWebRequest.Get($"http://localhost/MuseumWeb/Processing.aspx?command=change_img&id={img.Id}&number={newFrame}");

        yield return request.SendWebRequest();


    }
    public  void DeleteImage(Image img)
    {
        StartCoroutine(DeleteImageCoroutine(img));
    }
    private  IEnumerator DeleteImageCoroutine(Image img)
    {
        var request = UnityWebRequest.Get($"http://localhost/MuseumWeb/Processing.aspx?command=delete_img&id={img.Id}");

        yield return request.SendWebRequest();

        Debug.Log($"Результат удаления: {request.downloadHandler.text}");
    }
    private IEnumerator DownloadAdminData(Action<List<Admin>> adminsCallback)
    {

        var request = UnityWebRequest.Get("http://localhost/MuseumWeb/Processing.aspx?command=get_adm_data");
        yield return request.SendWebRequest();
        List<Admin> admins = new List<Admin>();
        var serializer = new DataContractJsonSerializer(typeof(List<Admin>));
        using (var stream = new MemoryStream(request.downloadHandler.data))
        {
            admins = serializer.ReadObject(stream) as List<Admin>;

        };
        adminsCallback.Invoke(admins);
    }

    private IEnumerator DownloadPictures(Action<List<Image>> callback)
    {
        var request = UnityWebRequest.Get("http://localhost/MuseumWeb/Processing.aspx?command=get_images");

        yield return request.SendWebRequest();
        if (request.downloadHandler != null)
        {

            List<Image> downloadedPictures = new List<Image>();
            var serializer = new DataContractJsonSerializer(typeof(List<Image>));

            using (var stream = new MemoryStream(request.downloadHandler.data))
            {
                downloadedPictures = serializer.ReadObject(stream) as List<Image>;

            };
            callback.Invoke(downloadedPictures);
        }
        request.Dispose();
    }
  

}
