using UnityEngine;

/// <summary>
/// GameObject is destroyed, remove it.
/// </summary>
public class DestroyAfter : MonoBehaviour
{
    private ParticleSystem ps;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ps.Play();
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(!ps.isPlaying && !audioSource.isPlaying) {
            Destroy(gameObject);
        }
    }
}
