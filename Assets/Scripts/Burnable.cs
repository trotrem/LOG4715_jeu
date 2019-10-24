using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnable : MonoBehaviour
{
    // Start is called before the first frame update

    float timer = 0f;
    FireArrowScript arrow;

    [SerializeField]
    float burnTime = 1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(-90, 0, 0);
        timer += Time.deltaTime;
        if(timer >= burnTime)
        {
            Destroy(arrow.gameObject);
            Destroy(transform.parent.gameObject);
        }
    }

    public void Burn(FireArrowScript arrow)
    {
        this.arrow = arrow;
        gameObject.SetActive(true);
    }
}
