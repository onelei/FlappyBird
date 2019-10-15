using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bird : MonoBehaviour
{
    public PipelineGroup pipelineGroup;
    public Text Text_Score;

    private BoxCollider2D collider2D;
    private Rigidbody2D rigidbody2D;
    private bool bDead = false;
    private bool bInit = false;

    private int _count;
    private int count
    {
        get
        {
            return _count;
        }
        set
        {
            _count = value;
            Text_Score.text = _count.ToString();
        }
    }

    private void Awake()
    {
        collider2D = GetComponent<BoxCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (!bInit)
            return;

        if (bDead)
        {
            Init();
        }
        else
        {
            //鼠标按下的时候，给Bird一个向上的力
            if (Input.GetMouseButtonDown(0))
            {
                transform.localRotation = Quaternion.identity;
                rigidbody2D.AddForce(Vector2.up * 30, ForceMode2D.Impulse);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Door")
        {
            return;
        }
        bDead = true;
    }

    //void OnCollisionExit2D(Collision2D collider)
    //{
    //    if (collider.gameObject.tag == "Door")
    //    {
    //        ++count;
    //        return;
    //    }
    //}

    void OnTriggerExit2D(Collider2D collider)
    {
        //Debug.Log("接触结束");
        if (collider.gameObject.tag == "Door")
        {
            ++count;
        }
    }

    private void Init()
    {
        this.bDead = false;
        rigidbody2D.velocity = Vector2.zero;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        pipelineGroup.Init();
        this.count = 0;
        this.bInit = true;
    } 
}
