using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDirectory
{
    public string name;
    public MyDirectory parent;
    public List<MyDirectory> children = new List<MyDirectory>();
    [System.NonSerialized]
    public bool foldout = true;
    public string newChildName;
    public string path;
    public bool selected = false;
    public bool isPackage = false;
    public List<string> pathPackage = new List<string>();
    public MyDirectory(string name, MyDirectory parent)
    {
        this.name = name;
        this.parent = parent;
    }
}
