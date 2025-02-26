using System.Collections;
using UnityEngine;

public class ghostDummy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Despawn(int time, Collider2D collider)
    {

        yield return new WaitForSeconds(time);

        if (collider.gameObject.TryGetComponent(out robberTutorial component))
        {
            component.lossOfLife = true;
        }

        Destroy(this.gameObject);
    }    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Robber"))
        {
            Despawn(3, collision);
        }
    }
}
