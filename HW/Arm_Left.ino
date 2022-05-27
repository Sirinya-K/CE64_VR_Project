/***************************************************
  NodeMCU
****************************************************/
//#include <ESP8266WiFi.h>
#include <WiFi.h>
#include "Adafruit_MQTT.h"
#include "Adafruit_MQTT_Client.h"
/************************* WiFi Access Point *********************************/
#define WLAN_SSID       "HTF_AP"        //Wifi name
#define WLAN_PASS       "11111111"      //Wifi password
#define MQTT_SERVER      "192.168.1.55" // static ip address
#define MQTT_PORT         1883
#define MQTT_USERNAME    ""
#define MQTT_PASSWORD         ""
#define LED_PIN     5                // Pin connected to the LED. GPIO 2 (D4) 
#define BUTTON_PIN  4               // Pin connected to the button. GPIO 15 (D8) 
#define ADC_READ_PIN  A0
#define VALLUME1_PIN  0
#define VALLUME2_PIN  2
float newVal = 0.0;
float sensorVal = 0.0;
float filterConstant = 0.95; // filter constant
int maxval = 0, minval = 1024;
float smooth(float data, float filterVal, float smoothedVal) {
  smoothedVal = (data * (1 - filterVal)) + (smoothedVal  *  filterVal);
  return smoothedVal;
}
/************ Global State ******************/
// Create an ESP8266 WiFiClient class to connect to the MQTT server.
WiFiClient client;
// Setup the MQTT client class by passing in the WiFi client and MQTT server and login details.
Adafruit_MQTT_Client mqtt(&client, MQTT_SERVER, MQTT_PORT, MQTT_USERNAME, MQTT_PASSWORD);
/****************************** Feeds ***************************************/
// Setup a feed called 'pi_led' for publishing.
// Notice MQTT paths for AIO follow the form: <username>/feeds/<feedname>
Adafruit_MQTT_Publish un_in = Adafruit_MQTT_Publish(&mqtt, MQTT_USERNAME "/ui/");
// Setup a feed called 'esp8266_led' for subscribing to changes.
Adafruit_MQTT_Subscribe un_out = Adafruit_MQTT_Subscribe(&mqtt, MQTT_USERNAME "/al/");
/*************************** Sketch Code ************************************/
char now='F',old;


