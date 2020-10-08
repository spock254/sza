using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightButtonClickController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject rightButtonClick_panel;
    public GameObject spawnPoint;
    public GameObject item;

    public Controller controller;
    public RectTransform UICanval;
    //public ActionPanelController actionPanelController;
    bool panelIsOpen = false;

    // время для того что бы выполнить все onClick события
    float closePanelDelay = 0.17f;
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && panelIsOpen)
        {
            StartCoroutine(LoadSceneAfterDelay(closePanelDelay));
        }

        if (!panelIsOpen) 
        {
            StopAllCoroutines();
        }
    }

    public IEnumerator LoadSceneAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        OnClosePanel();
    }

    public void RightButtonClick(RaycastHit2D[] hits, Vector2 mousePosition) 
    {
        DestroyItems();

        Debug.Log(mousePosition.x +" "+mousePosition.y);
        rightButtonClick_panel.SetActive(true);
        //rightButtonClick_panel.transform.position = SetPanelPosition(mousePosition);
        //TODO
        rightButtonClick_panel.GetComponent<RectTransform>().anchoredPosition = SetPanelPosition(mousePosition);

        float step = GetItemHeight();

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.name.Contains("item"))
            {
                float spawnY = i * step;
                Vector3 pos = new Vector3(spawnPoint.transform.position.x, spawnY,
                                            spawnPoint.transform.position.z);
                GameObject spawnedItem = (GameObject)Instantiate(item, pos, spawnPoint.transform.rotation);
                ItemInButton itemInButton = spawnedItem.GetComponent<ItemInButton>();
                Button itemButton = spawnedItem.GetComponent<Button>();

                SetItemName(spawnedItem, hits[i].collider.GetComponent<ItemCell>().item.itemName);


                itemInButton.item = hits[i].collider.gameObject;
                itemInButton.controller = controller;

                itemButton.onClick.AddListener(itemInButton.OnClickPickUp);
                itemButton.onClick.AddListener(OnClosePanel);

                spawnedItem.transform.SetParent(spawnPoint.transform, false);
            }
        }
        panelIsOpen = true;
    }

    Vector3 SetPanelPosition(Vector2 mousePosition) 
    {
        RectTransform rt = UICanval;
        //Vector2 localpoint;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, GetComponentInParent<Canvas>().worldCamera, out localpoint);

        //Vector2 normalizedPoint = Rect.PointToNormalized(rt.rect, localpoint);
        //return normalizedPoint;
        //Debug.Log(normalizedPoint);


        Vector2 viewportPoint = Camera.main.ScreenToViewportPoint(mousePosition);

        float leftCornerX = 0;//rt.rect.width / 2;
        float rightCornerY = 0;//rt.rect.height / 2;

        return new Vector3(viewportPoint.x - leftCornerX, viewportPoint.y - rightCornerY, rightButtonClick_panel.transform.position.z);
        //return new Vector3()
    }

    void DestroyItems() 
    {
        for (int j = 0; j < spawnPoint.transform.childCount; j++)
        {
            GameObject.Destroy(spawnPoint.transform.GetChild(j).gameObject);
        }
    }

    float GetItemHeight() 
    {
        return item.GetComponent<RectTransform>().rect.height;
    }

    void SetItemName(GameObject spawnedItem, string name) 
    {
        spawnedItem.transform.GetChild(0).GetComponent<Text>().text = name;
    }

    void OnClosePanel() 
    {
        rightButtonClick_panel.SetActive(false);
        panelIsOpen = false;
    }
}
