using UnityEngine;
using UnityEditor;
using System.Xml;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.Rendering;
using System;

public class EDITOR : Editor
{
    private static Shader shader;

    [MenuItem("Assets/SaveScene")]
    static void SaveScene()
    {

        string LevelName = SceneManager.GetActiveScene().name;
        if (LevelName == "")
            Debug.LogError("Please add your scene to build settings");
        string filepath = EditorUtility.SaveFilePanel("Save Resource", "", "SaveScene", "xml");
        //string filepath = Application.dataPath + @"/StreamingAssets/lv01.xml";
        if (!File.Exists(filepath))
        {
            File.Delete(filepath);
        }
        if (filepath.Length != 0)
        {

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement root = xmlDoc.CreateElement("gameObjects");
            //Debug.Log(UnityEditor.EditorBuildSettings.scenes[0]);
            //Check how many scene in build settings and check how many object in that settings
            foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
            {

                Debug.Log("loaded");
                //save this scenes first before you do anything
                EditorSceneManager.SaveOpenScenes();
                //check if this scene is enabled
                if (S.enabled)
                {
                    //get this scene names
                    string name = S.path;
                    //open this scene
                    EditorSceneManager.OpenScene(name);

                    
                    XmlElement scenes = xmlDoc.CreateElement("scenes");
                    scenes.SetAttribute("name", name);

                    //now check every gameoject in this scene
                    foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
                    {
                        XmlElement gameObject = xmlDoc.CreateElement("gameObjects");
                        //get this object's name
                        gameObject.SetAttribute("name", obj.name);

                        if (obj.transform.parent == null)
                        {
                         
                     
                            
                            //set up a prefab name
                           // gameObject.SetAttribute("asset", obj.name + ".prefab");

                            //PrefabUtility.SaveAsPrefabAsset(obj, "Assets/Resources/Prefab/" + obj.name + ".prefab");
                            AssetDatabase.SaveAssets();
                            //get this object's transform value
                            XmlElement transform = xmlDoc.CreateElement("transform");

                            // get this object's position value
                            XmlElement position = xmlDoc.CreateElement("position");
                            XmlElement position_x = xmlDoc.CreateElement("x");
                            position_x.InnerText = obj.transform.position.x + "";
                            XmlElement position_y = xmlDoc.CreateElement("y");
                            position_y.InnerText = obj.transform.position.y + "";
                            XmlElement position_z = xmlDoc.CreateElement("z");
                            position_z.InnerText = obj.transform.position.z + "";
                            position.AppendChild(position_x);
                            position.AppendChild(position_y);
                            position.AppendChild(position_z);

                            // get this object's rotation value
                            XmlElement rotation = xmlDoc.CreateElement("rotation");
                            XmlElement rotation_x = xmlDoc.CreateElement("x");
                            rotation_x.InnerText = obj.transform.rotation.eulerAngles.x + "";
                            XmlElement rotation_y = xmlDoc.CreateElement("y");
                            rotation_y.InnerText = obj.transform.rotation.eulerAngles.y + "";
                            XmlElement rotation_z = xmlDoc.CreateElement("z");
                            rotation_z.InnerText = obj.transform.rotation.eulerAngles.z + "";
                            rotation.AppendChild(rotation_x);
                            rotation.AppendChild(rotation_y);
                            rotation.AppendChild(rotation_z);

                            // get this object's scale value
                            XmlElement scale = xmlDoc.CreateElement("scale");
                            XmlElement scale_x = xmlDoc.CreateElement("x");
                            scale_x.InnerText = obj.transform.localScale.x + "";
                            XmlElement scale_y = xmlDoc.CreateElement("y");
                            scale_y.InnerText = obj.transform.localScale.y + "";
                            XmlElement scale_z = xmlDoc.CreateElement("z");
                            scale_z.InnerText = obj.transform.localScale.z + "";

                            scale.AppendChild(scale_x);
                            scale.AppendChild(scale_y);
                            scale.AppendChild(scale_z);
                          

                            transform.AppendChild(position);
                            transform.AppendChild(rotation);
                            transform.AppendChild(scale);

                         
                            gameObject.AppendChild(transform);
                        
                        }
                        if (obj.GetComponent<MeshFilter>() != null)
                        {
                            XmlElement meshfilter = xmlDoc.CreateElement("meshfilter");

                            XmlElement mesh = xmlDoc.CreateElement("mesh");
                            mesh.InnerText = obj.GetComponent<MeshFilter>().sharedMesh.name;


                            meshfilter.AppendChild(mesh);
                            gameObject.AppendChild(meshfilter);

                        }
                        if (obj.GetComponent<BoxCollider>() != null)
                        {
                            XmlElement boxcollider = xmlDoc.CreateElement("boxcollider");

                            XmlElement istrigger = xmlDoc.CreateElement("istrigger");
                            istrigger.InnerText = obj.GetComponent<BoxCollider>().isTrigger.ToString();

                            XmlElement center = xmlDoc.CreateElement("center");
                            XmlElement center_x = xmlDoc.CreateElement("x");
                            center_x.InnerText = obj.GetComponent<BoxCollider>().center.x + "";
                            XmlElement center_y = xmlDoc.CreateElement("y");
                            center_y.InnerText = obj.GetComponent<BoxCollider>().center.y + "";
                            XmlElement center_z = xmlDoc.CreateElement("z");
                            center_z.InnerText = obj.GetComponent<BoxCollider>().center.z + "";
                            center.AppendChild(center_x);
                            center.AppendChild(center_y);
                            center.AppendChild(center_z);

                            XmlElement bSize = xmlDoc.CreateElement("size");
                            XmlElement bSize_x = xmlDoc.CreateElement("x");
                            bSize_x.InnerText = obj.GetComponent<BoxCollider>().size.x + "";
                            XmlElement bSize_y = xmlDoc.CreateElement("y");
                            bSize_y.InnerText = obj.GetComponent<BoxCollider>().size.y + "";
                            XmlElement bSize_z = xmlDoc.CreateElement("z");
                            bSize_z.InnerText = obj.GetComponent<BoxCollider>().size.z + "";
                            bSize.AppendChild(bSize_x);
                            bSize.AppendChild(bSize_y);
                            bSize.AppendChild(bSize_z);


                            boxcollider.AppendChild(istrigger);
                            boxcollider.AppendChild(center);
                            boxcollider.AppendChild(bSize);
                            gameObject.AppendChild(boxcollider);


                        }
                        if (obj.GetComponent<SphereCollider>() != null)
                        {
                            XmlElement spherecollider = xmlDoc.CreateElement("spherecollider");

                            XmlElement istrigger = xmlDoc.CreateElement("istrigger");
                            istrigger.InnerText = obj.GetComponent<SphereCollider>().isTrigger.ToString();

                            XmlElement center = xmlDoc.CreateElement("center");
                            XmlElement center_x = xmlDoc.CreateElement("x");
                            center_x.InnerText = obj.GetComponent<SphereCollider>().center.x + "";
                            XmlElement center_y = xmlDoc.CreateElement("y");
                            center_y.InnerText = obj.GetComponent<SphereCollider>().center.y + "";
                            XmlElement center_z = xmlDoc.CreateElement("z");
                            center_z.InnerText = obj.GetComponent<SphereCollider>().center.z + "";
                            center.AppendChild(center_x);
                            center.AppendChild(center_y);
                            center.AppendChild(center_z);


                            XmlElement radius = xmlDoc.CreateElement("radius");
                            radius.InnerText = obj.GetComponent<SphereCollider>().radius + "";

                            spherecollider.AppendChild(istrigger);
                            spherecollider.AppendChild(center);
                            spherecollider.AppendChild(radius);
                            gameObject.AppendChild(spherecollider);


                        }
                        if (obj.GetComponent<CapsuleCollider>() != null)
                        {
                            XmlElement capsulecollider = xmlDoc.CreateElement("capsulecollider");

                            XmlElement istrigger = xmlDoc.CreateElement("istrigger");
                            istrigger.InnerText = obj.GetComponent<CapsuleCollider>().isTrigger.ToString();

                            XmlElement center = xmlDoc.CreateElement("center");
                            XmlElement center_x = xmlDoc.CreateElement("x");
                            center_x.InnerText = obj.GetComponent<CapsuleCollider>().center.x + "";
                            XmlElement center_y = xmlDoc.CreateElement("y");
                            center_y.InnerText = obj.GetComponent<CapsuleCollider>().center.y + "";
                            XmlElement center_z = xmlDoc.CreateElement("z");
                            center_z.InnerText = obj.GetComponent<CapsuleCollider>().center.z + "";
                            center.AppendChild(center_x);
                            center.AppendChild(center_y);
                            center.AppendChild(center_z);


                            XmlElement radius = xmlDoc.CreateElement("radius");
                            radius.InnerText = obj.GetComponent<CapsuleCollider>().radius + "";

                            XmlElement height = xmlDoc.CreateElement("height");
                            height.InnerText = obj.GetComponent<CapsuleCollider>().height + "";

                            XmlElement direction = xmlDoc.CreateElement("direction");
                            direction.InnerText = obj.GetComponent<CapsuleCollider>().direction + "";

                            capsulecollider.AppendChild(istrigger);
                            capsulecollider.AppendChild(center);
                            capsulecollider.AppendChild(radius);
                            capsulecollider.AppendChild(height);
                            capsulecollider.AppendChild(direction);
                            gameObject.AppendChild(capsulecollider);


                        }
                        if (obj.GetComponent<MeshCollider>() != null)
                        {
                            XmlElement meshcollider = xmlDoc.CreateElement("meshcollider");
                            XmlElement convex = xmlDoc.CreateElement("convex");
                            convex.InnerText = obj.GetComponent<MeshCollider>().convex + "";

                            XmlElement istrigger = xmlDoc.CreateElement("istrigger");
                            istrigger.InnerText = obj.GetComponent<MeshCollider>().isTrigger.ToString();

                            XmlElement mesh = xmlDoc.CreateElement("mesh");
                            mesh.InnerText = obj.GetComponent<MeshCollider>().sharedMesh.name;
                        }

                            if (obj.GetComponent<MeshRenderer>() != null)
                        {
                            XmlElement meshrenderer = xmlDoc.CreateElement("meshrenderer");

                            XmlElement materials = xmlDoc.CreateElement("materials");
                            XmlElement size = xmlDoc.CreateElement("size");
                            size.InnerText = "1";

                            XmlElement element = xmlDoc.CreateElement("element");
                            element.InnerText = obj.GetComponent<MeshRenderer>().sharedMaterial + "";
                            
                            materials.AppendChild(size);
                            materials.AppendChild(element);

                            XmlElement lighting = xmlDoc.CreateElement("lighting");
                            XmlElement castshadows = xmlDoc.CreateElement("castshadows");
                            castshadows.InnerText = obj.GetComponent<MeshRenderer>().shadowCastingMode.ToString();

                            XmlElement receiveshadows = xmlDoc.CreateElement("receiveshadows");
                            receiveshadows.InnerText = obj.GetComponent<MeshRenderer>().receiveShadows.ToString();

                            lighting.AppendChild(castshadows);
                            lighting.AppendChild(receiveshadows);

                            XmlElement probes = xmlDoc.CreateElement("probes");

                            XmlElement lightprobes = xmlDoc.CreateElement("lightprobes");
                            lightprobes.InnerText = obj.GetComponent<MeshRenderer>().lightProbeUsage+ "";

                            XmlElement reflectionprobes = xmlDoc.CreateElement("reflectionprobes");
                            reflectionprobes.InnerText = obj.GetComponent<MeshRenderer>().reflectionProbeUsage+"";

                            probes.AppendChild(lightprobes);
                            probes.AppendChild(reflectionprobes);

                            XmlElement additionalsettings = xmlDoc.CreateElement("additionalsettings");

                            XmlElement dynamicocclusion = xmlDoc.CreateElement("dynamicocclusion");
                            dynamicocclusion.InnerText = obj.GetComponent<MeshRenderer>().allowOcclusionWhenDynamic.ToString();

                            additionalsettings.AppendChild(dynamicocclusion);

                            meshrenderer.AppendChild(materials);
                            meshrenderer.AppendChild(lighting);
                            meshrenderer.AppendChild(probes);
                            meshrenderer.AppendChild(additionalsettings);
                            gameObject.AppendChild(meshrenderer);

                        }
                        //if (obj.GetComponent<Renderer>() != null)
                        //{
                        //    XmlElement defaultmaterial = xmlDoc.CreateElement("defaultmaterial");
                        //
                        //    defaultmaterial.InnerText = obj.GetComponent<Renderer>().material.ToString();
                        //
                        //    gameObject.AppendChild(defaultmaterial);
                        //
                        //}

                        scenes.AppendChild(gameObject);
                        root.AppendChild(scenes);
                        xmlDoc.AppendChild(root);
                        xmlDoc.Save(filepath);
                    }
                }
            }
            //With refresh datebase of this project otherwise you have to restart unity
            AssetDatabase.Refresh();
            Debug.Log("Scene saved!!");
        }
    }
  
