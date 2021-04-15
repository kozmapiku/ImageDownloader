using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ImageDownloader.Model
{
    class WebImage
    {
        private Uri _url;
        private Image _image;

        public Uri Url { get { return _url; } }
        public Image Image { get { return _image; } }

        public static async Task<WebImage> DownloadAsync(Uri url)
        {
            if(url != null && url.IsAbsoluteUri)
            {
                HttpClient client = new HttpClient();
                Stream stream = await client.GetStreamAsync(url);
                Image image = Image.FromStream(stream);
                WebImage back = new WebImage();
                back._url = url;
                back._image = image;
                return back;
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
