using UnityEngine;
using UnityEditor;

using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Node edotor...
/// 
/// By: Charles Humphrey @NemoKrad
/// On: 25/06/2015
/// 
/// </summary>
public class NodeEditor : EditorWindow
{
    /// <summary>
    /// Starting X position for the editable area
    /// </summary>
    float panX = 0;

    /// <summary>
    /// Startinf Y position for the editable area.
    /// </summary>
    float panY = 20;

    /// <summary>
    /// List of nodes to  be rendered.
    /// </summary>
    List<Node> nodeWindows = new List<Node>();

    /// <summary>
    /// X coord for next window added
    /// </summary>
    int lstX = 10;

    /// <summary>
    /// Y coord for the next window added
    /// </summary>
    int lstY = 10;

    /// <summary>
    /// List of ID's for editor windows open
    /// </summary>
    List<int> openEditWindows = new List<int>();

    /// <summary>
    /// Master scroll value for scroll bars
    /// </summary>
    Vector2 masterScroll = Vector2.zero;

    /// <summary>
    /// Current mouse position, needs wantsMouseMove = true;
    /// </summary>
    Vector2 mousePos { get { return Event.current.mousePosition; } }

    /// <summary>
    /// ID of node attaching from
    /// </summary>
    int AttachFrom = -1;

    /// <summary>
    /// ID of form attaching to.
    /// </summary>
    int AttachedTo = -1;

    /// <summary>
    /// Index of node array to attach to
    /// </summary>
    int AttachedToIdx = -1;

    /// <summary>
    /// ID offset used for edit window IDs
    /// </summary>
    const int idOffset = 9999;

    /// <summary>
    /// Dictionary of scroll bar values for each open edit window.
    /// </summary>
    Dictionary<int, Vector2> editorScrollers = new Dictionary<int, Vector2>();

    /// <summary>
    /// Flag, if set to true the add node window is shown
    /// </summary>
    bool showAddNodeWindow;

    /// <summary>
    /// ID for add node window.
    /// </summary>
    const int addNodeWindowID = 92223122;

    /// <summary>
    /// Width of node windows
    /// </summary>
    int NodeWidth = 120;

    /// <summary>
    /// Height (min) of node windows
    /// </summary>
    int NodeHeight = 100;

    /// <summary>
    /// Editor Window width
    /// </summary>
    int EditWidth = 300;

    /// <summary>
    /// Editr Window height
    /// </summary>
    int EditHeight = 200;

    /// <summary>
    /// This is the list of available node types that can be added in the editor, the key is the group name.
    /// </summary>
    public Dictionary<string, List<Type>> NodeTypes = new Dictionary<string, List<Type>>();

    /// <summary>
    /// This is the selected root node.
    /// </summary>
    public object RootNode = null;

    /// <summary>
    /// Scroller for add node window
    /// </summary>
    Vector2 typeScroller;

    /// <summary>
    /// List of selected nodes to be added.
    /// </summary>
    Dictionary<Type, bool> selected = new Dictionary<Type, bool>();

    /// <summary>
    /// This method tells unity to include it in the window menu.
    /// </summary>
    [MenuItem("Window/Node editor")]
    public static void ShowEditor()
    {
        NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
        editor.Init();
    }

    /// <summary>
    /// Initialise the variables ready for a new session, and if we have a root node, unpack it's links
    /// </summary>
    public void Init()
    {
        wantsMouseMove = true;

        panX = 0;
        panY = 20;

        nodeWindows.Clear();

        lstX = 10;
        lstY = 10;

        openEditWindows.Clear();
        masterScroll = Vector2.zero;

        AttachFrom = -1;
        AttachedTo = -1;
        AttachedToIdx = -1;

        editorScrollers.Clear();

        showAddNodeWindow = false;

        UnpackObject(RootNode);
    }

