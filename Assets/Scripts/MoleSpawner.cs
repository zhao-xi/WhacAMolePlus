using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoleSpawner : MonoBehaviour
{
    public GameObject molePrefab;
    private Text getReadyText;
    private float timeLeft;

    // indices the mole should turn red
    private HashSet<int> redMoleSet;

    public Material redMaterial;

    private Transform[] spawnPoints;

    [SerializeField] private float getReadyTime = 3f;
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private int redMoleNum = 10;
    [SerializeField] private int totalMoleNum = 100;

    private void Awake()
    {
        timeLeft = getReadyTime;
        getReadyText = GameObject.Find("getReadyText").GetComponent<Text>();
        spawnPoints = new Transform[GameObject.Find("Spawn Points").transform.childCount];
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i] = GameObject.Find("Spawn Points").transform.GetChild(i);
        }

        // random while well distributed red mole show up
        redMoleSet = new HashSet<int>();
        int index = (totalMoleNum / redMoleNum) / 2;
        for(int i = 0; i < redMoleNum; i++)
        {
            int randomIndex = index + Random.Range(-3, 3);
            Debug.Log(randomIndex);
            redMoleSet.Add(randomIndex);
            index += totalMoleNum / redMoleNum;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", getReadyTime, spawnInterval);
    }

    void Spawn()
    {
        if (totalMoleNum > 0)
        {
            int i = Random.Range(0, spawnPoints.Length);

            while (spawnPoints[i].childCount > 0)
            {
                // avoid choosing a spawn point that already has a mole
                i = Random.Range(0, spawnPoints.Length);
            }
            GameObject mole = Instantiate(molePrefab, spawnPoints[i]);

            totalMoleNum--;

            if (redMoleSet.Contains(totalMoleNum))
            {
                mole.GetComponent<Renderer>().material = redMaterial;
            }
        }
        else
        {
            CancelInvoke();
        }
    }

    // update getReady text
    private void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0) GameObject.Find("getReadyText").SetActive(false);
        else getReadyText.text = "Get Ready : " + ((int)timeLeft + 1).ToString();
    }
}
