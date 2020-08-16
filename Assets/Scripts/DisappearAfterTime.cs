using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearAfterTime : MonoBehaviour
{
    public float life = 5f;
    public GameObject gameController;
    public float fullLife;

    private void Start()
    {
        fullLife = life;
        gameController = GameObject.Find("GameController");
    }

    private void Update()
    {
        life -= Time.deltaTime;
        if (life <= 0f) Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if(gameObject.tag == "red")
        {
            gameController.GetComponent<GameManager>().hasRed = false;
        }
        else if(gameObject.tag == "last")
        {
            gameController.GetComponent<GameManager>().lastDisappeared = true;
        }
    }
}
