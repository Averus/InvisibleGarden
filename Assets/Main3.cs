using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class Main3 : MonoBehaviour
{

    // Ok the map draws upside down, because in Unity world increasing co ordinates moves you right and up, wheras in Isle we calculate your Y coord from the top.
    //EDIT lol when is this even from? Who are you?

    

    Text message;

    public bool GPSenabled = true; // for testing purposes set to false and change the players GPS decimal digits manually
    public bool averageCompass = true;// toggles compass averaging (maps to the button)
    public bool averagePosition = true;// toggles position averaging (maps to the button)
    public bool positionLerping = true;// toggles position lerping for test purposes
    public float TESTnumber = 0;


    double oneMeterOfLongitudeX = 0.00001447;
    double oneMeterOfLatitudeY = 0.000008995;

    public int mapWidthInMeters = 50; // later get this info by reading the map csv?
    public int mapHeightInMeters = 50;


    // I called these two LonX and LatY because I kept forgetting which was which. Lon is the X axis and Lat is the Y axis.

    public double mapTopLeftLonX = -0.001880; //Eastdean: 0.210537; //Stratford: -0.001880; // This is a known point to measure the player distance from, they will act as the 0,0 coords of the map
    public double mapTopLeftLatY = 51.547810; //Eastdean: 50.765503; //Stratford: 51.547810;

    public double playerLongitudeX = 0; // -0.0008543f; //players lon
    public double playerLatitudeY = 0; // 51.54728f; //players lat

    public double averagePlayerLongitudeX = 0;
    public double averagePlayerLatitudeY = 0;


    public Quaternion currentAngle; //added 01/03/17
    public Quaternion targetAngle; //added 01/03/17

    public float compass;
    public float averageCompassValue;

    public float playerX = 0;
    public float playerY = 0;

    public List<double> lastTenLatitude = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public List<double> lastTenLongitude = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public List<float> lastTenCompass = new List<float>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};


    InputField LerpSpeedInput;


    IEnumerator Start()
 {

       LerpSpeedInput = GameObject.Find("LerpSpeed").GetComponent<InputField>();


        currentAngle = new Quaternion();

        Screen.sleepTimeout = SleepTimeout.NeverSleep; //Ensure that the phone does not lock and stop the game.
     message = GameObject.Find("message").GetComponent<Text>();
     message.text = ("Starting...");

     message.text = ("StartLocationService...");

     // First, check if user has location service enabled
     if (!Input.location.isEnabledByUser)
     {
         message.text = ("Location Service not enabled (is your GPS enabled?)");
         yield break;
     }

     


     // Start service before querying location
     Input.location.Start(1, 1); //These numbers indicate the desired accuracy and distance until next update in meteres.
     Input.compass.enabled = true;

     message.text = ("Location Service initializing...");


     // Wait until service initializes
     int maxWait = 20;
     while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
     {
         yield return new WaitForSeconds(1);
         maxWait--;
     }

     // Service didn't initialize in 20 seconds
     if (maxWait < 1)
     {
         message.text = ("Timed out");
         yield break;
     }

     // Connection has failed
     if (Input.location.status == LocationServiceStatus.Failed)
     {
         message.text = ("Unable to determine device location");
         yield break;
     }

        
    
     message.text = ("Location Service initialized!");

 }

   public void ToggleCompassAveraging()
    {
        averageCompass = !averageCompass;
    }

    public void TogglePositionAveraging()
    {
        averagePosition = !averagePosition;
    }

    public void TogglePositionLerping()
    {
        positionLerping = !positionLerping;
    }

    void UpdatePositionData() //Here I actually stop averageCompas, compass etc from updating at all when not being used. This was just to get the feedback from the text to see if it was actrually working. 
                              // The if statements can be removed from this method and all values can always update as soon as I'm sure they work as intended.
    {
     //Update the players position via GPS
        if (GPSenabled && !averagePosition)
        {
         playerLatitudeY = Input.location.lastData.latitude;
         playerLongitudeX = Input.location.lastData.longitude;
         }

        if (GPSenabled && averagePosition)
        {
            lastTenLatitude.RemoveAt(0);
            lastTenLatitude.Add(Input.location.lastData.latitude);

            averagePlayerLatitudeY = AverageDoubleValues(lastTenLatitude);



            lastTenLongitude.RemoveAt(0);
            lastTenLongitude.Add(Input.location.lastData.longitude);

            averagePlayerLongitudeX = AverageDoubleValues(lastTenLongitude);
        }


    }

    void UpdateHeading()
    {


    
        //update the players heading from the device compass

        compass = Input.compass.trueHeading; // We take this reading to display the value on the screen for testing purposes.

        if (!averageCompass) //This code constantly Slerps the players heading towards the most recent compass reading.
        {
            currentAngle = gameObject.transform.rotation;
            targetAngle = Quaternion.Euler(gameObject.transform.rotation.x, Input.compass.trueHeading, transform.rotation.z);


            gameObject.transform.rotation = Quaternion.Slerp(currentAngle, targetAngle, TESTnumber); //changed this for a test, might ned to be Time.deltatime * this
        }

        if (averageCompass)
        {
            //Remove the oldest value from the list and add the latest value from the devices compass
            lastTenCompass.RemoveAt(0);
            lastTenCompass.Add(Input.compass.trueHeading);

            //Average the last X compass values
            averageCompassValue = AverageFloatValues(lastTenCompass);

            currentAngle = gameObject.transform.rotation;

            targetAngle = Quaternion.Euler(transform.rotation.x, averageCompassValue, transform.rotation.z);


            transform.rotation = Quaternion.Slerp(currentAngle, targetAngle, Time.deltaTime * TESTnumber);

        }


        
    }

    public void UpdateTESTnumber()
    {
       float.TryParse(LerpSpeedInput.text, out TESTnumber);
        Debug.Log("TESTnumber is " + TESTnumber);
    }

    float AverageFloatValues(List<float> list)
    {

        float sum = 0;
        float average = 0;


        for (int i = 0; i < list.Count; i++)
        {       
            sum += list[i];
        }

        average = (sum / list.Count);
        return average;
    }

    double AverageDoubleValues(List<double> list)
    {

        double sum = 0;
        double average = 0;


        for (int i = 0; i < list.Count; i++)
        {
            sum += list[i];
        }

        average = (sum / list.Count);
        return average;
    }

    void UpdateGridPosition()
 {


        if (!averagePosition && !positionLerping)
        {
            playerX = (float)((playerLongitudeX - mapTopLeftLonX) / oneMeterOfLongitudeX);

            //This deal is to try and fix the fact that it draws the map inverting the Y axis
            float i = (float)((mapTopLeftLatY - playerLatitudeY) / oneMeterOfLatitudeY);
            playerY = mapHeightInMeters - i;


            gameObject.transform.position = new Vector3(playerX, 1.5f, playerY);

      
        }


        if (!averagePosition && positionLerping)
        {
            playerX = (float)((playerLongitudeX - mapTopLeftLonX) / oneMeterOfLongitudeX);

            //This deal is to try and fix the fact that it draws the map inverting the Y axis
            float i = (float)((mapTopLeftLatY - playerLatitudeY) / oneMeterOfLatitudeY);
            playerY = mapHeightInMeters - i;

            Vector3 newPosition = new Vector3(playerX, 1.5f, playerY);

            gameObject.transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * TESTnumber);
        }




        if (averagePosition && !positionLerping)
        {
            playerX = (float)((averagePlayerLongitudeX - mapTopLeftLonX) / oneMeterOfLongitudeX);

            //This deal is to try and fix the fact that it draws the map inverting the Y axis
            float i = (float)((mapTopLeftLatY - averagePlayerLatitudeY) / oneMeterOfLatitudeY);
            playerY = mapHeightInMeters - i;


            gameObject.transform.position = new Vector3(playerX, 1.5f, playerY);
        }

        if (averagePosition && positionLerping)
        {
            playerX = (float)((averagePlayerLongitudeX - mapTopLeftLonX) / oneMeterOfLongitudeX);

            //This deal is to try and fix the fact that it draws the map inverting the Y axis
            float i = (float)((mapTopLeftLatY - averagePlayerLatitudeY) / oneMeterOfLatitudeY);
            playerY = mapHeightInMeters - i;

            Vector3 newPosition = new Vector3(playerX, 1.5f, playerY);

            gameObject.transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * TESTnumber);
        }


        /*     //Updating the actual players heading is now done in UpdateHeading() - this code was old anyway.

                if (!averageCompass)
                {
                    gameObject.transform.rotation = Quaternion.Euler(0, compass, 0);
                }
                if (averageCompass)
                {
                    gameObject.transform.rotation = Quaternion.Euler(0, averageCompassValue, 0);
                }

            */

    }

 int[] GetMapDimensions(string mapPath)
 {
     int[] widthAndHeight = { 0, 0 };

     StreamReader reader = new StreamReader(mapPath);

     if (reader.ReadLine() == null)
     {
         return widthAndHeight;
     }

     string s = reader.ReadLine(); //get the first line of the map table

     widthAndHeight[1] = 2; //count two 'height' for the two times readline has been invoked thus far

     widthAndHeight[0] = s.Split(',').Length; //count all the cells for the width

     using (reader)
     {
         while (reader.ReadLine() != null)
         {
             widthAndHeight[1]++; //count the remaining height
         }
     }
     return widthAndHeight;
 }

 void UpdateText()
 {
        message.text = "Lat: " + playerLatitudeY + "\n" +
                        "Lon: " + playerLongitudeX + "\n" +
                        "Av Lat: " + averagePlayerLatitudeY + "\n" +
                        "Av Lon: " + averagePlayerLongitudeX + "\n" +
                        "Altitude: " + Input.location.lastData.altitude + "\n" +
                        "Horizontal Accuracy: " + Input.location.lastData.horizontalAccuracy + "\n" +
                        "Vertical Accuracy: " + Input.location.lastData.verticalAccuracy + "\n" +
                        "Timestamp: " + Input.location.lastData.timestamp + "\n" +
                        "X: " + playerX + "\n" +
                        "Y: " + playerY + "\n" +
                        "Heading: " + compass + "\n" +
                        "Average Heading: " + averageCompassValue;

 }

 public void CenterMap()
 {
     mapTopLeftLonX = playerLongitudeX - (oneMeterOfLongitudeX * (mapWidthInMeters / 2));
     mapTopLeftLatY = playerLatitudeY + (oneMeterOfLatitudeY * (mapHeightInMeters / 2));
 }

 void Update()
 {
     UpdatePositionData();

    // UpdateHeading(); //testing the replacement code GyroController thingamy - Tim

     UpdateGridPosition();

     UpdateText();

        if (!GPSenabled)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.localPosition += transform.forward * 8 * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.localPosition -= transform.forward * 8 * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(0,20 * Time.deltaTime,0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(0, -20 * Time.deltaTime, 0); ;
            }
        }


 }

 
}
