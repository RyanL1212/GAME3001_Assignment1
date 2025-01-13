using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioManager audioManager;

    void Start()
    {
        audioManager = GetComponent<AudioManager>();
        DontDestroyOnLoad(audioManager);
    }
}
