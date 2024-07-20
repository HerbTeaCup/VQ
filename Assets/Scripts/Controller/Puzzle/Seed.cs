using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : InteractableClass
{
    private void Start()
    {
        type = InteracitveObjType.Seed;
    }

    public override void ElementHit(ElementType type)
    {
        base.ElementHit(type);
        if(ready == false) { return; }
    }

    public override void Interactive()
    {
        if(ready == false) { return; }

        base.Interactive();
        GameManager.Player.GetComponent<PlayerStatus>().SeedPoint.SetActive(true);
        Destroy(this.gameObject);
    }
}
