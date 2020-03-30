using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SelectFocus : MonoBehaviour
{

    public Transform[] FocusObjects;
    public CinemachineTargetGroup CinemachineGroup;

    private IEnumerator coroutine;

    public void StartAnimate(Transform target)
    {
        coroutine = Animate(target);
        gameObject.SetActive(true);
        StartCoroutine(coroutine);
    }

    private IEnumerator Animate(Transform target)
    {
        float minDist = float.PositiveInfinity;
        int minDistObj = 0;

        for (int i = 0; i < FocusObjects.Length; i++)
        {
            CinemachineGroup.RemoveMember(FocusObjects[i]);
            if (FocusObjects[i].gameObject.active)
            {
                float dist = Vector3.Distance(FocusObjects[i].position, target.position);
                if (minDist > dist)
                {
                    minDist = dist;
                    minDistObj = i;
                }
            }
        }

        CinemachineGroup.AddMember(FocusObjects[minDistObj], 1, 0);

        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}

