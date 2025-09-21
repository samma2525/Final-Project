using UnityEngine;
using TMPro;

public class KeyUi : MonoBehaviour
{
    TextMeshProUGUI keyText;

    void Awake() => keyText = GetComponent<TextMeshProUGUI>();

    void OnEnable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnKeysChanged += OnKeysChanged;

        Refresh();
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnKeysChanged -= OnKeysChanged;
        }
    }

    void Start() => Refresh();

    void OnKeysChanged(int newValue) => keyText.text = newValue.ToString();

    void Refresh()
    {
        if (GameManager.Instance != null)
        {
            keyText.text = GameManager.Instance.keys.ToString();
        }
        else
        {
            keyText.text = "0";
        }
    }

}
