using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

https://stackoverflow.com/questions/63865707/unity-3d-how-can-i-detect-if-mouse-is-over-a-2d-sprite-even-if-the-sprite-is-no
answered Sep 13, 2020 at 2:03
user avatar
Kale_Surfer_Dude

*/

public class PrintObjectsMouseOver : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hits = Physics2D.GetRayIntersectionAll(ray, 1500f);

            foreach (var hit in hits)
            {
                print($"Mouse is over {hit.collider.name}");
            }
        }
    }
}
