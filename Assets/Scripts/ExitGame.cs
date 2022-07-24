using UnityEngine;

public class ExitGame : MonoBehaviour
{
    private bool dieReady = false;
    private bool oliveReady = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dieReady && oliveReady)
        {
            Application.Quit();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Olive"))
        {
            oliveReady = true;
        }
        if (collision.CompareTag("Die"))
        {
            dieReady = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Olive"))
        {
            oliveReady = false;
        }
        if (collision.CompareTag("Die"))
        {
            dieReady = false;
        }
    }
}
