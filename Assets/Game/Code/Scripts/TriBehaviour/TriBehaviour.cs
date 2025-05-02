using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriBehaviour : MonoBehaviour
{
    // [SerializeField] protected ObjCtrl objCtrl;

    protected virtual void Start()
    {
        // for ovveride
    }
    protected virtual void Awake()
    {
      //  objCtrl = transform.parent.GetComponent<ObjCtrl>();
        LoadComponent();
    }

    protected  virtual void Reset()
    {
        LoadComponent();
    }
    
    protected virtual  void LoadComponent()
    {
        // for ovveride
    }
    protected virtual void LoadPrefabs()
    {
        // for ovveride
    }
    public virtual void OnEnable()
    {
        // for ovveride
    }
    
}
