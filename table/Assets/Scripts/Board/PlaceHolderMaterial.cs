using UnityEngine;

class PlaceHolderMaterial : MonoBehaviour
{
    float alpha = 1.0f;
    bool down = true;
    private float speed = 0.005f;
    private void Update()
    {
        Color c = transform.GetComponent<MeshRenderer>().material.color;
        c.a = alpha;
        transform.GetComponent<MeshRenderer>().material.SetColor("_Color", c);
        if (down)
            alpha -= speed;
        else
            alpha += speed;
        if (alpha < 0.0f || alpha > 1.0f)
            down = !down;
    }
}
