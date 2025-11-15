using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class TextureScroll : MonoBehaviour
{
    [SerializeField] float scrollSpeedX = 0.04f;
    [SerializeField] float scrollSpeedY;

    Renderer rend;
    Vector2 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rend = GetComponent<Renderer>();
        offset = rend.material.mainTextureOffset;
    }

    // Update is called once per frame
    void Update()
    {
        offset.x += scrollSpeedX * Time.deltaTime;
        offset.y += scrollSpeedY * Time.deltaTime;
        rend.material.mainTextureOffset = offset;
    }
}
