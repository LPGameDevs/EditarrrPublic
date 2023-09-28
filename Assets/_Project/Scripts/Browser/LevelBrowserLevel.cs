using System;
using CorgiExtension;
using Level.Storage;
using UnityEngine;

public class LevelBrowserLevel : EditorLevel
{
    public static event Action<string> OnBrowserLevelDownload;

    public void OnUploadButtonPressed()
    {
        Debug.Log("up pressed");
        UploadItem();
    }

    public void OnDownloadButtonPressed()
    {
        string code = Title.text;
        OnBrowserLevelDownload?.Invoke(code);
    }


    private async void UploadItem()
    {
        // Get filepath for file.

        var progress = new UploadProgress();
        var result = await Steamworks.Ugc.Editor.NewCommunityFile
            .WithTitle("My New FIle")
            .WithDescription("This is a description")
            .WithPublicVisibility()
            .WithContent("/home/yanniboi/.config/unity3d/GameDevFieldGuide/Editarrr2023/levels/00002/")
            .WithPreviewFile(
                "/home/yanniboi/.config/unity3d/GameDevFieldGuide/Editarrr2023/levels/00002/screenshot.png")
            .WithTag("awesome")
            .WithTag("small")
            .WithTag("level")
            .SubmitAsync(progress);

        while (progress.Value < 1)
        {
            Debug.Log($"Progress: {progress.Value}");
        }

        Debug.Log($"Result: {result.Result}");
    }

    private async void DownloadItem()
    {


    }
}

public class UploadProgress : IProgress<float>
{
    public float Value { get; private set; }

    public void Report(float value)
    {
        Value = value;

        Debug.Log($"Download Progress: {value}");

        if (value >= 1f)
        {
            //something.
        }
    }
}
