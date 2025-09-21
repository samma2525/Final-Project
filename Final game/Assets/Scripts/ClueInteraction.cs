using UnityEngine;
using UnityEngine.EventSystems;

public class ClueInteraction : MonoBehaviour, IPointerClickHandler
{
    public GameObject pictureSprite;
    public AudioClip CollectionClip;
    public Player player;


    private bool isVisible = false;
    private bool hasGivenKey = false;
    public GameManager GameManager;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (pictureSprite != null)
            pictureSprite.SetActive(false);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        isVisible = !isVisible;
        if (pictureSprite != null)
            pictureSprite.SetActive(isVisible);
    }
    void Update()
    {
        if (isVisible && Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Check if the mouse is over this clue's sprite
            Collider2D hit = Physics2D.OverlapPoint(worldPoint);
            if (hit != null && hit.gameObject == gameObject)
            {
                // Clicked on the clue — do nothing, OnPointerClick already handled it
                return;
            }

            // Clicked outside — hide clue
            pictureSprite.SetActive(false);
            isVisible = false;

            if (!hasGivenKey)
            {
                GameManager.Instance.AddKey();
                audioSource.PlayOneShot(CollectionClip);
                hasGivenKey = true;
            }
        }
    }
}