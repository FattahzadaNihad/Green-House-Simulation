# Green-House-Simulation
#Nihad Fattahzada
#Please make the necessary feedback


This is a greenhouse simulation project using Arduino uno, Proteus Design and Windows forums (C#)

The main goal of this project is to create an autonomous soilless greenhouse for a small (about 25 square meters) area that can be controlled by a desktop application (because there is no real simulation, some components are selected and configured as approximate)

Arduino Uno
It is used to read the values ​​from the sensors, write them on the LCD screen, send them to the computer via the UART connection, and control the connected devices (motors, heaters, humidifiers).
A time configuration was created with the DS1307. The temperature and humidity of the greenhouse were set, as well as the wind speed and direction.

Windows forums
It is used to view these values ​​or add new values ​​in several windows by analyzing the data coming from the serial connection
It is divided into 4 main parts: Home, Port Config, Controls and Sensor data (within data)
In the Home window, we see whether the connected devices are active or passive.
In the Port Config window, we connect to the necessary port and check whether the information arrives.
We print the values ​​from the sensors in the sensor window
In the Controls window, we define and control the conditions under which the connected devices should work.

The whole prototype project was checked on Proteus Design Suite 8.13, there are some shortcomings because it was not tested with a real process (device connections are almost impossible in proteus, so I replaced them with simple LEDs).

A tutorial is provided in the uploaded video
