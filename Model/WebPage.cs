using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageDownloader.Model
{
    class WebPage
    {
        private List<WebImage> _images;
        private Uri _baseUrl;
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;

        public event EventHandler<WebImage> ImageLoaded;
        public event EventHandler<int> LoadProgress;

        public Uri BaseUrl { get { return _baseUrl; } }

        public int ImageCount { get { return _images.Count; } }

        public ICollection<WebImage> Images { get { return _images; } }

        
        public void CancelLoad()
        {
            _cancellationTokenSource.Cancel();
        }
        public WebPage(Uri baseUrl)
        {
            _baseUrl = baseUrl;
            _images = new List<WebImage>();
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;

        }
        public async Task LoadImagesAsync()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(BaseUrl, _cancellationToken); // HttpResponseMessage
            var content = await response.Content.ReadAsStringAsync(); // string
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(content);
            var nodes = doc.DocumentNode.SelectNodes("//img");

            if(nodes == null)
            {
                throw new Exception();
            }
            int i = 0;
            foreach (var node in nodes)
            {
                if (_cancellationToken.IsCancellationRequested)
                    break;
                OnLoadProgress(nodes.Count-i);
                if (!node.Attributes.Contains("src"))
                {
                    i++;
                    continue;
                }
                Uri imageUrl = new Uri(node.Attributes["src"].Value, UriKind.RelativeOrAbsolute);
                if (!imageUrl.IsAbsoluteUri)
                {
                    imageUrl = new Uri(BaseUrl, imageUrl);
                }
                try
                {
                    var image = await WebImage.DownloadAsync(imageUrl);
                    _images.Add(image);
                    OnImageLoaded(image);
                }
                catch
                {
                    // ignored
                    i++;
                }
            }
            OnLoadProgress(nodes.Count - i);
        }
        public void OnImageLoaded(WebImage image)
        {
            if(ImageLoaded != null)
            {
                ImageLoaded(this, image);
            }
        }
        public void OnLoadProgress(int all)
        {
            if(LoadProgress != null)
            {
                Debug.WriteLine(all + " " + ImageCount);
                LoadProgress(this, (int)((double)ImageCount / all * 100));
            }
        }
    }
}
