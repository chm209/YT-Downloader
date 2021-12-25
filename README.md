## UTube downloader
공개, 일부공개 플레이 리스트 항목을 다운로드 할 수 있습니다.

``` Youtube api를 공부하는겸 만들어봤습니다. ```

---

##### 2021.12.25 수정사항
1. .NET 6.0으로 변경
2. 모든 워닝 해결 (CS8600, CS8602)

``` CS8600 ```

Severity	Code	Description	Project	File	Line	Suppression State

Warning	CS8600	Converting null literal or possible null value to non-nullable type.

``` c#
변경전
string playListID, outputPath;
변경후
string ? playListID, outputPath;
```

?. 연산자는 [ Null이 아니라면 참조하고, Null이라면 Null로 처리 ]하라는 뜻이다.

(참고) https://developstudy.tistory.com/69

``` CS8602 ```

Severity	Code	Description	Project	File	Line	Suppression State

Warning	CS8602	Dereference of a possibly null reference.


``` c#
변경전
if (downloadType == 1)
{
    var videoInfos = await youtube.GetAllVideosAsync(textValue[k]);
    var downloadInfo = videoInfos.Where(i => i.AudioFormat == AudioFormat.Aac && i.AudioBitrate == 128).FirstOrDefault();
    File.WriteAllBytes(outputPath + downloadInfo.FullName + ".mp3", downloadInfo.GetBytes());
}

변경후
if (downloadType == 1)
{
    var videoInfos = await youtube.GetAllVideosAsync(textValue[k]);
    var downloadInfo = videoInfos.Where(i => i.AudioFormat == AudioFormat.Aac && i.AudioBitrate == 128).FirstOrDefault();
    if (downloadInfo != null)
    {
        File.WriteAllBytes(outputPath + downloadInfo.FullName + ".mp3", downloadInfo.GetBytes());
    }
}
```

(참고) https://docs.microsoft.com/ko-kr/dotnet/csharp/language-reference/operators/null-forgiving

---

#### 사용한 Nuget Package
1. Google.Apis.YouTube.v3
2. VideoLibrary

#### Google Console Youtube API 발급
1. https://console.cloud.google.com/

#### 참고한 사이트
1. https://developers.google.com/youtube/v3/code_samples/dotnet?hl=ko#search_by_keyword
2. https://www.csharpstudy.com/Practical/Prac-youtube-api.aspx
3. https://imyeonn.github.io/blog/web/39/
4. https://csharp.hotexamples.com/examples/-/YoutubeService/-/php-youtubeservice-class-examples.html
5. https://stackoverflow.com/questions/34143202/get-all-videos-from-channel-youtube-api-v3-c-sharp
6. https://www.niteshluharuka.com/list-videos-from-a-channel-using-youtube-api-v3-in-c/

---
## 라이센스

***GNU General Public License v3.0***

***Copyright 2021. chm209 all right reserved***