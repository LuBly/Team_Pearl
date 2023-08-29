using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Generic Type으로 Singleton을 구현
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {   
        // 부모 오브젝트가 있는 경우 
        if(transform.parent != null && transform.root != null)
        {
            DontDestroyOnLoad(this.transform.root.gameObject);
        }
        // 본인 스스로가 최상위라면
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
