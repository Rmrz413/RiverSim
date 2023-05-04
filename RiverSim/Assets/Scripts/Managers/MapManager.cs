using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : SingletonPersistent<MapManager>
{
    [SerializeField] private GameObject holder;
    [SerializeField] private ToggleGroup tgroup;
    [SerializeField] private GameObject toggleprefab;
    [SerializeField] private SettingsManager settingsHolder;
    [SerializeField] private Texture2D[] defaultMaps;

    private Object[] textures;
    private List<GameObject> selectables;
    private string path = "./Maps";

    protected override void Awake()
    {
        base.Awake();

        if (tgroup == null)
        {
            tgroup = transform.GetComponent<ToggleGroup>();
        }
        selectables = new List<GameObject>();

        Reload();
    }

    private void Start()
    {
        if(settingsHolder == null) settingsHolder = GameObject.FindObjectOfType<SettingsManager>();
    }

    public void Reload()
    {
        Clear();
        Load_Default();
        Load();
    }

    private void Load_Default()
    {
        foreach (Texture2D tex in defaultMaps)
        {
            GameObject go = Instantiate(toggleprefab, holder.transform, false);
            selectables.Add(go);
            go.name = tex.name.Replace(".exr", "");
            Toggle tg = go.GetComponent<Toggle>();
            tg.group = tgroup;
            go.GetComponentInChildren<RawImage>().texture = tex; // TODO NULL check
            tg.onValueChanged.AddListener(v => 
            { 
                if (v) settingsHolder.SelectMap(go.GetComponentInChildren<RawImage>().texture as Texture2D);
                else settingsHolder.DeselectMap();
            });
        }
    }

    private void Load()
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        List<string> filepaths = new List<string>();
        filepaths.AddRange(Directory.GetFiles(path, "*.png"));
        filepaths.AddRange(Directory.GetFiles(path, "*.jpg"));
        
        foreach (var path in filepaths)
        {
            byte[] imageData = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(1, 1);
            tex.LoadImage(imageData);
            tex.name = Path.GetFileName(path).Replace(".png", "").Replace(".jpg","");
            GameObject go = Instantiate(toggleprefab, holder.transform, false);
            selectables.Add(go);
            go.name = tex.name;
            Toggle tg = go.GetComponent<Toggle>();
            tg.group = tgroup;
            go.GetComponentInChildren<RawImage>().texture = tex; // TODO NULL check
            tg.onValueChanged.AddListener(v => 
            { 
                if (v) settingsHolder.SelectMap(go.GetComponentInChildren<RawImage>().texture as Texture2D);
                else settingsHolder.DeselectMap();
            });
        }
    }

    private void Clear()
    {
        foreach (GameObject item in selectables)
        {
            Destroy(item);
        }
        selectables.Clear();
    }
}
