using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;
using MoreMountains.NiceVibrations;

public class GameManager : MonoBehaviour
{
    #region VARIABLES

    //GENERAL
    public static GameManager instance;
    public Camera mainCamera;
    public bool canTouch;
    private bool firstOpen = true;
    public Flow script_Flow;
    public float realSpeed;
    [SerializeField] private GameObject slowCube;

    //GENERATE LEVEL
    private float cubeScaleX, cubeScaleY, cubeScaleZ;
    [SerializeField] private Transform startUpPoint, endUpPoint;
    [SerializeField] private GameObject prefab_level;
    public GameObject level;
    [SerializeField] private Material[] materials;
    [SerializeField] private GameObject[] prefab_figureCubeModels;
    private int alwaysOpenFigureCount = 1;
    public Level[] levels;
    private List<Material> list_levelMaterials = new List<Material>();
    private List<GameObject> list_levelFigureCubes = new List<GameObject>();
    private List<GameObject> list_levelPowerUpCubes = new List<GameObject>();

    [SerializeField] private GameObject prefab_cubeNoFigure;
    [SerializeField] private GameObject prefab_theBombCube;
    [SerializeField] private GameObject prefab_theCatCube;
    private int spawnedPareOfCubeCount;
    int figureCount;
    int figureCubePosition;
    bool isFigureCubeSpawned;
    int cubeCountInLevel;
    int partitionQuantityForFigureCubes;
    int spawnedFigureCubeCount;

    GameObject willSpawnCube;

    private int willSpawnBombCubeCount;
    private int willSpawnCatCubeCount;

    private int spawnedPowerUpCubeCount;

    private int bombCubeLevel = 5;
    private int moreBombCubeLevel = 13;

    private int catCubeLevel = 11;
    private int moreCatCubeLevel = 21;

    private int bombAndCatCubesLevel = 29;

    private int cubeCountInLevelWithoutFigureCubes;
    private int totalPowerUpCubesCount;
    private int partitionQuantityForPowerUpCubes;
    private bool isPowerUpCubeSpawned;
    private int powerUpCubePosition;
    private int spawnedPareOfCubeCountWithoutFigureCubes;


    private List<int> positions = new List<int>();
    private int maxPosition;
    [SerializeField] private GameObject prefab_finalObject;

    //DEFAULT VALUES
    public Material[] backgroundMaterials;
    private Level currentLevel;
    private GameObject finalObject;
    [SerializeField] private GameObject wall;
    [SerializeField] private Renderer renderer_backgroundWall;

    //MATCHING
    public GameObject[] prefab_combos;
    private float timer;
    public GameObject combos;
    public GameObject points;
    [SerializeField] private GameObject pawForPawCube;
    public Vector3 combothreshold;

    public Text lastScore;
    public int totalScore;

    [SerializeField] private GameObject particle_trueMatch;

    //CHOOSE CUBE
    private LayerMask layer_cube;
    private Cube choosedCube_1;
    private Cube choosedCube_2;

    //FINISH CONTROLS
    private Cube[] array_script_cubes;
    public List<Cube> list_script_cubes = new List<Cube>();
    public bool isFinished;

    //LEVEL COMPLETED
    public GameObject springInFinalObject;
    [SerializeField] private GameObject completedConfetti;
    public bool levelCompleted;

    //LEVEL FAILED
    public bool levelFailed;

    //POW SYSTEM
    [SerializeField] private Transform targetPati;
    public Animation myanimaton;
    private Vector3 targetPosition;


    //Sound Effects
    [SerializeField] private AudioSource CorrectTouch;
    public AudioSource CubeDropSound;
    [SerializeField] private AudioSource LoseSound;
    [SerializeField] private AudioSource WrongMatch;
    [SerializeField] private AudioSource CubeTouch;

    //TUTORIAL
    //  public Tutorial script_tutorial;
    public TutorialHowToPlay script_tutorialHowToPlay;
    public TutorialWrongMatch script_tutorialWrongMatch;
    public GameObject tutorialHand;
    public GameObject effectCircle;

