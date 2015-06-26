using UnityEngine;
using UnityEditor;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;

/// <summary>
/// Class to handle individual nodes and their position and links within the editor
/// 
/// By: Charles Humphrey @NemoKrad
/// On: 25/06/2015
/// 
/// </summary>
public class Node
{
    /// <summary>
    /// static used to store unique ID for this node.
    /// </summary>
    static int _ID;

    /// <summary>
    ///  Stored unique ID for the node
    /// </summary>
    int _id;

    /// <summary>
    /// Property for unique ID
    /// </summary>
    public int ID { get { return _id; } }

    /// <summary>
    /// Display name of the node.
    /// </summary>
    public string Name;

    /// <summary>
    /// Dictionary of node links, the key is the index in the objects Data node array.
    /// </summary>
    public Dictionary<int, Node> LinkedNodes = new Dictionary<int, Node>();

    /// <summary>
    /// Position of the node within the editor
    /// </summary>
    public Rect Position;

    /// <summary>
    /// Is this node the root node?
    /// </summary>
    public bool IsRoot { get; set; }

    /// <summary>
    /// Actual instance of the node data.
    /// </summary>
    object _data = null;

    /// <summary>
    /// Property for the ndoe data. When set the relevent properties and fields are extracted
    /// </summary>
    public object Data 
    { 
        get { return _data; }

        set
        {
            _data = value;

            if (_data != null)
            {
                AvailableProperties.Clear();
                AvailableFields.Clear();

                PropertyInfo[] props = _data.GetType().GetProperties();
                foreach (PropertyInfo prop in props)
                    AvailableProperties.Add(prop.Name, GetPropertyValue(prop));

                FieldInfo[] fields = _data.GetType().GetFields();
                foreach (FieldInfo field in fields)
                    AvailableFields.Add(field.Name, GetFieldValue(field));
            }
        }
    }

    /// <summary>
    /// A dictionary of the available properties and their values.
    /// </summary>
    public Dictionary<string, object> AvailableProperties = new Dictionary<string, object>();

    /// <summary>
    /// A dictionary of the available fields and thrie values;
    /// </summary>
    public Dictionary<string, object> AvailableFields = new Dictionary<string, object>();

    /// <summary>
    /// ctor
    /// </summary>
    public Node()
    {
        _ID++;
        _id = _ID;
        Name = string.Format("Node [{0}]", ID);

        LinkedNodes = new Dictionary<int, Node>();
    }

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="name">Display name for the node</param>
    /// <param name="data">Data instance for the node</param>
    public Node(string name, object data = null) : this()
    {
        Name = name;
        Data = data;
    }

    /// <summary>
    /// static method to create a data instance.
    /// </summary>
    /// <param name="t">Type to be created</param>
    /// <returns>Instance of Type</returns>
    public static object CreateInstance(Type t)
    {
        object retVal = null;

        ConstructorInfo[] ctors = t.GetConstructors();
        retVal = ctors[0].Invoke(null);
            
        return retVal;
    }

    /// <summary>
    /// Method to query if a given property is read only property { get { } }
    /// </summary>
    /// <param name="name">Property name</param>
    /// <returns>True is read only, False if not</returns>
    public bool IsReadOnlyProperty(string name)
    {
        PropertyInfo prop = _data.GetType().GetProperty(name);

        return !prop.CanWrite;
    }

    /// <summary>
    /// Method to get a properties Type
    /// </summary>
    /// <param name="name">Name of Property</param>
    /// <returns>Type for given property</returns>
    public Type GetPropertyType(string name)
    {
        return _data.GetType().GetProperty(name).PropertyType;
    }

    /// <summary>
    /// Method to get a fields Type
    /// </summary>
    /// <param name="name">Name of Field</param>
    /// <returns>Type for given field</returns>
    public Type GetFieldType(string name)
    {
        return _data.GetType().GetField(name).FieldType;
    }

    /// <summary>
    /// Method to set a given property with a given value
    /// </summary>
    /// <param name="name">Name of property to set</param>
    /// <param name="value">Value to set it to.</param>
    public void SetPropertyValue(string name, object value)
    {
        PropertyInfo prop = _data.GetType().GetProperty(name);

        AvailableProperties[name] = value;
        SetPropertyValue(prop, value);
    }

    /// <summary>
    /// Method to set a given field with a given value
    /// </summary>
    /// <param name="name">Name of field to set</param>
    /// <param name="value">Value to set it to.</param>
    public void SetFieldValue(string name, object value)
    {
        FieldInfo field = _data.GetType().GetField(name);

        AvailableFields[name] = value;
        SetFieldValue(field, value);
    }

