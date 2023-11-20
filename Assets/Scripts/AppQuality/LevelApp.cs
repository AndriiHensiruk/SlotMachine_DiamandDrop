using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ICouldGames
{
    public class LevelApp : MonoBehaviour
    {
         [SerializeField]
    GameObject slotPanel, playButton, levelLogo;


    void Start()
    {
        LeanTween.scale(levelLogo, new Vector3(100f, 100f, 100f), 2f).setDelay(.5f).setEase(LeanTweenType.easeOutElastic).setOnComplete(LevelComplete);
        LeanTween.moveLocal(levelLogo, new Vector3(0f, 0f, 0f), 0.7f).setDelay(2f).setEase(LeanTweenType.easeInOutCubic);
    }

    void LevelComplete()
    {

        LeanTween.moveLocal(slotPanel, new Vector3(0f, -3f, 0f), 0.7f).setDelay(.5f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(playButton, new Vector3(1f, 1f, 1f), 2f).setDelay(.8f).setEase(LeanTweenType.easeOutElastic);
        
        //Blank for header animation
        //LeanTween.alpha(score.GetComponent<RectTransform>(), 1f, .5f).setDelay(1f);
        //LeanTween.alpha(coins.GetComponent<RectTransform>(), 1f, .5f).setDelay(1.1f);
        //LeanTween.alpha(gems.GetComponent<RectTransform>(), 1f, .5f).setDelay(1.2f);
    }

   
    }
}
