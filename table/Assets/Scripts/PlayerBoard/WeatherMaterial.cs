using UnityEngine;

public class WeatherMaterial : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private Texture2D _image;
    

    private void Start()
    {
        _meshRenderer = transform.GetComponent<MeshRenderer>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _meshRenderer.material.mainTexture = _image;
        _meshRenderer.material.SetTexture("_EmissionMap", _image);
    }

    public void showWeatherImage(string image)
    {
        _image = Resources.Load<Texture2D>("Images/board/weather_" + image);
        gameObject.SetActive(true);
    }
    public void removeWeatherImage()
    {
        gameObject.SetActive(false);
    }
}
