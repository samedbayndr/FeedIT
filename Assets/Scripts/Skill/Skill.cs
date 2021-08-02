using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Skill : MonoBehaviour
{
    //cast edilme süresi
    public double castTime;
    [HideInInspector]
    public double currentCastTime;
    //Ne kadar süre aktif olacak.
    public double cooldown;
    [HideInInspector]
    public double currentCooldown;
    //Ne kadar süre sonra tekrar kullanılabilecek. 
    public double countdown;
    [HideInInspector]
    public double currentCountdown;

    public KeyCode activationKey;

    public UnityEngine.UI.Image skillImage;

    public bool isAvailable;

    public UnityEvent SkillFinishedEvent = new UnityEvent();
    public UnityEvent SkillStartedEvent = new UnityEvent();
    public UnityEvent SkillRefreshEvent = new UnityEvent();





    public void resetSkill()
    {
        //Garantiye almak için...
        isActivateSkillRoutineRunning = false;
        isReloadSkillRoutineRunning = false;

        currentCastTime = 0;
        currentCooldown = cooldown;
        currentCountdown = countdown;
        skillImage.fillAmount = 1;
        skillImage.color = new Color(skillImage.color.r, skillImage.color.g, skillImage.color.b, 1f);
        skillImage.enabled = false;
        isAvailable = true;
        this.gameObject.transform.SetAsFirstSibling();
    }

    #region Activate skill
    public Coroutine activateSkillCoroutine;
    public bool isActivateSkillRoutineRunning;
    public void activateSkill()
    {
        if (!isActivateSkillRoutineRunning)
        {
            skillImage.enabled = true;
            isAvailable = false;
            activateSkillCoroutine = StartCoroutine(activateSkillRoutine());
        }
    }
    IEnumerator activateSkillRoutine()
    {
        Debug.Log("activateSkillRoutine run");
        isActivateSkillRoutineRunning = true;
        while (currentCooldown >= 0)
        {
            currentCooldown -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        SkillFinishedEvent.Invoke();
        isActivateSkillRoutineRunning = false;
    }


    #endregion

    #region Reload skill
    public Coroutine reloadSkillCoroutine;
    public bool isReloadSkillRoutineRunning;
    public void reloadSkill()
    {
        if (!isReloadSkillRoutineRunning)
        {
            skillImage.color = new Color(skillImage.color.r, skillImage.color.g, skillImage.color.b, 0.5f);
            reloadSkillCoroutine = StartCoroutine(reloadSkillRoutine());
        }
    }
    IEnumerator reloadSkillRoutine()
    {

        Debug.Log("reloadSkillRoutine run");
        isReloadSkillRoutineRunning = true;
        while (currentCountdown >= 0)
        {
            skillImage.fillAmount = skillImage.fillAmount - (Time.deltaTime / (float)countdown);
            currentCountdown -= Time.deltaTime;
            Debug.Log(currentCountdown);
            yield return new WaitForEndOfFrame();
        }

        SkillRefreshEvent.Invoke();
        isReloadSkillRoutineRunning = false;

    }


    #endregion

}
