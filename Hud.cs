using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public GameObject XP_text;
    public GameObject DeathScene;
    Animator animator;
    private float lerpTimer;
    public float maxHealth = 100f;
    public float maxStamina = 100f;
    public float maxHunger = 100f;
    public float maxThirst = 100f;
    public float maxXP = 100f;
    public float stamina;
    public float health;
    public float hunger;
    public float thirst;
    public Transform DeathSceneTransform;
    public Transform RespawnPointTransform;
    public bool is_sprinting_bool = false;
    public float xp = 0f;
    private float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;
    public Image xpBarFiller;
    public Image staminaBarFiller;
    public Image hungerBarFiller;
    public Image thirstBarFiller;
    public GameObject inventory_sprite;
    public GameObject crafting_sprite;
    public GameObject help_sprite;
    public float XP_level;
    private Text xp_txt;
    public float is_winded_start_time = 0;
    public float is_winded_end_time = 0;
    public float is_winded_time_elapsed = 0;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        hunger = maxHunger;
        thirst = maxThirst;
        stamina = maxStamina;
        xp_txt = XP_text.GetComponent<Text>();
        XP_level = 0;
        UpdateStatsUI();
        inventory_sprite.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        is_winded_end_time = Time.time;
        xp_txt.text = XP_level.ToString();
        health = Mathf.Clamp(health, 0, maxHealth);
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        hunger = Mathf.Clamp(hunger, 0, maxHunger);
        thirst = Mathf.Clamp(thirst, 0, maxThirst);
        UpdateStatsUI();
        LoseHunger(0.001f);
        LoseThirst(0.001f);
        if (Input.GetKeyDown(KeyCode.M))
        {
            GainHealth(Random.Range(5, 10));
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            GainHealth(Random.Range(-5, -10));
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            GainXp(Random.Range(5, 10));
        }
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0.5)
        {
            is_sprinting_bool = true;
            LoseStamina(0.05f);
        }
        if (stamina >= 30)
        {
            staminaBarFiller.color = Color.white;
        }
        if (stamina <= 30)
        {
            staminaBarFiller.color = Color.red;
        }
        if (stamina <= 0.5) 
        {
            is_sprinting_bool = false;
        }
        if (is_winded_start_time != 0)
        {
            is_winded_time_elapsed = (is_winded_end_time - is_winded_start_time);
            if (is_winded_time_elapsed >= 5)
            {
                is_winded_end_time = 0;
                is_winded_start_time = 0;
            }
        }
        if (is_winded_time_elapsed >= 5 && is_sprinting_bool == false)
        {
            LoseStamina(-0.1f);
        }
        if (Input.GetKey(KeyCode.LeftShift) && is_winded_start_time == 0)
        {
            is_winded_start_time = Time.time;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            is_sprinting_bool = false;
        }
        if (xp >= maxXP)
        {
            XP_level += 1;
            xp = 0;
            UpdateStatsUI();
        }
        if (health <= 0)
        {
            health = 100;
            DeathScene.SetActive(true);
            transform.position = DeathSceneTransform.position;
        }
        if(Input.GetKeyDown(KeyCode.I)){
            inventory_sprite.SetActive(true);
            crafting_sprite.SetActive(true);
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            inventory_sprite.SetActive(false);
            crafting_sprite.SetActive(false);
            help_sprite.SetActive(false);         
        }
        if(Input.GetKeyDown(KeyCode.H)){
            help_sprite.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = RespawnPointTransform.position;
            DeathScene.SetActive(false);
        }
    }

    public void UpdateStatsUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float fillX = xpBarFiller.fillAmount;
        float fillS = staminaBarFiller.fillAmount;
        float fillH = hungerBarFiller.fillAmount;
        float fillT = thirstBarFiller.fillAmount;
        float hFraction = health / maxHealth;
        float xpFraction = xp / maxXP;
        float staminaFraction = stamina / maxStamina;
        float hungerFraction = hunger / maxHunger;
        float thirstFraction = thirst / maxThirst;

        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            frontHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
            frontHealthBar.color = Color.white;
        }
        if (fillF < hFraction)
        {
            frontHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
            frontHealthBar.color = Color.white;
        }
        if (fillX < xpFraction)
        {
            xpBarFiller.fillAmount = xpFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            xpBarFiller.fillAmount = Mathf.Lerp(fillX, xpBarFiller.fillAmount, percentComplete);
        }
        if (fillX > xpFraction)
        {
            xpBarFiller.fillAmount = xpFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            xpBarFiller.fillAmount = Mathf.Lerp(fillX, xpFraction, percentComplete);
        }
        if (fillS < staminaFraction)
        {
            staminaBarFiller.fillAmount = staminaFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            staminaBarFiller.fillAmount = Mathf.Lerp(fillS, staminaBarFiller.fillAmount, percentComplete);
        }
        if (fillS > staminaFraction)
        {
            staminaBarFiller.fillAmount = staminaFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            staminaBarFiller.fillAmount = Mathf.Lerp(fillS, staminaFraction, percentComplete);
        }
        if (fillH < hungerFraction)
        {
            hungerBarFiller.fillAmount = hungerFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            hungerBarFiller.fillAmount = Mathf.Lerp(fillH, hungerBarFiller.fillAmount, percentComplete);
        }
        if (fillH > hungerFraction)
        {
            hungerBarFiller.fillAmount = hungerFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            hungerBarFiller.fillAmount = Mathf.Lerp(fillH, hungerFraction, percentComplete);
        }
        if (fillT < thirstFraction)
        {
            thirstBarFiller.fillAmount = thirstFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            thirstBarFiller.fillAmount = Mathf.Lerp(fillT, thirstBarFiller.fillAmount, percentComplete);
        }
        if (fillT > thirstFraction)
        {
            thirstBarFiller.fillAmount = thirstFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            thirstBarFiller.fillAmount = Mathf.Lerp(fillT, thirstFraction, percentComplete);
        }

    }

    public void GainHealth(float statAmount)
    {
        health += statAmount;
        lerpTimer = 0f;
    }
    public void LoseHunger(float statAmount)
    {
        hunger -= statAmount;
    }
    public void LoseThirst(float statAmount)
    {
        thirst -= statAmount;
    }
    public void GainXp(float statAmount)
    {
        xp += statAmount;
        lerpTimer = 0f;
    }
    public void LoseStamina(float staminaAmount)
    {
        stamina -= staminaAmount;
        is_winded_start_time = 0;
    }
}