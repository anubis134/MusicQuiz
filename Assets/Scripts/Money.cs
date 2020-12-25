using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
   public Text TextMoney;
   public static int money{get;set;}

   
 void Start(){
   
 }

 void Update(){
     TextMoney.text = "" + money;
 }
}
