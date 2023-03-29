# uTube downloader

```
* u튜브 공개, 일부공개 플레이리스트 항목을 다운로드 받을 수 있다.
```

<br>

# 프로젝트

## 개발 목적
```
* 영상 다운로드가 불편하고, 시중에 나와있는 프로그램들은 광고가 너무 많아서 제작함
* 닷넷과 api를 학습
```

## 변경 사항

```
* .NET 6.0으로 변경
* 워닝문제 해결

** 23.03.29
* M1 Mac Ventura 실행 확인
```

## 사용 방법
```
* Google Console Youtube API 발급 키를 생성해서 Program.cs에 키를 할당해줘야함
* exe 파일을 생성해서 사용
```

## `CS8600`

* Severity Code Description Project File Line Suppression State
* Warning CS8600 Converting null literal or possible null value to non-nullable type.

```c#
변경전
string playListID, outputPath;
변경후
string ? playListID, outputPath;
```

?. 연산자는 [ Null이 아니라면 참조하고, Null이라면 Null로 처리 ]하라는 뜻이다.

(참고) https://developstudy.tistory.com/69

## `CS8602`

* Severity Code Description Project File Line Suppression State
* Warning CS8602 Dereference of a possibly null reference.

```c#
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


## 사용한 Nuget Package

```
* Google.Apis.YouTube.v3
* VideoLibrary
```

## Google Console Youtube API 발급
* [Google Console Cloud](https://console.cloud.google.com/)

## 참고한 사이트

* https://developers.google.com/youtube/v3/code_samples/dotnet?hl=ko#search_by_keyword
* https://www.csharpstudy.com/Practical/Prac-youtube-api.aspx
* https://imyeonn.github.io/blog/web/39/
* https://csharp.hotexamples.com/examples/-/YoutubeService/-/php-youtubeservice-class-examples.html
* https://stackoverflow.com/questions/34143202/get-all-videos-from-channel-youtube-api-v3-c-sharp
* https://www.niteshluharuka.com/list-videos-from-a-channel-using-youtube-api-v3-in-c/

---

## 라이센스

##### _GNU General Public License v3.0_

##### _Copyright 2021-2022. chm209 all right reserved_
