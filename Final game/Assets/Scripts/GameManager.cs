using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int keys;
    public TextMeshProUGUI keyText;
    public float health = 100f;
    public Image healthImage;

    public event Action<int> OnKeysChanged;



    void Update()
    {
        if (healthImage != null)
            healthImage.fillAmount = health / 100f;
    }



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddKey()
    {
        keys++;
        OnKeysChanged?.Invoke(keys);
        if (keyText != null)
            keyText.text = keys.ToString();
        Debug.Log($"GameManager: keys = {keys}");
    }

    public void ForceNotifyUI()
    {
        OnKeysChanged?.Invoke(keys);
    }

    public void ResetKeys()
    {
        keys = 0;
        ForceNotifyUI();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var healthObj = GameObject.FindWithTag("HealthBar");
        if (healthObj != null)
        {
            healthImage = healthObj.GetComponent<Image>();
        }
        var keyObj = GameObject.FindWithTag("KeyText");
        if (keyObj != null)
        {
            keyText = keyObj.GetComponent<TextMeshProUGUI>();
            ForceNotifyUI();
        }
    }
}

