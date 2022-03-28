using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [SerializeField] private GameObject towerPrefab;

    [SerializeField] ParticleSystem particleSystem;

    public GameManager gameManager;

    public bool placingTower;
    public int placingTowerIndex;

    private Tower selectedTower;


    // Start is called before the first frame update
    void Start()
    {
    }

    public void EquipTower(int index)
    {
        if (selectedTower != null)
        {
            selectedTower.OnDeselect(true);
            gameManager.uiManager.HideTowerInfo();
            selectedTower = null;
        }

        gameManager.towersPrefabs[placingTowerIndex].SetActive(false);
        placingTower = true;
        placingTowerIndex = index;
    }

    public void PlaceTower(Ray ray, int towerIndex)
    {
        if (selectedTower != null)
        {
            selectedTower.OnDeselect();
            gameManager.uiManager.HideTowerInfo();
            selectedTower = null;
        }

        bool isMouseOverUnplacable = false;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("Ground"))
            {
                towerPrefab = gameManager.towersPrefabs[towerIndex];
                towerPrefab.SetActive(true);
                towerPrefab.transform.position = Vector3.Lerp(towerPrefab.transform.position, new Vector3(hit.point.x, 1.4f, hit.point.z), 0.8f);
            }

            if (hit.transform.CompareTag("Path") || hit.transform.CompareTag("Enemy"))
            {
                isMouseOverUnplacable = true;
            }

            //Range circle
            particleSystem.gameObject.SetActive(true);
            particleSystem.transform.position = towerPrefab.transform.position;
            ParticleSystem.ShapeModule shape = particleSystem.shape;
            shape.radius = towerPrefab.GetComponent<Tower>().range;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isMouseOverUnplacable && !EventSystem.current.IsPointerOverGameObject())
        {
            if (towerPrefab.GetComponent<Tower>().cost <= gameManager.GetMoney())
            {
                GameObject go = Instantiate(towerPrefab, towerPrefab.transform.position, towerPrefab.transform.rotation);
                go.GetComponent<Collider>().enabled = true;
                gameManager.RegisterTower(go.GetComponent<Tower>());
                ResetPlacing();
            }
        }
    }

    public void SelectTower(Ray ray)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !placingTower && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Tower tower = hit.transform.GetComponent<Tower>();
                if (tower != null)
                {
                    ResetSelect();

                    tower.OnSelect(particleSystem);
                    gameManager.uiManager.DisplayTowerInfo(tower);
                    selectedTower = tower;
                }
                else
                {
                    ResetSelect();
                }
            }
        }
    }

    public void SellTower()
    {
        if (selectedTower != null)
        {
            selectedTower.OnDeselect(true);
            gameManager.uiManager.HideTowerInfo();
            gameManager.RemoveTower(selectedTower);
            gameManager.UpdateMoney(selectedTower.cost / 3);
            selectedTower = null;
        }
    }

    public void ResetSelect()
    {
        if (selectedTower != null)
        {
            selectedTower.OnDeselect(true);
            gameManager.uiManager.HideTowerInfo();
            selectedTower = null;
        }
    }

    public void ResetPlacing()
    {
        placingTower = false;
        particleSystem.gameObject.SetActive(false);
        towerPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);

        SelectTower(ray);

        if (placingTower)
        {
            PlaceTower(ray, placingTowerIndex);
        }

        // if (Input.GetKeyDown(KeyCode.D) && !placingTower)
        // {
        //     placingTower = true;
        //     placingTowerIndex = 0;
        // }

        // if (Input.GetKeyDown(KeyCode.E) && !placingTower)
        // {
        //     placingTower = true;
        //     placingTowerIndex = 1;
        // }

        if (Input.GetKeyDown(KeyCode.Q) && placingTower)
        {
            ResetPlacing();
            ResetSelect();
        }

        if (Input.GetKeyDown(KeyCode.S) && selectedTower != null)
        {   
            SellTower();
        }
    }
}