    /// <summary>
    /// Recursive method to unpack a node and it's data links
    /// </summary>
    /// <param name="data">Data instance</param>
    /// <param name="lnkIdx">index that this is linked to, defaults to -1 if none needed or given</param>
    /// <param name="linkedTo">Node to be linked too, null by default.</param>
    void UnpackObject(object data, int lnkIdx = -1, Node linkedTo = null)
    {
        if (data != null)
        {
            Node thisNode = new Node(data.GetType().Name, data);
            Rect pos;

            if (linkedTo != null)
            {
                if ((20 * (thisNode.TotalInputCount)) > NodeHeight - 60)
                    pos = new Rect(linkedTo.Position.x - (linkedTo.Position.width * 1.5f), linkedTo.Position.y + (NodeHeight * lnkIdx), NodeWidth, NodeHeight + (20 * (thisNode.TotalInputCount)));
                else
                    pos = new Rect(linkedTo.Position.x - (linkedTo.Position.width * 1.5f), linkedTo.Position.y + (NodeHeight * lnkIdx), NodeWidth, NodeHeight);

                linkedTo.LinkedNodes.Add(lnkIdx, thisNode);
            }
            else
            {
                if ((20 * (thisNode.TotalInputCount)) > NodeHeight - 60)
                    pos = new Rect(position.width - (NodeWidth * 1.5f), NodeHeight, NodeWidth, NodeHeight + (20 * (thisNode.TotalInputCount)));
                else
                    pos = new Rect(position.width - (NodeWidth * 1.5f), NodeHeight, NodeWidth, NodeHeight);
            }

            if (pos.x < NodeWidth)
                pos = new Rect(NodeWidth, pos.y, pos.width, pos.height);

            if (pos.y < NodeHeight)
                pos = new Rect(pos.x, NodeHeight, pos.width, pos.height);

            AddNode(thisNode,pos);

            Dictionary<int, object> lNodes = thisNode.GetLinkedDataNodes();
            foreach (int key in lNodes.Keys)
            {
                UnpackObject(lNodes[key], key, thisNode);
            }
        }
    }

    /// <summary>
    /// This is the GUI render thread fort the entire editor
    /// </summary>
    void OnGUI()
    {
        // Set up the scroll bars
        masterScroll = GUI.BeginScrollView(new Rect(panX, panY, position.width - 4, position.height - panY), masterScroll, new Rect(panX, panY, float.MaxValue, float.MaxValue));

        // Render connection lines
        if (AttachFrom != -1)
        {
            if (AttachedTo == -1)
            {
                Node thisNode = nodeWindows.Single(n => n.ID == AttachFrom);
                Rect spr = new Rect(thisNode.Position.x, thisNode.Position.y, thisNode.Position.width, thisNode.Position.height + 20);
                DrawNodeCurve(spr, new Rect(mousePos.x, mousePos.y, 10, 10), Color.green);
            }
            else
            {
                // Add to node list in node.
                if (nodeWindows.Single(n => n.ID == AttachFrom).LinkedNodes.SingleOrDefault(n => n.Key == AttachedToIdx &&  n.Value.ID == AttachedTo).Value == null)
                {
                    Node node = nodeWindows.Single(n => n.ID == AttachFrom);
                    nodeWindows.Single(n => n.ID == AttachedTo).AttachNode(node, AttachedToIdx);
                }

                AttachFrom = -1;
                AttachedTo = -1;
                AttachedToIdx = -1;
            }
        }
        
        // Start rendering the window.
        BeginWindows();

        // Render the node windows
        foreach (Node node in nodeWindows)
        {
            node.Position = GUI.Window(node.ID, node.Position, DrawNodeWindow, node.Name);

            // Do we need to render an edit wondow for this node?
            if (openEditWindows.Contains(node.ID))
            {
                Rect edRect = node.Position;
                edRect.width = EditWidth;
                edRect.height = EditHeight;


                GUI.Window(node.ID + idOffset, edRect, DrawEditWindow, string.Format("{0} Properties", node.Name));
            }

            // Render the noodes links.
            foreach (int key in node.LinkedNodes.Keys)
            {
                Rect tpr = new Rect(node.Position.x, node.Position.y + (20 * key), node.Position.width, node.Position.height +20);
                Rect spr = new Rect(node.LinkedNodes[key].Position.x, node.LinkedNodes[key].Position.y, node.LinkedNodes[key].Position.width, node.LinkedNodes[key].Position.height + 20);

                DrawNodeCurve(spr, tpr, Color.blue);
            }
        }

        // Are we adding a new node to the editor?
        if (showAddNodeWindow)
        {
            GUI.Window(addNodeWindowID, new Rect(position.width / 4, position.height / 4, position.width / 2, position.height / 2), DrawAddNodeWindow, "Add Node");
        }

        EndWindows();
        GUI.EndScrollView();

        // Render box
        GUI.Box(new Rect(0, 0, position.width, panY), "");
        DrawControlBoxContent();

        // Update on mouse move.
        if (Event.current.type == EventType.MouseMove)
            Repaint();
    }

