using System;
using CorgiExtension;
using UnityEngine;

public class LevelBrowserLevel : EditorLevel
{
    public static event Action<Steamworks.Ugc.Item> OnBrowserLevelDownload;
    public static event Action<string> OnBrowserLevelDownloadComplete;

    public void OnUploadButtonPressed()
    {
        Debug.Log("up pressed");
        UploadItem();
    }

    public void OnDownloadButtonPressed()
    {
        Debug.Log("down pressed");
        DownloadItem();
    }


    private async void UploadItem()
    {
        // Get filepath for file.

        var progress = new UploadProgress();
        var result = await Steamworks.Ugc.Editor.NewCommunityFile
            .WithTitle( "My New FIle" )
            .WithDescription( "This is a description" )
            .WithPublicVisibility()
            .WithContent( "/home/yanniboi/.config/unity3d/GameDevFieldGuide/Editarrr2023/levels/00002/" )
            .WithPreviewFile( "/home/yanniboi/.config/unity3d/GameDevFieldGuide/Editarrr2023/levels/00002/screenshot.png" )
            .WithTag( "awesome" )
            .WithTag( "small" )
            .WithTag( "level" )
            .SubmitAsync( progress );

        while (progress.Value < 1)
        {
            Debug.Log( $"Progress: {progress.Value}" );
        }
        Debug.Log( $"Result: {result.Result}" );
    }

    private async void DownloadItem()
    {
        var itemInfo = await Steamworks.Ugc.Item.GetAsync( 3038232767 );

        if (!itemInfo.HasValue)
        {
            Debug.Log("Item not found");
            return;
        }

        var item = itemInfo.Value;

        Debug.Log( $"Title: {item.Title}" );
        Debug.Log( $"IsInstalled: {item.IsInstalled}" );
        Debug.Log( $"IsDownloading: {item.IsDownloading}" );
        Debug.Log( $"IsDownloadPending: {item.IsDownloadPending}" );
        Debug.Log( $"IsSubscribed: {item.IsSubscribed}" );
        Debug.Log( $"NeedsUpdate: {item.NeedsUpdate}" );
        Debug.Log( $"Description: {item.Description}" );

        if (!(item.IsDownloading) && !(item.IsDownloadPending))
        {
            var downloadProgress = new Action<float>( ( progress ) =>
            {
                Debug.Log( $"Download Progress: {progress}" );

                if (progress >= 1f)
                {
                    OnBrowserLevelDownloadComplete?.Invoke( item.Id.ToString() ?? "" );
                }
            } );

            OnBrowserLevelDownload?.Invoke( item);

            bool isSubscribing = await item.Subscribe();
            bool isDownloading = await item.DownloadAsync(downloadProgress);
            Debug.Log("Subscribing" + isSubscribing);
            Debug.Log("Downloading" + isDownloading);
        }
    }

}

public class UploadProgress : IProgress<float>
{
    public float Value { get; private set; }

    public void Report(float value)
    {
        Value = value;

        Debug.Log( $"Download Progress: {value}" );

        if (value >= 1f)
        {
           //something.
        }
    }
}
