﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateMap : MonoBehaviour {

    private static GenerateMap generateMap = null;
    public static GenerateMap GetGenerateMap
    {
        get
        {
            return generateMap;
        }
    }

    public Color mapColor = Color.red;
    public Color changeColor = Color.grey;
    public int mapSizeX;
    public int mapSizeY;
    public Texture2D map = null;
    public RawImage image;
    public int provinceNum = 0;
    
    public List<Vector2> borderPointList = new List<Vector2>();

    public bool isGetPointOver = true;
    public bool isDrawMeshOver = true;

    public bool isContinue = false;
    void Start () {

        generateMap = this;
        map = Resources.Load("map") as Texture2D;
        map=GameObject.Instantiate(map);
        image.texture = map;
        image.SetNativeSize();
        mapSizeX = map.width;
        mapSizeY = map.height;
        StartCoroutine(GetCitys());
    }

    void FixedUpdate()
    {
        if (Input.anyKeyDown)
            isContinue = true;
    }

    IEnumerator GetCitys()
    {
        for (int i = mapSizeX; i >= 0; i -= 16)
        {
            for (int j = mapSizeY; j >= 0; j -= 16)
            {
                if (map.GetPixel(i, j) == mapColor)
                {
                    isGetPointOver = false;
                    isDrawMeshOver = false;
                    StartCoroutine(Province.GetProvince.GetCityFromPoint(i, j));
                    while (!isGetPointOver)
                    {
                        yield return new WaitForSeconds(0.1f);
                    }
                    if (borderPointList.Count > 2)
                    {
                        Map.GetMap.DeleteDeathPoints();
                        yield return new WaitForSeconds(0.1f);
                        Map.GetMap.NormalizeBorderPoint();
                        StartCoroutine(Map.GetMap.MakeCityMesh());
                        while (!isDrawMeshOver)
                        {
                            yield return new WaitForSeconds(0.1f);
                        }
                    }
                }
            }
        }
    }
}
