using UnityEngine;
using System.IO;
using Juto.UI;
using Juto;
using UnityEngine.Events;


public class App : Singleton<App>
{
    // (Optional) Prevent non-singleton constructor use.
    protected App() { }



    public bool isPlaying = true;
    public bool isNewGame = false;

    public Settings settings;
    public PoolManager poolManager;
    public UIManager uiManager;
    public ShakeBehavior shake;
    public KeyTimeEvent keyTimeEvent;
    public KeyManager keyManager;
    public Inventory inventory;

    public Canvas canvas;

    private void Start()
    {
        poolManager = FindObjectOfType<PoolManager>();
        uiManager = FindObjectOfType<UIManager>();
        shake = FindObjectOfType<ShakeBehavior>();
        keyTimeEvent = FindObjectOfType<KeyTimeEvent>();
        keyManager = new KeyManager();
        inventory = FindObjectOfType<Inventory>();

        Load();
    }


    void OnApplicationFocus(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Save();
        }
        else
        {
            Load();
        }
    }


    public void Save()
    {
        Serialization.Save(Application.persistentDataPath + "/settings.json", settings);
    }

    public void Load()
    {
            
        settings = new Settings();

        if (File.Exists(Application.persistentDataPath + "/settings.json"))
        {
            settings = Serialization.Load<Settings>(Application.persistentDataPath + "/settings.json");
        }
        else
        {
            isNewGame = true;
        }
    }

    private void OnDestroy()
    {
        Save();
    }
}
