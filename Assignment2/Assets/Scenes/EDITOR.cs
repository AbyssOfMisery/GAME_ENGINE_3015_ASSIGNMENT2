using UnityEngine;
using UnityEditor;
using System.Xml;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class EDITOR : Editor
{

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
            Debug.Log(UnityEditor.EditorBuildSettings.scenes[0]);
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
                       
                        if (obj.transform.parent == null)
                        {
                         
                            XmlElement gameObject = xmlDoc.CreateElement("gameObjects");
                            //get this object's name
                            gameObject.SetAttribute("name", obj.name);
                            
                            //set up a prefab name
                            gameObject.SetAttribute("asset", obj.name + ".prefab");

                            PrefabUtility.SaveAsPrefabAsset(obj, "Assets/Resources/Prefab/" + obj.name + ".prefab");
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
                            scenes.AppendChild(gameObject);
                            root.AppendChild(scenes);
                            xmlDoc.AppendChild(root);
                            xmlDoc.Save(filepath);

                        }
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
                    string asset = "Prefab/" + gameObjects.GetAttribute("name");
                    Debug.Log(asset);
                    Vector3 pos = Vector3.zero;
                    Vector3 rot = Vector3.zero;
                    Vector3 sca = Vector3.zero;
                  
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
                        //clone a new gameobject (loaded position, rotation, and scale value from xml file)
                        GameObject go = (GameObject)Instantiate(Resources.Load(asset), pos, Quaternion.Euler(rot));
                        go.transform.localScale = sca;
                        go.name = gameObjects.GetAttribute("name");
                    }
                }
            }
        }
    }
   
}