    /// <summary>
    /// Method to get a given properties value
    /// </summary>
    /// <param name="property">Property to get the value from</param>
    /// <returns>Properties value</returns>
    object GetPropertyValue(PropertyInfo property)
    {
        object retVal = null;

        if(!property.PropertyType.IsAbstract)
            retVal = property.GetValue(_data,null);

        return retVal;
    }

    /// <summary>
    /// Method to get a given field value
    /// </summary>
    /// <param name="property">Field to get the value from</param>
    /// <returns>Fields value</returns>
    object GetFieldValue(FieldInfo field)
    {
        object retVal = null;

        retVal = field.GetValue(_data);

        return retVal;
    }

    /// <summary>
    /// Method to set a given properties value
    /// </summary>
    /// <param name="property">Property to be set</param>
    /// <param name="value">Value to be set</param>
    void SetPropertyValue(PropertyInfo property, object value)
    {
        property.SetValue(_data, Convert.ChangeType(value, property.PropertyType), null);
    }

    /// <summary>
    /// Method to set a given field value
    /// </summary>
    /// <param name="property">Field to be set</param>
    /// <param name="value">Value to be set</param>
    void SetFieldValue(FieldInfo field, object value)
    {
        field.SetValue(_data, Convert.ChangeType(value, field.FieldType));
    }

    /// <summary>
    /// Method to attach a node to this node.
    /// </summary>
    /// <param name="node">Node to be attahced</param>
    /// <param name="idx">Index to attach the node to in the Linked Data Array</param>
    public void AttachNode(Node node, int idx)
    {
        // Find the property that is an array of my base type        
        Array array = GetNodeDataArray();
        if (array != null)
        {
            if (array.GetValue(idx) == null)
            {
                array.SetValue(node.Data, idx);
                LinkedNodes.Add(idx, node);
            }
        }                
    }

    /// <summary>
    /// Method to get a dictionary populated with nodes in the Linked Data Array.
    /// </summary>
    /// <returns>Dictionart of data
    /// Key is the index in the array, value the data.
    /// </returns>
    public Dictionary<int,object> GetLinkedDataNodes()
    {
        Dictionary<int, object> nodes = new Dictionary<int, object>();

        Array array = GetNodeDataArray();
        if (array != null)
        {
            for (int n = 0; n < array.Length; n++)
            {
                object data = array.GetValue(n);
                if (data != null)
                    nodes.Add(n, data);
            }
        }

        return nodes;
    }

    /// <summary>
    /// Method to determin if a node in the linkde data array is empty
    /// </summary>
    /// <param name="idx">Index in the array</param>
    /// <returns>True if index is null, false if array is null or not empty</returns>
    public bool LinkedDataNodeEmpty(int idx)
    {
        bool retval = false;

        Array array = GetNodeDataArray();
        if (array != null)
        {
            retval = array.GetValue(idx) == null;
        }

        return retval;
    }

    /// <summary>
    /// Property to get how many elements are in the linked data array
    /// </summary>
    public int TotalInputCount
    {
        get
        {
            Array array = GetNodeDataArray();

            if (array != null)
                return array.Length;

            return 0;
        }
    }

    /// <summary>
    /// Property to get the number of populated linked data items in the array
    /// </summary>
    public int InputsUsed
    {
        get
        {
            Array array = GetNodeDataArray();
            int cnt = 0;
            if (array != null)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (array.GetValue(i) != null)
                    {
                        cnt++;
                    }
                }
            }

            return cnt;
        }
    }

    /// <summary>
    /// MEthod to obtain the current data array
    /// </summary>
    /// <returns>Linked Data Array</returns>
    Array GetNodeDataArray()
    {
        PropertyInfo[] allProps = _data.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (PropertyInfo prop in allProps)
        {
            if (prop.PropertyType.IsArray) // We have an array, but is it of the type we want?
            {
                if (prop.PropertyType.Name.Replace("[]", "") == _data.GetType().BaseType.Name) // Is it the same as the base class
                {
                    return (Array)prop.GetValue(_data, null);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Method to detach an attached node from the Link data array
    /// </summary>
    /// <param name="idx">Index in the array to be removed</param>
    public void DetatchNode(int idx)
    {
        Array array = GetNodeDataArray();
        if (array != null)
            array.SetValue(null, idx);
        
        LinkedNodes.Remove(idx);
    }
}
