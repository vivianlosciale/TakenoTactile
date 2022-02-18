using UnityEngine;

public class ActionMaterial : MonoBehaviour
{
    private float _metallic = 1.0f;
    private bool _down = true;
    private readonly float _speed = 0.01f;
    private MeshRenderer meshRenderer;
    private bool used = false;
    public Texture2D image;
    

    private void Start()
    {
        meshRenderer = transform.GetComponent<MeshRenderer>();
    }
    private void FixedUpdate()
    {
        if (!used)
        {
            meshRenderer.material.SetFloat("_Metallic", _metallic);
            if (_down)
                _metallic -= _speed;
            else
                _metallic += _speed;
            if (_metallic < 0.0f || _metallic > 1.0f)
                _down = !_down;
        }
        else
        {
            meshRenderer.material.SetColor("_EmissionColor",new Color(0.4f,0.4f, 0.4f));
        }
        
    }

    private void OnDisable()
    {
        used = false;
        meshRenderer.material.SetColor("_EmissionColor", Color.white);
        _metallic = 1.0f;
        _down = true;
    }

    private void OnEnable()
    {
        meshRenderer.material.mainTexture = image;
        meshRenderer.material.SetTexture("_EmissionMap", image);
    }
}
