
/// ----------------------------------------

/// Author: Grant Smith (40111906 / migiesmith)

/// ----------------------------------------



using System.Collections;

using System.Collections.Generic;

using UnityEngine;



[RequireComponent(typeof(SteamVR_LoadLevel))]

public class MenuShipController : MonoBehaviour
{



    public float snapDist = 0.01f;



    public GameObject uiObject = null;



    public Material lineMat = null;



    protected string shipName = "";

    public TextMesh levelLabel;

    public TextMesh resourceValLabel;



    public float spawnWarningThreshold = 0.7f;

    public float minSpawnRate = 0.3f;

    public float maxSpawnRate = 0.8f;

    public TextMesh spawnWarning;



    [Header("Generation Parameters")]

    public DungeonParams dgnParams;



    private Vector3 initialPos;

    private Quaternion initialRotation;



    private LineRenderer lineRend;



    private bool returnToStart = false;



    private SteamVR_LoadLevel levelLoader;



    private NewtonVR.NVRPlayer player;





    // Use this for initialization

    void Start()
    {

        this.initialPos = this.transform.position;

        this.initialRotation = this.transform.rotation;



        this.lineRend = this.GetComponent<LineRenderer>();

        if (this.lineRend != null)
        {

            this.lineRend.material = lineMat;

        }



        this.levelLoader = this.GetComponent<SteamVR_LoadLevel>();

        player = FindObjectOfType<NewtonVR.NVRPlayer>();



        randomiseDungeonParams();

        generateName();

        updateResourceLabel();

        updateWarningLabel();

    }



    protected void randomiseDungeonParams()

    {

        // Randomise levels slightly

        for (int i = 0; i < dgnParams.items.Count; ++i)

        {

            dgnParams.items[i].weight *= Random.Range(0.0f, 10.0f);

        }



        // Randomise spawn rate

        dgnParams.enemySpawnRate = Random.Range(minSpawnRate, maxSpawnRate);

    }



    protected void generateName()

    {

        List<string> part0 = new List<string> { "Zek", "Erigo", "Xia", "Rha", "Nilex", "Torka", "Ich", "Naro", "Plek" };

        List<string> part1 = new List<string> { "Orr", "Hishk", "Bal", "Knuk", "Bien", "Vennor", "Xissa" };

        List<string> part2a = new List<string> { "I", "X", "V" };

        List<string> part2b = new List<string> { "Alpha", "Beta", "Delta" };



        shipName = part0[Random.Range(0, part0.Count)] + " ";



        if (Random.Range(0.0f, 1.0f) < 0.5f)

            shipName += part1[Random.Range(0, part1.Count)] + " ";





        if (Random.Range(0.0f, 1.0f) < 0.75f)

        {

            for (int i = 0; i < Random.Range(1, 3); ++i)

            {

                shipName += part2a[Random.Range(0, part2a.Count)];

            }

        }
        else
        {

            shipName += part2b[Random.Range(0, part2b.Count)];

        }



        levelLabel.text = shipName;

    }



    protected void updateResourceLabel()

    {

        float organics = 0;

        float metals = 0;

        float fuel = 0;

        float radioactive = 0;



        // Calculate values

        for (int i = 0; i < dgnParams.items.Count; ++i)

        {

            float weight = dgnParams.items[i].weight;

            ShopValues sv = dgnParams.items[i].item.prefab.GetComponent<ShopValues>();

            organics += sv.organics * weight;

            metals += sv.metals * weight;

            fuel += sv.fuel * weight;

            radioactive += sv.radioactive * weight;

        }



        float sum = organics + metals + fuel + radioactive;

        organics /= sum;

        metals /= sum;

        fuel /= sum;

        radioactive /= sum;



        // Update Label

        resourceValLabel.text = " " + Mathf.RoundToInt(organics * 100.0f) + "%\n\n"

                            + " " + Mathf.RoundToInt(metals * 100.0f) + "%\n\n"

                            + " " + Mathf.RoundToInt(fuel * 100.0f) + "%\n\n"

                            + " " + Mathf.RoundToInt(radioactive * 100.0f) + "%\n";

    }



    protected void updateWarningLabel()

    {

        if (dgnParams.enemySpawnRate >= spawnWarningThreshold)

            spawnWarning.gameObject.SetActive(false);

    }



    // Update is called once per frame

    void Update()
    {

        // If this is not being held by the player

        if (this.returnToStart)
        {



            Rigidbody rb = this.GetComponent<Rigidbody>();

            if (rb != null)
            {

                rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);

                rb.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);

            }





            bool locationSnap = false;

            bool rotationSnap = false;



            // Location



            // Move towards the initial position

            this.transform.position = Vector3.Lerp(this.transform.position, this.initialPos, 0.1f);



            // If close to initial position, snap to it

            if (Vector3.Magnitude(this.transform.position - this.initialPos) <= this.snapDist)
            {

                this.transform.position = this.initialPos;

                locationSnap = true;

            }



            // Rotation



            // Rotate towards the initial rotation

            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.initialRotation, 0.1f);



            // If close to initial rotation, snap to it

            if (Quaternion.Angle(this.transform.rotation, this.initialRotation) < 0.1f)
            {

                this.transform.rotation = this.initialRotation;

                rotationSnap = false;

            }



            // If both location and rotation snapped to then we can stop returning

            if (locationSnap && rotationSnap)
            {

                this.returnToStart = false;

            }





        }



        // If we have a line renderer and there is a seperation from initialPos then render a line

        if (this.lineRend != null)
        {

            if (Vector3.Magnitude(this.initialPos - this.transform.position) <= this.snapDist)
            {

                this.lineRend.numPositions = 0;

            }
            else
            {

                this.lineRend.numPositions = 2;

                this.lineRend.SetPositions(new Vector3[] { this.initialPos, this.transform.position });

            }

        }



    }



    public void startLevel()
    {

        Persistence persistence = GameObject.FindObjectOfType<Persistence>();

        if (persistence != null)

        {

            // Set the dungeon parameters

            DungeonArgs dgnArgs = new DungeonArgs();

            dgnArgs.setDgnParams(dgnParams);

            persistence.setSceneArgs(dgnArgs);

        }

        levelLoader.enabled = true;

        DontDestroyOnLoad(player.gameObject);
        if (player.LeftHand.CurrentlyInteracting != null && player.LeftHand.CurrentlyInteracting.transform.root.gameObject != transform.root.gameObject)
            DontDestroyOnLoad(player.LeftHand.CurrentlyInteracting.transform.root.gameObject);
        if (player.RightHand.CurrentlyInteracting != null && player.RightHand.CurrentlyInteracting.gameObject != gameObject)
            DontDestroyOnLoad(player.RightHand.CurrentlyInteracting.transform.root.gameObject);
        levelLoader.Trigger();
    }

    public void OnPickUp()
    {
        this.returnToStart = false;

        if (this.uiObject != null)
        {
            this.uiObject.SetActive(true);
        }

    }

    public void OnDrop()
    {
        this.returnToStart = true;
        if (this.uiObject != null)
        {
            this.uiObject.SetActive(false);
        }
    }

}
