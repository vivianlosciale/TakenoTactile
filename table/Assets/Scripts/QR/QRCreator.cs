using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class QRCreator : MonoBehaviour
{
    public InputField addressInput;

    public GameObject players;
    public RawImage[] QRImages;
    public string address;

    private void Awake()
    {
        QRImages = players.GetComponentsInChildren<RawImage>();
        foreach (RawImage qr in QRImages)
        {
            qr.gameObject.SetActive(false);
        }
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

    public void DisplayQR()
    {

        for(int i = 0; i < 4; i++)
        {
            try
            {
                Texture2D myQr = generateQR(addressInput.text + "," + i);
                QRImages[i].texture = myQr;
                address = addressInput.text;
                QRImages[i].gameObject.SetActive(true);
            }
            catch (System.ArgumentException e)
            {
                Debug.LogWarning("Please specify an IP address [" + e.Message + "]");
            }
        }
    }
}
