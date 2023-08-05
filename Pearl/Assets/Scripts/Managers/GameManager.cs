using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //바로 메모리에 올려버린다.
    public static GameManager instance;
    public PoolManager pool;
    public Spawner spawner;
    public Player player;
    public int stageIdx;

    void Awake()
    {
        instance = this;    
    }
}
