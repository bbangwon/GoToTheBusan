using UnityEngine;
using System.Collections;

public class Road : MonoBehaviour {

    public RoadManager.RoadType roadType;
    public RoadManager.PropertyType propertyType;
 
    // Use this for initialization
    void Start () {
        SpriteChange();
	}

    void SpriteChange()
    {
        RoadManager roadManager = GameObject.Find("RoadManager").GetComponent<RoadManager>();

        switch(roadType)
        {
            case RoadManager.RoadType.ITYPE:
                switch (propertyType)
                {
                    case RoadManager.PropertyType.ICE:
                        GetComponent<SpriteRenderer>().sprite = roadManager.IIceSprite;
                        break;
                    case RoadManager.PropertyType.WATER:
                        GetComponent<SpriteRenderer>().sprite = roadManager.IWaterSprite;
                        break;
                    case RoadManager.PropertyType.FIRE:
                        GetComponent<SpriteRenderer>().sprite = roadManager.IFireSprite;
                        break;
                    case RoadManager.PropertyType.RADIATION:
                        GetComponent<SpriteRenderer>().sprite = roadManager.IRadiationSprite;
                        break;
                    case RoadManager.PropertyType.JUNGLE:
                        GetComponent<SpriteRenderer>().sprite = roadManager.IJungleSprite;
                        break;
                }
                break;
            case RoadManager.RoadType.UTYPE:
                switch (propertyType)
                {
                    case RoadManager.PropertyType.ICE:
                        GetComponent<SpriteRenderer>().sprite = roadManager.UIceSprite;
                        break;
                    case RoadManager.PropertyType.WATER:
                        GetComponent<SpriteRenderer>().sprite = roadManager.UWaterSprite;
                        break;
                    case RoadManager.PropertyType.FIRE:
                        GetComponent<SpriteRenderer>().sprite = roadManager.UFireSprite;
                        break;
                    case RoadManager.PropertyType.RADIATION:
                        GetComponent<SpriteRenderer>().sprite = roadManager.URadiationSprite;
                        break;
                    case RoadManager.PropertyType.JUNGLE:
                        GetComponent<SpriteRenderer>().sprite = roadManager.UJungleSprite;
                        break;
                }
                break;
        }


    }
	
	// Update is called once per frame
	void Update () {
        MoveDown();
    }

    void MoveDown()
    {        
        transform.Translate(Vector3.down * Time.deltaTime * transform.parent.GetComponent<RoadManager>().roadMoveSpeed); 
    }
}
