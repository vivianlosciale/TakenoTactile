using System;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

public class Scan : MonoBehaviour
{
    
    public RawImage display;
    public Text text;
    
    private int _currentCamIndex;
    private WebCamTexture _camTexture;

    private void Start()
    {
        _currentCamIndex = 0;

        WebCamDevice device = WebCamTexture.devices[_currentCamIndex];
        _camTexture = new WebCamTexture(device.name);
        display.texture = _camTexture;
        
        _camTexture.Play();
        
        text.color = Color.white;
    }

    private void Update()
    {
        if (_camTexture.videoRotationAngle == 90 || _camTexture.videoRotationAngle == 270)
        {
            float antiRotate = -(180 - _camTexture.videoRotationAngle);
            Quaternion quatRot = new Quaternion();
            quatRot.eulerAngles = new Vector3(0, 0, antiRotate);
            display.transform.rotation = quatRot;
        }
        else
        {
            float antiRotate = -(360 - _camTexture.videoRotationAngle);
            Quaternion quatRot = new Quaternion();
            quatRot.eulerAngles = new Vector3(0, 0, antiRotate);
            display.transform.rotation = quatRot;
        }

        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            // decode the current frame
            var result = barcodeReader.Decode(_camTexture.GetPixels32(),
                _camTexture.width, _camTexture.height);
            if (result != null)
            {
                Debug.Log("DECODED TEXT FROM QR: " + result.Text);
                text.text = result.Text;
                //client.Connect(result.Text);
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex.Message);
        }
    }

    private void OnDisable()
    {
        _camTexture.Stop();
    }


    /*    public Text text;

        private Image myGUITexture;
        private Boolean camAvailable;
        private WebCamTexture camTexture;
        public Quaternion baseRotation;
        private Rect screenRect;

        private Texture defaultBackground;
        public RawImage background;
        public AspectRatioFitter fit;

        int currentCamIndex = 0;*/




    /*    private void Start()
        {
            if (WebCamTexture.devices.Length > 0)
            {
                currentCamIndex += 1;
                currentCamIndex %= WebCamTexture.devices.Length;
            }

            WebCamDevice device = WebCamTexture.devices[currentCamIndex];
            camTexture = new WebCamTexture(device.name);
            background.texture = camTexture;

            camTexture.Play();
        }*/
}

    // Start is called before the first frame update
    /*void Start()
    {
        *//*defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        screenRect = new Rect(0, 0, Screen.width, Screen.height);

        for (int i=0; i<devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                camTexture = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }*//*

        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        camTexture = new WebCamTexture();
        camTexture.requestedHeight = Screen.height;
        camTexture.requestedWidth = Screen.width;

        if (camTexture != null)
        {
            camTexture.Play();
            background.texture = camTexture;

            camAvailable = true;
        }
        
    }

    private void Update()
    {
        if (camAvailable)
        {
            float ratio = (float)camTexture.width / (float)camTexture.height;
            fit.aspectRatio = ratio;

            float scaleY = camTexture.videoVerticallyMirrored ? -1f : 1f;
            background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

            int orient = -camTexture.videoRotationAngle;
            background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
        }
    }

    void OnGUI()
    {
        // drawing the camera on screen

            GUI.DrawTexture(screenRect, camTexture, ScaleMode.ScaleToFit);


        // do the reading � you might want to attempt to read less often than you draw on the screen for performance sake
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            // decode the current frame
            var result = barcodeReader.Decode(camTexture.GetPixels32(),
              camTexture.width, camTexture.height);
            if (result != null)
            {
                Debug.Log("DECODED TEXT FROM QR:" + result.Text);
                text.text = result.Text;
            }
        }
        catch (Exception ex) { Debug.LogWarning(ex.Message); }
    }
}
*/