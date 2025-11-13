using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Lava : MonoBehaviour
{
    [SerializeField] float scrollSpeedX = 0.04f;
    [SerializeField] float scrollSpeedY;
    [SerializeField] bool scrollTexture = true;
    //[SerializeField] AudioSource audioSource;
    //[SerializeField] AudioClip lavaClip;
    [SerializeField] ParticleSystem PS;

    Renderer rend;
    Vector2 offset;
    ParticleSystem.ShapeModule shape;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rend = GetComponent<Renderer>();
        offset = rend.material.mainTextureOffset;

        shape = PS.shape;

        shape.scale = new Vector3(transform.localScale.x - 0.5f, transform.localScale.z - 0.5f, transform.localScale.y);

        //audioSource.clip = lavaClip;
        //audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (scrollTexture)
        {
            offset.x += scrollSpeedX * Time.deltaTime;
            offset.y += scrollSpeedY * Time.deltaTime;
            rend.material.mainTextureOffset = offset;
        }
    }
}
