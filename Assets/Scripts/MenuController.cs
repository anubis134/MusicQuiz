using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuController : MonoBehaviour
{
    public int RoomID;
    public bool EnableTimer = false;
    
    public Text TimerText;
    public Text MoneyText;
    public int Minutes;
    public float timer;
    public float Timer{
get{
return timer;
}
set{
if(timer >= 60f){
    timer = 0f; 
    Minutes++;
     }
}
    }
    
  public GameObject FindPanel;
    void Start(){
      
      try{
        Tcp.RequestConnection();
      }catch(Exception ex){
        print(ex.Message);
      }
        
 PlayerPrefs.SetString("Scene","Menu");
        PlayerPrefs.Save();

if(PlayerPrefs.GetString("UserID") == ""){
    PlayerPrefs.SetString("UserID",GenerateID());
    PlayerPrefs.Save();
    Tcp.SendData("UserID|" + PlayerPrefs.GetString("UserID") + "|");
}else{
   Tcp.SendData("UserID|" + PlayerPrefs.GetString("UserID") + "|");
}

print(PlayerPrefs.GetString("UserID"));
    }

void Update(){
   
    Timer = timer;
    TimerText.text = $"{Minutes} : {(int)timer}";
    if(EnableTimer){
        timer += Time.deltaTime;
    }
}

private string GenerateID(){
  string chars = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm";
  string CharPartID = "";
  for(int i = 0; i< 8; i++){
      int randomNumber = UnityEngine.Random.Range(0,51);
CharPartID += chars[randomNumber];
  }

    string ID = "" + UnityEngine.Random.Range(100000,999999) + CharPartID;
    return ID;
}

    public void FindPanelClick(){
       
        EnableTimer = true;
FindPanel.SetActive(true);
Tcp.SendData("FindGame|");



    }

    public void Disconnect(){
      try{
         if(Tcp.socket.Connected){
Tcp.SendData("Disconnect|");
Tcp.RefreshSocket();
         }
      }catch(Exception ex){
print(ex.Message);
      }
   
FindPanel.SetActive(false);
EnableTimer = false;
timer = 0f;
Minutes = 0;
}


}


