using UnityEngine;

public class ActionMaterial : MonoBehaviour
{
    private float _metallic = 1.0f;
    private bool _down = true;
    private readonly float _speed = 0.01f;
    private MeshRenderer _meshRenderer;
    public bool used = false;
    private Texture2D _image;
    

    private void Start()
    {
        _meshRenderer = transform.GetComponent<MeshRenderer>();
        gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (!used)
        {
            _meshRenderer.material.SetFloat("_Metallic", _metallic);
            if (_down)
                _metallic -= _speed;
            else
                _metallic += _speed;
            if (_metallic < -0.5f || _metallic > 1.0f)
                _down = !_down;
        }
        else
        {
            _meshRenderer.material.SetColor("_EmissionColor",new Color(0.4f,0.4f, 0.4f));
        }
        
    }

    private void OnDisable()
    {
        used = false;
        _meshRenderer.material.SetColor("_EmissionColor", Color.white);
        _metallic = 1.0f;
        _down = true;
    }

    private void OnEnable()
    {
        _meshRenderer.material.mainTexture = _image;
        _meshRenderer.material.SetTexture("_EmissionMap", _image);
    }

    public void AddIcon(string image)
    {
        _image = Resources.Load<Texture2D>("Images/board/board_" + image);
        gameObject.SetActive(true);
    }
    public void RemoveIcon()
    {
        gameObject.SetActive(false);
    }
}
