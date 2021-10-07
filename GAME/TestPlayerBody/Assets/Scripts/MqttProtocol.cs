using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;
using System.Net.Security;
using System.Text;



public class MqttProtocol : MonoBehaviour
{

    private MqttClient client;
    public string brokerHostname = "192.168.1.55";
    public int brokerPort = 8883;
    public string userName = "test";
    public string password = "test";
    public TextAsset certificate;
    static string subTopic = "/un/in/#";

	public string data = "";

	// private HandPresence handPresence;

    // private void Awake() {
    //     handPresence = GameObject.FindObjectOfType<HandPresence> ();    
	// 	handPresence.ReturnString()

    // }

    // Start is called before the first frame update
    void Start()
    {
		
        if (brokerHostname != null && userName != null && password != null)
		{
			Debug.Log("connecting to " + brokerHostname + ":" + brokerPort);
			// Connect();
			client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
			byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE };
			client.Subscribe(new string[] { subTopic }, qosLevels);
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Connect()
	{
		Debug.Log("about to connect on '" + brokerHostname + "'");
		// Forming a certificate based on a TextAsset
		client = new MqttClient(brokerHostname);
		string clientId = System.Guid.NewGuid().ToString();
		Debug.Log("About to connect using '" + userName + "' / '" + password + "'");
		try
		{
			client.Connect(clientId, userName, password);
		}
		catch (System.Exception e)
		{
			Debug.LogError("Connection error: " + e);
		}
	}

	// รับ Data
    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
	{
		string msg = System.Text.Encoding.UTF8.GetString(e.Message);
		data = msg;
		// Debug.Log ("Received message from " + e.Topic + " : " + msg);
	}

	// ** ยังไม่ใช้ตอนนี้
	public void clear_data(){
		data = "";
	}
    
	// ส่ง Data
    public void Publish(string _topic, string msg)
	{
		client.Publish(
			_topic, Encoding.UTF8.GetBytes(msg),
			MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
	}
}
