using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Conference.Contracts.Models;
using System.IO;
using System.Net;

namespace DownloadSpeakerPhotosTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var json = File.ReadAllText("data.json");
            var data = JsonConvert.DeserializeObject<ConferenceData>(json);
            var pictureUrls = data.Speakers.Select(s => s.PictureUrl);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var pictureUrl in pictureUrls)
            {
                stringBuilder.Append(pictureUrl).Append(",");
            }
            stringBuilder.Remove(stringBuilder.Length-1, 1);
            string final = stringBuilder.ToString();

            //var client = new WebClient();
            //if(!Directory.Exists("SpeakerPhotos"))
            //{
            //    Directory.CreateDirectory("SpeakerPhotos");
            //}
            //foreach (var speaker in data.Speakers)
            //{
            //    var photoUrl = "http://tarabica.msforge.net/"+speaker.PictureUrl;
            //    var photo = client.DownloadData(photoUrl);
            //    File.WriteAllBytes("SpeakerPhotos" + photoUrl.Substring(photoUrl.LastIndexOf('/')), photo);
            //}
        }
    }
}
