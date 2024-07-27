void send_msg(char first,char last,String_int value){
  char array[10];
  sprintf(array, "%c%d-%d%c",first,value.first,value.last,last);
  Serial.print(array);
}
void send_msgT(char first,char last,period value){
  char array[15];
  sprintf(array, "%c%d:%d:%d%c",first,value.hour,value.minute,value.second,last);
  Serial.print(array);
}
void send_msgB(char first,char last,bool value){
  char array[5];
  sprintf(array, "%c%d%c",first,value,last);
  Serial.print(array);
}

bool timer_count(period watch_time,period time_interval_,time now,bool selection,int counter){
  int time_plus;
  int time_interval=time_interval_.hour*60+time_interval_.minute;
  if (watch_time.hour==now.hours&&watch_time.minute==now.minutes&&watch_time.second==now.seconds) {
    selection=true;
    counter=0;
  }
  if(selection&&watch_time.second==now.seconds){
    time_plus++;
    if (time_plus==1) {
      counter++;
    }
  }
  else {
    time_plus=0;
  }
  if (time_interval==counter) {
    selection=false;
  }
  return selection;
}
bool timer_ldr(period watch_time,String_int time_interval_,time now,bool selection,int counter){
  int time_plus;
  int time_interval=time_interval_.last*60;
  if (watch_time.hour==now.hours&&watch_time.minute==now.minutes&&watch_time.second==now.seconds) {
    selection=true;
    counter=0;
  }
  if(selection&&watch_time.second==now.seconds){
    time_plus++;
    if (time_plus==1) {
      counter++;
    }
  }
  else {
    time_plus=0;
  }
  if (time_interval==counter) {
    selection=false;
  }
  return selection;
}

bool time_trust(period start,period stop,time now,  unsigned long long  count,bool value){
  if(start.hour<=now.hours&&now.hours<=stop.hour){
    value=true;
    if(start.hour==now.hours){
      if(start.minute<=now.minutes){
          value=true;
      }
      else {
          value=false;
      }
    }
    if(stop.hour==now.hours){
      if(stop.minute>=now.minutes){
          value=true;
      }
      else {
          value=false;
      }
    }
  }

  
  else if (now.hours>stop.hour) {
    value=false;
  }
  else if (start.hour>now.hours) {
    value=false;
  }
  return value;
  
}

String_int findValue(String text,String kewyword,String separator1,String separator2,String lastSerator,String_int default_value){
  String_int _value_;
  if(text.startsWith(kewyword)){
    String selection_str = text.substring(text.indexOf(separator1) + 1, text.indexOf(lastSerator));
    String selection_str_ = text.substring(text.indexOf(lastSerator) + 1, text.indexOf(separator2));
    _value_.first=atoi(selection_str.c_str());
    _value_.last=atoi(selection_str_.c_str());


  }
  else {
    _value_.first=default_value.first;
    _value_.last=default_value.last;
  }
  return _value_;
}
period findTime(String text,String kewyword,String separator1,String separator2,String lastSerator1,String lastSerator2,period default_value){
  period _value_;
  if(text.startsWith(kewyword)){

    String hour_ = text.substring(text.indexOf(separator1) + 1, text.indexOf(lastSerator1));
    String minute_ = text.substring(text.indexOf(lastSerator1) + 1, text.indexOf(lastSerator2));
    String second_ = text.substring(text.indexOf(lastSerator2) + 1, text.indexOf(separator2));

    _value_.hour=atoi(hour_.c_str());
    _value_.minute=atoi(minute_.c_str());
    _value_.second=atoi(second_.c_str());

  }
  else {
    _value_.hour=default_value.hour;
    _value_.minute=default_value.minute;
    _value_.second=default_value.second;

  }
  return _value_;
}

period include_time(String text,String separator1,String separator2,String lastSerator1,String lastSerator2){
  period _value_;

  String hour_ = text.substring(text.indexOf(separator1) + 1, text.indexOf(lastSerator1));
  String minute_ = text.substring(text.indexOf(lastSerator1) + 1, text.indexOf(lastSerator2));
  String second_ = text.substring(text.indexOf(lastSerator2) + 1, text.indexOf(separator2));

  _value_.hour=atoi(hour_.c_str());
  _value_.minute=atoi(minute_.c_str());
  _value_.second=atoi(second_.c_str());

  return _value_;
}

start_stop find_start_stop(String text,String kewyword,String firstseperator,String lastseparator,String middle,start_stop default_value){
  start_stop _value_;
  
  if(text.startsWith(kewyword)){

    String start = text.substring(text.indexOf(firstseperator) + 1, text.indexOf(middle));
    String stop = text.substring(text.indexOf(middle) + 1, text.indexOf(lastseparator));

  _value_.start=":"+start+";";
  _value_.stop=":"+stop+";";
  }
  else {
    _value_.start=default_value.start;
    _value_.stop=default_value.stop;

  }
  return _value_ ;
}



