# Unity FlappyBird游戏实现

## FlappyBird

![QQ截图20191015210941](https://github.com/onelei/FlappyBird/blob/master/Images/QQ截图20191015210941.png)

下面看看实现效果

![Video_2019-10-15_211237](https://github.com/onelei/FlappyBird/blob/master/Images/Video_2019-10-15_211237.gif)

## 实现

我们需要规划一下Bird的位置为坐标（0,0），Bird不用在水平方向上移动，只需要将整个背景向左移动即可。首先地板BackGround分为两块，两块地板一起向左移动，当地板超出视野的时候将其移动到最右边，然后接着向左移动，以此来实现循环滚动的效果。

![QQ截图20191016144021](https://github.com/onelei/FlappyBird/blob/master/Images/QQ截图20191016144021.png)

BackGround_Left坐标是（0,0,0），BackGround_Right坐标是（334,0,0）（原本是（336,0,0））因为两张图片会有接缝，所以移动了2像素。

![QQ截图20191016144714](https://github.com/onelei/FlappyBird/blob/master/Images/QQ截图20191016144714.png)

BackGround.cs代码如下

```
using UnityEngine;

public class BackGround : MonoBehaviour
{
    private const float ImageWith = 334;
    private float LeftX = ImageWith;
    private Vector3 RightPos = new Vector3(ImageWith, 0, 0);

    void LateUpdate()
    {
        transform.Translate(Vector3.left * Time.deltaTime * Util.MOVE_SPEED);

        if (transform.localPosition.x <= -LeftX)
        {
            transform.localPosition = RightPos;
        }
    }
}
```

移动进行100倍加速

```
public class Util
{
    public static float MOVE_SPEED = 100;
}
```

同样的，3根管道也需要这些操作，不过管道需要在Y方向上面进行随机处理。

Pipeline.cs代码如下

```
using UnityEngine;

public class Pipeline : MonoBehaviour
{
    private const float ImageWith = 200;
    private float LeftX = ImageWith;
    private Vector3 RightPos = new Vector3(ImageWith * 2, 0, 0);

    private bool bInit = false;
    public void Init(int index)
    {
        transform.localPosition = new Vector3(ImageWith * (index + 1), 0, 0);
        RandomY();

        bInit = true;
    }

    void LateUpdate()
    {
        if (!bInit)
            return;

        transform.Translate(Vector3.left * Time.deltaTime * Util.MOVE_SPEED);

        if (transform.localPosition.x <= -LeftX)
        {
            transform.localPosition = RightPos;
            RandomY();
        }
    }

    void RandomY()
    {
        transform.Translate(Vector3.up * Random.Range(-50, 50));
    }
}
```

我们增加一个PipelineGroup.cs用来统一管理3根管道，方便Bird撞到管道，进行重置操作。

```
using UnityEngine;

public class PipelineGroup : MonoBehaviour
{
    public void Init()
    {
        Pipeline[] pipelines = GetComponentsInChildren<Pipeline>();
        for (int i = 0; i < pipelines.Length; i++)
        {
            pipelines[i].Init(i);
        }
    }
}
```

我们需要在3根管道中间增加一个Boxcollider2D

![QQ截图20191016150806](https://github.com/onelei/FlappyBird/blob/master/Images/QQ截图20191016150806.png)

然后勾选Is Trigger选项，表明该碰撞体是可以穿透的，不会阻挡Bird的前进，同时设置一下其tag为Door，方便后面碰撞检测的判断。当Bird离开该BoxCollider2D的时候分数+1

![QQ截图20191016150925](https://github.com/onelei/FlappyBird/blob/master/Images/QQ截图20191016150925.png)

Bird的代码如下

```
using UnityEngine;
using UnityEngine.UI;

public class Bird : MonoBehaviour
{
    public PipelineGroup pipelineGroup;
    public Text Text_Score;

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
    	//Debug.Log("进入碰撞");
        if (collider.gameObject.tag == "Door")
        {
            return;
        }
        bDead = true;
    } 

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
```

艺术字Score的显示可以参考上一篇博客 https://blog.csdn.net/onelei1994/article/details/102571115 

接下来运行游戏即可。

参考：

[使用Unity训练AI玩《Flappy》] : https://mp.weixin.qq.com/s?__biz=MzU5MjQ1NTEwOA==&amp;mid=2247495565&amp;idx=1&amp;sn=113f4de5d996525ba7f49c0e87d86fec&amp;chksm=fe1ddb26c96a52309d75fcf9293e5f95e510c91dcc80689dd57323f96c04f6a94c0bc6708bc9&amp;scene=21#wechat_redirect