    //TEMP, WILL BE DELETED
    public Button tapToConutinue;
    public Button tapToConutinue2;
    public Button awardButton;
    private int matchingCount;

    //POWER-UPS
    private bool isCatMatched;
    private int matchedCatCount;
    private PowerUps script_powerUps;
    private List<Material> list_levelMaterialsWithoutPowerUps = new List<Material>();
    private List<Cube> list_bombCubesInlevel = new List<Cube>();
    [SerializeField] private GameObject particle_bombEffect;

    public GameObject hand;
    #endregion

    private void Awake()
    {
        instance = this;
        targetPosition = targetPati.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        script_tutorialWrongMatch = GetComponent<TutorialWrongMatch>();
        script_powerUps = GetComponent<PowerUps>();

        mainCamera = Camera.main;

        layer_cube = LayerMask.GetMask("Cube", "CubeForOutline");
        canTouch = false;
        totalScore = PlayerPrefs.GetInt("Score");
        lastScore.text = totalScore.ToString();

        cubeScaleX = cubeScaleY = cubeScaleZ = 1f;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canTouch)
        {
            Choosing();
        }
        timer += Time.deltaTime;

        //  hand.transform.position = list_script_cubes[0].transform.position + new Vector3(0, 0, -2f);
    }

    #region Choosing Methods

    private void Choosing()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer_cube))
        {
            if (choosedCube_1)
            {
                if (!hit.collider.GetComponentInParent<Cube>().isPressible)
                {
                    return;
                }

                if (hit.collider.GetComponentInParent<Cube>() != choosedCube_1)
                {
                    ChooseCube(ref choosedCube_2, hit.collider.GetComponentInParent<Cube>(), false);
                    CompareColor(choosedCube_1, choosedCube_2);

                    choosedCube_1 = null;
                    choosedCube_2 = null;

                    CubeTouch.Stop();
                }
                else
                {
                    UnchooseCube(ref choosedCube_1);
                }
            }
            else
            {
                if (!hit.collider.GetComponentInParent<Cube>().isPressible)
                {
                    return;
                }

                ChooseCube(ref choosedCube_1, hit.collider.GetComponentInParent<Cube>(), true);
            }
        }
    }

    private void ChooseCube(ref Cube whichCube, Cube hitCube, bool sound)
    {
        if (!hitCube.isPressible)
        {
            return;
        }

        whichCube = hitCube;
        whichCube.isChoosen = true;
        MMVibrationManager.Haptic(HapticTypes.HeavyImpact, false);

        if (sound)
        {
            CubeTouch.Play();
        }
        whichCube.outlineFilterObj.OutlineEnable();
    }

    private void UnchooseCube(ref Cube whichCube)
    {
        whichCube.outlineFilterObj.OutlineDisable();
        whichCube.isChoosen = false;
        whichCube = null;
        timer = 0f;


    }

    #endregion

    private void CompareColor(Cube cube1, Cube cube2)
    {
        if (cube1.color == cube2.color && cube1.figureName == cube2.figureName)
        {
            //TrueMatchScale(cube1, cube2);
            //  TrueMatchParticle(cube1, cube2);
            if (cube1.isSpecialCube)
            {
                if (cube1.bombCube)
                {
                    print("bombcube");
                    StartCoroutine(script_powerUps.BombSameColorsCube(list_script_cubes, cube1.color, cube1, cube2));
                }
                else if (cube1.catCube)
                {
                    isCatMatched = true;
                    matchedCatCount++;
                    StartCoroutine(script_powerUps.CatSpargePaws(list_script_cubes, cube1, cube2));
                }
            }
            TrueMatchRotation(cube1, cube2);
        }
        else
        {
            //yanlis eslesme
            if ((PlayerPrefs.GetInt("WrongMatchTutorial") == 0))
            {
                WrongMatchTutorial(cube1, cube2);
                return;
            }
            else
            {
                WrongMatchShakeCamera();
                WrongMatch.Play();
                MMVibrationManager.Haptic(HapticTypes.Failure, false);
                timer = 0f;

                StartCoroutine(OutlineDisable(cube1));
                StartCoroutine(OutlineDisable(cube2));
            }



            // WrongMatchShakeCamera();
        }
        choosedCube_1 = null;
        choosedCube_2 = null;
    }

    #region True Match Methods

    private void TrueMatchScale(Cube cube1, Cube cube2)
    {
        cube1.transform.DOScale(Vector3.zero, .5f);
        cube2.transform.DOScale(Vector3.zero, .5f);

        if (FinishControl(cube1, cube2))
        {
            LevelCompleted();
        }

    }

    private void TrueMatchParticle(Cube cube1, Cube cube2)
    {
        particle_trueMatch.transform.GetChild(2).GetComponent<Renderer>().sharedMaterial.SetColor("_TintColor", cube1.color);

        GameObject particle1 = Instantiate(particle_trueMatch, cube1.transform.position, Quaternion.identity);
        GameObject particle2 = Instantiate(particle_trueMatch, cube2.transform.position, Quaternion.identity);

        matchingCount++;
        CorrectTouch.Play();
        PawPoint(cube1, cube2);


        ComboMatch(cube1, cube2);

        if (FinishControl(cube1, cube2))
        {
            LevelCompleted();
        }

        Destroy(cube1.gameObject);
        Destroy(cube2.gameObject);

        Destroy(particle1, 2f);
        Destroy(particle2, 2f);


    }

    private void TrueMatchRotation(Cube cube1, Cube cube2)
    {
        cube1.isMatched = true;
        cube2.isMatched = true;

        cube1.childCube.transform.DORotate(new Vector3(0, 360f, 0), .2f, RotateMode.LocalAxisAdd).OnComplete(() =>
        {
            cube1.childCube.transform.DOScale(cube1.childCube.transform.localScale * 1.2f, .1f).OnComplete(() =>
              {
                  cube1.transform.DOScale(Vector3.zero, .2f).OnComplete(() =>
                  {
                      Destroy(cube1.gameObject, 5f);
                  });
              });
        });
        cube2.childCube.transform.DORotate(new Vector3(0, -360f, 0), .2f, RotateMode.LocalAxisAdd).OnComplete(() =>
        {
            cube2.childCube.transform.DOScale(cube2.childCube.transform.localScale * 1.2f, .1f).OnComplete(() =>
              {
                  cube2.transform.DOScale(Vector3.zero, .2f).OnComplete(() =>
                  {
                      if (FinishControl(cube1, cube2))
                      {
                          LevelCompleted();
                      }
                      Destroy(cube2.gameObject, 5f);
                  });
              });
        });

        CorrectTouch.Play();
        PawPoint(cube1, cube2);

        ComboMatch(cube1, cube2);
    }

    #endregion

    #region Wrong Match Methods

    private void WrongMatchShakeCube(Cube cube)
    {
        cube.shakeCubes.enabled = true;
        cube.shakeCubes.ShakeIt();
    }

    private void WrongMatchShakeCamera()
    {
        mainCamera.GetComponent<ShakeObject>().enabled = true;
        mainCamera.GetComponent<ShakeObject>().ShakeIt();
    }

    #endregion

    private bool FinishControl(Cube cube1, Cube cube2)
    {
        if (list_script_cubes.Contains(cube1) && list_script_cubes.Contains(cube2))
        {
            list_script_cubes.Remove(cube1);
            list_script_cubes.Remove(cube2);
        }

        if (list_script_cubes.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #region Level Start & Finish Methods

    public void GenerateLevel()
    {
        list_levelMaterialsWithoutPowerUps.Clear();
        list_bombCubesInlevel.Clear();

        maxPosition = currentLevel.columnCount * currentLevel.rowCount;

        figureCount = PlayerPrefs.GetInt("ImageOrder");
        figureCubePosition = 0;
        isFigureCubeSpawned = false;
        partitionQuantityForFigureCubes = 0;
        spawnedFigureCubeCount = 0;

        isPowerUpCubeSpawned = false;

        int levelNumber = LevelManager.instance.currentLevel;

        if (levelNumber >= bombAndCatCubesLevel)
        {
            if (Random.Range(0, 1) == 0)
            {
                int maxCount = Mathf.FloorToInt(levelNumber / 10)+2;
                print(Mathf.FloorToInt(levelNumber / 10));
                willSpawnBombCubeCount = Random.Range(0, maxCount);
                willSpawnCatCubeCount = Random.Range(0, maxCount);
            }
        }
        else if (levelNumber >= moreCatCubeLevel)
        {
            if (Random.Range(0, 1) == 0)
            {
                if (Random.Range(0, 2) == 0)
                {
                    int maxCount = Mathf.FloorToInt(levelNumber / 10)+2;
                    willSpawnBombCubeCount = Random.Range(1, maxCount);
                }
                else
                {
                    int maxCount = Mathf.FloorToInt(levelNumber / 10)+2;
                    willSpawnCatCubeCount = Random.Range(1, maxCount);
                }
            }
        }
        else if (levelNumber >= moreBombCubeLevel)
        {
            if (Random.Range(0, 1) == 0)
            {
                if (Random.Range(0, 2) == 0)
                {
                    int maxCount = Mathf.FloorToInt(levelNumber / 10)+2;
                    willSpawnBombCubeCount = Random.Range(1, maxCount);
                }
                else
                {
                    willSpawnCatCubeCount = 1;
                }
            }
        }
        else if (levelNumber >= catCubeLevel)
        {
            if (Random.Range(0, 1) == 0)
            {
                if (Random.Range(0, 2) == 0)
                {
                    willSpawnBombCubeCount = 1;
                }
                else
                {
                    willSpawnCatCubeCount = 1;
                }
            }
        }
        else if (levelNumber >= bombCubeLevel)
        {
            if (Random.Range(0, 1) == 0)
            {
                willSpawnBombCubeCount = 1;
            }
        }

        totalPowerUpCubesCount = willSpawnBombCubeCount + willSpawnCatCubeCount;



        if (figureCount != 0)
        {
            isFigureCubeSpawned = true;
            cubeCountInLevel = maxPosition;
            partitionQuantityForFigureCubes = Mathf.FloorToInt((cubeCountInLevel / 2) / figureCount);
        }

        if (totalPowerUpCubesCount != 0)
        {
            isPowerUpCubeSpawned = true;
            cubeCountInLevelWithoutFigureCubes = maxPosition - figureCount;
            totalPowerUpCubesCount = willSpawnBombCubeCount + willSpawnCatCubeCount;
            partitionQuantityForPowerUpCubes = Mathf.FloorToInt((cubeCountInLevelWithoutFigureCubes / 2) / totalPowerUpCubesCount);

            for (int i = 0; i < willSpawnBombCubeCount; i++)
            {
                list_levelPowerUpCubes.Add(prefab_theBombCube);
            }

            for (int i = 0; i < willSpawnCatCubeCount; i++)
            {
                list_levelPowerUpCubes.Add(prefab_theCatCube);
            }
        }


        for (int i = 0; i < maxPosition; i++)
        {
            positions.Add(i);
        }

        for (int i = 0; i < maxPosition; i++)
        {
            if (positions.Contains(i))
            {
                int y = Mathf.FloorToInt(i / currentLevel.columnCount);
                int x = i - y * currentLevel.columnCount;

                if (isFigureCubeSpawned && list_levelFigureCubes.Count != 0)
                {
                    figureCubePosition = Random.Range(spawnedFigureCubeCount * partitionQuantityForFigureCubes, (spawnedFigureCubeCount + 1) * partitionQuantityForFigureCubes);
                    isFigureCubeSpawned = false;
                }

                if (isPowerUpCubeSpawned && list_levelPowerUpCubes.Count != 0)
                {
                    powerUpCubePosition = Random.Range(spawnedPowerUpCubeCount * partitionQuantityForPowerUpCubes, (spawnedPowerUpCubeCount + 1) * partitionQuantityForPowerUpCubes);
                    isPowerUpCubeSpawned = false;
                }

                //   int cubeNumber = Random.Range(0, list_levelCubes.Count); //burayı incele
                int cubeNumber;
                int materialNumber = Random.Range(0, list_levelMaterials.Count);

                if (spawnedPareOfCubeCount == figureCubePosition && list_levelFigureCubes.Count != 0)
                {
                    cubeNumber = Random.Range(0, list_levelFigureCubes.Count);
                    willSpawnCube = list_levelFigureCubes[cubeNumber];
                    list_levelFigureCubes.Remove(list_levelFigureCubes[cubeNumber]);
                    isFigureCubeSpawned = true;
                    spawnedFigureCubeCount++;
                }
                else if (spawnedPareOfCubeCountWithoutFigureCubes == powerUpCubePosition && list_levelPowerUpCubes.Count != 0)
                {
                    cubeNumber = Random.Range(0, list_levelPowerUpCubes.Count);
                    willSpawnCube = list_levelPowerUpCubes[cubeNumber];
                    list_levelPowerUpCubes.Remove(list_levelPowerUpCubes[cubeNumber]);
                    isPowerUpCubeSpawned = true;
                    spawnedPowerUpCubeCount++;
                }
                else
                {
                    willSpawnCube = prefab_cubeNoFigure;
                }

                Cube cubeForControl = willSpawnCube.GetComponent<Cube>();

                if(!cubeForControl.isSpecialCube)
                {
                    willSpawnCube.GetComponent<Cube>().childCube.GetComponent<Renderer>().sharedMaterial = list_levelMaterials[materialNumber];
                    Renderer[] renderers_crackCube = willSpawnCube.GetComponent<Cube>().crackCube.GetComponentsInChildren<Renderer>();

                    foreach (Renderer rend in renderers_crackCube)
                    {
                        rend.sharedMaterial = list_levelMaterials[materialNumber];
                    }
                }


                GameObject cube = Instantiate(willSpawnCube, new Vector3(startUpPoint.position.x + x * cubeScaleX, startUpPoint.transform.position.y - y * cubeScaleY, startUpPoint.position.z), Quaternion.identity);
                if (!cube.GetComponent<Cube>().isSpecialCube)
                {
                    list_levelMaterialsWithoutPowerUps.Add(cube.GetComponent<Cube>().childCube.GetComponent<Renderer>().material);
                }

                list_levelMaterials.Remove(list_levelMaterials[materialNumber]);


                if (list_levelMaterials.Count == 0)
                {
                    foreach (Material mat in currentLevel.levelMaterials)
                    {
                        list_levelMaterials.Add(mat);
                    }
                }

                int secondPosition, secondX, secondY;
                int possibleMaxPosition = Mathf.FloorToInt((i + currentLevel.rowFar * currentLevel.columnCount) / currentLevel.columnCount) * currentLevel.columnCount;

                positions.Remove(i);
                List<int> possiblePositions = new List<int>();

                foreach (int pos in positions)
                {
                    if (pos < possibleMaxPosition)
                    {
                        possiblePositions.Add(pos);
                    }
                }

                int random = Random.Range(0, possiblePositions.Count);
                secondPosition = possiblePositions[random];

                positions.Remove(secondPosition);

                secondY = Mathf.FloorToInt(secondPosition / currentLevel.columnCount);
                secondX = secondPosition - secondY * currentLevel.columnCount;

                cube.transform.localScale = new Vector3(cubeScaleX, cubeScaleY, cubeScaleZ);

                GameObject cube2 = Instantiate(cube, new Vector3(startUpPoint.position.x + secondX * cubeScaleX, startUpPoint.transform.position.y - secondY * cubeScaleY, startUpPoint.position.z), Quaternion.identity);

                cube.transform.SetParent(level.transform);
                cube2.transform.SetParent(level.transform);

                if (cube.GetComponent<Cube>().bombCube)
                {
                    list_bombCubesInlevel.Add(cube.GetComponent<Cube>());
                    list_bombCubesInlevel.Add(cube2.GetComponent<Cube>());
                }

                spawnedPareOfCubeCount++;
                if (!isFigureCubeSpawned)
                {
                    spawnedPareOfCubeCountWithoutFigureCubes++;
                }
            }
        }

        for(int i = 0; i < list_bombCubesInlevel.Count; i += 2)
        {
            int randomColorValue = Random.Range(0, list_levelMaterialsWithoutPowerUps.Count);
            list_bombCubesInlevel[i].bombRenderer.sharedMaterial = list_levelMaterialsWithoutPowerUps[randomColorValue];
            list_bombCubesInlevel[i+1].bombRenderer.sharedMaterial = list_levelMaterialsWithoutPowerUps[randomColorValue];

            if (list_levelMaterialsWithoutPowerUps[randomColorValue].HasProperty("_Color"))
            {
                list_bombCubesInlevel[i].color = list_levelMaterialsWithoutPowerUps[randomColorValue].color;
                list_bombCubesInlevel[i + 1].color = list_levelMaterialsWithoutPowerUps[randomColorValue].color;
            }
            else
            {
                list_bombCubesInlevel[i].color = list_levelMaterialsWithoutPowerUps[randomColorValue].GetColor("_Color1_F");
                list_bombCubesInlevel[i + 1].color = list_levelMaterialsWithoutPowerUps[randomColorValue].GetColor("_Color1_F");
            }
            

            list_levelMaterialsWithoutPowerUps.Remove(list_levelMaterialsWithoutPowerUps[randomColorValue]);
        }

        finalObject = (GameObject)Instantiate(prefab_finalObject, new Vector3(0, startUpPoint.transform.position.y - currentLevel.rowCount * cubeScaleY, startUpPoint.position.z), Quaternion.identity);
        finalObject.transform.SetParent(level.transform);
    }

    private void LevelCompleted()
    {
        print("level completed");
        canTouch = false;
        isFinished = true;
        levelCompleted = true;
        LevelManager.instance.NextLevel();

        completedConfetti.SetActive(true);

        StartCoroutine(LateShowLevelCompletedPanel());
        StartCoroutine(Waiting());

        PlayerPrefs.SetInt("Score", totalScore);
        lastScore.text = totalScore.ToString();


        script_Flow.speed = -0.01f;

    }

    public void LevelFailed()
    {
        canTouch = false;
        isFinished = true;
        levelFailed = true;
        LoseSound.Play();


        StartCoroutine(Waiting());
        //  FailedEffect();

        //  UIManager.instance.ShowLevelFailedPanel();
    }

    #endregion

    public void DefaultValues()
    {
        #region Generate Level

        currentLevel = levels[LevelManager.instance.levelToBeLoaded];

        startUpPoint.position = new Vector3(-currentLevel.columnCount * .5f, startUpPoint.position.y, startUpPoint.position.z);
        endUpPoint.position = new Vector3(currentLevel.columnCount * .5f, endUpPoint.position.y, endUpPoint.position.z);

        //  cubeScaleX = cubeScaleY = cubeScaleZ = Vector3.Distance(startUpPoint.position, endUpPoint.position) / currentLevel.columnCount;
        level = (GameObject)Instantiate(prefab_level);

        list_levelFigureCubes.Clear();
        list_levelPowerUpCubes.Clear();

        willSpawnBombCubeCount = 0;
        willSpawnCatCubeCount = 0;
        spawnedPowerUpCubeCount = 0;
        cubeCountInLevelWithoutFigureCubes = 0;
        totalPowerUpCubesCount = 0;



        list_levelMaterials.Clear();
        foreach (Material mat in currentLevel.levelMaterials)
        {
            list_levelMaterials.Add(mat);
        }

        int figureCubeCount = PlayerPrefs.GetInt("ImageOrder");
        for (int i = 0; i < figureCubeCount; i++)
        {
            list_levelFigureCubes.Add(prefab_figureCubeModels[i]);
        }

        if (currentLevel.isSpecialLevel)
        {
            level = (GameObject)Instantiate(currentLevel.level);//, startUpPoint.position, Quaternion.identity);
            finalObject = GameObject.FindGameObjectWithTag("FinalObjectForFail");
        }
        else
        {
            spawnedPareOfCubeCount = 0;
            spawnedPareOfCubeCountWithoutFigureCubes = 0;
            GenerateLevel();
        }

        realSpeed = 0.0097f * Mathf.Exp(0.0043f * (LevelManager.instance.levelNumberForSpeed));

        script_Flow.speed = .1f;

        if (firstOpen)
        {
            firstOpen = false;
        }
        else
        {
            canTouch = true;
            DropControl();
        }

        springInFinalObject = finalObject.transform.GetChild(1).GetChild(1).gameObject;
        #endregion

        //  script_tutorial = level.GetComponent<Tutorial>();

        int backgorundMaterialNumber = Random.Range(0, backgroundMaterials.Length);
        renderer_backgroundWall.material = backgroundMaterials[backgorundMaterialNumber];


        array_script_cubes = level.GetComponentsInChildren<Cube>();
        list_script_cubes.Clear();
        foreach (Cube script_cube in array_script_cubes)
        {
            list_script_cubes.Add(script_cube);
            script_cube.outlineFilterObj.m_OutlineFilter = mainCamera.GetComponent<OutlineFilter>();
        }


        Physics.gravity = new Vector3(0, -15f, 0);



        completedConfetti.SetActive(false);

        wall.SetActive(true);
        script_Flow.gameObject.SetActive(true);
        slowCube.SetActive(true);

        isFinished = false;
        levelCompleted = false;
        levelFailed = false;

        isCatMatched = false;

        matchedCatCount = 0;

        if (PlayerPrefs.GetInt("FirstTutorial") == 0)
        {
            HowToPlayTutorial();
        }

        CubeDropSound.mute = false;
    }

    public void ComboMatch(Cube cube1, Cube cube2)
    {
        if (timer <= 2.5f)
        {
            GameObject comboss = Instantiate(prefab_combos[Random.Range(0, prefab_combos.Length)], cube2.transform.position + combothreshold, Quaternion.identity);
            comboss.GetComponent<Animation>().Play("Fading");
            Destroy(comboss, 3f);


        }
        else
        {
            timer = 0f;
        }
    }

    public void PawPoint(Cube cube1, Cube cube2)
    {
        int random = Random.Range(0, 10);
        if (random < 5 || matchingCount == 4 || isCatMatched || cube1.isFigureCube)
        {
            if (isCatMatched)
            {
                totalScore += matchedCatCount;
            }
            else
            {
                totalScore++;
            }

            GameObject pointss = Instantiate(points, (cube2.transform.position + cube1.transform.position) / 2f + combothreshold, points.transform.rotation);
            pointss.transform.DOMove(targetPosition, .5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                lastScore.text = totalScore.ToString();
                matchingCount = 0;
                Destroy(pointss, 0.01f);
            });
        }
    }

    #region PowerUps

    public void DestroySameColorsCube(Cube cube, Cube bombCube1, Cube bombCube2, int whichCube)
    {
        GameObject theBomb;
        if (whichCube == 1)
        {
            theBomb = (GameObject)Instantiate(bombCube1.bombModel, bombCube1.childCube.transform.position+new Vector3(0,0,-1f), Quaternion.identity);
        }
        else
        {
            theBomb = (GameObject)Instantiate(bombCube2.bombModel, bombCube2.childCube.transform.position + new Vector3(0, 0, -1f), Quaternion.identity);
        }
        theBomb.transform.localScale = new Vector3(1, 1, 1);
        theBomb.transform.DOMoveX(cube.childCube.transform.position.x, .4f).OnComplete(() =>
        {
            list_script_cubes.Remove(cube);

            if (list_script_cubes.Count == 0)
            {
                LevelCompleted();
            }

            particle_bombEffect.GetComponent<Renderer>().sharedMaterial.SetColor("_TintColor", cube.color);
            GameObject theParticle = Instantiate(particle_bombEffect, cube.childCube.transform.position+new Vector3(0,0,-1f), Quaternion.identity);

            Destroy(theParticle, 1f);
            Destroy(cube.gameObject);
            Destroy(theBomb);
        });
        theBomb.transform.DOMoveY(cube.childCube.transform.position.y, .4f);
        theBomb.transform.DOMoveZ(cube.childCube.transform.position.z - 2f, .2f).OnComplete(() =>
        {
            theBomb.transform.DOMoveZ(cube.childCube.transform.position.z, .2f);
        });
    }

    public void SpargePaws(Cube cube, Cube catCube1, Cube catCube2, int whichCube)
    {
        GameObject thePaw;
        if (whichCube == 1)
        {
            thePaw = (GameObject)Instantiate(pawForPawCube, catCube1.childCube.transform.position, Quaternion.identity);
        }
        else
        {
            thePaw = (GameObject)Instantiate(pawForPawCube, catCube2.childCube.transform.position, Quaternion.identity);
        }

        thePaw.transform.DOMoveX(cube.childCube.transform.position.x, .4f).OnComplete(() =>
        {
            Destroy(thePaw);
        });
        thePaw.transform.DOMoveY(cube.childCube.transform.position.y, .4f);
        thePaw.transform.DOMoveZ(cube.childCube.transform.position.z - 2f, .2f).OnComplete(() =>
        {
            thePaw.transform.DOMoveZ(cube.childCube.transform.position.z, .2f);
        });
    }

    #endregion

    private void HowToPlayTutorial()
    {
        script_tutorialHowToPlay.HowToPlayTutorial();
    }

    private void WrongMatchTutorial(Cube firstCube, Cube wrongMatchCube)
    {
        WrongMatchShakeCamera();
        WrongMatch.Play();
        MMVibrationManager.Haptic(HapticTypes.Failure, false);
        timer = 0f;

        StartCoroutine(OutlineDisable(wrongMatchCube));

        ChooseCube(ref choosedCube_1, firstCube, false);

        script_tutorialWrongMatch.enabled = true;
        script_tutorialWrongMatch.WrongMatchTutorial(firstCube);
    }

    private IEnumerator LateShowLevelCompletedPanel()
    {
        yield return new WaitForSeconds(1f);
        UIManager.instance.ShowLevelCompletedPanel();
    }

    private IEnumerator DropControlDelay()
    {
        yield return new WaitForSecondsRealtime(2f);
        foreach (Cube cube in list_script_cubes)
        {
            cube.dropControl = true;
        }
    }

    public void DropControl()
    {
        StartCoroutine(DropControlDelay());
    }

    public IEnumerator Waiting()
    {
        tapToConutinue.interactable = false;
        tapToConutinue2.interactable = false;
        awardButton.interactable = false;
        yield return new WaitForSecondsRealtime(1f);
        tapToConutinue.interactable = true;
        tapToConutinue2.interactable = true;
        awardButton.interactable = true;
    }

    private IEnumerator OutlineDisable(Cube cube)
    {
        yield return new WaitForSeconds(.1f);
        cube.outlineFilterObj.OutlineDisable();
    }
}