int ds_convert(int value){
  int new_value=value/16*10+value%16;
  return new_value;
}
time ds_1307(){
  time result;
  Wire.beginTransmission(0x68); 
  Wire.write(0x00); 
  Wire.endTransmission();
  Wire.requestFrom(0x68, 7); 
  if (Wire.available() == 7) { 
    result.seconds = ds_convert(Wire.read());
    result.minutes = ds_convert(Wire.read());
    result.hours =ds_convert(Wire.read());
    result.days = ds_convert(Wire.read());
    result.dates = ds_convert(Wire.read());
    result.months = ds_convert(Wire.read());
    result.year = ds_convert(Wire.read());
  }
  return result;
}
sht_data SHT25(){
  sht_data result;
  Wire.beginTransmission(0x40); 
  Wire.write(0xE3); 
  Wire.endTransmission();
  delay(100);
  Wire.requestFrom(0x40, 2); 
  if (Wire.available() >= 2) { 
    uint16_t rawTemperature = Wire.read() << 8 | Wire.read();
    result.temperature = -46.85 + 175.72 * (float)rawTemperature / 65536.0;
  }
  Wire.beginTransmission(0x40); 
  Wire.write(0xE5); 
  Wire.endTransmission();
  delay(100);
  Wire.requestFrom(0x40, 2); 
  if (Wire.available() >= 2) { 
    uint16_t rawhumidty = Wire.read() << 8 | Wire.read();
    result.humidity = -6 + 125 * (float)rawhumidty / 65536.0;
  }
  return result;
}
int ph_value (int analogvalue){
  float voltage = analogvalue / 1024. * 5;
  int ph=(-5.6548*voltage)+15.509;
  return ph;
}
float ldr_lux(int analogvalue){
  float voltage = analogvalue / 1024. * 5;
  
  const float GAMMA =0.7;
  const float RL10 = 50;
  float resistance = 2000  * voltage / (1 - voltage / 5);
  float lux = pow(RL10 * 1e3 * pow(10, GAMMA) / resistance, (1 / GAMMA));

  return lux;
}
float ntc_calc(int analogvalue){
  const float BETA = 3950; 
  float celsius = 1 / (log(1 / (1023. / analogvalue - 1)) / BETA + 1.0 / 298.15) - 273.15;
  return celsius;
}
void plus_func(){
  lcd_select=HIGH;
  lcd_selection++;
  if (lcd_selection==7) {
    lcd_selection=1;
  }
}
void minus_func(){
  lcd_select=HIGH;
  lcd_selection--;
  if (lcd_selection==0) {
    lcd_selection=5;
  }
}
int LM75AD(){
  int temp;
  Wire.beginTransmission(0x48);
  Wire.write(0); 
  Wire.endTransmission();
  Wire.requestFrom(0x48, 2);

  if (Wire.available() == 2) {
    byte msb = Wire.read();
    byte lsb = Wire.read();
    
    temp = (msb << 8) | lsb;
    temp >>= 5; 
  }
  return temp * 0.125; 

}
int calc_hcsr04(int echo,int trig){
  digitalWrite(trig, LOW);
  delayMicroseconds(2);
  digitalWrite(trig, HIGH);
  delayMicroseconds(10);
  digitalWrite(trig, LOW);

  int distance=pulseIn(echo, HIGH)*0.034/2;

  return ceil(distance) ;

}
void date_time(){
  time value=ds_1307();
  CL();
  lcd.setCursor(0, 0);
  lcd.print("Time:");
  lcd_write_time(value.hours);
  lcd.print(":");
  lcd_write_time(value.minutes);
  lcd.print(":");
  lcd_write_time(value.seconds);
  lcd.setCursor(0, 1);
  lcd.print("Date:");
  lcd_write_time(value.dates);
  lcd.print(":");
  lcd_write_time(value.months);
  lcd.print(":");
  lcd_write_time(value.year);
}
void temperature(int temp,int hum){
  CL();
  lcd.setCursor(0, 0);
  lcd.print("TEMP: ");
  lcd.print(temp);
  lcd.print(Csymbol);
  lcd.print("C");
  lcd.setCursor(0, 1);
  lcd.print("HUMIDITY: ");
  lcd.print(hum);
  lcd.print("%RH");
  
}
void light(long light){
  CL();
  lcd.setCursor(0, 0);
  lcd.print("LIGHT: ");
  lcd.print(light);
  lcd.print("lux");
   lcd.setCursor(0, 1);
  lcd.print("LUMEN: ");
  lcd.print(light*0.0135);
  lcd.print("lm");
  
}
void PH_EC(int ph,int ec,int temp){
  CL();
  lcd.setCursor(0, 0);
  lcd.print("PH : ");
  lcd.print(ph);
  lcd.setCursor(0, 1);
  lcd.print("EC : ");
  lcd.print(ec);
  lcd.print("  ");
  lcd.print(temp);
  lcd.print(Csymbol);
  lcd.print("C");

}
void water_level(int lt,int size){
  CL();
  lcd.setCursor(0, 0);
  lcd.print("WATER LEV: ");
  lcd.print(lt);
  lcd.print(" Lt");
  lcd.setCursor(0, 1);
  lcd.print("WATER Max: ");
  lcd.print(size);
  lcd.print(" Lt");

}
void wind(String direct,int speed){
  CL();
  lcd.setCursor(0, 0);
  lcd.print("  ");
  lcd.print(direct);
  lcd.setCursor(0, 1);
  lcd.print("WIND S: ");
  lcd.print(speed);
  lcd.print(" km/h");
}
void lcd_clear(int row,int begin,int last){
  for (int i=begin; i<=last;i++) {
    lcd.setCursor(i, row);
    lcd.print(" ");
  }
}
void lcd_write_time(int time){
  lcd.print(time/10);
  lcd.print(time%10);
}
void CL(){
  if (lcd_select) {
    lcd.clear();
    lcd_select=LOW;
  }
}
int Water_tank_calc(int SSvalue,int litr,int height){
  return float(litr)-(float(litr)/float(height)*(SSvalue-3));
}


