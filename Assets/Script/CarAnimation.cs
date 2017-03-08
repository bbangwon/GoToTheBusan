using UnityEngine;
using System.Collections;

public class CarAnimation : MonoBehaviour {

    public Sprite IceCar1;
    public Sprite IceCar2;

    public Sprite WaterCar1;
    public Sprite WaterCar2;

    public Sprite FireCar1;
    public Sprite FireCar2;

    public Sprite RadiationCar1;
    public Sprite RadiationCar2;

    public Sprite JungleCar1;
    public Sprite JungleCar2;

    Sprite CarAni1;
    Sprite CarAni2;

    // Use this for initialization
    void Start () {
        StartCoroutine("CarAnim");
    }
	
	// Update is called once per frame
	void Update () {
        CarAniSprChange();
    }

    void CarAniSprChange()
    {
        switch (GetComponent<Bus>().state)
        {
            case RoadManager.PropertyType.FIRE:
                CarAni1 = FireCar1;
                CarAni2 = FireCar2;
                break;
            case RoadManager.PropertyType.ICE:
                CarAni1 = IceCar1;
                CarAni2 = IceCar2;
                break;
            case RoadManager.PropertyType.JUNGLE:
                CarAni1 = JungleCar1;
                CarAni2 = JungleCar2;
                break;
            case RoadManager.PropertyType.RADIATION:
                CarAni1 = RadiationCar1;
                CarAni2 = RadiationCar2;
                break;
            case RoadManager.PropertyType.WATER:
                CarAni1 = WaterCar1;
                CarAni2 = WaterCar2;
                break;
        }
    }

    IEnumerator CarAnim()
    {
        while (true)
        {
            GetComponent<SpriteRenderer>().sprite = CarAni1;
            yield return new WaitForSeconds(0.3f);
            GetComponent<SpriteRenderer>().sprite = CarAni2;
            yield return new WaitForSeconds(0.3f);
        }
    }
}
