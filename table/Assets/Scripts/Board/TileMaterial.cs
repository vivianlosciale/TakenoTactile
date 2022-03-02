using System.Collections;
using UnityEngine;

public class TileMaterial : MonoBehaviour
{
    private readonly float _speed = 0.04f;
    private float _value;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = transform.GetComponent<MeshRenderer>();
        _value = meshRenderer.material.color.r;
    }

    public void DeactivateTile()
    {

        StartCoroutine(DeactivateTileAnimation());
    }

    public void TwinkleTile()
    {

        StartCoroutine(TwinkleTileAnimation());
    }

    private IEnumerator DeactivateTileAnimation()
    {
        Color c = meshRenderer.material.color;

        while (c.r > 70.0 / 255)
        {
            _value -= _speed;
            c.r = _value;
            c.g = _value;
            c.b = _value;
            meshRenderer.materials[1].SetColor("_Color", c);
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator TwinkleTileAnimation()
    {
        Color c = meshRenderer.material.color;
        bool down = true;
        while (true)
        {
            if(down)
                _value -= _speed;
            else
                _value += _speed;

            c.r = _value;
            c.g = 0;
            c.b = 0;
            if (c.r < 0.0f || c.r > 1.0f)
                down = !down;
            meshRenderer.materials[1].SetColor("_Color", c);
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void OnDestroy()
    {
        meshRenderer.materials[1].SetColor("_Color", Color.white);
    }
}
