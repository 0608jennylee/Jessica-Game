﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour {

    private List<GameObject> items = new List<GameObject>();

    private List<Sprite> itemSprites = new List<Sprite>();

    private Collider2D currentItemZone;

    private GameObject freeItemSlot;

    private GameObject glow;

    private bool inItemZone = false;

    private PlayerController player;


    // Use this for initialization
    void Start() {

        GameObject itemSlots = GameObject.FindGameObjectWithTag("ItemSlots");

        player = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerController>();


        //Loads item slots
        for (int i = 0; i < itemSlots.transform.childCount; i++)
        {
            items.Add(itemSlots.transform.GetChild(i).gameObject);
        }

        Object[] sprites = Resources.LoadAll("Sprites", typeof(Sprite));
        //Loads item sprites
        for (int i = 0; i < sprites.Length; i++)
        {
            Sprite sprite = (Sprite)sprites[i];
            itemSprites.Add(sprite);
        }


        // Make sure halo is visible.
        GameObject[] withHalo = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in withHalo) {
            //item.GetComponent<ParticleSystemRenderer>().sortingLayerName = "Objects";
        }

        glow = GameObject.Find("Glow");
        glow.SetActive(false);

    }

    // Update is called once per frame
    void Update() {

        if (inItemZone)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !currentItemZone.Equals(null))
            {
                freeItemSlot.GetComponent<Image>().sprite = currentItemZone.GetComponent<SpriteRenderer>().sprite;
                items.Remove(currentItemZone.gameObject);
                Destroy(currentItemZone.gameObject);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag.Equals("Item"))
        {
            Vector2 temp = collision.gameObject.transform.position;
            glow.transform.position = temp;
            glow.SetActive(true);
            currentItemZone = collision;
            inItemZone = true;

            //Loop through item slots to find one that is free
            foreach (GameObject item in items)
            {
                if (item.GetComponent<Image>().sprite.name.Equals("Background"))
                {

                    freeItemSlot = item;

                    foreach (Sprite sprite in itemSprites)
                    {

                        if (collision.gameObject.GetComponent<SpriteRenderer>().sprite.Equals(sprite))
                        {
                            item.GetComponent<Image>().sprite = sprite;
                            player.ChangeConfidence(20);
                            break;
                        }
                    }

                    break;

                }
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Item"))
        {
            glow.SetActive(false);
            currentItemZone = null;
            freeItemSlot = null;
            inItemZone = false;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        //ONLY USED FOR NUNIT TESTING------------------------------------------
        freeItemSlot.GetComponent<Image>().sprite = currentItemZone.GetComponent<SpriteRenderer>().sprite;
        items.Remove(currentItemZone.gameObject);
        Destroy(currentItemZone.gameObject);
        //---------------------------------------------------
    }

}
