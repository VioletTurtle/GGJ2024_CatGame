using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DynamicMusic : MonoBehaviour
{
    private PlayerController player;
    public ManyLoopPlayer musicPlayer;
    public ManyLoopedBGM layeredTrack;

    private void Awake()
    {
        musicPlayer = GetComponentInChildren<ManyLoopPlayer>();
        layeredTrack = GetComponentInChildren<ManyLoopedBGM>();
        player = GetComponentInParent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        musicPlayer.setSongId(0);
        InitializeVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private async void InitializeVolume()
    {
        //float targetVolume;
        layeredTrack.SetVolume(0);
        await Task.Delay(500);
        //DOTween.To()
        layeredTrack.SetVolume(1);
    }
}
