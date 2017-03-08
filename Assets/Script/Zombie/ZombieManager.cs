using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieManager : MonoBehaviour {

    public float minGenerateDelayTime, maxGenerateDelayTime;

    public Transform[] generatePos;
    public GameObject[] prefabObjects;

    public bool isGenerate;

    public int maxZombieCount;

    int zombieCount;

    List<GameObject> activeZombieList;
    List<GameObject> followZombieList;

    public Transform[] followPosArr;

    Bus player;

    void Awake()
    {
        Screen.SetResolution(720, 1280, true);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Bus>();
        activeZombieList = new List<GameObject>();
        followZombieList = new List<GameObject>();
        StartCoroutine(GenerateZombie());
    }

    void Update()
    {
        if (zombieCount>=maxZombieCount)
        {
            isGenerate = false;
        } 
        else
        {
            isGenerate = true;
        }
        //ColorProcess();
    }
    
    public List<GameObject> GetFollowList()
    {
        return followZombieList;
    }

    void ColorProcess()
    {
        if (followZombieList.Count <= 0)
            return;

        switch (player.state)
        {
            case RoadManager.PropertyType.FIRE:
                foreach (GameObject obj in followZombieList)
                {
                    Zombie zombie = obj.GetComponent<Zombie>();
                    if (zombie.currentColor==Zombie.ZombieColor.Blue||
                        zombie.currentColor == Zombie.ZombieColor.Green)
                    {
                        int order = zombie.GetFollowOrder();
                        zombie.SetFollowOrder(order++);
                    }
                    else if(zombie.currentColor == Zombie.ZombieColor.Red)
                    {
                        int order = zombie.GetFollowOrder();
                        zombie.SetFollowOrder(order--);
                    }
                }
                break;
            case RoadManager.PropertyType.ICE:
                foreach (GameObject obj in followZombieList)
                {
                    Zombie zombie = obj.GetComponent<Zombie>();
                    if (zombie.currentColor == Zombie.ZombieColor.Red ||
                        zombie.currentColor == Zombie.ZombieColor.Blue)
                    {
                        int order = zombie.GetFollowOrder();
                        zombie.SetFollowOrder(order++);
                    }
                    else if (zombie.currentColor == Zombie.ZombieColor.SkyBlue)
                    {
                        int order = zombie.GetFollowOrder();
                        zombie.SetFollowOrder(order--);
                    }
                }
                break;
            case RoadManager.PropertyType.JUNGLE:
                foreach (GameObject obj in followZombieList)
                {
                    Zombie zombie = obj.GetComponent<Zombie>();
                    if (zombie.currentColor == Zombie.ZombieColor.SkyBlue ||
                        zombie.currentColor == Zombie.ZombieColor.Yellow)
                    {
                        int order = zombie.GetFollowOrder();
                        zombie.SetFollowOrder(order++);
                    }
                    else if (zombie.currentColor == Zombie.ZombieColor.Green)
                    {
                        int order = zombie.GetFollowOrder();
                        zombie.SetFollowOrder(order--);
                    }
                }
                break;
            case RoadManager.PropertyType.RADIATION:
                foreach (GameObject obj in followZombieList)
                {
                    Zombie zombie = obj.GetComponent<Zombie>();
                    if (zombie.currentColor == Zombie.ZombieColor.SkyBlue ||
                        zombie.currentColor == Zombie.ZombieColor.Green)
                    {
                        int order = zombie.GetFollowOrder();
                        zombie.SetFollowOrder(order++);
                    }
                    else if (zombie.currentColor == Zombie.ZombieColor.Yellow)
                    {
                        int order = zombie.GetFollowOrder();
                        zombie.SetFollowOrder(order--);
                    }
                }
                break;
            case RoadManager.PropertyType.WATER:
                foreach (GameObject obj in followZombieList)
                {
                    Zombie zombie = obj.GetComponent<Zombie>();
                    if (zombie.currentColor == Zombie.ZombieColor.Red ||
                        zombie.currentColor == Zombie.ZombieColor.Yellow)
                    {
                        int order = zombie.GetFollowOrder();
                        zombie.SetFollowOrder(order++);
                    }
                    else if (zombie.currentColor == Zombie.ZombieColor.Blue)
                    {
                        int order = zombie.GetFollowOrder();
                        zombie.SetFollowOrder(order--);
                    }
                }
                break;
        }
    }

    public void AddFollowList(GameObject obj)
    {
        followZombieList.Add(obj);
    }

    IEnumerator GenerateZombie()
    {
        while (isGenerate)
        {
            float delayTime = Random.Range(minGenerateDelayTime, maxGenerateDelayTime);

            yield return new WaitForSeconds(delayTime);

            int randPos = Random.Range(0, 5);
            int randIndex = Random.Range(0, 5);

            GameObject obj = Instantiate(prefabObjects[randIndex], generatePos[randPos].position, Quaternion.identity) as GameObject;

            if (obj != null)
                activeZombieList.Add(obj);

            zombieCount++;

        }
    }

}
