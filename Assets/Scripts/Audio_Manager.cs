using UnityEngine;
public class Audio_Manager : MonoBehaviour
{
    int Playing_BGM_Index = -1;
    private AudioSource Source;
    private void Awake()
    {
        var obj = FindObjectsOfType<Audio_Manager>();
        Source = GameObject.Find("BGM1").GetComponent<AudioSource>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Check_Music();
    }

    public void Check_Music()
    {

                Source = GameObject.Find("BGM1").GetComponent<AudioSource>();

            Source.Play();
        }
    
}

