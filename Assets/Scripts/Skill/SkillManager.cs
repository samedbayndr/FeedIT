using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private static SkillManager _instance;

    public static SkillManager Instance
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


    public ForceFollow forceFollow;
    public Whistle whistle;

    // Start is called before the first frame update
    void Start()
    {
            
    }

    public void makeSkillsUsable()
    {
        forceFollow.gameObject.SetActive(true);
        whistle.gameObject.SetActive(true);
        
    }

    public void makeSkillsUnusable()
    {
        forceFollow.gameObject.SetActive(false);
        whistle.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
