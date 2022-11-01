using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightColor : MonoBehaviour
{
    public Light light;

   public void ChangeColorToBlue(){
    //change emission color
    Debug.Log("ChangeColorToBlue");
    Debug.Log(light.color);
     light.color = Color.blue;
   }
    public void ChangeColorToRed(){
            light.color = Color.red;
    }

}
