using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterManager : MonoBehaviour, IPointerDownHandler,
    IPointerUpHandler, IDragHandler
{
    public Transform player;
    Vector2 move;
    public float speed=5;
    public float dashSpeed;
    bool walking;

    public RectTransform pad;
    public RectTransform stick;

    float releaseTime;

    Image touchPanel; // 터치 패널 이미지
    GameObject controllerCanvas;
    private void Start()
    {
        controllerCanvas = GameObject.Find("ControllerCanvas");
        touchPanel = controllerCanvas.transform.Find("touchPanel").GetComponent<Image>();
        pad = controllerCanvas.transform.Find("JoyStickBG").GetComponent<RectTransform>();
        stick = controllerCanvas.transform.Find("JoyStickBG/JoyStick").GetComponent<RectTransform>();
        player = GameObject.Find("Player").transform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        stick.position = eventData.position;

        stick.localPosition =
            Vector2.ClampMagnitude(eventData.position -
            (Vector2)pad.position, pad.rect.width * 0.5f);

        

        move = new Vector2(stick.localPosition.x, stick.localPosition.y).normalized;
        Debug.Log($"{stick.localPosition.x} : {stick.localPosition.y} :: {move}");
        if (!walking)
        {
            walking = true;
            player.GetChild(0).GetComponent<Animator>().SetBool("Walk", true);
        }
        if (releaseTime >= 0.2f)
        {
            
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(player == null) player = GameObject.Find("Player").transform;

        pad.gameObject.SetActive(true);

        pad.position = eventData.position;
        StartCoroutine("Movement");

        touchPanel.raycastTarget = false; // 터치시 패널 레이캐스트 비활성화        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //if (releaseTime < 0.2f && stick.localPosition.magnitude < pad.rect.width * 0.4f)
        //    player.GetComponent<Animator>().Play("Ani_Attack");

        stick.localPosition = Vector2.zero;
        pad.gameObject.SetActive(false);
        StopCoroutine("Movement");
        move = Vector2.zero;

        walking = false;
        player.GetChild(0).GetComponent<Animator>().SetBool("Walk", false);

        touchPanel.raycastTarget = true; // 터치 종료시 패널 레이캐스트 다시 활성화
    }

    IEnumerator Movement()
    {
        releaseTime = 0;
        speed = 1f;
        while (true)
        {
            releaseTime += Time.deltaTime;
            if (releaseTime >= 0.1f)
            {
                player.Translate(move * speed * Time.deltaTime);

                if(stick.localPosition.x < 0) player.localScale = new Vector3(-1, 1, 1);
                else player.localScale = new Vector3(1, 1, 1);
                //if (move != Vector3.zero)
                //    player.rotation = Quaternion.Slerp(player.rotation,
                //        Quaternion.LookRotation(move), 5 * Time.deltaTime);
            }
            
            yield return null;
        }
    }
}
