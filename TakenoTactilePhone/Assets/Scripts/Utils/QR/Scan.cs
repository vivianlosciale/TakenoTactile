using System;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

public class Scan : MonoBehaviour
{

    public RawImage display;
    public Text text;
    public MobileClient client;

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
                client.Connect(result.Text);
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
    
}