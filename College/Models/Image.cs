using System;
using System.Collections.Generic;

namespace College.Models
{
    public class JsonFile
    {
        public ImageBB Data { get; set; }
        public int Status { get; set; }
        public bool Success { get; set; }
    }
    public class Image
    {
        public string FileName { get; set; }
        public string Name { get; set; }
        public string Mime { get; set; }
        public string Extension { get; set; }
        public Uri Url { get; set; }
        public int Size { get; set; }
    }

    public class Thumb
    {
        public string FileName { get; set; }
        public string Name { get; set; }
        public string Mime { get; set; }
        public string Extension { get; set; }
        public Uri Url { get; set; }
        public int Size { get; set; }
    }

    public class Medium
    {
        public string FileName { get; set; }
        public string Name { get; set; }
        public string Mime { get; set; }
        public string Extension { get; set; }
        public Uri Url { get; set; }
        public int Size { get; set; }

    }
    public class ImageBB
    {
        public string Id { get; set; }
        public Uri Url_Viewer { get; set; }
        public Uri Url { get; set; }
        public Uri Display_Url { get; set; }
        public string Title { get; set; }
        public long Time { get; set; }
        public Image Image { get; set; }
        public Uri Delete_Url { get; set; }
    }
}
