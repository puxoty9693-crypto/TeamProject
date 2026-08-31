using System;
using UnityEngine;

public class CustomerAI : MonoBehaviour
{
    public Customer Customer { get; internal set; }

    internal void OnPatienceExpired()
    {
        throw new NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
