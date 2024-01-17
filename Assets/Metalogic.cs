using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Metalogic : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private InventoryManager inventory;
    [SerializeField] private GameObject airplane;
    [SerializeField] private GameObject enemyHolder;
    [SerializeField] private DayNightCycle dayNightCycle;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private FadeToBlack uiFade;

    [Header("Alerts")]
    [SerializeField] private Text uiNightAlert;
    [SerializeField] private Text uiDayAlert;
    [SerializeField] private Text uiAttackAlert;
    [SerializeField] private AudioSource warningSound;
    [SerializeField] private Gradient AlertGradient;

    public bool peaceful = false;
    public static float Volume = 0.7f;
    [SerializeField] private AnimationCurve difficultyCurve;
    // How many enemies to spawn based on repair progress
    [SerializeField] private GameObject enemyPrefab1;
    [SerializeField] private GameObject enemyPrefab2;
    [SerializeField] private float spawnDist = 150;
    [SerializeField] private float spawnHeight = 12;

    // enemy prefab
    struct SpawnData{
        public GameObject enemy;
        public Vector3 spawnLocation;
        public float time;
    }
    private List<SpawnData> spawnList = new List<SpawnData>();

    void Start(){
        PauseMenu.gameIsPaused = false;
        difficultyCurve = PersistentData.difficultyCurve;
        onVolumeChange(PersistentData.volume);
        onMouseSensitivityChange(PersistentData.mouseSensitivity);
    }

    public void doPlayerDeath(){
        uiFade.DoFade(true, 1f);
        StartCoroutine(executeAfterTime(playerDeathReset, 2.5f));
    }

    IEnumerator executeAfterTime(Action func, float time)
    {
        yield return new WaitForSeconds(time);
        func();
    }

    // Makes all enemy corpses disappear
    public void clearCorpses(){
        foreach (Transform enemy in enemyHolder.transform) {
            if (enemy.GetComponent<Spider>().isDead)
                enemy.GetComponent<Spider>().fallThroughGround();
        }
    }

    void playerDeathReset(){
        
        // reset player position
        // have to disable CC before teleport
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = playerSpawnPoint.transform.position;
        player.transform.rotation = playerSpawnPoint.transform.rotation;
        player.GetComponent<CharacterController>().enabled = true;
        
        // Reset isDead bool and refill HP
        player.GetComponent<PlayerHealth>().reset();

        // do damage to plane in lunatic mode
        if (PersistentData.difficulty == "Lunatic")
            airplane.GetComponent<IDamageable>().OnHit(15f);

        // take away all wood
        inventory.WoodCount = 0;
        
        // change time
        dayNightCycle.time = 0.3f;
        dayNightCycle.dayCount += 1;

        // clear out enemies
        foreach (Transform child in enemyHolder.transform) {
            GameObject.Destroy(child.gameObject);
        }

        // make screen visible
        uiFade.DoFade(false, 1f);
    }

    void gotoWinScreen(){
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Ending", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
    void gotoMainMenu(){
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Title", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void showNightAlert(){
        StartCoroutine(AlertFade(uiNightAlert));
    }
    public void showDayAlert(int dayNum){
        uiDayAlert.text = "Day " + dayNum;
        StartCoroutine(AlertFade(uiDayAlert));
    }

    public void showPlaneDamageAlert(){
        //uiAttackAlert.text = "Your plane is being damaged!";
        if (uiAttackAlert.color.a < 0.2f && player.transform.position.magnitude > 40f) warningSound.Play();
        StartCoroutine(AlertFade(uiAttackAlert));
    }

    IEnumerator AlertFade(Text uiItem, float dur = 4f){

        // loop over 1 second backwards
        for (float i = 0; i <= 1; i += Time.deltaTime/dur)
        {
            // set color with i as alpha
            uiItem.color = AlertGradient.Evaluate(i);
            yield return null;
        }
        uiItem.color = AlertGradient.Evaluate(1f);
    }

    void Update(){
        // generate list
        if (spawnList.Count==0 && dayNightCycle.time > 0.7f && dayNightCycle.time < 0.75f){
            generateSpawnList();
        }

        // do spawns
        if (spawnList.Count>0 && !peaceful && (
            (spawnList[0].time <= dayNightCycle.time)
            || (dayNightCycle.time < 0.1)
            )){
            // spawn enemy and remove them from the list
            spawnEnemy(spawnList[0]);
            spawnList.RemoveAt(0);
        }

        // keep player within bounds
        if (player.transform.position.magnitude > 200)
            // instakill
            player.GetComponent<PlayerHealth>().OnHit(1000);
    }

    public void triggerWin(){
        // make sure we don't die
        player.GetComponent<PlayerHealth>().isInvincible = true;

        PersistentData.endDays = dayNightCycle.dayCount;
        if (dayNightCycle.time > 0.25) PersistentData.endDays++;
        PersistentData.endNights = dayNightCycle.dayCount;
        if (dayNightCycle.time > 0.75f) PersistentData.endNights++;
        
        uiFade.DoFade(true, 2f);
        StartCoroutine(executeAfterTime(gotoWinScreen, 2.5f));
    }
    public void triggerMainMenu(){
        // make sure we don't die
        player.GetComponent<PlayerHealth>().isInvincible = true;
        
        uiFade.DoFade(true, 2f);
        StartCoroutine(executeAfterTime(gotoMainMenu, 2.5f));
    }
    public void triggerQuit(){
        // make sure we don't die
        player.GetComponent<PlayerHealth>().isInvincible = true;
        
        uiFade.DoFade(true, 2f);
        StartCoroutine(executeAfterTime(Application.Quit, 2.5f));
    }

    public void onMouseSensitivityChange(float value){
        //Debug.Log(value);
        PersistentData.mouseSensitivity = value;
        player.GetComponentInChildren<MouseLook>().mouseSensitivity = 2*value + 0.6f;
    }
    public void onVolumeChange(float value){
        PersistentData.volume = value;
        AudioListener.volume = value;
    }

    void generateSpawnList(){
        //Debug.Log("Generating list...");
        if (spawnList.Count > 0) {
            Debug.LogError("Spawnlist was not empty before generation!");
            spawnList.Clear();
        }
        int numSpawns = Mathf.RoundToInt(difficultyCurve.Evaluate(airplane.GetComponent<Airplane>().lifePercent));
        List<float> spawnTimes = new List<float>();
        for (int i=0; i<numSpawns; i++){
            spawnTimes.Add(UnityEngine.Random.Range(0f, 0.25f)+0.75f);
        }
        spawnTimes.Sort();
        foreach(float time in spawnTimes){
            spawnList.Add(generateSpawnData(time));
            //Debug.Log("spawnlist length: " + spawnList.Count);
        }
    }
    SpawnData generateSpawnData(float time){
        //Debug.Log("Generating spawn " + time.ToString());
        SpawnData data = new SpawnData();
        data.time = time;
        data.enemy = UnityEngine.Random.value > 0.5f ? enemyPrefab1 : enemyPrefab2;
        data.spawnLocation = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 360f), Vector3.up) * Vector3.right * spawnDist + Vector3.up * spawnHeight;
        //Debug.Log("data gen complete");
        return data;
    }

    void spawnEnemy(SpawnData data){
        //Debug.Log("spawn at " + data.spawnLocation.ToString());
        var newEnemy = Instantiate(data.enemy, data.spawnLocation, Quaternion.identity);
        newEnemy.transform.parent = enemyHolder.transform;
    }
}
