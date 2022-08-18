using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChargeTrigger : MonoBehaviour
{
    public Trigger trigger;
    public GameObject target;
    
    // Start is called before the first frame update
    void Start()
    {
        trigger.isTriggerOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if trigger has been turn on, do something
        if (trigger.isTriggerOn == true)
        {
            // trigger.Print();
            target.GetComponent<GenericTriggerObject>().doTrigger();
            //trigger.isTriggerOn = false;
        }
    }

    //turn on trigger via laser gun
    public void SetTriggerOn()
    {
        trigger.isTriggerOn = true;
    }

}
