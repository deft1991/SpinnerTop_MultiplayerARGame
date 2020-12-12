using UnityEngine;

/**
 * Singleton object
 */
public class MusicPlayerScript : MonoBehaviour
{
    private AudioSource m_AudioSource;

    private void Awake()
    {
        float numMusicPlayers = FindObjectsOfType<MusicPlayerScript>().Length;
        Debug.Log("Music players count: " + numMusicPlayers);

        if (numMusicPlayers > 1) // if more than one MusicPlayerScript - destroy ourselves
        {
            Destroy(this);
        }
        else // do not destroy
        {
            DontDestroyOnLoad(gameObject);
            m_AudioSource = GetComponent<AudioSource>();
            // _audioSource.loop = true;
            PlayMusic();
        }
    }

    public void PlayMusic()
    {
        if (m_AudioSource.isPlaying) return;
        m_AudioSource.Play();
    }

    public void StopMusic()
    {
        m_AudioSource.Stop();
    }
}