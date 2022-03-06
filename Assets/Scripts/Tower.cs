using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameManager gameManager;

    public float range = 10f;
    public float fireRate = 1f;
    public float damage = 1f;
    public int cost = 10;

    private float nextAttackTime;

    private ParticleSystem tempParticle;

    public virtual void OnPlaced()
    {
    }

    public virtual void OnRemoved()
    {
    }

    public virtual void OnSelect()
    {

        //Draw circle around tower
        //Draw range of tower

        int density = 32;
        float radius = range;

        float theta = (2f * Mathf.PI) / density;
        float angle = 0;
        for (int i = 0; i < density; i++)
        {
            float x = gameObject.transform.position.x + radius * Mathf.Cos(angle);
            float z = gameObject.transform.position.z + radius * Mathf.Sin(angle);
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.SetParent(gameObject.transform.GetChild(0));
            cube.transform.position = new Vector3(x, 1, z);
            angle += theta;
        }

    }

    public virtual void OnSelect(ParticleSystem particle)
    {

        //Draw circle around tower
        //Draw range of tower
        ParticleSystem newParticle = Instantiate(particle);
        newParticle.transform.position = gameObject.transform.position;
        ParticleSystem.ShapeModule shape = newParticle.shape;
        shape.radius = range;
        newParticle.gameObject.SetActive(true);
        tempParticle = newParticle;
    }

    public virtual void OnDeselect()
    {

        //Remove circle around tower
        //! This can stop working if the tower's hierarchy is moved!!!
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            Destroy(transform.GetChild(0).GetChild(i).gameObject);
        }

    }

    public virtual void OnDeselect(bool particle)
    {
        Destroy(tempParticle.gameObject);
    }

    public virtual void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public Enemy GetNearestEnemy()
    {
        Enemy closestEnemy = null;
        float closestDistance = 200;
        for (int i = 0; i < gameManager.enemies.Count; i++)
        {
            Enemy currentEnemy = gameManager.enemies[i];
            if (Vector3.Distance(transform.position, currentEnemy.transform.position) < closestDistance)
            {
                closestEnemy = currentEnemy;
                closestDistance = Vector3.Distance(transform.position, currentEnemy.transform.position);
            }
        }

        return closestEnemy;
    }

    public virtual void Attack()
    {
        if (nextAttackTime <= Time.time)
        {
            nextAttackTime = Time.time + fireRate;
            Enemy nearestEnemy = GetNearestEnemy();
            if (nearestEnemy != null && Vector3.Distance(transform.position, nearestEnemy.transform.position) <= range)
            {
                //Shoot a line from the tower to the enemy

                LineRenderer line = gameObject.GetComponent<LineRenderer>();
                line.SetPosition(0, transform.position);
                line.SetPosition(1, nearestEnemy.transform.position);
                line.enabled = true;
                StartCoroutine(DisableLine(line));

                nearestEnemy.TakeDamage(1);
            }
        }
    }

    private IEnumerator DisableLine(LineRenderer line)
    {
        yield return new WaitForSeconds(0.05f);
        line.enabled = false;
    }
}