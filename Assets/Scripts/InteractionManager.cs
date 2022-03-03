using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [SerializeField] private GameObject towerPrefab;

    public GameManager gameManager;

    private bool placingTower;
    private int placingTowerIndex;
    
    public List<GameObject> towersPrefabs;
    
    

    // Start is called before the first frame update
    void Start()
    {
    }

    public void PlaceTower(Ray ray, int towerIndex)
    {
        bool isMouseOverUnplacable = false;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("Ground"))
            {
                towerPrefab = towersPrefabs[towerIndex];
                towerPrefab.transform.position = Vector3.Lerp(towerPrefab.transform.position, new Vector3(hit.point.x, 1.4f, hit.point.z), 0.8f);
            }

            if (hit.transform.CompareTag("Path") || hit.transform.CompareTag("Enemy"))
            {
                isMouseOverUnplacable = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isMouseOverUnplacable)
        {
            GameObject go = Instantiate(towerPrefab, towerPrefab.transform.position, towerPrefab.transform.rotation);
            gameManager.RegisterTower(go.GetComponent<Tower>());
            placingTower = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);

        if (placingTower)
        {
            PlaceTower(ray, placingTowerIndex);
        }

        if (Input.GetKeyDown(KeyCode.D) && !placingTower)
        {
            placingTower = true;
            placingTowerIndex = 0;
        }
        
        if (Input.GetKeyDown(KeyCode.E) && !placingTower)
        {
            placingTower = true;
            placingTowerIndex = 1;
        }
    }
}