using UnityEngine;

class PlaceHolderMaterial : MonoBehaviour
{
    private float _alpha = 1.0f;
    private bool _down = true;
    private readonly float _speed = 0.01f;
    private void FixedUpdate()
    {
        Color c = transform.GetComponent<MeshRenderer>().material.color;
        c.a = _alpha;
        transform.GetComponent<MeshRenderer>().material.SetColor("_Color", c);
        if (_down)
            _alpha -= _speed;
        else
            _alpha += _speed;
        if (_alpha < 0.0f || _alpha > 1.0f)
            _down = !_down;
    }
}
