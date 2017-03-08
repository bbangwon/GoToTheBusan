using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour {
    
    public GameObject storyPanel;

    public RectTransform _ticketPos;
    public RectTransform _storyTextPos;

    bool _isClick = false;
	
    void Start()
    {
        Time.timeScale = 1.0f;        
    }

	// Update is called once per frame
	void Update () {

        if(!_isClick)
        {
            if (_ticketPos.position.y > 200)
            {
                _isClick = true;                
                StartCoroutine(GameStartCoroutine());
            }
        }
    }

    public void TicketDrag()
    {
        Vector2 clickPos = Input.mousePosition;
        _ticketPos.position = new Vector2(_ticketPos.position.x, clickPos.y);
    }

    IEnumerator GameStartCoroutine()
    {
        storyPanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        while(true)
        {        
            if (Input.GetMouseButtonDown(0))
            {
                yield return new WaitForSeconds(0.5f);
                SceneManager.LoadScene(1);
            }
            yield return null;
        }
    }
}
