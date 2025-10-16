using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject menuCam;
    public GameObject gameCam;
    public Player player;
    public Boss boss;
    public GameObject itemShop;
    public GameObject weaponShop;
    public GameObject startZone;
    public int stage;
    public float playTime;
    public bool isBattle;
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;
    public int enemyCntD;

    public Transform[] enenmyZones;
    public GameObject[] enemies;
    public List<int> enemyList;

    public GameObject menuPanel;
    public GameObject gamePanel;
    public Text maxScoreTXT;
    public Text scoreTXT;
    public Text stageTXT;
    public Text playTimeTXT;
    public Text playerHealthTXT;
    public Text playerAmmoTXT;
    public Text playercoinTXT;
    public Text enemyATXT;
    public Text enemyBTXT;
    public Text enemyCTXT;

    public Image weapon1IMG;
    public Image weapon2IMG;
    public Image weapon3IMG;
    public Image weaponRIMG;
    public RectTransform bossHealthGroup;
    public RectTransform bossHealthBar;


    void Awake()
    {
        enemyList = new List<int>();
        maxScoreTXT.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore")) ;
    }

    public void GameStart()
    {
        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
    }

    public void StageStart()
    {
        itemShop.SetActive(false);
        weaponShop.SetActive(false);
        startZone.SetActive(false);

        foreach(Transform zone in enenmyZones)
        zone.gameObject.SetActive(true);

        isBattle = true;
        StartCoroutine(InBattle());
    }

    public void StageEnd()
    {
        player.transform.position = Vector3.up * 1.1f;
        itemShop.SetActive(true);
        weaponShop.SetActive(true);
        startZone.SetActive(true);

        isBattle = false;
        stage++;
    }

    IEnumerator InBattle()
    {
        for (int i = 0; i < stage; i++) 
        {
            if(stage % 5 == 0)
            {
                GameObject instantEnemy = Instantiate(enemies[3], enenmyZones[0].position, enenmyZones[0].rotation);
                Enenmy enemy = instantEnemy.GetComponent<Enenmy>();
                enemy.target = player.transform;
                enemy.manager = this;
                boss = instantEnemy.GetComponent<Boss>();

            } else
            {
                int ran = Random.Range(0, 3);
                enemyList.Add(ran);

                switch (ran)
                {
                    case 0:
                        enemyCntA++;
                        break;
                    case 1:
                        enemyCntB++;
                        break;
                    case 2:
                        enemyCntC++;
                        break;

                }
            }
            
        }

        while (enemyList.Count > 0)
        {
            int ranZone = Random.Range(0, 4);
            GameObject instantEnemy = Instantiate(enemies[enemyList[0]], enenmyZones[ranZone].position, enenmyZones[ranZone].rotation);
            Enenmy enemy = instantEnemy.GetComponent<Enenmy>();
            enemy.target = player.transform;
            enemy.manager = this;
            enemyList.RemoveAt(0);
            yield return new WaitForSeconds(4f);
        }

        while (enemyCntA + enemyCntB + enemyCntC + enemyCntD > 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(4f);
        StageEnd();
    }

    void Update()
    {
        if (isBattle)
            playTime += Time.deltaTime;
    }

    void LateUpdate() //Update가 끝난 후 호출됨
    {
        //상단 UI
        scoreTXT.text = string.Format("{0:n0}", player.score);
        stageTXT.text = "STAGE:" + stage;
        playTimeTXT.text = "";

        int hour= (int)(playTime / 3600);
        int min= (int)((playTime - hour * 3600)/ 60);
        int sec= (int)(playTime % 60);
        playTimeTXT.text = string.Format("{0:00}", hour) + ":" + string.Format("{0:00}", min) + ":" + string.Format("{0:00}", sec);

        //플레이어 UI
        playerHealthTXT.text = player.health + "/" + player.maxHealth;
        playercoinTXT.text = string.Format("{0:n0}", player.coin);
        if (player.equipWeapon == null)
        {
            playerAmmoTXT.text = "- /" + player.ammo;
        } else if (player.equipWeapon.type == Weapon.Type.Melee)
        {
            playerAmmoTXT.text = "- /" + player.ammo;
        } else playerAmmoTXT.text = player.equipWeapon.curAmmo + " /" + player.ammo;

        //무기 UI
        weapon1IMG.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        weapon2IMG.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        weapon3IMG.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);
        weaponRIMG.color = new Color(1, 1, 1, player.hasGrenades > 0 ? 1 : 0);

        //몬스터 숫자UI
        enemyATXT.text = enemyCntA.ToString();
        enemyBTXT.text = enemyCntB.ToString();
        enemyCTXT.text = enemyCntC.ToString();

        //보스 체력 UI
        if(boss != null)
        bossHealthBar.localScale = new Vector3((float)boss.curHealth / boss.maxHealth, 1,1);
    }
}
