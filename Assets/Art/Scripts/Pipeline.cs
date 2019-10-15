using System.Collections;
using System.Collections.Generic;
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
