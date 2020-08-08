using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearAfterTime : MonoBehaviour
{
    [SerializeField] private float life = 2f;
    // Start is called before the first frame update
    void Awake()
    {
        Destroy(gameObject, life);
    }
}
