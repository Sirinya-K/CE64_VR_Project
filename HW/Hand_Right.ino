/***************************************************
  NodeMCU
****************************************************/
//#include <ESP8266WiFi.h>
#include <ESP32Servo.h>
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
Servo myservo[5];
bool haveCalibat = false;
int sent_per_sec = 10;
float newVal[5] = {0.0, 0.0, 0.0, 0.0, 0.0};
int sensorpin[5] = {36, 39, 34, 35, 33}; //pin 32 ของบอร์ดมีปัญหา
//  int sensorpin[5]={36,39,34,35,32};
int servopin[5] = {22, 21, 19, 18, 5};
float servoMax[5] = {180.0, 180.0, 180.0, 180.0, 180.0};
float sensorVal = 0.0;
bool nowCalibat = false;
unsigned long timer1 = millis(), timer2 = millis();
float filterConstant = 0.95; // filter constant
float old[5] = {0.0, 0.0, 0.0, 0.0, 0.0}, maxval[5] = {2850.0, 2850.0, 2850.0, 2850.0, 2850.0}, minval[5] = {0.0, 0.0, 0.0, 0.0, 0.0};

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
Adafruit_MQTT_Subscribe un_out = Adafruit_MQTT_Subscribe(&mqtt, MQTT_USERNAME "/hr/");
/*************************** Sketch Code ************************************/


void MQTT_connect();
void setup() {
  Serial.begin(115200);
  delay(10);
  myservo[0].attach(servopin[0]);
  myservo[1].attach(servopin[1]);
  myservo[2].attach(servopin[2]);
  myservo[3].attach(servopin[3]);
  myservo[4].attach(servopin[4]);
  myservo[0].write(180);
  myservo[1].write(180);
  myservo[2].write(180);
  myservo[3].write(180);
  myservo[4].write(180);
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
void loop() {
  // Check if the button has been pressed by looking for a change from high to
  // low signal (with a small delay to debounce).
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
      if (ms_in == "C") {
        Serial.println("is C");
        if (nowCalibat == true) {
          nowCalibat = false;
          for (int i = 0; i < 5; i++) {
            servoMax[i] = (map(maxval[i], 0, 2850, 0, 180));
          }
          haveCalibat = true;
        }
        else {
          nowCalibat = true;

          for (int i = 0; i < 5; i++) {
            maxval[i] = 0;
            minval[i] = 2850;
            myservo[i].write(180);
          }

        }
      }
      else if (ms_in.substring(0, 1) == "s") {// max servo
        int inData[5];
        char charbuf;
        int n = sscanf(message, "%c %d %d %d %d %d", &charbuf, &inData[0], &inData[1], &inData[2], &inData[3], &inData[4]);
        Serial.println("data here M");
        for (int i = 0; i < 5; i++) {
          Serial.println(inData[i]);
          servoMax[i] = inData[i];
          maxval[i] = map(inData[i], 0, 180, 0, 2850);
        }
        haveCalibat = true;
      }
      else if (nowCalibat == false) {
        String ms_in = message;
        int sub = ms_in.substring(1).toInt();
        Serial.println("data here");
        Serial.println(sub);
        if (ms_in[0] == 't') {
          myservo[0].write(map(sub, 0, 20, 0, servoMax[0]));
        }
        else if (ms_in[0] == 'i') {
          myservo[1].write(map(sub, 0, 20, 0, servoMax[1]));
        }
        else if (ms_in[0] == 'm') {
          myservo[2].write(map(sub, 0, 20, 0, servoMax[2]));
        }
        else if (ms_in[0] == 'r') {
          myservo[3].write(map(sub, 0, 20, 0, servoMax[3]));
        }
        else if (ms_in[0] == 'p') {
          myservo[4].write(map(sub, 0, 20, 0, servoMax[4]));
        }
        //        int inData[5];
        //        int n = sscanf(message, "%d %d %d %d %d", &inData[0], &inData[1], &inData[2], &inData[3], &inData[4]);
        //        Serial.println("data here");
        //        for (int i = 0; i < 5; i++) {
        //          Serial.println(inData[i]);
        //          myservo[i].write(map(inData[i], 0, 20, 0, servoMax[i]));
        //        }
      }
      if (ms_in.substring(0, 2) == "hr") {

      }
    }
  }
  if (haveCalibat == true) {
    if (nowCalibat == true) {
      for (int j = 0; j < 5; j++) {
        int buf = analogRead(sensorpin[j]);
        timer2 = millis();
        sensorVal = buf;
        newVal[j] = smooth(sensorVal, filterConstant, newVal[j]);
        if (newVal[j] < minval[j]) {
          minval[j] = (newVal[j]) - 1;
        }
        if (newVal[j] > maxval[j]) {
          maxval[j] = (newVal[j]) + 1;
        }
        while ((millis() - timer2) > 5) {
        }
      }
      if ((millis() - timer1) > sent_per_sec) {
        char message[29];
        String test = "cp";//calibate processing
        test.toCharArray(message, 29);
        un_in.publish(message);
        timer1 = millis();
      }

    }
    else {
      int buf1[5] = {0.0, 0.0, 0.0, 0.0, 0.0};
      for (int j = 0; j < 5; j++) {
        int buf = analogRead(sensorpin[j]);
        timer2 = millis();
        sensorVal = buf;
        newVal[j] = smooth(sensorVal, filterConstant, newVal[j]);

        //    buf1[j]=map(snewVal[j],maxval[j],minval[j],20,0);
        buf1[j] = map(newVal[j], maxval[j], minval[j], 20, 0);
        //    myservo[j].write(map(buf1[j],0,20,0,90));
        //    Serial.print(buf1[j]);
        while ((millis() - timer2) > 5) {
        }
      }

      if ((millis() - timer1) > sent_per_sec) {
        char message[29];
        //      String test = "hr:" + String((millis()-timer1));
        timer1 = millis();
        String test = "hr:" + String(buf1[0]) + " " + String(buf1[1]) + " " + String(buf1[2]) + " " + String(buf1[3]) + " " + String(buf1[4]);
        test.toCharArray(message, 29);
        un_in.publish(message);
      }
    }
  }
  else {
    if ((millis() - timer1) > sent_per_sec) {
      char message[29];
      String test = "nc";////need calibate
      test.toCharArray(message, 29);
      un_in.publish(message);
      timer1 = millis();
    }
  }


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
