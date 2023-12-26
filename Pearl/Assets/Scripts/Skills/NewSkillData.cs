using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSkillData : MonoBehaviour
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
    public GameObject Joystick;
    public AnimationClip skillAnimation;
    public float castTime = 0.5f;
    public float skillDelay = 0.3f;
    public int coolDown = 1;
    public string description = "";
    public GameObject castEffect;
    public string sendMsg = "";//Send Message calling function when use this skill.
    public AudioClip soundEffect;
}

