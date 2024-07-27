#include <LiquidCrystal_I2C.h>
#include "Wire.h"
#include <stdio.h>
#include "DFRobot_EC.h"
#include <math.h>
#include <FreqCount.h>

#define LDR A0
#define PH A1
#define EC A2
#define WINDVANE A3

#define MINUS 2
#define PLUS 3
#define TRIG 4
#define ECHO 6

#define MOTOR_PUMP 7
#define VENTILATION 8
#define LIGHT 9
#define HEATER 10
#define VALVE 11
#define WATER_TANK_M 12
#define HUMILATOR 13


char incomingByte,readValue ;
unsigned long long  countimers[2];
char Csymbol=223;

typedef struct{ 
  int seconds;
  int minutes;
  int hours;
  int days;
  int dates;
  int months;
  int year;
} time;

typedef struct{
  int hour;
  int minute;
  int second;
}period;

typedef struct{
  String start;
  String stop;
}start_stop;

typedef struct{
  float temperature;
  float humidity;
}sht_data;

typedef struct{
  int first;
  int last;
}String_int;



String_int controls_calibration[6]={20,25,60,70,400,0,50,100,0,10,5,7};
start_stop Light_Motor_ST[2]={":6-30_00;",":14-30_00;",":15-30_00;",":17-30_00;"};
period time_calibration[4];


bool ven_t=false,controls_start[5];

String_int prev_value_1;
period prev_value_2;
bool prev;

unsigned int time_counter[2];

String Direction;
char data;
unsigned int anemometr_count;
int lcd_selection=1,NTC_value;

bool lcd_select=HIGH;

LiquidCrystal_I2C lcd(0x27,16,2);

DFRobot_EC ec;



void setup(){
  Wire.begin();
  Serial.begin(9600);
  
  FreqCount.begin(1000);
  lcd.init();
  ec.begin();

  pinMode(PLUS, INPUT_PULLUP);
  pinMode(MINUS, INPUT_PULLUP);
  pinMode(TRIG, OUTPUT);
  pinMode(ECHO, INPUT);

  for(int i=7;i<=13;i++){
    pinMode(i, OUTPUT);
  }

  attachInterrupt(digitalPinToInterrupt(PLUS), plus_func, RISING);
  attachInterrupt(digitalPinToInterrupt(MINUS), minus_func, RISING);
}

