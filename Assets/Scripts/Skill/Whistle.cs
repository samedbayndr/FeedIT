using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whistle : Skill
{

    public AudioSource whistleAudioSource;
    public ParticleSystem whistleParticleEffectA;
    public ParticleSystem whistleParticleEffectB;

    public void OnEnable()
    {
        //Init...
        resetSkill();

        SkillStartedEvent.AddListener(activateSkill);
        SkillStartedEvent.AddListener(runSkillEffect);
        SkillRefreshEvent.AddListener(resetSkill);
        SkillFinishedEvent.AddListener(reloadSkill);

    }

    public void OnDisable()
    {

        SkillStartedEvent.RemoveListener(activateSkill);
        SkillStartedEvent.RemoveListener(runSkillEffect);
        SkillRefreshEvent.RemoveListener(resetSkill);
        SkillFinishedEvent.RemoveListener(reloadSkill);

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
        whistleAudioSource.gameObject.transform.position = WorkerManager.Instance.currentShepherd.transform.position;
        whistleAudioSource.volume = TableManager.Instance.audioSettingTable.environmentLevel;
        whistleAudioSource.Play();
    }

    public void playWhistleParticleEffect()
    {
        whistleParticleEffectA.gameObject.transform.position = new Vector3(WorkerManager.Instance.currentShepherd.transform.position.x, WorkerManager.Instance.currentShepherd.transform.position.y+1, WorkerManager.Instance.currentShepherd.transform.position.z);
        whistleParticleEffectA.Play();

        whistleParticleEffectB.gameObject.transform.position = new Vector3(WorkerManager.Instance.currentShepherd.transform.position.x, WorkerManager.Instance.currentShepherd.transform.position.y + 1, WorkerManager.Instance.currentShepherd.transform.position.z);
        whistleParticleEffectB.Play();
    }

    public Coroutine runSkillCoroutine;

    public float baseFollowProbability = 40f;
    public void runSkillEffect()
    {
        playWhistleAudioSource();
        playWhistleParticleEffect();

        GrazingMode.Instance.updateHerdAnimalsOnPastureList();
        for (var i = 0; i < GrazingMode.Instance.herdAnimalsOnPasture.Count; i++)
        {
            PastureAnimal pastureAnimal = GrazingMode.Instance.herdAnimalsOnPasture[i];
            if (pastureAnimal.currentStates.getState() != StateList.shepherdFollowing && Probability.Roll(0, 100) <= (baseFollowProbability * GrazingMode.Instance.forceFollowModifier))
            {
                AnimalEmotionUI.Instance.fireUnwillingEmotion(pastureAnimal.transform.position);
                continue;
            }

            if (Vector3.Distance(WorkerManager.Instance.currentShepherd.transform.position, pastureAnimal.transform.position) <= GrazingMode.Instance.whistleHeardDistance)
            {
                AnimalEmotionUI.Instance.fireHypnosEmotion(pastureAnimal.gameObject.transform.position);
                pastureAnimal.currentStates.setState(StateList.shepherdFollowing);
            }
        }
    }

}
