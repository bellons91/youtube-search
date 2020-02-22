using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace YouTubeSearch
{
    class Program
    {
         static  void Main(string[] args)
        {
            var configurations = Configurations.GetConfig();
            var videos = GetVideosList(configurations).GetAwaiter().GetResult();
            foreach (var video in videos)
            {

                Console.WriteLine($"Video {video.Id} - Title: {video.Title}");
                Console.WriteLine(video.Description);
                Console.WriteLine($"Thumbnail: {video.Thumbnail}");
                Console.WriteLine("************");
            }

            Console.ReadKey();
        }

        async static Task<IEnumerable<YouTubeVideo>> GetVideosList(Configurations configurations, string searchText = "", int maxResult = 20)
        {
            List<YouTubeVideo> videos = new List<YouTubeVideo>();

            using (var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = configurations.ApiKey
            }))
            {
                var searchListRequest = youtubeService.Search.List("snippet");
                searchListRequest.Q = searchText;
                searchListRequest.MaxResults = maxResult;
                searchListRequest.ChannelId = configurations.ChannelId;
                searchListRequest.Type = "video";
                searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Relevance;


                var searchListResponse = await searchListRequest.ExecuteAsync();


                foreach (var responseVideo in searchListResponse.Items)
                {
                    videos.Add(new YouTubeVideo()
                    {
                        Id = responseVideo.Id.VideoId,
                        Description = responseVideo.Snippet.Description,
                        Title = responseVideo.Snippet.Title,
                        Picture = GetMainImg(responseVideo.Snippet.Thumbnails),
                        Thumbnail = GetThumbnailImg(responseVideo.Snippet.Thumbnails)
                    });
                }

                return videos;
            }

        }


        private static string GetThumbnailImg(ThumbnailDetails thumbnailDetails)
        {
            if (thumbnailDetails == null)
                return string.Empty;
            return (thumbnailDetails.Medium ?? thumbnailDetails.Default__ ?? thumbnailDetails.High)?.Url;
        }


        private static string GetMainImg(ThumbnailDetails thumbnailDetails)
        {
            if (thumbnailDetails == null)
                return string.Empty;
            return (thumbnailDetails.Maxres ?? thumbnailDetails.Standard ?? thumbnailDetails.High)?.Url;
        }

    }
}
