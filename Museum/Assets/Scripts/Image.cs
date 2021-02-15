using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class Image
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int FrameNumber { get; set; }
    public string Picture { get; set; }
    public byte[] PictureBytes => Convert.FromBase64String(Picture);
}
