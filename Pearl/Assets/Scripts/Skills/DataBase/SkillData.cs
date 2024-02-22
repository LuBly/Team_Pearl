using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : MonoBehaviour
{
    public SkillSetting[] skill = new SkillSetting[3];
}

[System.Serializable]
public class SkillSetting
{
    public string skillName = "";
    public int skillIdx;
    public SkillType skillType;
    public Texture2D icon;
    public Sprite iconSprite;
    public int skillLevel = 1;
    public GameObject skillPrefabs;
    public GameObject skillUI;
    public AnimationClip skillAnimation;
    public float coolDown = 1000;
    public float activeTime = 100;
    public string description = "";
    public GameObject castEffect;
    public string sendMsg = "";//Send Message calling function when use this skill.
    public AudioClip soundEffect;
}

