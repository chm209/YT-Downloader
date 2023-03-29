using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using VideoLibrary;

namespace YTdownloader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            int download_type = 0;
            string ? list_id, page_token = string.Empty;
            string output_path = "./output/";
            string[] text_value;

            // 유튜브 API 설정
            var youtube = YouTube.Default;
            YouTubeService youtubeService = new YouTubeService(new BaseClientService.Initializer() { ApiKey = "API KEY" });

            // 플레이리스트 아이디 입력
            Console.Write("재생목록 ID 입력: ");
            list_id = Console.ReadLine();

            // 폴더 생성
            DirectoryInfo di = new DirectoryInfo(output_path);

            // 만약 폴더가 존재하지 않으면
            if (di.Exists == false)
            {
                di.Create();
            }

            // URL 추출후 텍스트 파일로 출력
            while (page_token != null)
            {
                // 재생목록 정보 전달
                var playlistRequest = youtubeService.PlaylistItems.List("snippet");
                playlistRequest.PlaylistId = list_id;
                playlistRequest.MaxResults = 50;
                playlistRequest.PageToken = page_token;

                //  재생목록 동기화
                var videos = await playlistRequest.ExecuteAsync();
                // 영상 URL 추출
                foreach (var video in videos.Items)
                {
                    Console.WriteLine("영상제목: " + video.Snippet.Title);
                    Console.WriteLine("URL: https://www.youtube.com/watch?v=" + video.Snippet.ResourceId.VideoId);

                    using (StreamWriter outputFile = new StreamWriter(output_path + "list_url.txt", true))
                    {
                        outputFile.WriteLine("https://www.youtube.com/watch?v=" + video.Snippet.ResourceId.VideoId);
                    }
                    page_token = videos.NextPageToken;
                }
            }

            // 다운로드 시작
            text_value = File.ReadAllLines(output_path + "list_url.txt");
            Console.WriteLine("1: MP3 / 2: MP4");
            Console.Write("입력: ");
            download_type = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("다운받을 영상 " + text_value.Length + "EA");

            // 다운로드
            if (text_value.Length > 0)
            {
                for (int k = 0; k < text_value.Length; k++)
                {
                    var video = YouTube.Default.GetAllVideos(text_value[k]).First(v => v.Resolution == 720);

                    Console.WriteLine(video.FullName + " 다운로드 시작" + "[" + (k + 1) + "]/[" + text_value.Length + "]");
                    if (download_type == 1)
                    {
                        var videoInfos = await youtube.GetAllVideosAsync(text_value[k]);
                        var downloadInfo = videoInfos.Where(i => i.AudioFormat == AudioFormat.Aac && i.AudioBitrate == 128).FirstOrDefault();
                        if (downloadInfo != null)
                        {
                            File.WriteAllBytes(output_path + downloadInfo.FullName + ".mp3", downloadInfo.GetBytes());
                        }
                    }
                    else
                    {
                        var videoInfos = await youtube.GetAllVideosAsync(text_value[k]);
                        var downloadInfo = videoInfos.Where(i => i.Format == VideoFormat.Mp4 && i.Resolution == 720).FirstOrDefault();

                        if(downloadInfo != null)
                        {
                            File.WriteAllBytes(output_path + downloadInfo.FullName + ".mp4", downloadInfo.GetBytes());
                        }
                    }
                    Console.WriteLine("다운로드 완료" + "[" + (k + 1) + "]/[" + text_value.Length + "]");
                }
            }
            else
            {
                Console.WriteLine("다운받을 수 있는 영상이 없습니다.\n");
            }
        }
    }
}