    /// <summary>
    /// Removes ALL nodes
    /// </summary>
     void ClearAll()
    {
        nodeWindows.Clear();
        lstX = 10;
        lstY = 10;
        RootNode = null;
    }

    /// <summary>
    /// Draws the controll buttons at the top.
    /// </summary>
    void DrawControlBoxContent()
    {
        if (GUI.Button(new Rect(0, 0, 100, 20), "Add Node"))
        {
            //AddNode(new Node());
            if (!showAddNodeWindow)
            {
                showAddNodeWindow = true;
                typeScroller = Vector2.zero;
                selected.Clear();
            }
        }

        if (GUI.Button(new Rect(110, 0, 100, 20), "Clear All"))
        {
            ClearAll();
        }

        if (GUI.Button(new Rect(220, 0, 100, 20), "Tidy Up"))
        {
            Init();
        }
    }

   
    /// <summary>
    /// Method to add a node to the editor in a specific place on screen
    /// </summary>
    /// <param name="node">Node to be added</param>
    /// <param name="rect">Where is it to be rendered</param>
    void AddNode(Node node, Rect rect)
    {
        node.Position = rect;

        AddWindowNode(node);
    }

    /// <summary>
    /// Method to add a node to the editor
    /// </summary>
    /// <param name="node">Node to be added</param>
    void AddNode(Node node)
    {
        if((20 * (node.TotalInputCount)) > NodeHeight - 60)
            node.Position = new Rect(lstX + panX, lstY + panY, NodeWidth, NodeHeight + (20 * (node.TotalInputCount)));
        else
            node.Position = new Rect(lstX + panX, lstY + panY, NodeWidth, NodeHeight);

        AddWindowNode(node);
    }

    /// <summary>
    /// Method to add a window for the given node
    /// </summary>
    /// <param name="node">Node to be represented by this window</param>
    void AddWindowNode(Node node)
    {
        nodeWindows.Add(node);

        lstX += 25;
        lstY += 25;

        if (lstY > position.width)
            lstY = 10;

        if (lstX > position.height)
            lstX = 10;
    }

