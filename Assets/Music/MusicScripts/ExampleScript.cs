using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleScript: MonoBehaviour
{
    public MusicSelector musicSelector;
    // Start is called before the first frame update
    void Start()
    {
        musicSelector.MusicModeSelector(MusicSelector.MusicMode.Stress);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
