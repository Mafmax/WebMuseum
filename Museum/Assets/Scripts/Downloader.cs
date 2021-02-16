
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class Downloader : MonoBehaviour
{
    public static bool ready => picReady && admReady;
    private static bool picReady = false;
    private static bool admReady = false;
    private string connectionString;
    private static string RemoveServerComment(string data)
    {
        string newData = data;

        var regex = Regex.Match(data, @"<!--[^#]*").ToString();
        if (regex != "")
        {
            newData = data.Replace(regex, "");
        }
        return newData;
    }
    // Start is called before the first frame update
    void Start()
    {
        //  connectionString = "http://localhost/MuseumWeb/Processing.aspx?";

        connectionString = "http://www.avtor.somee.com/Processing.aspx?";



        var frames = GameObject.FindObjectsOfType<PictureFrame>();



        Action<List<Image>> downloadPicturesCallback = (pictures) =>
        {
            PictureFrame.allImages = pictures;
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
    public static void ChangeImg(Image image, int newFrame)
    {
        var downloader = FindObjectOfType<Downloader>();
        downloader.ChangeImage(image, newFrame);

    }
    public void ChangeImage(Image img, int newFrame)
    {
        StartCoroutine(ChangeImageCoroutine(img, newFrame));
    }
    private IEnumerator ChangeImageCoroutine(Image img, int newFrame)
    {
        var request = UnityWebRequest.Get($"{connectionString}command=change_img&id={img.Id}&number={newFrame}");

        yield return request.SendWebRequest();


    }
    public void DeleteImage(Image img)
    {
        StartCoroutine(DeleteImageCoroutine(img));
    }
    private IEnumerator DeleteImageCoroutine(Image img)
    {
        var request = UnityWebRequest.Get($"{connectionString}command=delete_img&id={img.Id}");

        yield return request.SendWebRequest();

    }
    private IEnumerator DownloadAdminData(Action<List<Admin>> adminsCallback)
    {

        var request = UnityWebRequest.Get($"{connectionString}command=get_adm_data");
        yield return request.SendWebRequest();
        Debug.LogError($"Полученный текст админы: {request.downloadHandler.text}");
        List<Admin> admins = new List<Admin>();
        var serializer = new DataContractJsonSerializer(typeof(List<Admin>));
        var data = Encoding.UTF8.GetBytes(RemoveServerComment(request.downloadHandler.text));
        using (var stream = new MemoryStream(data))
        {
            admins = serializer.ReadObject(stream) as List<Admin>;

        };
        adminsCallback.Invoke(admins);
    }

    private IEnumerator DownloadPictures(Action<List<Image>> callback)
    {
        var request = UnityWebRequest.Get($"{connectionString}command=get_images");

        yield return request.SendWebRequest();
        Debug.LogError($"Полученный текст картины: {request.downloadHandler.text}");
        if (request.downloadHandler != null)
        {

            List<Image> downloadedPictures = new List<Image>();
            var serializer = new DataContractJsonSerializer(typeof(List<Image>));
            var data = Encoding.UTF8.GetBytes(RemoveServerComment(request.downloadHandler.text));
            using (var stream = new MemoryStream(data))
            {
                downloadedPictures = serializer.ReadObject(stream) as List<Image>;

            };
            callback.Invoke(downloadedPictures);
        }
        request.Dispose();
    }


}
