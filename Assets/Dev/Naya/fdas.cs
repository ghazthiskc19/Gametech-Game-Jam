using Ami.BroAudio;
using UnityEngine;

public class fdas : MonoBehaviour
{
    public SoundID sda;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    [ContextMenu("Play")]
    public void PlayAudio()
    {
        sda.Play().AsBGM();
    }
}
