using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Bus : MonoBehaviour
{
    public RoadManager.PropertyType state = RoadManager.PropertyType.FIRE;

    public int _hp;
    public float _speed;
    public float _crashspeed;

    public int healPoint;

    public Slider _hpBar;

    Vector2 _targetPos;
    Vector2 _playerPos;
    Vector2 _tmpPos;

    float _naPos;
    public bool _isWallCrash;
    public bool _isMove = false;
    
    public Sprite[] _buses;

    public SpriteRenderer _spriteRenderer;

    public GameObject gameOverPanelObj;

    public AudioSource audioSource;
    public AudioSource carSound;
    public AudioClip zombi;

    public Text killCountText;
    int killCount;
    RoadManager roadManager;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        killCount = 0;
        roadManager = GameObject.Find("RoadManager").GetComponent<RoadManager>();
    }

    // Update is called once per frame
    void Update()
    {
        killCountText.text = killCount.ToString();

        if (Input.GetKeyDown(KeyCode.A))
        {
            Damage(10);
        }
        BosMove();
    }

    public void Init(RoadManager.PropertyType property)
    {
        // 버스 랜덤 속성 시작
        ChangeBusState(property);
    }

    // 버스 터치움직임
    void BosMove()
    {
        // 벽에 충돌 시 클릭 x 
        if (_isWallCrash) return;

        if (Input.GetMouseButton(0))
        {
            TargetSetting();

            if (_isMove) return;

            StartCoroutine(FollowBusMoveCoroutine());
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 dir = Vector3.left;
            transform.position += dir * 10.0f * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 dir = Vector3.right;
            transform.position += dir * 10.0f * Time.deltaTime;
        }
    }

    // 클릭장소 이동 세팅
    void TargetSetting()
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _targetPos = new Vector2(wp.x, 0);
        _playerPos = transform.position;

        _naPos = _targetPos.x - _playerPos.x;
        //Debug.Log(naPos);
    }

    // 이동 코루틴
    IEnumerator FollowBusMoveCoroutine()
    {
        if (!_isMove)
        {
            _isMove = true;
            while (Mathf.Abs(_playerPos.x - _targetPos.x) >= 0.01f)
            {
                _playerPos = transform.position;

                if (_isWallCrash)
                {
                    // 벽에 충돌되면 원래 스피드보단 느린 속도로 조금 이동된다.
                    transform.Translate(Vector2.right * _naPos * _crashspeed * Time.deltaTime);
                }
                else
                {
                    transform.Translate(Vector2.right * _naPos * _speed * Time.deltaTime);
                }

                yield return new WaitForSeconds(0.0001f);
            }
            _isMove = false;
            if (_isWallCrash)
            {
                _isWallCrash = false;
            }
        }
        //Debug.Log(Mathf.Abs(playerPos.x - touchPos.x));
    }

    // 스테이트 바꾸기
    public void ChangeBusState(RoadManager.PropertyType type)
    {
        state = type;

        switch (state) {
            case RoadManager.PropertyType.FIRE:
                _spriteRenderer.sprite = _buses[0];
                break;
            case RoadManager.PropertyType.ICE:
                _spriteRenderer.sprite = _buses[1];
                break;
            case RoadManager.PropertyType.JUNGLE:
                _spriteRenderer.sprite = _buses[2];
                break;
            case RoadManager.PropertyType.RADIATION:
                _spriteRenderer.sprite = _buses[3];
                break;
            case RoadManager.PropertyType.WATER:
                _spriteRenderer.sprite = _buses[4];
                break;
        }


    }

    // 데미지
    public void Damage(int damamge)
    {
        _hp -= damamge;

        _hpBar.value = (float)_hp / 100f;

        // 죽음
        if (_hp <= 0)
        {
            Debug.Log("Die");
            //SceneManager.LoadScene("End");
            carSound.Stop();
            Time.timeScale = 0;
            roadManager.GameOver();
            gameOverPanelObj.SetActive(true);
        }
    }

    public void Heal(int point)
    {
        _hp += point;
        _hpBar.value = (float)_hp / 100f;
    }

    public void GoMainBtnClick()
    {
        SceneManager.LoadScene("Intro");
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Zombie")
        {
            Zombie zombie = coll.gameObject.GetComponent<Zombie>();

            int randDamage = Random.Range(1, 3);
            Damage(randDamage);
            healPoint = Random.Range(3, 15);
            switch (state)
            {
                case RoadManager.PropertyType.FIRE:
                    if (zombie.currentColor == Zombie.ZombieColor.Red)
                    {
                        Heal(healPoint);
                    }
                    break;
                case RoadManager.PropertyType.ICE:
                    if (zombie.currentColor == Zombie.ZombieColor.SkyBlue)
                    {
                        Heal(healPoint);
                    }
                    break;
                case RoadManager.PropertyType.JUNGLE:
                    if (zombie.currentColor == Zombie.ZombieColor.Green)
                    {
                        Heal(healPoint);
                    }
                    break;
                case RoadManager.PropertyType.RADIATION:
                    if (zombie.currentColor == Zombie.ZombieColor.Yellow)
                    {
                        Heal(healPoint);
                    }
                    break;
                case RoadManager.PropertyType.WATER:
                    if (zombie.currentColor == Zombie.ZombieColor.Blue)
                    {
                        Heal(healPoint);
                    }
                    break;
            }
            audioSource.PlayOneShot(zombi);
            killCount++;
            zombie.currentState = Zombie.ZombieState.Dead;
        }
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "LeftWall")
        {
            Debug.Log("left");
            Damage(10);
        }
        else if (col.gameObject.tag == "RightWall")
        {
            Debug.Log("right");
            Damage(10);
        }
        
    }

    // 벽 충돌시 버스 도로로 되돌리기
    public void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "LeftWall")
        {
            _isWallCrash = true;
            _tmpPos = transform.position;
            _tmpPos = new Vector2(_tmpPos.x + 0.2f, _tmpPos.y);
            _targetPos = _tmpPos;
            _naPos = _targetPos.x - _playerPos.x;
            if (!_isMove)
            {
                StartCoroutine(FollowBusMoveCoroutine());
            }
        }
        else if (col.gameObject.tag == "RightWall")
        {
            _isWallCrash = true;
            _tmpPos = transform.position;
            _tmpPos = new Vector2(_tmpPos.x - 0.2f, _tmpPos.y);
            _targetPos = _tmpPos;
            _naPos = _targetPos.x - _playerPos.x;
            if (!_isMove)
            {
                StartCoroutine(FollowBusMoveCoroutine());
            }
        }
    }

    
}
