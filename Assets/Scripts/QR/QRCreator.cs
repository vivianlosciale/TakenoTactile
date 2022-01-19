using UnityEngine;
using ZXing;
using ZXing.QrCode;

public class QRCreator : MonoBehaviour
{

    public GameObject qrRenderer;
    public string address;

    private void Start()
    {
        qrRenderer.SetActive(false);
    }

    void OnGUI()
    {
        Texture2D myQr = generateQR(address);
        Rect rect = new Rect(0,0,myQr.width, myQr.height);
        rect.center = new Vector2(Screen.width / 2, Screen.height / 2);
        if (GUI.Button(rect, myQr, GUIStyle.none)) { }
    }

    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }

    public Texture2D generateQR(string text)
    {
        var encoded = new Texture2D(256, 256);
        var color32 = Encode(text, encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();
        return encoded;
    }
}
