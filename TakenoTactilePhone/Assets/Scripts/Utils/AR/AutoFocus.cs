using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class AutoFocus : MonoBehaviour
{
    void Start()
    {
        VuforiaApplication.Instance.OnVuforiaStarted += OnVuforiaStarted;
        VuforiaApplication.Instance.OnVuforiaPaused += OnPaused;
    }

    private void OnVuforiaStarted()
    {
        VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(
            FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        VuforiaBehaviour.Instance.CameraDevice.SetCameraMode(Vuforia.CameraMode.MODE_DEFAULT);
    }

    private void OnPaused(bool paused)
    {
        if (!paused) // Resumed
        {
            // Set again autofocus mode when app is resumed
            VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(
                FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        }
    }
}
