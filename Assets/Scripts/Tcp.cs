using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using UnityEngine.SceneManagement;

public class Tcp : MonoBehaviour
{
 
  public static Socket socket = null;
  static MainController mainController;
 static  byte[] buffer = new byte[10000];
   public static string GetLocalIPAddress()
{
       var host = Dns.GetHostEntry(Dns.GetHostName());
       foreach (var ip in host.AddressList)
       {
           if (ip.AddressFamily == AddressFamily.InterNetwork)
           {
               return ip.ToString();
           }
       }
       throw new Exception("No network adapters with an IPv4 address in the system!");
}
   
   
   void Start(){
       if(PlayerPrefs.GetString("Scene") == "Main"){
 mainController = Camera.main.GetComponent<MainController>();
       }else if(PlayerPrefs.GetString("Scene") == "Menu"){
       }
      
      
   }

   public static void RequestConnection(){
       try{
           RefreshSocket();
 socket.BeginConnect(new IPEndPoint(IPAddress.Parse("109.122.25.239"),1500), ConnectCallback, socket);
 print("Sucsessful Connect");
       }catch{
print("UnSucsessful Connect");
       }
   }

   

public static void ConnectCallback(IAsyncResult ar)
{
            Socket handler = (Socket)ar.AsyncState;
            handler.EndConnect(ar);
        
            handler.BeginReceive(buffer, 0, buffer.Length, 0, ReadCallback, handler);
}

 static void  ReadCallback(IAsyncResult ar)
        {
            Socket handler = (Socket)ar.AsyncState;
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead != 0)
            {
                string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                string[] command = data.Split('|');

                switch(command[0]){
                    case "RoomID":
                    print("RoomIDAccepted");
MainController.LoadScene();
                    break;

                    case "Answer":
 mainController.UnTrueAnswersofOpponent++;
 mainController.ChangeStateofMarkers();
 mainController.Log.text += "Answer";
                    break;
                     case "RepeatCreateRoom":
                     mainController.Log.text += "RepeatCreateRoom";
 mainController.AllowButtonClick = true;
 mainController.timer = 10f;
 mainController.TimerStart = true;
                    break;

                     case "GameResult":
 mainController.CalculateGameResult(command[1]);
                    break;
                      case "Disconnect":

 SendData("Disconnect|");
 mainController.TimerStart = false;
mainController.Disconnected();
 mainController.Log.text = "LoadScene 0";
 mainController.InvokeLeftRoom();
 socket = null;
                    break;

                      case "GetMoney":
                     
    
 
 Money.money =int.Parse(command[1]);
 print("Money Sendeed");
  Camera.main.GetComponent<Money>().TextMoney.enabled = true;
                    break;
  case "Music":
  
    mainController.SetTrueAnswer(mainController.AnswersText,command[2]);
    
    mainController.SetUnTrueAnswer(mainController.AnswersText,command[3]);
    mainController.SetUnTrueAnswer(mainController.AnswersText,command[4]);
  
                    break;

default:
mainController.Log.text += data;
break;
                }

            }
            handler.BeginReceive(buffer, 0, buffer.Length, 0, ReadCallback, handler);
           
        }

       public static void GetAnswers(){
           mainController.InvokeEvent();
       }

public static void SendData(string data){
    RepeatConnect:
    try{
       
  
    
   byte[] buffer = Encoding.ASCII.GetBytes(data);
     socket.BeginSend(buffer,0,buffer.Length,0,SendCallback,socket);
    print("Sucsess"); 

        
    }
    catch(NullReferenceException){}
    catch(Exception)
    {
      
goto RepeatConnect;
    }   
}

static void SendCallback(IAsyncResult ar)
        {
            Socket handler = (Socket)ar.AsyncState;
            handler.EndSend(ar);

        }



#region  AnotherFunctions
void OnApplicationQuit(){
SendData("Disconnect|");
}



#endregion

public static void RefreshSocket(){
   socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

} 

}

