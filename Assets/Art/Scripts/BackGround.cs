using System.Collections;
using System.Collections.Generic;
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
