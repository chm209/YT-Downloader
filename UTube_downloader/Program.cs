using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using VideoLibrary;

namespace YTdownloader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            int downloadType;
            string ? playListID, outputPath;
            string NextPageToken = string.Empty;
            string[] textValue;
            
            // ↓ URI 추출
            // 유튜브 API 사용
            var youtube = YouTube.Default;
            YouTubeService youtubeService = new YouTubeService(new BaseClientService.Initializer() { ApiKey = "API 키 입력" });

            Console.WriteLine("EX) D:\\save\\");ddd
            Console.Write("파일 저장 위치: ");
            outputPath = Console.ReadLine();
            Console.Write("재생목록 ID 입력: ");
            playListID = Console.ReadLine();

            // URI 추출후 txt 파일로 저장
            while (NextPageToken != null)
            {
                // 재생목록 정보 전달
                var playlistRequest = youtubeService.PlaylistItems.List("snippet");
                playlistRequest.PlaylistId = playListID;
                playlistRequest.MaxResults = 50;
                playlistRequest.PageToken = NextPageToken;

                //  재생목록 동기화
                var videos = await playlistRequest.ExecuteAsync();
                // 영상 url 추출
                foreach (var video in videos.Items)
                {
                    Console.WriteLine("영상제목: " + video.Snippet.Title);
                    Console.WriteLine("URL: https://www.youtube.com/watch?v=" + video.Snippet.ResourceId.VideoId);

                    using (StreamWriter outputFile = new StreamWriter(outputPath + "listURI.txt", true))
                    {
                        outputFile.WriteLine("https://www.youtube.com/watch?v=" + video.Snippet.ResourceId.VideoId);
                    }
                    NextPageToken = videos.NextPageToken;
                }
            }
            // ↓ 다운로드 시작
            textValue = File.ReadAllLines(outputPath + "listURI.txt");
            Console.WriteLine("1: MP3 / 2: MP4");
            Console.Write("입력: ");
            downloadType = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("다운받을 영상 " + textValue.Length + "EA");

            // 다운로드
            if (textValue.Length > 0)
            {
                for (int k = 0; k < textValue.Length; k++)
                {
                    var video = YouTube.Default.GetAllVideos(textValue[k]).First(v => v.Resolution == 720);

                    Console.WriteLine(video.FullName + " 다운로드 시작" + "[" + (k + 1) + "]/[" + textValue.Length + "]");
                    if (downloadType == 1)
                    {
                        var videoInfos = await youtube.GetAllVideosAsync(textValue[k]);
                        var downloadInfo = videoInfos.Where(i => i.AudioFormat == AudioFormat.Aac && i.AudioBitrate == 128).FirstOrDefault();
                        if (downloadInfo != null)
                        {
                            File.WriteAllBytes(outputPath + downloadInfo.FullName + ".mp3", downloadInfo.GetBytes());
                        }
                    }
                    else
                    {
                        var videoInfos = await youtube.GetAllVideosAsync(textValue[k]);
                        var downloadInfo = videoInfos.Where(i => i.Format == VideoFormat.Mp4 && i.Resolution == 720).FirstOrDefault();

                        if(downloadInfo != null)
                        {
                            File.WriteAllBytes(outputPath + downloadInfo.FullName + ".mp4", downloadInfo.GetBytes());
                        }
                    }
                    Console.WriteLine("다운로드 완료" + "[" + (k + 1) + "]/[" + textValue.Length + "]");
                }
            }
            else
            {
                Console.WriteLine("파일이 비었습니다.\n");
            }
        }
    }
}