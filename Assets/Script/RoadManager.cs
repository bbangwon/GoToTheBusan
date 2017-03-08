using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadManager : MonoBehaviour {

    //Road생성용 프리팹 등록
    public GameObject ITypeRoad;
    public GameObject UTypeRoad;

    //Road속성용 스프라이트 등록
    public Sprite IIceSprite;
    public Sprite IWaterSprite;
    public Sprite IFireSprite;
    public Sprite IRadiationSprite;
    public Sprite IJungleSprite;

    public Sprite UIceSprite;
    public Sprite UWaterSprite;
    public Sprite UFireSprite;
    public Sprite URadiationSprite;
    public Sprite UJungleSprite;

    public GameObject ChangePrefab;

    // Road 이동 속도
    public float roadMoveSpeed = 2.0f;

    public float UroadCameraMove = 2.65f;

    //이동한 거리 측정
    private float moveDistanceKM = 0;

    //길 타입
    public enum RoadType
    {
        ITYPE,
        UTYPE
    }

    //길의 속성
    public enum PropertyType
    {
        ICE,
        WATER,
        FIRE,
        RADIATION,
        JUNGLE
    }

    public enum GameState
    {
        PLAY,
        GAMEOVER
    }


    public Road currentRoad;
    public List<Road> lastRoads = new List<Road>();
    public List<Road> allCreatedRoads = new List<Road>();
    public GameState state;
    GameUIManager gameUImanager;

    // Use this for initialization
    void Start () {

        gameUImanager = GameObject.Find("DistanceBackground").GetComponent<GameUIManager>();
        //시작하면 첫 길은 랜덤타입으로 시작

        PropertyType randomPT = (PropertyType)System.Enum.ToObject(typeof(PropertyType), Random.Range(0, 5));
        currentRoad.propertyType = randomPT;
        allCreatedRoads.Add(currentRoad);

        //시작하면 최초 같은 IType 1개를 더 생성한다.
        RoadCreateFromI(currentRoad, RoadType.UTYPE, currentRoad.propertyType);

        GameObject.Find("bus").GetComponent<Bus>().Init(randomPT);
        StartCoroutine(AddDistance());
        StartCoroutine(AddSpeed());

        state = GameState.PLAY;

    }
	
	// Update is called once per frame
	void Update () {
        //
        if (state == GameState.PLAY)
        {
            AutoCreateRoad();
            RoadDestroy();
            gameUImanager.DistanceChange((int)(moveDistanceKM / 5));
        }     
                
    }

    IEnumerator AddSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            roadMoveSpeed+=0.2f;
        }
    }

    IEnumerator AddDistance()
    {
        while(true)
        { 
            yield return null;
            moveDistanceKM+=roadMoveSpeed*0.01f;
        }
    }

    public void GameOver()
    {
        state = GameState.GAMEOVER;
    }

    void AutoCreateRoad()
    {
        Road[] roofLastRoads = new Road[lastRoads.Count];
        lastRoads.CopyTo(roofLastRoads);
        foreach (Road r in roofLastRoads)
        {
            if(r == currentRoad)
            {
                RoadType randomRT = (RoadType)System.Enum.ToObject(typeof(RoadType), Random.Range(0, 2));
                RoadCreateFromI(r, randomRT, currentRoad.propertyType);
            }
        }
    }

    //
    void RoadCreateFromI(Road connectionRoad, RoadType roadType, PropertyType propertyType)
    {
        GameObject road = null;

        Vector3 RoadConnectionPoint = connectionRoad.transform.FindChild("ConnectionPoint").position;
        if (roadType == RoadType.ITYPE)
        {
            road = (GameObject)Instantiate(ITypeRoad, RoadConnectionPoint, Quaternion.identity);

            road.transform.parent = transform;

            road.GetComponent<Road>().roadType = roadType;
            road.GetComponent<Road>().propertyType = propertyType;

            lastRoads.Clear();
            lastRoads.Add(road.GetComponent<Road>());
            allCreatedRoads.Add(road.GetComponent<Road>());
          }
        else
        {
            road = (GameObject)Instantiate(UTypeRoad, RoadConnectionPoint, Quaternion.identity);

            road.transform.parent = transform;

            road.GetComponent<Road>().roadType = roadType;
            road.GetComponent<Road>().propertyType = propertyType;

            allCreatedRoads.Add(road.GetComponent<Road>());

            //change image 추가
            GameObject changePrefab = (GameObject)Instantiate(ChangePrefab, RoadConnectionPoint, Quaternion.identity);
            changePrefab.transform.parent = road.transform;

            //I자형 길을 2개 더 만들어 연결시킨다.

            PropertyType randomLeft = (PropertyType)System.Enum.ToObject(typeof(PropertyType), Random.Range(0, 5));
            PropertyType randomRight = (PropertyType)System.Enum.ToObject(typeof(PropertyType), Random.Range(0, 5));

            RoadCreateFromU(road.GetComponent<Road>(), RoadType.ITYPE, randomLeft, RoadType.ITYPE, randomRight);
        }
    }

    void RoadCreateFromU(Road connectionRoad, RoadType roadTypeLeft, PropertyType propertyTypeLeft, RoadType roadTypeRight, PropertyType propertyTypeRight)
    {
        GameObject LeftRoad = null;
        GameObject RightRoad = null;

        Vector3 RoadConnectionPointLeft = connectionRoad.transform.FindChild("ConnectionPointLeft").position;
        Vector3 RoadConnectionPointRight = connectionRoad.transform.FindChild("ConnectionPointRight").position;

        //왼쪽 길 처리
        if (roadTypeLeft == RoadType.ITYPE)
            LeftRoad = (GameObject)Instantiate(ITypeRoad, RoadConnectionPointLeft, Quaternion.identity);
        else
            LeftRoad = (GameObject)Instantiate(UTypeRoad, RoadConnectionPointLeft, Quaternion.identity);

        LeftRoad.transform.parent = transform;

        
        LeftRoad.GetComponent<Road>().roadType = roadTypeLeft;
        LeftRoad.GetComponent<Road>().propertyType = propertyTypeLeft;

        //오른쪽 길 처리
        if (roadTypeRight == RoadType.ITYPE)
            RightRoad = (GameObject)Instantiate(ITypeRoad, RoadConnectionPointRight, Quaternion.identity);
        else
            RightRoad = (GameObject)Instantiate(UTypeRoad, RoadConnectionPointRight, Quaternion.identity);

        RightRoad.transform.parent = transform;

        RightRoad.GetComponent<Road>().roadType = roadTypeRight;
        RightRoad.GetComponent<Road>().propertyType = propertyTypeRight;

        lastRoads.Clear();
        lastRoads.Add(LeftRoad.GetComponent<Road>());
        lastRoads.Add(RightRoad.GetComponent<Road>());
        allCreatedRoads.Add(LeftRoad.GetComponent<Road>());
        allCreatedRoads.Add(RightRoad.GetComponent<Road>());
    }

    void RoadDestroy()
    {
        List<Road> RemoveList = new List<Road>();
        foreach(Road r in allCreatedRoads)
        {
            if (r.transform.position.y <= -20f)
                RemoveList.Add(r);
        }

        foreach(Road r in RemoveList)
        {
            allCreatedRoads.Remove(r);
            Destroy(r.gameObject);
        }
    }
}
