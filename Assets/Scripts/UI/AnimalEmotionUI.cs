using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalEmotionUI : MonoBehaviour
{
    private static AnimalEmotionUI _instance;

    public static AnimalEmotionUI Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null || _instance != this)
        {
            Destroy(_instance);
        }

        _instance = this;
    }

    public GameObject pastureAnimalEmotionParent;
    public GameObject unwillingEmotionPrefab;
    public GameObject loveEmotionPrefab;
    public GameObject hypnoticEmotionPrefab;

    public void fireUnwillingEmotion(Vector3 animalPosition)
    {
        GameObject unwillingEmotionGO = Instantiate(unwillingEmotionPrefab, pastureAnimalEmotionParent.transform);
        unwillingEmotionGO.transform.position = animalPosition;
        Animator emoAnimController = unwillingEmotionGO.transform.GetChild(0).GetComponent<Animator>();
        emoAnimController.enabled = true;
        StartCoroutine(emotionDestructRoutine(unwillingEmotionGO, emoAnimController.GetCurrentAnimatorStateInfo(0).length));
    }

    public void fireLoveEmotion(Vector3 animalPosition)
    {
        GameObject loveEmotionGO = Instantiate(loveEmotionPrefab, pastureAnimalEmotionParent.transform);
        loveEmotionGO.transform.position = animalPosition;
        Animator emoAnimController = loveEmotionGO.transform.GetChild(0).GetComponent<Animator>();
        emoAnimController.enabled = true;
        StartCoroutine(emotionDestructRoutine(loveEmotionGO, emoAnimController.GetCurrentAnimatorStateInfo(0).length));
    }

    public void fireHypnosEmotion(Vector3 animalPosition)
    {
        GameObject hypnosEmotionGO = Instantiate(hypnoticEmotionPrefab, pastureAnimalEmotionParent.transform);
        hypnosEmotionGO.transform.position = animalPosition;
        Animator emoAnimController = hypnosEmotionGO.transform.GetChild(0).GetComponent<Animator>();
        emoAnimController.enabled = true;
        StartCoroutine(emotionDestructRoutine(hypnosEmotionGO, emoAnimController.GetCurrentAnimatorStateInfo(0).length));
    }


    IEnumerator emotionDestructRoutine(GameObject destroyableEmotion, float animTime)
    {
        yield return new WaitForSeconds(animTime);
        Destroy(destroyableEmotion);
    }
}
