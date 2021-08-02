using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ForceFollow : Skill
{


    public AudioSource forceFollowAudioSource;
    public ParticleSystem forceFollowParticleEffectA;
    public ParticleSystem forceFollowParticleEffectB;

    public void OnEnable()
    {
        //Init...
        resetSkill();

        SkillStartedEvent.AddListener(activateSkill);
        SkillStartedEvent.AddListener(runSkillEffect);
        SkillRefreshEvent.AddListener(resetSkill);
        SkillFinishedEvent.AddListener(reloadSkill);
        SkillFinishedEvent.AddListener(removeSkillEffect);

    }

    public void OnDisable()
    {

        SkillStartedEvent.RemoveListener(activateSkill);
        SkillStartedEvent.RemoveListener(runSkillEffect);
        SkillRefreshEvent.RemoveListener(resetSkill);
        SkillFinishedEvent.RemoveListener(reloadSkill);
        SkillFinishedEvent.RemoveListener(removeSkillEffect);

    }

    private float passedTime;
    private bool canLongPressActive = false;
    void Update()
    {
        if (WorkerManager.Instance.shepherdFlag && isAvailable)
        {

            if (Input.GetKeyDown(activationKey))
            {
                passedTime = Time.time;
                canLongPressActive = true;
            }

            if (canLongPressActive == true && Time.time - passedTime > castTime)
            {

                SkillStartedEvent.Invoke();
                canLongPressActive = false;
            }
            
            
            if (Input.GetKeyUp(activationKey))
            {
                if (canLongPressActive == true && Time.time - passedTime < castTime)
                {
                    canLongPressActive = false;
                    
                }
            }


        }
    }

    public void playWhistleAudioSource()
    {
        forceFollowAudioSource.gameObject.transform.position = WorkerManager.Instance.currentShepherd.transform.position;
        forceFollowAudioSource.volume = TableManager.Instance.audioSettingTable.environmentLevel;
        forceFollowAudioSource.Play();
    }

    public void playWhistleParticleEffect()
    {
        forceFollowParticleEffectA.gameObject.transform.position = new Vector3(WorkerManager.Instance.currentShepherd.transform.position.x, WorkerManager.Instance.currentShepherd.transform.position.y+1, WorkerManager.Instance.currentShepherd.transform.position.z);
        forceFollowParticleEffectA.Play();                                                                                                  
                                                                                                                                            
        forceFollowParticleEffectB.gameObject.transform.position = new Vector3(WorkerManager.Instance.currentShepherd.transform.position.x, WorkerManager.Instance.currentShepherd.transform.position.y + 1, WorkerManager.Instance.currentShepherd.transform.position.z);
        forceFollowParticleEffectB.Play();
    }

    public Coroutine runSkillCoroutine;
    public void runSkillEffect()
    {
        runSkillCoroutine = StartCoroutine(runSkillRoutine());
    }

    public IEnumerator runSkillRoutine()
    {
        playWhistleAudioSource();
        while (isActivateSkillRoutineRunning)
        {
            playWhistleParticleEffect();
            GrazingMode.Instance.updateHerdAnimalsOnPastureList();
            for (var i = 0; i < GrazingMode.Instance.herdAnimalsOnPasture.Count; i++)
            {
                PastureAnimal pastureAnimal = GrazingMode.Instance.herdAnimalsOnPasture[i];
                if (pastureAnimal.currentStates.getState() == StateList.shepherdFollowing)
                {
                    AnimalEmotionUI.Instance.fireHypnosEmotion(pastureAnimal.gameObject.transform.position);
                    continue;
                }

                if (Vector3.Distance(WorkerManager.Instance.currentShepherd.transform.position, pastureAnimal.transform.position) <= GrazingMode.Instance.whistleHeardDistance * 3)
                {
                    AnimalEmotionUI.Instance.fireHypnosEmotion(pastureAnimal.gameObject.transform.position);
                    pastureAnimal.isForcedFollow = true;
                }
            }

            yield return new WaitForSeconds(2.5f);
        }
    }



    public void removeSkillEffect()
    {
        for (var i = 0; i < GrazingMode.Instance.herdAnimalsOnPasture.Count; i++)
        {

            PastureAnimal pastureAnimal = GrazingMode.Instance.herdAnimalsOnPasture[i];
            pastureAnimal.isForcedFollow = false;

        }
    }
}
