using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D clickingPointer;
    [SerializeField] private Texture2D defaultCursor;
    public bool checkForPeople = false;

    private void Start()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    void Update()
    {
        if (checkForPeople)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("People"))
                {
                    Cursor.SetCursor(clickingPointer, Vector2.zero, CursorMode.Auto);
                    if (Input.GetMouseButtonDown(0))
                    {
                        hit.collider.gameObject.GetComponent<PersonInformation>().SelectPerson();
                    }
                }
                else
                {
                    Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
                }
            }
            else
            {
                Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
            }
        }
    }

    public void StopCursorChange()
    {
        checkForPeople = false;
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }
}