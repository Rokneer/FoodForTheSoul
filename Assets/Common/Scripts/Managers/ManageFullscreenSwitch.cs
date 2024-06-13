using UnityEngine;

public class ManageFullscreenSwitch : MonoBehaviour
{
    private int fullscreenWidth = 0;
    private int fullscreenHeight = 0;

    private bool fullscreen = false;

    private void Start()
    {
        fullscreen = Screen.fullScreen;
        SetFullScreenValues();
    }

    private void Update()
    {
        if (fullscreen != Screen.fullScreen)
        {
            if (Screen.fullScreen)
            {
                RestoreFullscreenResolution();
            }

            fullscreen = Screen.fullScreen;
        }
    }

    private void RestoreFullscreenResolution()
    {
        Screen.SetResolution(fullscreenWidth, fullscreenHeight, FullScreenMode.FullScreenWindow);
    }

    private void SetFullScreenValues()
    {
        // Set the screen width and height
        int systemWidth = Display.main.systemWidth;
        int systemHeight = Display.main.systemHeight;

        // Get a list of all supported resolutions
        Resolution[] supportedResolutions = Screen.resolutions;

        // Find the closest supported resolution to the native resolution
        Resolution closestResolution = supportedResolutions[0];
        int smallestGapInResolution = int.MaxValue;

        foreach (Resolution resolution in supportedResolutions)
        {
            int gap =
                Mathf.Abs(resolution.width - systemWidth)
                + Mathf.Abs(resolution.height - systemHeight);

            if (gap < smallestGapInResolution)
            {
                smallestGapInResolution = gap;
                closestResolution = resolution;
            }
        }

        fullscreenWidth = closestResolution.width;
        fullscreenHeight = closestResolution.height;
    }
}