    /// <summary>
    /// Method to draw the add node window
    /// </summary>
    /// <param name="id"></param>
    void DrawAddNodeWindow(int id)
    {
      
        Vector2 selectionDims = new Vector2((position.width / 4) + 150 , (position.height / 2.5f) - 50);

        if (GUI.Button(new Rect((position.width / 2) - 30, 20, 25, 25), "X"))
        {
            showAddNodeWindow = false;
        }
        else
        {
            // what are the types...
            GUI.Label(new Rect(20, 50, 100, 20), "Node Type:");
            
            
            // Expnd out our scroller
            GUI.Box(new Rect(100, 50, selectionDims.x - 15, selectionDims.y - 15), "");

            float h = 0;
            foreach (string key in NodeTypes.Keys)
            {
                h++;
                foreach (Type t in NodeTypes[key])
                {
                    if (!selected.Keys.Contains(t))
                        selected.Add(t, false);
                    h++;
                }
            }

            h *= 20;

            typeScroller = GUI.BeginScrollView(new Rect(100, 50, selectionDims.x, selectionDims.y), typeScroller, new Rect(100, 55, 600, h));

            int line = 0;
            foreach (string key in NodeTypes.Keys)
            {
                // Key as header
                DrawLine(new Vector3(105, 55 + (20 * line), 0), new Vector3(selectionDims.x, 55 + (20 * line), 0));
                GUI.Label(new Rect(110, 55 + (20 * line), 100, 20), key);
                DrawLine(new Vector3(105, 72 + (20 * line), 0), new Vector3(selectionDims.x, 72 + (20 * line), 0));
                line++;

                foreach (Type t in NodeTypes[key])
                {
                    selected[t] = GUI.Toggle(new Rect(110, 55 + (20 * line), selectionDims.x, 20), selected[t], t.Name);
                    line++;
                }

            }

            GUI.EndScrollView();

            if (GUI.Button(new Rect((position.width / 2) - 110, (position.height / 2) - 30, 100, 25), "Add Selected"))
            {
                showAddNodeWindow = false;
                foreach (Type t in selected.Keys)
                {
                    if (selected[t])
                        AddNode(new Node(t.Name) { Data = Node.CreateInstance(t) });
                }
            }
        }
    }

    /// <summary>
    /// Method to draw the edit window
    /// </summary>
    /// <param name="id"></param>
    void DrawEditWindow(int id)
    {
        Node thisNode = nodeWindows.Single(n => n.ID == id - idOffset);

        float width = EditWidth;
        float height = EditHeight;

        if (GUI.Button(new Rect(width - 20, 15, 20, 20), "X"))
        {
            if (openEditWindows.Contains(id - idOffset))
            {
                openEditWindows.Remove(id - idOffset);
                editorScrollers.Remove(id - idOffset);
            }
        }
        else
        {
            thisNode.IsRoot = GUI.Toggle(new Rect(8, 20, 100, 20), thisNode.IsRoot, "Is Root?");

            if (thisNode.IsRoot) // Only one can be root.
            {
                RootNode = thisNode.Data;
                foreach (Node node in nodeWindows)
                {
                    if (node != thisNode)
                        node.IsRoot = false;
                }
            }
            else
            {
                if (nodeWindows.SingleOrDefault(n => n.IsRoot) == null)
                    RootNode = null;
            }

            if (thisNode.Data == null)
                return;

            int h = (thisNode.AvailableFields.Count + thisNode.AvailableProperties.Count) * 20;
            //h = h * 20;

            editorScrollers[id - idOffset] = GUI.BeginScrollView(new Rect(2, 40, width - 4, height - 52), editorScrollers[id - idOffset], new Rect(0, 0, width, h));

            // Properties
            int line = 0;
            List<string> keys = thisNode.AvailableProperties.Keys.ToList();
            foreach (string key in keys)
            {
                if (!thisNode.IsReadOnlyProperty(key) && IsValidType(thisNode.GetPropertyType(key)))
                {
                    GUI.Label(new Rect(8, (20 * line), 100, 20), key);
                    thisNode.SetPropertyValue(key, GUI.TextField(new Rect(112, (20 * line++), 100, 20), thisNode.AvailableProperties[key].ToString()));
                }
            }

            // Fields
            keys = thisNode.AvailableFields.Keys.ToList();
            foreach (string key in keys)
            {
                if (IsValidType(thisNode.GetFieldType(key)))
                {
                    GUI.Label(new Rect(8, (20 * line), 100, 20), key);
                    thisNode.SetFieldValue(key, GUI.TextField(new Rect(112, (20 * line++), 100, 20), thisNode.AvailableFields[key].ToString()));
                }
            }

            GUI.EndScrollView();
        }
        
        GUI.DragWindow();
    }

