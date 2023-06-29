using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    private List<GameObject> _pipes = new();

    public bool IsVisible => _pipes.All(pipe => pipe.GetComponent<SpriteRenderer>().isVisible);

    public void AddPipe(GameObject go)
    { 
        _pipes.Add(go);
    }

    private void OnBecameInvisible()
    {
        Debug.Log("INVISIBLE");
        Destroy(gameObject);
    }
}