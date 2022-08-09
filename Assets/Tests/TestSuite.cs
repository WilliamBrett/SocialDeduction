using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEditor;
using System.Linq;
using UnityEditor.SceneManagement;

public class TestSuite
{
    
    public CoreGameScript CoreScript;
    private GameObject[] CountOfHeads; //used to count "Avatar" items to be assigned to Actors
    private GameObject Textbox; //The object that text is written to


    public void SetReferences()
    {
        CoreScript = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<CoreGameScript>();
        Textbox = GameObject.FindGameObjectWithTag("Textbox"); //Textbox is setup
        CountOfHeads = GameObject.FindGameObjectsWithTag("Actor"); //CountOfHeads is setup
    }
    public void InitializeGame()
    {
        Textbox.GetComponent<TextboxScript>().Start(); 
        CoreScript.Initialize();
        CoreScript.Start();
    }
    /*private static void SetGameScene()
    {
        //int GameSceneIndex = 1;
        //SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);
        //SceneManager.LoadSceneAsync(1,LoadSceneMode.Additive);
        //SceneManager.LoadScene("GameScene");
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameScene"));
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }*/


    /*public void referenceEventSystem()
    {
        GameObject EventSystem = GameObject.FindGameObjectWithTag("Textbox");
    }*/


    /*[Test]
    public void DebugTest()
    {
        SetCoreScript();
        Assert.Equals(1, CoreScript.debug);

    }*/

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        EditorSceneManager.LoadSceneInPlayMode("Assets/Scenes/GameScene.unity", new LoadSceneParameters(LoadSceneMode.Single));
        //SetReferences();
    }
    
    [UnityTest]
    public IEnumerator VerifyTextbox()
    {
        //SetReferences();
        Assert.AreEqual(CoreScript.getTextbox(), Textbox);
        yield return null;
    }

    [UnityTest]
    public IEnumerator VerifyExists()
    {
        Assert.NotNull(CoreScript);
        yield return null;

    }

    [Test]
    public void OnePlusTwoEqualsFour()
    {
        //_ = Assert.Equals("1", "1");//fault located
        Assert.AreEqual(1, 1);
    }

    /*[Test]//[Test, Scene("GameScene")]
    public void BasicGameObjectFound()
    {
        //an even more basic test used for diagnosing SetCoreGameScript();
        //SetGameScene();
        Assert.NotNull(GameObject.FindWithTag("EventSystem"));
    }*/
    
    [UnityTest]
    public IEnumerator UnityTestTest()
    {
        Assert.NotNull(GameObject.FindWithTag("EventSystem"));
        yield return null;
    }

    [UnityTest]
    public IEnumerator UnityStartTest()
    {
        CoreScript = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<CoreGameScript>();
        CoreScript.Initialize();
        CoreScript.Start();
        Textbox = GameObject.FindGameObjectWithTag("Textbox"); //Textbox is setup
        Textbox.GetComponent<TextboxScript>().Start();
        Assert.AreEqual(CoreScript.getTextbox(), Textbox);
        yield return null;
    }

    [UnityTest]
    public IEnumerator SetupTextboxTest()
    {
        SetReferences();
        InitializeGame();
        Assert.AreEqual(CoreScript.getTextbox(), Textbox);
        yield return null;
    }

    [UnityTest]
    public IEnumerator SetupPlayersTest()
    {
        SetReferences();
        InitializeGame();
        Assert.AreEqual(12, CountOfHeads.Length);
        yield return null;
    }


    /*[Test]
    public void SceneLoaded()
    {
        //EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/GameScene.unity", new LoadSceneParameters(LoadSceneMode.Single));
        //SceneManager.LoadScene(1);
        //SceneManager.LoadScene("GameScene");
        //SceneManager.SetActiveScene("GameScene");
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
        Assert.AreEqual("GameScene", SceneManager.GetActiveScene().name);
    }*/
    /*[Test]
    public void SceneLoaded2()
    {
        //EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/GameScene.unity", new LoadSceneParameters(LoadSceneMode.Single));
        //SceneManager.LoadScene(1);
        //SceneManager.LoadScene("GameScene");
        //SceneManager.SetActiveScene("GameScene");
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
        Assert.AreEqual("GameScene", SceneManager.GetActiveScene().name);
    }*/

    /*[Test]
    public void BasicDelayTest()
    {
        //an EXTREMELY basic test to verify SetCoreGameScript() works
        SetCoreGameScript();
        Assert.Equals(CoreScript.PhaseDelay, CoreScript.ShortDelay);
    }*/


    /*[Test]
    public void InitializeTest()
    {
        //More complicated test might require CoreGameScript or Textbox to be defined, so their defining it tested seperetly
        SetCoreGameScript();
        SetTextbox();
        Assert.Equals(CoreScript.getTextbox(), Textbox);
    }*/


    /*[Test]
    public void TextboxTest()
    {
        SetCoreGameScript();
        SetTextbox();
        CoreScript.TextboxAppend("Test");
        Assert.Equals(Textbox.GetComponent<TextMeshProUGUI>().text, "Test");
    }*/
    
    // A Test behaves as an ordinary method
    /*[Test]
    public void TestingTestScriptSimplePasses()
    {
        //Assert.Equals(1, debug);// Use the Assert class to test conditions
    }*/

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    /*[UnityTest]
    public IEnumerator TestingTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }*/
}
