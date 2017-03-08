using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour {
    
    public GameObject _oneNum;
    public GameObject _tenNum;
    public GameObject _hundredNum;

    public Sprite[] _Nums;

    public Image _oneNumImage;
    public Image _tenNumImage;
    public Image _hundredNumImage;

    void Start()
    {
    }
    
    public void DistanceChange(int num)
    {
        if (num >= 100)
        {
            _hundredNum.SetActive(true);
            _tenNum.SetActive(true);
            _oneNum.SetActive(true);

            int HundredTemp = num / 100;
            _hundredNumImage.sprite = _Nums[HundredTemp];
            int TenTemp = (num - (HundredTemp * 100))/10;
            _tenNumImage.sprite = _Nums[TenTemp];
            int OneTemp = num % 10;
            _oneNumImage.sprite = _Nums[OneTemp];
            
        }
        else if (num >= 10)
        {
            _hundredNum.SetActive(false);
            _tenNum.SetActive(true);
            _oneNum.SetActive(true);

            int TenTemp = num / 10;
            _tenNumImage.sprite = _Nums[TenTemp];
            int OneTemp = num % 10;
            _oneNumImage.sprite = _Nums[OneTemp];
        }
        else if (num >= 0)
        {
            _hundredNum.SetActive(false);
            _tenNum.SetActive(false);
            _oneNum.SetActive(true);

            _oneNumImage.sprite = _Nums[num];
        }
    }

}
