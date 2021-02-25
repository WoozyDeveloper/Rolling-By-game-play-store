using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    Vector3 myRightMovement, myLeftMovement;

    public Rigidbody myCamera;

    public GameObject[] myPlatform = new GameObject[3];

    public float myCameraSpeed;
    public Text scoreText;
    static public int points, highscore;
    private Vector3 initialPosition1 = new Vector3(0f, 45f, -10f);
    private Vector3 initialPosition2 = new Vector3(10f, 45f, -10f);
    private Vector3 initialPosition3 = new Vector3(20f, 45f, -30f);
    private Vector3 initialPosition4 = new Vector3(-15f, 45f, -40f);
    private Vector3 initialPosition5 = new Vector3(20f, 45f, -40f);
    private Vector3 initialPosition6 = new Vector3(0f, 45f, -45f);
    private Vector3 initialPosition7 = new Vector3(-20f, 45f, -50f);
    private Vector3 initialPosition8 = new Vector3(10f, 45f, -55f);
    private Vector3 initialPosition9 = new Vector3(-15f, 45f, -50f);
    private Vector3 initialPosition10 = new Vector3(15f, 45f, -55f);


    private const float spawnRateZ = 60.8f, spawnRateY = 45f;
    private float zDistance = 3f;

    private int noTouch, deletedPlatforms, platformsUsed;
    private bool pandaDirection = false;
    private float xValue;
    private float zValue;

    //TODO add more cubes (10)
    public GameObject[] myEnemy = new GameObject[10];

    Rigidbody myPanda;
    // Start is called before the first frame update
    void Start()
    {
        #region Begin terrain

        Instantiate(myEnemy[0], initialPosition1, Quaternion.Euler(0f, 0f, 0f));
        Instantiate(myEnemy[1], initialPosition2, Quaternion.Euler(0f, 0f, 0f));
        Instantiate(myEnemy[2], initialPosition3, Quaternion.Euler(0f, 0f, 0f));
        Instantiate(myEnemy[3], initialPosition4, Quaternion.Euler(0f, 0f, 0f));
        Instantiate(myEnemy[4], initialPosition5, Quaternion.Euler(0f, 0f, 0f));
        Instantiate(myEnemy[5], initialPosition6, Quaternion.Euler(0f, 0f, 0f));
        Instantiate(myEnemy[6], initialPosition7, Quaternion.Euler(0f, 0f, 0f));
        Instantiate(myEnemy[7], initialPosition8, Quaternion.Euler(0f, 0f, 0f));
        Instantiate(myEnemy[8], initialPosition9, Quaternion.Euler(0f, 0f, 0f));
        Instantiate(myEnemy[9], initialPosition10, Quaternion.Euler(0f, 0f, 0f));

        Instantiate(myPlatform[0], new Vector3(0f, 0f, zDistance), Quaternion.Euler(0f, 0f, 0f));
        zDistance -= spawnRateZ;

        Instantiate(myPlatform[1], new Vector3(0f, 0f, zDistance), Quaternion.Euler(0f, 0f, 0f));
        zDistance -= spawnRateZ;

        Instantiate(myPlatform[2], new Vector3(0f, 0f, zDistance), Quaternion.Euler(0f, 0f, 0f));
        zDistance -= spawnRateZ;
        #endregion

        #region Initialisations
        noTouch = deletedPlatforms = 0;
        platformsUsed = 1;
        myPanda = GetComponent<Rigidbody>();
        deletedPlatforms = 0;
        myRightMovement = myLeftMovement = Vector3.zero;

        highscore = PlayerPrefs.GetInt("player_score", highscore);
        points = 0;
        pandaDirection = false;
        xValue = 16f;
        zValue = -16f;
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        points = (int)Mathf.Abs(Mathf.Floor(getOneAxisPosition('z')));
        PlayerPrefs.SetInt("player_score", points);

        scoreText.text = "Score: " + PlayerPrefs.GetInt("player_score").ToString();

        #region Extra Death Condition
        if (getOneAxisPosition('y') < 0f)
            Death();
        #endregion

        onScreenTap();
        rollingSpliffs(myCamera, myCameraSpeed);
    }

    #region Platform/Enemy Spawn

    private void enemySpawn()
    {
        int xRand = Random.Range(-45, 45);
        int zRand = Random.Range(-45, 45);


        while (Mathf.Abs(zRand - xRand) > 5)
            zRand = Random.Range(-45, 45);
        Instantiate(myEnemy[0], new Vector3(xRand, 45f, zRand), Quaternion.Euler(0f, 0f, 0f));
        Instantiate(myEnemy[1], new Vector3(xRand, 45f, zRand), Quaternion.Euler(0f, 0f, 0f));
        Instantiate(myEnemy[2], new Vector3(xRand, 45f, zRand), Quaternion.Euler(0f, 0f, 0f));
        Instantiate(myEnemy[3], new Vector3(xRand, 45f, zRand), Quaternion.Euler(0f, 0f, 0f));
        Instantiate(myEnemy[4], new Vector3(xRand, 45f, zRand), Quaternion.Euler(0f, 0f, 0f));
        Instantiate(myEnemy[5], new Vector3(xRand, 45f, zRand), Quaternion.Euler(0f, 0f, 0f));
        Instantiate(myEnemy[6], new Vector3(xRand, 45f, zRand), Quaternion.Euler(0f, 0f, 0f));
        Instantiate(myEnemy[7], new Vector3(xRand, 45f, zRand), Quaternion.Euler(0f, 0f, 0f));
        Instantiate(myEnemy[8], new Vector3(xRand, 45f, zRand), Quaternion.Euler(0f, 0f, 0f));
        Instantiate(myEnemy[9], new Vector3(xRand, 45f, zRand), Quaternion.Euler(0f, 0f, 0f));
    }

    /**private void deleteOldLand()
    {
        for (int i = 0; i <= 9; ++i)
        {
            Destroy(myEnemy[i]);
            Destroy(myPlatform[i]);
        }
    }**/

    private void spawnLand()
    {
        if (getOneAxisPosition('z') < -50f * platformsUsed && getOneAxisPosition('z') > (-50f * platformsUsed) - 30f) 
        {
            //deleteOldLand();

            Instantiate(myPlatform[deletedPlatforms], new Vector3(0f, 0f, -100f * (platformsUsed + 1)), Quaternion.Euler(0f, 0f, 0f));
            platformsUsed++;
            deletedPlatforms++;
            if (deletedPlatforms == 3)
                deletedPlatforms -= 3;
            int xRand = Random.Range(-23, 23);
            int yRand = Random.Range(40, 60);
            int zRand = Random.Range(-45, 45);
            Instantiate(myEnemy[0], new Vector3(xRand, yRand, -50f * (platformsUsed + 1) - zRand), Quaternion.Euler(0f, 0f, 0f));

            xRand = Random.Range(-23, 23);
            yRand = Random.Range(40, 60);
            zRand = Random.Range(-45, 45);
            Instantiate(myEnemy[1], new Vector3(xRand, yRand, -50f * (platformsUsed + 1) - zRand), Quaternion.Euler(0f, 0f, 0f));

            xRand = Random.Range(-23, 23);
            yRand = Random.Range(40, 60);
            zRand = Random.Range(-45, 45);
            Instantiate(myEnemy[2], new Vector3(xRand, yRand, -50f * (platformsUsed + 1) - zRand), Quaternion.Euler(0f, 0f, 0f));

            xRand = Random.Range(-23, 23);
            yRand = Random.Range(40, 60);
            zRand = Random.Range(-45, 45);
            Instantiate(myEnemy[3], new Vector3(xRand, yRand, -50f * (platformsUsed + 1) - zRand), Quaternion.Euler(0f, 0f, 0f));

            xRand = Random.Range(-23, 23);
            yRand = Random.Range(40, 60);
            zRand = Random.Range(-45, 45);
            Instantiate(myEnemy[4], new Vector3(xRand, yRand, -50f * (platformsUsed + 1) - zRand), Quaternion.Euler(0f, 0f, 0f));

            xRand = Random.Range(-23, 23);
            yRand = Random.Range(40, 60);
            zRand = Random.Range(-45, 45);
            Instantiate(myEnemy[5], new Vector3(xRand, yRand, -50f * (platformsUsed + 1) - zRand), Quaternion.Euler(0f, 0f, 0f));

            xRand = Random.Range(-23, 23);
            yRand = Random.Range(40, 60);
            zRand = Random.Range(-45, 45);
            Instantiate(myEnemy[6], new Vector3(xRand, yRand, -50f * (platformsUsed + 1) - zRand), Quaternion.Euler(0f, 0f, 0f));

            xRand = Random.Range(-23, 23);
            yRand = Random.Range(40, 60);
            zRand = Random.Range(-45, 45);
            Instantiate(myEnemy[7], new Vector3(xRand, yRand, -50f * (platformsUsed + 1) - zRand), Quaternion.Euler(0f, 0f, 0f));

            xRand = Random.Range(-23, 23);
            yRand = Random.Range(40, 60);
            zRand = Random.Range(-45, 45);
            Instantiate(myEnemy[8], new Vector3(xRand, yRand, -50f * (platformsUsed + 1) - zRand), Quaternion.Euler(0f, 0f, 0f));

            xRand = Random.Range(-23, 23);
            yRand = Random.Range(40, 60);
            zRand = Random.Range(-45, 45);
            Instantiate(myEnemy[9], new Vector3(xRand, yRand, -50f * (platformsUsed + 1) - zRand), Quaternion.Euler(0f, 0f, 0f));
          
        }
    }

    #endregion

    #region Death functions

    public void Death()
    {
        if (points > highscore)
            highscore = points;

        PlayerPrefs.SetInt("player_highscore", highscore);
        PlayerPrefs.SetInt("player_score", points);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            Death();
        }
    }

    #endregion

    #region Getters
    private float getOneAxisPosition(char axis)
    {
        if (axis == 'x' || axis == 'X')
            return myPanda.transform.position.x;

        else if (axis == 'y' || axis == 'Y')
            return myPanda.transform.position.y;

        else if (axis == 'z' || axis == 'Z')
            return myPanda.transform.position.z;

        return 0;
    }

    private Vector3 getPosition()
    {
        float xPos = getOneAxisPosition('x');
        float yPos = getOneAxisPosition('y');
        float zPos = getOneAxisPosition('z');

        Vector3 myLocation = new Vector3(xPos, yPos, zPos);
        return myLocation;
    }
    #endregion

    #region Control
    private void goToTheRight()
    {
        spawnLand();
        while (pandaDirection && pandaDirection == true)
        {
            Vector3 myRightMovement = new Vector3(xValue, getOneAxisPosition('y'), zValue);
            myPanda.velocity = myRightMovement;
            if (Input.GetMouseButtonDown(0))
                pandaDirection = false;
        }
    }

    private void goToTheLeft()
    {
        spawnLand();
        while (!pandaDirection && pandaDirection == false)
        {
            Vector3 myRightMovement = new Vector3(-xValue, getOneAxisPosition('y'), zValue);
            myPanda.velocity = myRightMovement;
            if (Input.GetMouseButtonDown(0))
                pandaDirection = true;
        }
    }
    #endregion

    #region Rolling The Ball

    private void rollingSpliffs(Rigidbody bowlingRotationBall, float cameraSpeed)
    {
        bowlingRotationBall.velocity = new Vector3(0f,0f,cameraSpeed);
    }

    #endregion

    #region onTap
    private void onScreenTap()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (noTouch % 2 == 1)
                goToTheRight();
            else
                goToTheLeft();
            noTouch++;
        }

        switch (pandaDirection)
        {
            //right
            case true:
                Vector3 myRightMovement = new Vector3(xValue, -1.5f, zValue);
                myPanda.velocity = myRightMovement;
                break;

            //left
            default:
                Vector3 myLeftMovement = new Vector3(-xValue, -1.5f, zValue);
                myPanda.velocity = myLeftMovement;
                break;
        }
    }
    #endregion
}
