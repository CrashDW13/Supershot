using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

enum EnemyState
{

}
public class Enemy : MonoBehaviour

{
    private Animator animator;
    private float health = 10f;
    private float damage;

    //Variables for hurt flash. Need the original material, as well as all of the MeshRenderers in the child objects. 
    private SkinnedMeshRenderer[] meshRenderers;
    private Material originalMat;
    [SerializeField] private Material hurtMat; 
    [SerializeField] private float flashTimer = 0.5f;

    //Variables for death animation.
    MaterialPropertyBlock propertyBlock; 

    //Variables for AI.
    private Transform target;
    [SerializeField] private float speed = 20f; 
    [SerializeField] private float aggroRadius = 30f;
    [SerializeField] private float attackRadius = 10f; 
    [SerializeField] private float attackTimer = 20f;
    private NavMeshAgent navMesh; 

    private void Start()
    {
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        propertyBlock = new MaterialPropertyBlock(); 
        animator = GetComponent<Animator>(); 
        navMesh = GetComponent<NavMeshAgent>(); 
        originalMat = meshRenderers[0].material; //Doesn't matter which material in the array we choose since they're all the same. 
        if (GameObject.Find("Player"))
        {
            target = GameObject.Find("Player").transform; 
        }
    }

    private void FixedUpdate()
    {
        float dist = Vector3.Distance(transform.position, target.position);

        if (dist <= aggroRadius)
        {
            animator.SetFloat("Run", speed);
            navMesh.speed = speed; 
            transform.LookAt(target);
            Vector3 moveTo = Vector3.MoveTowards(transform.position, target.position, 100f);
            navMesh.destination = moveTo; 
            
            if (dist <= attackRadius)
            {
                animator.SetTrigger("Attack"); 
            }
        }

        else if (dist > aggroRadius)
        {
            animator.SetFloat("Run", -1);
            navMesh.speed = 0; 
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            health--; 

            foreach (SkinnedMeshRenderer meshRenderer in meshRenderers)
            {
                Debug.Log("GOING THROUGH LIST...");
                meshRenderer.material = hurtMat; 
                StartCoroutine(ResetColor(meshRenderer));
            }

            if (health <= 0)
            {
                animator.SetBool("Dead", true);
                for (var i = 0; i < meshRenderers.Length; i++)
                {
                    StartCoroutine(Fade(0, 2.0f, meshRenderers[i]));
                }
            }
        }
    }

    private IEnumerator ResetColor(SkinnedMeshRenderer meshRenderer)
    {
        yield return new WaitForSeconds(flashTimer);
        meshRenderer.material = originalMat; 
        yield return null;
    }

    private IEnumerator Fade(float alphaGoal, float fadeTime, SkinnedMeshRenderer meshRenderer)
    {
        for (float t = 0; t < 1; t += Time.deltaTime / fadeTime)
        {
            meshRenderer.GetPropertyBlock(propertyBlock); 
            propertyBlock.SetFloat("_Alpha", Mathf.Lerp(meshRenderer.material.color.a, alphaGoal, t));
            meshRenderer.SetPropertyBlock(propertyBlock);  
            yield return null; 
        }

        Destroy(gameObject); 
    }
}