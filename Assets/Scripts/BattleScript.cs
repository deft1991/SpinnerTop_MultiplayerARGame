using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class BattleScript : MonoBehaviourPun
{
    public Spinner spinnerScript;
    public Image spinSpeedBarImage;
    public TextMeshProUGUI spinSpeedRatioText;
    public GameObject ui3DGameObject;
    public GameObject deathPanelUiPrefab;
    public float commonDamageCoefficient = 0.04F;
    public bool isAttacker;
    public bool isDefender;
    public bool isBot;

    public List<GameObject> pooledObjects;
    public int amountToPool = 8;
    public GameObject collisionEffectPrefab;

    [Header("Player Type Damage Coefficients")]
    public float doDamageCoefficientAttacker = 10; // do more damage than defender - ADVANTAGE

    public float getDamagedCoefficientAttacker = 1.2F; // gets more damage - DISADVANTAGE
    public float doDamageCoefficientDefender = 0.75F; // do less damage - DISADVANTAGE
    public float getDamagedCoefficientDefender = 0.2F; // gets less damage - ADVANTAGE

    private float _starSpinSpeed;
    private Rigidbody _rigidbody;
    private float _currentSpinSpeed;
    private float _defaultSpeedDamage = 3600f;
    private GameObject _deathPanelGameObject;
    private bool _isDead = false;
    private int _score;

    private GameObject battleArena; // need to check position and send die if leave arena

    // Start is called before the first frame update
    void Start()
    {
        battleArena = GameObject.Find("BattleArena");

        CheckPlayerType();

        _rigidbody = GetComponent<Rigidbody>();
        if (photonView.IsMine)
        {
            // instantiate 8 collision gameObjects, which contains visual and sounds effects together
            // And we do it only for local player. We don`t want instantiate it twice
            // Then add this objects to list and deactivate them.
            pooledObjects = new List<GameObject>();
            for (int i = 0; i < amountToPool; i++)
            {
                GameObject obj = Instantiate(collisionEffectPrefab, Vector3.zero, Quaternion.identity);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    /**
     * When collision happens we try to grab a Collision game object from existing
     * de-activated Collision game objects
     */
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return null;
    }

    // Update is called once per frame
    void Update()
    {
        // CheckThatWeOnBattleArenaOrSendDie();
    }

    //// todo check reborn, because we reborn on current position
    // private void CheckThatWeOnBattleArenaOrSendDie()
    // {
    //     if (gameObject.GetComponent<PhotonView>().IsMine && !_isDead)
    //     {
    //         var transformPosition = gameObject.transform.position;
    //         Vector3 battleArenaPosition = battleArena.transform.position;
    //
    //         bool leaveViaX = transformPosition.x > battleArenaPosition.x + 1 ||
    //                          transformPosition.x < battleArenaPosition.x - 1;
    //         bool leaveViaY = transformPosition.y > battleArenaPosition.y + 1 ||
    //                          transformPosition.y < battleArenaPosition.y - 1;
    //         bool leaveViaZ = transformPosition.z > battleArenaPosition.z + 1 ||
    //                          transformPosition.z < battleArenaPosition.z - 1;
    //         if (leaveViaX || leaveViaY || leaveViaZ)
    //         {
    //             if (photonView.IsMine)
    //             {
    //                 Die();
    //             }
    //             else
    //             {
    //                 gameObject.GetComponent<PhotonView>()
    //                     .RPC("LeaveArena", RpcTarget.AllBuffered);
    //             }
    //         }
    //     }
    // }

    #region CORUTINE Methods

    public IEnumerator ReSpawnCountDown(int score)
    {
        GameObject canvasGameObject = GameObject.Find("Canvas");

        if (_deathPanelGameObject == null)
        {
            _deathPanelGameObject = Instantiate(deathPanelUiPrefab, canvasGameObject.transform);
        }
        else
        {
            _deathPanelGameObject.SetActive(true);
        }

        Text scoreText = _deathPanelGameObject.transform.Find("ScoreCountText").GetComponent<Text>();
        Text respawnTimeText = _deathPanelGameObject.transform.Find("RespawnTimeText").GetComponent<Text>();

        float respawnTime = 8;
        scoreText.text = score.ToString();
        respawnTimeText.text = respawnTime.ToString(".00");
        while (respawnTime > 0)
        {
            yield return new WaitForSeconds(1.0F);
            respawnTime--;
            respawnTimeText.text = respawnTime.ToString(".00");

            GetComponent<MovementController>().enabled = false;
        }

        _deathPanelGameObject.SetActive(false);
        GetComponent<MovementController>().enabled = true;

        // after die and respawn time count down, we should respawn our player
        photonView.RPC("Reborn", RpcTarget.AllBuffered);
    }

    public void StopReSpawnCountDown()
    {
        StopCoroutine(_reSpawnCountDown);

        _deathPanelGameObject.SetActive(false);
        GetComponent<MovementController>().enabled = true;

        // after die and respawn time count down, we should respawn our player
        photonView.RPC("Reborn", RpcTarget.AllBuffered);
    }


    IEnumerator RebornBot(float sec)
    {
        while (sec > 0)
        {
            yield return new WaitForSeconds(1.0F);
            sec--;
        }

        spinnerScript.spinnerSpeed = _starSpinSpeed;
        _currentSpinSpeed = _starSpinSpeed;

        spinSpeedBarImage.fillAmount = _currentSpinSpeed / _starSpinSpeed;
        spinSpeedRatioText.text = _currentSpinSpeed + "/" + _starSpinSpeed;

        // reactivate freeze rotation
        _rigidbody.freezeRotation = true;
        transform.rotation = Quaternion.Euler(Vector3.zero);

        // reactivate 3D UI
        ui3DGameObject.SetActive(true);

        _isDead = false;
    }

    IEnumerator DeactivateAfterSeconds(GameObject mGameObject, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        mGameObject.SetActive(false);
    }

    public IEnumerator FreezePlayer(float seconds, MovementController movementController)
    {
        movementController.joystick.enabled = false;
        yield return new WaitForSeconds(seconds);
        movementController.joystick.enabled = true;
    }

    #endregion

    #region PUN methods

    [PunRPC]
    private void Reborn()
    {
        spinnerScript.spinnerSpeed = _starSpinSpeed;
        _currentSpinSpeed = _starSpinSpeed;

        spinSpeedBarImage.fillAmount = _currentSpinSpeed / _starSpinSpeed;
        spinSpeedRatioText.text = _currentSpinSpeed + "/" + _starSpinSpeed;

        // reactivate freeze rotation
        _rigidbody.freezeRotation = true;
        transform.rotation = Quaternion.Euler(Vector3.zero);

        // reactivate 3D UI
        ui3DGameObject.SetActive(true);

        if (!isBot && _isDead && PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            SpinningTopsGameManager gameManager = FindObjectOfType<SpinningTopsGameManager>();
            StartCoroutine(gameManager.SpawnBotAfterSeconds(new Random().Next(3, 7)));
        }

        _isDead = false;
    }

    [PunRPC]
    public void DoDamage(float damageAmount)
    {
        // do damage only if player alive
        if (!_isDead)
        {
            damageAmount = CalculateDamageAmount(damageAmount);
            damageAmount = CrunchIfBothAttackers(damageAmount);

            spinnerScript.spinnerSpeed -= damageAmount;
            // update current speed
            _currentSpinSpeed = spinnerScript.spinnerSpeed;
            spinSpeedBarImage.fillAmount = _currentSpinSpeed / _starSpinSpeed;
            // ToString("F0") - without fractional part
            spinSpeedRatioText.text = _currentSpinSpeed.ToString("F0") + "/" + _starSpinSpeed;

            // if current speed less than 100
            if (_currentSpinSpeed < 100)
            {
                Die();
            }
        }
    }

    public void LeaveArena()
    {
        Die();
    }

    public void ApplyBooster(float speedAmount)
    {
        // apply booster only if player alive
        try
        {
            if (!_isDead)
            {
                spinnerScript.spinnerSpeed += speedAmount;
                // update current speed
                _currentSpinSpeed = spinnerScript.spinnerSpeed;
                spinSpeedBarImage.fillAmount = _currentSpinSpeed / _starSpinSpeed;
                // ToString("F0") - without fractional part
                spinSpeedRatioText.text = _currentSpinSpeed.ToString("F0") + "/" + _starSpinSpeed;

                // if current speed less than 100
                if (_currentSpinSpeed < 100)
                {
                    Die();
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("ApplyBooster err" + e);
        }
    }

    #endregion

    #region PRIVATE Methods

    private void Awake()
    {
        // get start spinner speed from spinner script
        _starSpinSpeed = spinnerScript.spinnerSpeed;
        _currentSpinSpeed = spinnerScript.spinnerSpeed;

        // fill spin speed bar based on current and start speed 
        spinSpeedBarImage.fillAmount = _currentSpinSpeed / _starSpinSpeed;
    }

    private void CheckPlayerType()
    {
        if (gameObject.name.Contains("Attacker"))
        {
            isAttacker = true;
            isDefender = false;
        }
        else if (gameObject.name.Contains("Defender"))
        {
            isAttacker = false;
            isDefender = true;

            // set speed greater for defender
            // gets more health
            spinnerScript.spinnerSpeed = 4400;
            _starSpinSpeed = spinnerScript.spinnerSpeed;
            _currentSpinSpeed = spinnerScript.spinnerSpeed;

            spinSpeedRatioText.text = _currentSpinSpeed + "/" + _starSpinSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleBoosterCollision(collision);
        HandlePlayerCollision(collision);
    }

    private void HandleBoosterCollision(Collision collision)
    {
        if (collision.gameObject.CompareTag("Booster"))
        {
            // todo maybe do it for all
            if (photonView.IsMine)
            {
                StartCollisionEffect(collision);
            }

            BoosterScript boosterScript = collision.gameObject.GetComponent<BoosterScript>();

            try
            {
                // todo check it.
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.Destroy(collision.gameObject);
                }

                Destroy(collision.gameObject);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Debug.Log("collision with booster");
            switch (boosterScript.boosterType)
            {
                case BoosterScript.BoosterType.SPEED:
                    ApplySpeedBooster(boosterScript.increaseSpeed);
                    break;
                case BoosterScript.BoosterType.PUSH:
                    ApplyPushBooster(boosterScript.pushSpeed);
                    break;
                case BoosterScript.BoosterType.DAMAGE:
                    ApplyDamageBooster(boosterScript.damageAmount);
                    break;
                case BoosterScript.BoosterType.FREEZE:
                    ApplyFreezeBooster(boosterScript.freezeTime);
                    break;
            }
        }
    }

    private void HandlePlayerCollision(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (photonView.IsMine)
            {
                StartCollisionEffect(collision);
            }

            // Comparing the speed of the SpinnerTop
            // speed is actually the magnitude of velocity
            float mySpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;

            // local remote player
            // that is hit by my local player gameObject
            float otherPlayerSpeed = collision.collider.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

            Debug.Log("My speed: " + mySpeed + " ---- Other player speed: " + otherPlayerSpeed);

            if (mySpeed > otherPlayerSpeed)
            {
                DamageOtherPlayer(collision);
            }
            else if (!collision.collider.gameObject.GetComponent<PhotonView>().IsMine && _isDead
            ) // restore hp for winner
            {
                collision.collider.gameObject.GetComponent<PhotonView>()
                    .RPC("Reborn", RpcTarget.AllBuffered);
            }
            else if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine && _isDead && isBot
            ) // restore hp for winner with BOT game
            {
                collision.collider.gameObject.GetComponent<PhotonView>()
                    .RPC("Reborn", RpcTarget.AllBuffered);
            }
        }
    }

    private void DamageOtherPlayer(Collision collision)
    {
        Debug.Log("YOU Damage the other player");

        var defaultDamageAmount = CalculateDefaultDamageAmount();

        IncreaseScore(defaultDamageAmount);

        // check that gameObject isMine
        // if we do not check it, we will damage players as many as we have users in the room
        // RPC call will execute for app players in the room
        if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine && !_isDead
        ) // damage if we are not die =)
        {
            // Apply damage to slower player.
            // RpcTarget.AllBuffered - means send to all players in the room and to players who connected later
            // defaultDamageAmount  - amount of damage
            collision.collider.gameObject.GetComponent<PhotonView>()
                .RPC("DoDamage", RpcTarget.AllBuffered, defaultDamageAmount);
        }
    }

    private void StartCollisionEffect(Collision collision)
    {
        // get average positions of spinners when the collision happens
        // than add small elevation to show animation not at the bottom   
        Vector3 effectPosition = (gameObject.transform.position + collision.transform.position) / 2 +
                                 new Vector3(0, 0.05f, 0);

        //Instantiate Collision Effect ParticleSystem
        GameObject collisionEffectGameObject = GetPooledObject();
        if (collisionEffectGameObject != null)
        {
            collisionEffectGameObject.transform.position = effectPosition;
            collisionEffectGameObject.SetActive(true);
            collisionEffectGameObject.GetComponentInChildren<ParticleSystem>().Play();

            //De-activate Collision Effect Particle System after some seconds.
            StartCoroutine(DeactivateAfterSeconds(collisionEffectGameObject, 0.5f));
        }
    }

    private void ApplySpeedBooster(float speed)
    {
        Debug.Log("ApplySpeedBooster");
        spinnerScript.spinnerSpeed += speed;
        // update current speed
        _currentSpinSpeed = spinnerScript.spinnerSpeed;
        spinSpeedBarImage.fillAmount = _currentSpinSpeed / _starSpinSpeed;
        // ToString("F0") - without fractional part
        spinSpeedRatioText.text = _currentSpinSpeed.ToString("F0") + "/" + _starSpinSpeed;

        if (gameObject.GetComponent<PhotonView>().IsMine && !_isDead)
        {
            gameObject.GetComponent<PhotonView>()
                .RPC("ApplyBooster", RpcTarget.Others, speed);
        }
    }

    private void ApplyPushBooster(float pushSpeed)
    {
        Debug.Log("applyPushBooster");
        Vector3 transformPosition = gameObject.transform.position;
        Vector3 battleArenaPosition = battleArena.transform.position;
        gameObject.transform.position = new Vector3(battleArenaPosition.x, -0.1F, battleArenaPosition.z);
    }

    private void ApplyDamageBooster(float damage)
    {
        spinnerScript.spinnerSpeed -= damage;
        // update current speed
        _currentSpinSpeed = spinnerScript.spinnerSpeed;
        spinSpeedBarImage.fillAmount = _currentSpinSpeed / _starSpinSpeed;
        // ToString("F0") - without fractional part
        spinSpeedRatioText.text = _currentSpinSpeed.ToString("F0") + "/" + _starSpinSpeed;

        if (!gameObject.GetComponent<PhotonView>().IsMine)
        {
            gameObject.GetComponent<PhotonView>()
                .RPC("ApplyBooster", RpcTarget.AllBuffered, -damage);
        }
    }

    private void ApplyFreezeBooster(float freezeTime)
    {
        Debug.Log("applyFreezeBooster");
        MovementController movementController = gameObject.GetComponent<MovementController>();
        StartCoroutine(FreezePlayer(freezeTime, movementController));
    }

    private float CalculateDefaultDamageAmount()
    {
        // max magnitude for about 1.5, and _starSpinSpeed = 3600. 
        // We multiply on commonDamageCoefficient 0.04 it will be 216. Great.
        // 3600 / 216 ~= 16-17 on max speed
        float defaultDamageAmount =
            gameObject.GetComponent<Rigidbody>().velocity.magnitude * _defaultSpeedDamage *
            commonDamageCoefficient;

        if (isAttacker)
        {
            defaultDamageAmount *= doDamageCoefficientAttacker;
        }
        else if (isDefender)
        {
            defaultDamageAmount *= doDamageCoefficientDefender;
        }

        return defaultDamageAmount;
    }

    private static float CrunchIfBothAttackers(float damageAmount)
    {
        if (damageAmount > 1000)
        {
            damageAmount = 400;
        }

        return damageAmount;
    }

    private float CalculateDamageAmount(float damageAmount)
    {
        // if attacker attack defender - it will be 10 (attackerCoef) * 0,2 (getDmgDef) = 2 
        // it gives two times more damage
        // but if defender attack 
        // it will getDamagedCoefficientAttacker (1.2) * doDamageCoefficientDefender (0.75) = 0.9 times damage
        if (isAttacker)
        {
            damageAmount *= getDamagedCoefficientAttacker;
        }
        else if (isDefender)
        {
            damageAmount *= getDamagedCoefficientDefender;
        }

        return damageAmount;
    }

    private void IncreaseScore(float defaultDamageAmount)
    {
        if (!_isDead)
        {
            _score += (int) defaultDamageAmount;
        }
    }

    private IEnumerator _reSpawnCountDown;

    private void Die()
    {
        _isDead = true;
        // disable movement controller
        gameObject.GetComponent<MovementController>().enabled = false;

        _rigidbody.freezeRotation = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        spinnerScript.spinnerSpeed = 0.0F;

        // last deactivate UI 3D
        ui3DGameObject.SetActive(false);

        if (photonView.IsMine && !isBot)
        {
            // countdown for respawn
            _reSpawnCountDown = ReSpawnCountDown(_score);
            StartCoroutine(_reSpawnCountDown);
            RemoveBotFromRoom();
        }

        if (isBot)
        {
            Debug.Log("Bot DIE!");
            //todo add random to bot leave room
            bool isRemoveBot = new Random().NextDouble() >= 0.5;
            if (isRemoveBot)
            {
                StartCoroutine(RebornBot(new Random().Next(2, 8)));
            }
            else
            {
                // todo add remove bot after sec
                RemoveBotFromRoom();
            }
        }
    }

    private static void RemoveBotFromRoom()
    {
        BattleScript[] findObjectOfType = FindObjectsOfType<BattleScript>();
        foreach (var battleScript in findObjectOfType)
        {
            if (battleScript.isBot)
            {
                PhotonView.Destroy(battleScript.photonView);
                Destroy(battleScript.gameObject);
            }
        }
    }

    #endregion
}