void MQTT_connect();
void setup() {
  Serial.begin(115200);
  delay(10);
  pinMode(22, OUTPUT);
  pinMode(21, OUTPUT);
  pinMode(19, OUTPUT);
  pinMode(18, OUTPUT);
  pinMode(LED_PIN, OUTPUT);
  digitalWrite(LED_PIN, LOW);
  pinMode(VALLUME1_PIN, OUTPUT);
  digitalWrite(VALLUME1_PIN, LOW);
  pinMode(VALLUME2_PIN, OUTPUT);
  digitalWrite(VALLUME2_PIN, LOW);
  // Setup button as an input with internal pull-up.
  pinMode(BUTTON_PIN, INPUT_PULLUP);
  //pinMode(VALLUM_PIN, INPUT);
  Serial.println(F("RPi-ESP-MQTT"));
  // Connect to WiFi access point.
  Serial.println(); Serial.println();
  Serial.print("Connecting to ");
  Serial.println(WLAN_SSID);
  WiFi.begin(WLAN_SSID, WLAN_PASS);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println();
  Serial.println("WiFi connected");
  Serial.println("IP address: "); Serial.println(WiFi.localIP());
  // Setup MQTT subscription for esp8266_led feed.
  mqtt.subscribe(&un_out);
}
uint32_t x = 0;
void loop() {
  // Check if the button has been pressed by looking for a change from high to
  // low signal (with a small delay to debounce).
  int button_first = digitalRead(BUTTON_PIN);
  // Ensure the connection to the MQTT server is alive (this will make the first
  // connection and automatically reconnect when disconnected).  See the MQTT_connect
  MQTT_connect();
  // this is our 'wait for incoming subscription packets' busy subloop
  // try to spend your time here
  // Here its read the subscription
  Adafruit_MQTT_Subscribe *subscription;
  while ((subscription = mqtt.readSubscription())) {
    Serial.println((char *)un_out.lastread);
    //    Serial.print(subscription);
    if (subscription == &un_out) {////ส่วนนี้คือจุดเลือกรับ
      char *message = (char *)un_out.lastread;
      String ms_in = message;
        if (ms_in.substring(0) == "F") {
          digitalWrite(22, 0);
          digitalWrite(21, 0);
          digitalWrite(19, 0);
          digitalWrite(18, 0);
          now='F';
        }
        else if (ms_in.substring(0) == "S") {
          digitalWrite(22, 0);
          digitalWrite(21, 0);
          digitalWrite(19, 0);
          digitalWrite(18, 1);
          now='S';
        }
        else if (ms_in.substring(0) == "I") {
          for(int num=0;num<10;num++){
          move_one_step();  
          }
          if(now=='F'){
            digitalWrite(22, 0);
          digitalWrite(21, 0);
          digitalWrite(19, 0);
          digitalWrite(18, 0);
            }
          else{
            digitalWrite(22, 1);
          digitalWrite(21, 0);
          digitalWrite(19, 0);
          digitalWrite(18, 0);
            }
//          now='I';
        }
        else if (ms_in.substring(0) == "O") {
          for(int num=0;num<10;num++){
          moveR_one_step();  
          }
          if(now=='F'){
            digitalWrite(22, 0);
          digitalWrite(21, 0);
          digitalWrite(19, 0);
          digitalWrite(18, 0);
            }
          else{
            digitalWrite(22, 0);
          digitalWrite(21, 0);
          digitalWrite(19, 0);
          digitalWrite(18, 1);
            }
//          now='O';
        }
      
    }
  }
  // BUTTON_PIN
  //VALLUM_PIN
  // delay(20);
  //  Serial.println("BUTTON_PIN");
  //  Serial.println(!digitalRead(BUTTON_PIN));
  //    Serial.print(String(buf[0])+"1 ");
  //    Serial.print(String(buf[1])+"2 ");
  //    Serial.print(String(buf[2])+"3 ");
  //    Serial.print(String(buf[3])+"4 ");
  //    Serial.print(String(buf[4])+"5 ");
  //    Serial.println(" ");


if(now!=old){
  if(now=='F')
  un_in.publish("al:F");
  else if(now=='S')
  un_in.publish("al:S");
  else if(now=='I')
  un_in.publish("al:I");
  else if(now=='O')
  un_in.publish("al:O");
  old=now;
}
//    un_in.publish(message);



  //  Serial.println("VALLUM_PIN");
  //  Serial.println(map(analogRead(VALLUM_PIN), 0, 1024, 0, 9));
  //un_in.publish(buf);
  //un_in.publish(!digitalRead(BUTTON_PIN));
  // if ((button_first == HIGH) && (button_second == LOW)) {
  //   // Button was pressed!
  //   Serial.println("Button pressed!");
  //   un_in.publish("TOGGLE");
  // }
}
// Function to connect and reconnect as necessary to the MQTT server.
void MQTT_connect() {
  int8_t ret;
  // Stop if already connected.
  if (mqtt.connected()) {
    return;
  }
  Serial.print("Connecting to MQTT... ");
  uint8_t retries = 3;
  while ((ret = mqtt.connect()) != 0) { // connect will return 0 for connected
    Serial.println(mqtt.connectErrorString(ret));
    Serial.println("Retrying MQTT connection in 5 seconds...");
    mqtt.disconnect();
    delay(5000);  // wait 5 seconds
    retries--;
    if (retries == 0) {
      // basically die and wait for WDT to reset me
      while (1);
    }
  }
  Serial.println("MQTT Connected!");
}
void moveR_one_step(){
  digitalWrite(22,1);
    digitalWrite(21,0);
    digitalWrite(19,0);
    digitalWrite(18,0);
    delay(5);
    digitalWrite(22,0);
    digitalWrite(21,1);
    digitalWrite(19,0);
    digitalWrite(18,0);
    delay(5);
    digitalWrite(22,0);
    digitalWrite(21,0);
    digitalWrite(19,1);
    digitalWrite(18,0);
    delay(5);
    digitalWrite(22,0);
    digitalWrite(21,0);
    digitalWrite(19,0);
    digitalWrite(18,1);
    delay(5);
    
}
void move_one_step(){
  digitalWrite(22,0);
    digitalWrite(21,0);
    digitalWrite(19,0);
    digitalWrite(18,1);
    delay(5);
    digitalWrite(22,0);
    digitalWrite(21,0);
    digitalWrite(19,1);
    digitalWrite(18,0);
    delay(5);
    digitalWrite(22,0);
    digitalWrite(21,1);
    digitalWrite(19,0);
    digitalWrite(18,0);
    delay(5);
    digitalWrite(22,1);
    digitalWrite(21,0);
    digitalWrite(19,0);
    digitalWrite(18,0);
    delay(5);
}
