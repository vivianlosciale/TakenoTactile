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
        StartCoroutine(DeactivateTile());
    }

    private IEnumerator DeactivateTile()
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

    private void OnDestroy()
    {
        meshRenderer.material.SetColor("_Color", Color.white);
    }
}
