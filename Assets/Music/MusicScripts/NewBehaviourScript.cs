using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    public MusicSelector musicSelector;

    public void  startComfortMusic()
    {
        musicSelector.MusicModeSelector(MusicSelector.MusicMode.Comfort);
    }

    public void  startStressMusic()
    {
        musicSelector.MusicModeSelector(MusicSelector.MusicMode.Stress);
    }

    public void  startBattleMusic()
    {
        musicSelector.MusicModeSelector(MusicSelector.MusicMode.Battle);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
