using UnityEngine;
using TMPro;

public class Keys : MonoBehaviour
{
    public AudioClip CollectionClip;
    public TextMeshProUGUI keyText;
    private AudioSource audioSource;

    void Start()
    {
        keyText = GameObject.FindWithTag("KeyText").GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.AddKey();
            audioSource.PlayOneShot(CollectionClip);
            keyText.text = GameManager.Instance.keys.ToString();

            Destroy(gameObject);
        }
    }
}