void loop(){

  if (FreqCount.available) {
    anemometr_count=FreqCount.read()*2;
  }
  time time_now=ds_1307();
  int LDR_value = analogRead(LDR);
  int PHD_value = analogRead(PH);
  float voltage_EC = analogRead(EC) * (5.0 / 1023.0);
  int WINDVANE_value=analogRead(WINDVANE);

  WINDVANE_value=map(WINDVANE_value, 0, 1023, 0, 360)/45;
  float ecValue =  ec.readEC(voltage_EC,LM75AD());
  switch(WINDVANE_value){
    case 0: Direction="North";
    break;
    case 1: Direction="North-West";
    break;
    case 2: Direction="West";
    break;
    case 3: Direction="South-West";
    break;
    case 4: Direction="South";
    break;
    case 5: Direction="South-East";
    break;
    case 6: Direction="East";
    break;
    case 7: Direction="North-East";
    break;
    default: Direction="North";
    break;
  }
  sht_data value_sht=SHT25();
  
  time_calibration[0]=include_time(Light_Motor_ST[0].start,":", ";", "-", "_");
  time_calibration[1]=include_time(Light_Motor_ST[1].start,":", ";", "-", "_");
  time_calibration[2]=include_time(Light_Motor_ST[1].stop,":", ";", "-", "_");
  time_calibration[3]=include_time(Light_Motor_ST[0].stop,":", ";", "-", "_");



  if (Serial.available()) {
    String text = Serial.readString();

    controls_calibration[0]=findValue(text,"Temp",":",";","-",controls_calibration[0]);
    controls_calibration[1]=findValue(text,"Humd",":",";","-",controls_calibration[1]);
    controls_calibration[2]=findValue(text,"Light",":",";","-",controls_calibration[2]);
    controls_calibration[3]=findValue(text,"WTank",":",";","-",controls_calibration[3]);
    controls_calibration[4]=findValue(text,"EC",":",";","-",controls_calibration[4]);
    controls_calibration[5]=findValue(text,"PH",":",";","-",controls_calibration[5]);

    Light_Motor_ST[0]=find_start_stop(text, "Lumen", ":", ";", "+", Light_Motor_ST[0]);
    Light_Motor_ST[1]=find_start_stop(text, "Motor", ":", ";", "+", Light_Motor_ST[1]);

    if(text.startsWith("Valve")){
      String selection_str = text.substring(text.indexOf(":") + 1, text.indexOf(";"));
      controls_start[1]=atoi(selection_str.c_str());
    }
    else{
      controls_start[1]=controls_start[1];
    }
  }

  if(controls_calibration[0].first<=value_sht.temperature&&value_sht.temperature<=controls_calibration[0].last){
    digitalWrite(HEATER,LOW);
    digitalWrite(VENTILATION,LOW);
    ven_t=true;

  }
  else if(controls_calibration[0].first>=value_sht.temperature){
    digitalWrite(HEATER,HIGH);
    digitalWrite(VENTILATION,LOW);
    ven_t=false;
  }
  else if(controls_calibration[0].last<=value_sht.temperature){
    digitalWrite(HEATER,LOW);
    digitalWrite(VENTILATION,HIGH);
    ven_t=false;
  }
  if(ven_t){
    if(controls_calibration[1].first<=value_sht.humidity&&value_sht.humidity<=controls_calibration[1].last){
      digitalWrite(HUMILATOR,LOW);
      digitalWrite(VENTILATION,LOW);
    }
    else if(controls_calibration[1].first>=value_sht.humidity){
      digitalWrite(HUMILATOR,HIGH);
      digitalWrite(VENTILATION,LOW);
    }
    else if(controls_calibration[1].last<=value_sht.humidity){
      digitalWrite(HUMILATOR,LOW);
      digitalWrite(VENTILATION,HIGH);
    }
  }

  float calc_EC[2]={controls_calibration[4].first/10,controls_calibration[4].last/10};

  if(calc_EC[0]<=ecValue&&calc_EC[1]>=ecValue&&controls_calibration[5].first<=ph_value(PHD_value)&&controls_calibration[5].last>=ph_value(PHD_value)){
    controls_start[0]=true;
  }
  else  {
    controls_start[0]=false;
  }
  if(controls_start[1]){
    digitalWrite(VALVE, HIGH);
  }
  else {
    digitalWrite(VALVE, LOW);
  }

  //controls_start[2]=timer_count(time_calibration[1],time_calibration[2],time_now,controls_start[2],time_counter[0]);
  //controls_start[3]=timer_ldr(time_calibration[1],controls_calibration[2],time_now,controls_start[2],time_counter[0]);
  controls_start[3]=time_trust(time_calibration[0],time_calibration[3],time_now,countimers[0],controls_start[3]);
  controls_start[2]=time_trust(time_calibration[1],time_calibration[2],time_now,countimers[1],controls_start[2]);


  
  if(controls_start[3]&&controls_calibration[2].first>ldr_lux(LDR_value)*0.0135){
    digitalWrite(LIGHT,HIGH);
  }
  else if(!controls_start[3]||controls_calibration[2].first<ldr_lux(LDR_value)*0.0135){
    digitalWrite(LIGHT,LOW);
  }

  


    if(controls_start[2]&&!controls_start[1]&&controls_start[0]){
      digitalWrite(MOTOR_PUMP, HIGH);
    }
    else if(!controls_start[0]||!controls_start[1]||controls_start[2]) {
      digitalWrite(MOTOR_PUMP, LOW);
    }
  

  int litrtank = Water_tank_calc(calc_hcsr04(ECHO,TRIG),controls_calibration[3].first,controls_calibration[3].last);

  if (litrtank<3) {
    digitalWrite(WATER_TANK_M, HIGH);
  }
  else if (litrtank>controls_calibration[3].first-3) {
    digitalWrite(WATER_TANK_M, LOW);
  }
  int new_cap=0; 
  switch (lcd_selection) {
    case 1: temperature(value_sht.temperature,value_sht.humidity);
    break;
    case 2: light(ldr_lux(LDR_value));
    break;
    case 3: PH_EC(ph_value(PHD_value),ecValue,LM75AD());
    break;
    case 4: water_level(litrtank,controls_calibration[3].first);
    break;
    case 5: wind(Direction,anemometr_count);
    break;
    case 6: date_time();
    break;
  }

  //Serial Connection


  for(int i=0; i<7; i++){
      
    new_cap = new_cap + ((digitalRead(7 + i)) << i);
    if(i == 6){
        Serial.print("B");
        Serial.print(new_cap); 
        Serial.print("b");
      }
    }

  Serial.print("T");
  Serial.print(value_sht.temperature);
  Serial.print("t");
  Serial.print("H");
  Serial.print(value_sht.humidity);
  Serial.print("h");
  Serial.print("K");
  Serial.print(LM75AD());
  Serial.print("k");
  Serial.print("E");
  Serial.print(ecValue);
  Serial.print("e");
  Serial.print("L");
  Serial.print(ldr_lux(LDR_value));
  Serial.print("l");
  Serial.print("P");
  Serial.print(ph_value(PHD_value));
  Serial.print("p");
  Serial.print("W");
  Serial.print(litrtank);
  Serial.print("w");
  Serial.print("J");
  Serial.print(controls_calibration[3].first);
  Serial.print("j");
  Serial.print("D");
  Serial.print(WINDVANE_value);
  Serial.print("d");
  Serial.print("O");
  Serial.print(anemometr_count);
  Serial.print("o");
  send_msg('Q', 'q', controls_calibration[0]);
  send_msg('R', 'r', controls_calibration[1]);
  send_msg('Y', 'y', controls_calibration[2]);
  send_msg('U', 'u', controls_calibration[3]);
  send_msg('I', 'i', controls_calibration[4]);
  send_msg('F', 'f', controls_calibration[5]);
  send_msgT('G', 'g', time_calibration[0]);
  send_msgT('Z', 'z', time_calibration[1]);
  send_msgT('X', 'x', time_calibration[2]);
  send_msgT('N', 'n', time_calibration[3]);
  send_msgB('V', 'v', controls_start[1]);
  Serial.println("S");

}


