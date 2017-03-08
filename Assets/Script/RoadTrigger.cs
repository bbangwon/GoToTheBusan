using UnityEngine;
using System.Collections;

public class RoadTrigger : MonoBehaviour {

    public enum RoadTriggerType
    {
        PROPERTYCHANGE, //속성변경
        ULEFT,  //왼쪽 이동
        URIGHT,  //오른쪽 이동
        DEATH
    }

    public RoadTriggerType roadTriggerType;
    private RoadManager roadManager;
    bool bTrigger = false;

    void Start()
    {
        roadManager = GetComponentInParent<RoadManager>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //플레이어 속성 변경 페어런트 속성을 가져와 주인공 속성처리
        if (roadTriggerType == RoadTriggerType.PROPERTYCHANGE)
        {
            if (col.tag == "Player")
            {
                roadManager.currentRoad = GetComponentInParent<Road>(); //현재 길을 플레이어가 들어간 길로 변경
                col.GetComponent<Bus>().ChangeBusState(GetComponentInParent<Road>().propertyType);
            }
        }
        else if (roadTriggerType == RoadTriggerType.ULEFT || roadTriggerType == RoadTriggerType.URIGHT)
        {
            if (col.tag == "Player" && bTrigger == false)
            {
                bTrigger = true;
                StartCoroutine(MoveAllCreateRoads());
            }
        }
        else if(roadTriggerType == RoadTriggerType.DEATH)
        {
            if(col.tag == "Player")
            {
                col.GetComponent<Bus>().Damage(100);
            }
        }
    }

    IEnumerator MoveAllCreateRoads()
    {
        bool roop = true;
        Vector3 currentCameraPos = Camera.main.transform.position;

        if(roadTriggerType == RoadTriggerType.ULEFT)
        {
            Vector3 targetCameraMove = new Vector3(roadManager.lastRoads[0].transform.position.x, 0f, -10f);                        
            float orgRoadSpeed = roadManager.roadMoveSpeed;
             while (roop)
            {
                Camera.main.transform.Translate(Vector3.left * (Time.deltaTime * orgRoadSpeed));

                if (Camera.main.transform.position.x < targetCameraMove.x)
                    Camera.main.transform.position = targetCameraMove;

                if(Camera.main.transform.position == targetCameraMove)
                    break;

                yield return null;
            }            
        }
        else
        {
            Vector3 targetCameraMove = new Vector3(roadManager.lastRoads[1].transform.position.x, 0f, -10f);            
            float orgRoadSpeed = roadManager.roadMoveSpeed;
            while (roop)
            {
                Camera.main.transform.Translate(Vector3.right * (Time.deltaTime * orgRoadSpeed));

                if (Camera.main.transform.position.x > targetCameraMove.x)
                    Camera.main.transform.position = targetCameraMove;

                if (Camera.main.transform.position == targetCameraMove)
                    break;

                yield return null;
            }
        }
        bTrigger = false;
        yield return null;
    }


}
