using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerСontrol: MonoBehaviour
{
    public void Hit()
    {
        EventManager.Instance.SendEvent(EventId.UnitDamage);
    }

    public void Dead()
    {
        EventManager.Instance.SendEvent(EventId.UnitDead);
    }

    public void Shoot()
    {
        EventManager.Instance.SendEvent(EventId.UnitShoot);
    }
    public void BatHit()
    {
        EventManager.Instance.SendEvent(EventId.UnitBatHit);
    }
    public void FistHit()
    {
        EventManager.Instance.SendEvent(EventId.UnitFistHit);
    }

    public void StartRun()
    {
        EventManager.Instance.SendEvent(EventId.UnitStartStep);
    }

    public void StopRun()
    {
        EventManager.Instance.SendEvent(EventId.UnitStopStep);
    }



}