    [MenuItem("Assets/LoadScene")]
    static void LoadScene()
    {
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            DestroyImmediate(obj);
        }
        //where you load your xml file
        string filepath = EditorUtility.OpenFilePanel("load Resource", "", "xml");

        //if there is a xml file being found
        if (File.Exists(filepath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filepath);
            XmlNodeList nodeList = xmlDoc.SelectSingleNode("gameObjects").ChildNodes;
          

            foreach (XmlElement scene in nodeList)
            {
                //check every elements in this xml file
                foreach (XmlElement gameObjects in scene.ChildNodes)
                {
                    //this is the path where you put the prefabs
                    //string asset = "Prefab/" + gameObjects.GetAttribute("name");
       
                    Vector3 pos = Vector3.zero;
                    Vector3 rot = Vector3.zero;
                    Vector3 sca = Vector3.zero;

                    bool tri = false;

                    Vector3 ctx = Vector3.zero;
                    Vector3 sx = Vector3.zero;
                    string meshname = "";
                    Mesh meshtype = new Mesh();
                    Material material = new Material(Shader.Find("Diffuse"));

                    float radius = 0.0f;
                    float height = 0.0f;
                    int direction = 0;

                    bool convex = false;
                    ShadowCastingMode mode = ShadowCastingMode.On;
                    bool receiveshadows = true;
                    GameObject go = new GameObject();
                    foreach (XmlElement transform in gameObjects.ChildNodes)
                    {
                       
                        foreach (XmlElement transformValue in transform.ChildNodes)
                        {

                            if (transformValue.Name == "position")
                            {
                                foreach (XmlElement position in transformValue.ChildNodes)
                                {
                                    switch (position.Name)
                                    {
                                        case "x":
                                            pos.x = float.Parse(position.InnerText);
                                            break;
                                        case "y":
                                            pos.y = float.Parse(position.InnerText);
                                            break;
                                        case "z":
                                            pos.z = float.Parse(position.InnerText);
                                            break;
                                    }
                                }
                            }
                            else if (transformValue.Name == "rotation")
                            {
                                foreach (XmlElement rotation in transformValue.ChildNodes)
                                {
                                    switch (rotation.Name)
                                    {
                                        case "x":
                                            rot.x = float.Parse(rotation.InnerText);
                                            break;
                                        case "y":
                                            rot.y = float.Parse(rotation.InnerText);
                                            break;
                                        case "z":
                                            rot.z = float.Parse(rotation.InnerText);
                                            break;
                                    }
                                }
                            }
                            else if (transformValue.Name == "scale")
                            {
                                foreach (XmlElement scale in transformValue.ChildNodes)
                                {
                                    switch (scale.Name)
                                    {
                                        case "x":
                                            sca.x = float.Parse(scale.InnerText);
                                            break;
                                        case "y":
                                            sca.y = float.Parse(scale.InnerText);
                                            break;
                                        case "z":
                                            sca.z = float.Parse(scale.InnerText);
                                            break;
                                    }
                                }
                            }
                        }

              
                    }
                    foreach (XmlElement boxcollider in gameObjects.ChildNodes)
                    {
                        foreach(XmlElement boxcolliderValue in boxcollider.ChildNodes)
                        {
                            if (boxcolliderValue.Name == "istrigger")
                            {
                              
                                    tri = bool.Parse(boxcolliderValue.InnerText);
                                
                            }
                            if (boxcolliderValue.Name == "center")
                            {
                                foreach (XmlElement center in boxcolliderValue.ChildNodes)
                                {
                                    switch (center.Name)
                                    {
                                        case "x":
                                            ctx.x = float.Parse(center.InnerText);
                                            break;
                                        case "y":
                                            ctx.y = float.Parse(center.InnerText);
                                            break;
                                        case "z":
                                            ctx.z = float.Parse(center.InnerText);
                                            break;
                                    }
                                }
                            }

                            if (boxcolliderValue.Name == "size")
                            {
                                foreach (XmlElement bsize in boxcolliderValue.ChildNodes)
                                {
                                    switch (bsize.Name)
                                    {
                                        case "x":
                                            sx.x = float.Parse(bsize.InnerText);
                                            break;
                                        case "y":
                                            sx.y = float.Parse(bsize.InnerText);
                                            break;
                                        case "z":
                                            sx.z = float.Parse(bsize.InnerText);
                                            break;
                                    }
                                }
                            }



                        }

                    }
                    foreach (XmlElement spherecollider in gameObjects.ChildNodes)
                    {
                        foreach (XmlElement spherecolliderValue in spherecollider.ChildNodes)
                        {
                            if (spherecolliderValue.Name == "istrigger")
                            {

                                tri = bool.Parse(spherecolliderValue.InnerText);

                            }
                            if (spherecolliderValue.Name == "center")
                            {
                                foreach (XmlElement center in spherecolliderValue.ChildNodes)
                                {
                                    switch (center.Name)
                                    {
                                        case "x":
                                            ctx.x = float.Parse(center.InnerText);
                                            break;
                                        case "y":
                                            ctx.y = float.Parse(center.InnerText);
                                            break;
                                        case "z":
                                            ctx.z = float.Parse(center.InnerText);
                                            break;
                                    }
                                }
                            }

                            if (spherecolliderValue.Name == "radius")
                            {
                                radius = float.Parse(spherecolliderValue.InnerText);
                            }



                        }

                    }

                    foreach (XmlElement capsulecollider in gameObjects.ChildNodes)
                    {
                        foreach (XmlElement capsulecolliderValue in capsulecollider.ChildNodes)
                        {
                            if (capsulecolliderValue.Name == "istrigger")
                            {

                                tri = bool.Parse(capsulecolliderValue.InnerText);

                            }
                            if (capsulecolliderValue.Name == "center")
                            {
                                foreach (XmlElement center in capsulecolliderValue.ChildNodes)
                                {
                                    switch (center.Name)
                                    {
                                        case "x":
                                            ctx.x = float.Parse(center.InnerText);
                                            break;
                                        case "y":
                                            ctx.y = float.Parse(center.InnerText);
                                            break;
                                        case "z":
                                            ctx.z = float.Parse(center.InnerText);
                                            break;
                                    }
                                }
                            }

                            if (capsulecolliderValue.Name == "radius")
                            {
                                radius = float.Parse(capsulecolliderValue.InnerText);
                            }
                            if (capsulecolliderValue.Name == "height")
                            {
                                height = float.Parse(capsulecolliderValue.InnerText);
                            }
                            if (capsulecolliderValue.Name == "direction")
                            {
                                direction = int.Parse(capsulecolliderValue.InnerText);
                            }

                        }

                    }

                    foreach (XmlElement meshcollider in gameObjects.ChildNodes)
                    {
                        foreach (XmlElement meshcolliderValue in meshcollider.ChildNodes)
                        {
                            if (meshcolliderValue.Name == "istrigger")
                            {

                                tri = bool.Parse(meshcolliderValue.InnerText);

                            }

                            if (meshcolliderValue.Name == "convex")
                            {
                                convex = bool.Parse(meshcolliderValue.InnerText);
                            }
                            if (meshcolliderValue.Name == "mesh")
                            {
                                if (meshcolliderValue.InnerText == "Plane")
                                {
                                    GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                                    meshtype = plane.GetComponent<MeshCollider>().sharedMesh;
                                    DestroyImmediate(plane);
                                }
                            }
                            
                        }

                    }
                    foreach (XmlElement meshfilter in gameObjects.ChildNodes)
                    {
                        foreach (XmlElement meshfilterValue in meshfilter.ChildNodes)
                        {
                            if (meshfilterValue.Name == "mesh")
                            {
                                
                                if (meshfilterValue.InnerText == "Cube")
                                {
                                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                    meshtype = cube.GetComponent<MeshFilter>().sharedMesh;
                                    DestroyImmediate(cube);
                                }

                                if (meshfilterValue.InnerText == "Sphere")
                                {
                                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                    meshtype = cube.GetComponent<MeshFilter>().sharedMesh;
                                    DestroyImmediate(cube);
                                }

                                if (meshfilterValue.InnerText == "Capsule")
                                {
                                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                                    meshtype = cube.GetComponent<MeshFilter>().sharedMesh;
                                    DestroyImmediate(cube);
                                }
                                if (meshfilterValue.InnerText == "Cylinder")
                                {
                                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                                    meshtype = cube.GetComponent<MeshFilter>().sharedMesh;
                                    DestroyImmediate(cube);
                                }

                                if (meshfilterValue.InnerText == "Plane")
                                {
                                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Plane);
                                    meshtype = cube.GetComponent<MeshFilter>().sharedMesh;
                                    DestroyImmediate(cube);
                                }
                            }
                        }
                           
                    }
                     foreach (XmlElement meshrenderer in gameObjects.ChildNodes)
                     {
                        foreach (XmlElement meshrendererValue in meshrenderer.ChildNodes)
                        {

                            if (meshrendererValue.Name == "lighting")
                            {
                                foreach (XmlElement lighting in meshrendererValue.ChildNodes)
                                {
                                    if(lighting.InnerText == "castshadows")
                                    {
                                        //CastShawdows = ;
                                        //System.Enum.Parse(lighting.Name);
                                         mode = (ShadowCastingMode)Enum.Parse(typeof(ShadowCastingMode), lighting.InnerText);
                                    }

                                    if(lighting.InnerText == "receiveshadows")
                                    {
                                        receiveshadows = bool.Parse(lighting.InnerText);
                                    }
        
                                }
                            }
                            if (meshrendererValue.Name == "materials")
                            {
                                foreach (XmlElement materials in meshrendererValue.ChildNodes)
                                {
                                    if (!string.IsNullOrEmpty(materials.InnerText))
                                    {
                                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                        material = cube.GetComponent<MeshRenderer>().sharedMaterial;
                                        DestroyImmediate(cube);
                                        //sharedMaterialname = materials.InnerText;
                                    }
                                    
                                }
                            }
                        }
                            
                    
                     }


                    go.name = gameObjects.GetAttribute("name");
                    go.transform.position = pos;
                    //clone a new gameobject (loaded position, rotation, and scale value from xml file)
                    go.transform.localScale = sca;
                    if(go.name == "Cube")
                    {
                        go.AddComponent<BoxCollider>();
                        go.GetComponent<BoxCollider>().center = ctx;
                        go.GetComponent<BoxCollider>().size = sx;
                        go.GetComponent<BoxCollider>().isTrigger = tri;
                    }
                    if (go.name == "Sphere")
                    {
                        go.AddComponent<SphereCollider>();
                        go.GetComponent<SphereCollider>().center = ctx;
                        go.GetComponent<SphereCollider>().radius = radius;
                        go.GetComponent<SphereCollider>().isTrigger = tri;
                    }
                    if(go.name == "Capsule" || go.name== "Cylinder")
                    {
                        go.AddComponent<CapsuleCollider>();
                        go.GetComponent<CapsuleCollider>().center = ctx;
                        go.GetComponent<CapsuleCollider>().radius = radius;
                        go.GetComponent<CapsuleCollider>().isTrigger = tri;
                        go.GetComponent<CapsuleCollider>().height = height;
                        go.GetComponent<CapsuleCollider>().direction = direction;
                    }

                    if (go.name == "Plane")
                    {
                        go.AddComponent<MeshCollider>();
                        go.GetComponent<MeshCollider>().convex = convex;
                        go.GetComponent<MeshCollider>().sharedMesh = meshtype;
                        go.GetComponent<MeshCollider>().isTrigger = tri;
                    }

                    go.AddComponent<MeshFilter>();
                    go.GetComponent<MeshFilter>().sharedMesh = meshtype; 
                    Debug.Log(meshname);
                    go.AddComponent<MeshRenderer>();
                    go.GetComponent<MeshRenderer>().receiveShadows = receiveshadows;
                    go.GetComponent<MeshRenderer>().shadowCastingMode = mode;
                    go.GetComponent<MeshRenderer>().sharedMaterial = material;



                    //.name = sharedMaterialname;

                }

            }
        }
    }
   
}