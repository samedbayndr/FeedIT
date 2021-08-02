using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageSentenceThrower : MonoBehaviour
{
    private static MessageSentenceThrower _instance;

    public static MessageSentenceThrower Instance
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


    public GameObject throwCenter;
    public GameObject sentencePrefab;

    void Start()
    {
    }


    public void fireMessage(string pMessage)
    {
        throwSentence(pMessage);
    }

    public void throwSentence(string sentence)
    {
        string nextSentence = sentence;
        GameObject newSentenceGO = Instantiate(sentencePrefab, throwCenter.transform);
        TextMeshProUGUI newSentenceText = newSentenceGO.GetComponent<TextMeshProUGUI>();
        Animator newSentenceAnimController = newSentenceGO.GetComponent<Animator>();

        newSentenceText.SetText(nextSentence);
        newSentenceGO.SetActive(true);
        StartCoroutine(deleteThrowGO(newSentenceGO, newSentenceAnimController.GetCurrentAnimatorStateInfo(0).length));
    }

    public IEnumerator deleteThrowGO(GameObject throwSentence, float animTime)
    {
        yield return new WaitForSeconds(animTime);
        Destroy(throwSentence);
    }
}