    /// <summary>
    /// Method see if the type is a valid editable type for the editor
    /// </summary>
    /// <param name="t">Type to be validated</param>
    /// <returns>True if valid</returns>
    bool IsValidType(Type t)
    {
        if (t.IsAbstract || t.IsEnum)
            return false;

        if (t == typeof(string) || t == typeof(int) || t == typeof(float) || t == typeof(double) || t == typeof(long) || t == typeof(short) || t == typeof(char))
            return true;

        return false;
    }

    /// <summary>
    /// Method to draw node window.
    /// </summary>
    /// <param name="id"></param>
    void DrawNodeWindow(int id)
    {
        Node thisNode = nodeWindows.Single(n => n.ID == id);        

        if (GUI.Button(new Rect(NodeWidth - 22, 16, 20, 20), new GUIContent("X", "Close Node")))
        {
            AttachFrom = -1;

            thisNode.IsRoot = false;

            // Remove me from all others in/out params
            foreach (Node node in nodeWindows)
            {
                for (int n = 0; n < node.LinkedNodes.Count; n++)
                {
                    if (node.LinkedNodes[n] == thisNode)
                        node.DetatchNode(n);
                }
            }

            thisNode.LinkedNodes.Clear();

            nodeWindows.Remove(thisNode);

            if (nodeWindows.SingleOrDefault(n => n.IsRoot) == null)
                RootNode = null;
        }

        if (GUI.Button(new Rect(4, 16, 40, 20), new GUIContent("Edit", "Edit nodes properties")))
        {
            AttachFrom = -1;
            // Pop the edit window for this item.
            if (!openEditWindows.Contains(id))
            {
                openEditWindows.Add(id);
                editorScrollers.Add(id, Vector2.zero);
            }
        }

        // From button
        if (GUI.Button(new Rect(NodeWidth - 20, thisNode.Position.height / 2, 20, 20), new GUIContent(">", "Click to connect to another node.")))
        {
            if (AttachFrom == -1)
            {
                AttachFrom = id;
            }
            else
            {
                AttachFrom = -1;
                AttachedTo = -1;
                AttachedToIdx = -1;
            }
        }

        for (int n = 0; n < thisNode.TotalInputCount; n++)
        {
            if (GUI.Button(new Rect(0, thisNode.Position.height / 2 + (20 * n), 20, 20), new GUIContent(">", "Click to connecto from another node.")))
            {
                if (AttachFrom != -1)
                {
                    AttachedTo = id;
                    AttachedToIdx = n;                    
                }
                else
                {
                    AttachFrom = -1;
                    AttachedTo = -1;
                    AttachedToIdx = -1;

                    if (thisNode.LinkedNodes.Keys.Contains(n))
                        thisNode.DetatchNode(n);
                }
            }
        }

        GUI.DragWindow();
    }

    /// <summary>
    /// Method to draw a Bezier curve
    /// </summary>
    /// <param name="start">Start Position</param>
    /// <param name="end">End Position</param>
    /// <param name="color">Color</param>
    void DrawNodeCurve(Rect start, Rect end, Color color)
    {
        float curve = 100;

        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * curve;
        Vector3 endTan = endPos + Vector3.left * curve;
        Color shadowCol = new Color(0, 0, 0, 0.06f);
        for (int i = 0; i < 3; i++) // Draw a shadow           
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        Handles.DrawBezier(startPos, endPos, startTan, endTan, color, null, 1);

        DrawDisc(startPos, 4);
        DrawDisc(endPos, 4);
    }

    /// <summary>
    /// Method to draw a disc
    /// </summary>
    /// <param name="startPos">Position</param>
    /// <param name="radius">Size</param>
    void DrawDisc(Vector3 startPos, float radius)
    {
        Handles.DrawSolidDisc(startPos, Vector3.back, radius);
    }

    /// <summary>
    /// Method to draw a strait line
    /// </summary>
    /// <param name="v1">Start Position</param>
    /// <param name="v2">End Position</param>
    void DrawLine(Vector3 v1, Vector3 v2)
    {
        Handles.DrawLine(v1, v2);
    }
}