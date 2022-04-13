using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterManager : MonoBehaviour, IPointerDownHandler,
    IPointerUpHandler, IDragHandler
{   
    public class Player 
    {
        public int lv;
        public int exp;
        public int hp;
        public int money;
        public int dia;
        public string skill;
    }
    public static CharacterManager instance;

    public Player playerInfo;
    public Transform player;
    Transform cameraObj;
    Camera camera;

    Vector2 move;
    public float speed=5;
    public float dashSpeed;
    bool walking;

    public RectTransform pad;
    public RectTransform stick;

    float releaseTime;

    Image touchPanel; // 터치 패널 이미지
    GameObject controllerCanvas;
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        playerInfo = new Player();
        camera = Camera.main;
        cameraObj = GameObject.Find("Camera").transform;
        controllerCanvas = GameObject.Find("ControllerCanvas");
        touchPanel = controllerCanvas.transform.Find("touchPanel").GetComponent<Image>();
        pad = controllerCanvas.transform.Find("JoyStickBG").GetComponent<RectTransform>();
        stick = controllerCanvas.transform.Find("JoyStickBG/JoyStick").GetComponent<RectTransform>();
        player = GameObject.Find("Player").transform;
        camera.GetComponent<Transform>().SetParent(player);
        speed = 5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        stick.position = eventData.position;

        stick.localPosition =
            Vector2.ClampMagnitude(eventData.position -
            (Vector2)pad.position, pad.rect.width * 0.5f);

        

        move = new Vector2(stick.localPosition.x, stick.localPosition.y).normalized;
        //Debug.Log($"{stick.localPosition.x} : {stick.localPosition.y} :: {move}");
        if (!walking)
        {
            walking = true;
            player.Find("Dino(Clone)").GetComponent<Animator>().Play("dinoAniWalk");

            //player.Find("Dino(Clone)").GetComponent<Animator>().SetBool("Walk", true);
        }
        if (releaseTime >= 0.2f)
        {
            
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"speed : {speed}");
        if(player == null) player = GameObject.Find("Player").transform;

        pad.gameObject.SetActive(true);

        pad.position = eventData.position;
        StartCoroutine(nameof(Movement));

        touchPanel.raycastTarget = false; // 터치시 패널 레이캐스트 비활성화        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //if (releaseTime < 0.2f && stick.localPosition.magnitude < pad.rect.width * 0.4f)
        //    player.GetComponent<Animator>().Play("Ani_Attack");

        stick.localPosition = Vector2.zero;
        pad.gameObject.SetActive(false);
        StopCoroutine(nameof(Movement));
        move = Vector2.zero;

        walking = false;
        player.Find("Dino(Clone)").GetComponent<Animator>().Play("dinoAniIdle");
        //player.Find("Dino(Clone)").GetComponent<Animator>().SetBool("Walk", false);

        touchPanel.raycastTarget = true; // 터치 종료시 패널 레이캐스트 다시 활성화
    }

    IEnumerator Movement()
    {
        releaseTime = 0;
        
        while (true)
        {
            if (player.position.x < -5f)
                player.position = new Vector3(-5f, player.position.y, 0);
            if (player.position.x > 12f)
                player.position = new Vector3(12f, player.position.y, 0);

            if (player.position.y < -3f)
                player.position = new Vector3(player.position.x, -3f, 0);
            if (player.position.y > 3f)
                player.position = new Vector3(player.position.x, 3f, 0);
            //if (player.position.x < -5f || player.position.x > 12f || player.position.y < -3f || player.position.y > 3f)
            //{
            //    cameraObj.transform.position = player.position;
            //    camera.GetComponent<Transform>().SetParent(cameraObj);

            //    if (player.position.x < -5f)
            //        cameraObj.position = new Vector3(-5f, cameraObj.position.y, 0);
            //    if (player.position.x > 12f)
            //        cameraObj.position = new Vector3(12f, cameraObj.position.y, 0);

            //    if (player.position.y < -3f)
            //        cameraObj.position = new Vector3(cameraObj.position.x, -3f, 0);
            //    if (player.position.y > 3f)
            //        cameraObj.position = new Vector3(cameraObj.position.x, 3f, 0);
            //}
            //else
            //    camera.GetComponent<Transform>().SetParent(player);

            releaseTime += Time.deltaTime;
            if (releaseTime >= 0.1f)
            {
                player.Translate(move * speed * Time.deltaTime);

                if(stick.localPosition.x < 0) player.Find("Dino(Clone)").localScale = new Vector3(-4, 4, 4);
                else player.Find("Dino(Clone)").localScale = new Vector3(4, 4, 4);
                
            }
            
            yield return null;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Wall")
        {
            move = Vector2.zero;
            
        }
    }
}
