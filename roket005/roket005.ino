/*
  HMC5883L Triple Axis Digital Compass. Compass Example.
  Read more: http://www.jarzebski.pl/arduino/czujniki-i-sensory/3-osiowy-magnetometr-hmc5883l.html
  GIT: https://github.com/jarzebski/Arduino-HMC5883L
  Web: http://www.jarzebski.pl
  (c) 2014 by Korneliusz Jarzebski
*/
#include <Wire.h>
#include <HMC5883L.h>
#include <GY80.h>

GY80_scaled valGY;

GY80 GYsensor = GY80(); //create GY80 instance
HMC5883L compass;

uint8_t i, counter;
uint8_t packet[40];

struct accel{
  float
    ap, vp, sp, al, vl, sl;
};

struct accel Ax, Ay, Az, Gx, Gy, Gz;

void initcomp (void);
void readcom(void);
void updateGY(void);
void fill(float data);
void debug(void);
void sendp(void);
void Aint(float acc, struct accel * data);

void setup()
{
  Serial.begin(9600);
  initcomp();
  GYsensor.begin();

  Ax.al = Ax.vl = Ax.sl = 0;
  Ay.al = Ay.vl = Ay.sl = 0;
  Az.al = Az.vl = Az.sl = 0;
  delay(100);

}

void loop()
{
  i = 0;
  /* Total packet
   *  Header + data = 4 + 21 = 25
   */
   
  /* Header 
   *  size 
   *    4 x 1 = 4 
  */
  packet[i++] = 13;
  packet[i++] = 0;
  packet[i++] = 0;
  packet[i++] = 5;
  
  /* Data
   *  size 
   *  Gyro : 1 x 3 x 3 = 9
   *  Accelero : 1 x 3 x 3 = 9
   *  comp : 1 x 3 = 3
   *  
   *  Total = 21
   */
  updateGY();
  Aint(valGY.a_x, &Ax);
  Aint(valGY.a_y, &Ay);
  Aint(valGY.a_z, &Az);

  Aint(valGY.g_x, &Gx);
  Aint(valGY.g_y, &Gy);
  Aint(valGY.g_z, &Gz);
  
  fill(Ax.ap);
  fill(Ax.vp);
  fill(Ax.sp);

  fill(Gx.ap);
  fill(Gx.vp);
  fill(Gx.sp);

  fill(readcomp());

  //debug();
  sendp();

}
void Aint(float acc, struct accel * data){
    if (acc <= 0)
      data->ap = acc + 1 >= 0 ? 0 : acc;
    else
      data->ap = acc - 1 <= 0 ? 0 : acc;
      
    data->vp = data->vl + ((0.1/2)*(data->ap + data->al));
    data->sp = data->sl + ((0.1/2)*(data->vp + data->vl));

      if (data->sp < 0){
        data->sp = 0;
        data->vp = 0;
      }
        
    data->al = data->ap;  
    data->vl = data->vp;
    data->sl = data->sp;
}

void sendp(void){
    Serial.write(packet, i);
    Serial.println();
  delay(100);
}

void debug(){
  for(counter = 0; counter < i; counter++){
    Serial.print(packet[counter]);
    Serial.print(" ");
  }
  Serial.print("#Packet: ");
  Serial.println(i);
}
void fill(float data){
  int8_t tmp[3];
  int16_t padd;

  padd = data > 999 ? 999 : data < 0 ? 0 : data;
 // padd = data < 0 ? 0 : data;
  
  tmp[0] = (padd / 100) + 48;
  tmp[1] = ((padd % 100) / 10) + 48;
  tmp[2] = ((padd % 100) % 10) + 48;

  packet[i++] = 0x20;
  packet[i++] = tmp[0];
  packet[i++] = tmp[1];
  packet[i++] = tmp[2];
}

void initcomp(void){
  compass.begin();
    // Set measurement range
  compass.setRange(HMC5883L_RANGE_1_3GA);

  // Set measurement mode
  compass.setMeasurementMode(HMC5883L_CONTINOUS);

  // Set data rate
  compass.setDataRate(HMC5883L_DATARATE_30HZ);

  // Set number of samples averaged
  compass.setSamples(HMC5883L_SAMPLES_8);

  // Set calibration offset. See HMC5883L_calibration.ino
  compass.setOffset(0, 0);
}

float readcomp(void){
  
  Vector norm = compass.readNormalize();

  // Calculate heading
  float heading = atan2(norm.YAxis, norm.XAxis);

  // Set declination angle on your location and fix heading
  // You can find your declination on: http://magnetic-declination.com/
  // (+) Positive or (-) for negative
  // For Bytom / Poland declination angle is 4'26E (positive)
  // Formula: (deg + (min / 60.0)) / (180 / M_PI);
  float declinationAngle = (4.0 + (26.0 / 60.0)) / (180 / M_PI);
  heading += declinationAngle;

  // Correct for heading < 0deg and heading > 360deg
  if (heading < 0)
  {
    heading += 2 * PI;
  }

  if (heading > 2 * PI)
  {
    heading -= 2 * PI;
  }

  // Convert to degrees
  return (heading * 180/M_PI);
}

void updateGY(void){
  valGY = GYsensor.read_scaled();
  
}

