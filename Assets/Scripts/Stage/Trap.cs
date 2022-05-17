using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The "trap" class detects for player movement a certain distance above a certain platform. If it detects a player, it creates
//walls around the perimeter that prevent the player from exiting the arena until all of the enemies are cleared, at which point
//the walls get destroyed. 

public class Trap : MonoBehaviour
{
    //Variables for the wall, the layermask for the wall objects, as well as how high you want the player/enemy detection to be off the ground. 
    [SerializeField] private GameObject wall;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private int heightAdjustment; 

    //A list of all entities in the bounds of the arena. 
    private List<Collider> currentEntities; 

    //Collider of the attached object. 
    private MeshCollider col; 


    //Bools to figure out whether or not the trap is active, or has been cleared. 
    private bool isActive = false; 
    private bool isCleared = false;

    private void Start()
    {
        col = gameObject.GetComponent<MeshCollider>(); 
    }

    private void FixedUpdate()
    {
        DetectCollisions(); 
    }

    //Spawns the walls along the perimiter of a cube. Automatically adjusts with the size and rotation. 
    private void SpawnWalls()
    {
        #region Left Wall
        GameObject leftWall = Instantiate(wall);
        ParticleSystem[] particleSystems  = leftWall.GetComponentsInChildren<ParticleSystem>();
        BoxCollider col = leftWall.GetComponent<BoxCollider>(); 

        leftWall.transform.parent = transform;
        leftWall.transform.localPosition = new Vector3(-0.5f, 0, 0);
        leftWall.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        leftWall.transform.localScale = new Vector3(1, 1, 1);

            
        foreach (ParticleSystem ps in particleSystems)
        {
            var psShape = ps.shape;
            psShape.scale = new Vector3(1, transform.localScale.z, 1);
        }
        
        col.size = new Vector3(col.size.x, col.size.y, transform.localScale.z / 2);


        #endregion

        #region Right Wall        
        GameObject rightWall = Instantiate(wall);
        particleSystems = rightWall.GetComponentsInChildren<ParticleSystem>();
        col = rightWall.GetComponent<BoxCollider>();

        rightWall.transform.parent = transform;
        rightWall.transform.localPosition = new Vector3(0.5f, 0, 0);
        rightWall.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        rightWall.transform.localScale = new Vector3(1, 1, 1);


        foreach (ParticleSystem ps in particleSystems)
        {
            var psShape = ps.shape;
            psShape.scale = new Vector3(1, transform.localScale.z, 1);
        }

        col.size = new Vector3(col.size.x, col.size.y, transform.localScale.z / 2);
        #endregion

        #region Top Wall
        GameObject topWall = Instantiate(wall);
        particleSystems = topWall.GetComponentsInChildren<ParticleSystem>();
        col = topWall.GetComponent<BoxCollider>();

        topWall.transform.parent = transform;
        topWall.transform.localPosition = new Vector3(0, 0, 0.5f);
        topWall.transform.localRotation = Quaternion.Euler(-90, 0, 90);
        topWall.transform.localScale = new Vector3(1, 1, 1);


        foreach (ParticleSystem ps in particleSystems)
        {
            var psShape = ps.shape;
            psShape.scale = new Vector3(1, transform.localScale.x, 1);
        }

        col.size = new Vector3(col.size.x, col.size.y, transform.localScale.x / 2);
        #endregion

        #region Bottom Wall
        GameObject btmWall = Instantiate(wall);
        particleSystems = btmWall.GetComponentsInChildren<ParticleSystem>();
        col = btmWall.GetComponent<BoxCollider>();

        btmWall.transform.parent = transform;
        btmWall.transform.localPosition = new Vector3(0, 0, -0.5f);
        btmWall.transform.localRotation = Quaternion.Euler(-90, 0, -90);
        btmWall.transform.localScale = new Vector3(1, 1, 1);


        foreach (ParticleSystem ps in particleSystems)
        {
            var psShape = ps.shape;
            psShape.scale = new Vector3(1, transform.localScale.x, 1);
        }

        col.size = new Vector3(col.size.x, col.size.y, transform.localScale.x / 2);
        #endregion
    }

    //Destroys all walls.
    private void DestroyWalls()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Wall"))
            {
                Destroy(child.gameObject);
            }
        }
    }

    //Function that detects all of the entities (via colliders) in the zone. 
    private void DetectCollisions()
    {
        //Don't want to keep checking all of this stuff if the arena has already been cleared. 
        if (!isCleared)
        {
            currentEntities = new List<Collider>(Physics.OverlapBox(col.bounds.center + col.transform.up, new Vector3(transform.localScale.x, transform.localScale.y * heightAdjustment, transform.localScale.z), transform.rotation));

            int i = 0; 
            while (i < currentEntities.Count)
            {
                //If the player is in bounds, spawn the walls and mark as active.
                if (currentEntities[i].gameObject.CompareTag("Player"))
                {
                    if (isActive == false)
                    {
                        SpawnWalls();
                        isActive = true;
                    }
                }

                //If it's active, check until all of the enemies are cleared. When they are, destroy the walls.
                if (isActive)
                {
                    if (CheckEnemiesCleared(currentEntities))
                    {
                        DestroyWalls();
                        isActive = false; 
                        isCleared = true; 
                    }
                }

                i++;
            } 
        }
    }

    //Bool that checks and answers the question of whether or not there are any enemies within the zone of a trap. 
    private bool CheckEnemiesCleared(List<Collider> currentEntities)
    {
        int count = 0;
        foreach (Collider col in currentEntities)
        {
            if (col.gameObject.CompareTag("Enemy"))
            {
                count++; 
            }
        }

        if (count > 0)
        {
            Debug.Log(count); 
            return false;
        }

        else
        {
            return true;
        }
    }
}
