using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    #region Private
    [SerializeField]
    private List<GameObject> _revealers;
    private GameObject _building;
    [SerializeField]
    private int _width;
    [SerializeField]
    private int _height;
    [SerializeField]
    private Vector2 _mapSize;
    [SerializeField]
    private Material _fogMaterial;

    private Texture2D _shadowMap;
    private Color32[] _pixels;
    #endregion

    public GameObject plants;
    public float darkColor;
    public int fogOffset;
    public Transform terrainTransform;

	private int frameCounter=0;
	List<float> distances = new List<float>(); // tablica zawierający najmniejszy dystans pomiędzy obiektem a odkrywaczem


    public void addRevealer(GameObject revealer)
    {
        this._revealers.Add(revealer);
    }
    public void removeRevealer(GameObject revealer) { _revealers.Remove(revealer); }
    public void setBuilding(GameObject building)
    {
        _building = building;
    }

    private void Awake()
    {
        _shadowMap = new Texture2D(_width, _height, TextureFormat.RGB24, false);

        _pixels = _shadowMap.GetPixels32();

        for (var i = 0; i < _pixels.Length; ++i)
        {
            _pixels[i].r = 0;
            _pixels[i].g = 255;

        }




        _shadowMap.SetPixels32(_pixels);
        _shadowMap.Apply();

        _fogMaterial.SetTexture("_ShadowMap", _shadowMap);
    }
    private void UpdateShadowMap()
    {
		
        foreach (var revealer in _revealers)
        {
          
            if (revealer.GetComponent<BuildingInteractive>() == null)
				DrawFilledMidpointHexSinglePixelVisit((int)revealer.transform.position.x + 115, (int)revealer.transform.position.z + 135, revealer.GetComponent<ShowUnitInfo>().sight);
            else
            {
				DrawFilledMidpointHexSinglePixelVisit((int)revealer.transform.position.x + 115, (int)revealer.transform.position.z +135, revealer.GetComponent<ShowUnitInfo>().sight);

            }
  
        }
        
    }
    public void drawBuildingCircle(int centerX, int centerY, int radius)
    {


        //DOSTROJENIE X,Y JEDNOSTKI
        int Y, X, r, offset_grad;
        X = centerX + (int)_mapSize.x / 5;
        Y = centerY + (int)_mapSize.y / 12;
        r = radius;

        int[] topHalf = new int[radius * 2];
        int[] bottomHalf = new int[radius * 2];

        //ALGORYTM RYSUJĄCY KOŁO Z RÓWNANIA GEOMETRII ANALIYCZNEJ
        int start = X - radius;
        for (int i = 0; i < radius * 2; i++)
        {

            topHalf[i] = (int)Mathf.Sqrt(r * r - (start - X) * (start - X)) + Y;
            bottomHalf[i] = Y - (topHalf[i] - Y);
            start++;
        }

        //FOGOFFSET W ZAŁOŻENIU MA OKREŚLAC SZEROKOŚC ŁAGODNEGO PRZEJSCIA,  OFFSET_GRAD-JEGO GRADIENT  - DO DOPRACOWANIA,
        start = X - radius;
        for (int i = 0; i < radius * 2; i++)
        {
            for (int j = bottomHalf[i] + fogOffset; j < topHalf[i] - fogOffset; j++)
            {
                _pixels[start * _width + j].r = 255;

            }


            start++;
        }


    }
    private void DrawFilledMidpointCircleSinglePixelVisit(int centerX, int centerY, int radius)
    {
        //DOSTROJENIE X,Y JEDNOSTKI
        int Y, X, r, offset_grad;
        X = centerX + (int)_mapSize.x / 5;
        Y = centerY + (int)_mapSize.y / 12;
        r = radius;

        int[] topHalf = new int[radius * 2];
        int[] bottomHalf = new int[radius * 2];

        //ALGORYTM RYSUJĄCY KOŁO Z RÓWNANIA GEOMETRII ANALIYCZNEJ
        int start = X - radius;
        for (int i = 0; i < radius * 2; i++)
        {

            topHalf[i] = (int)Mathf.Sqrt(r * r - (start - X) * (start - X)) + Y;
            bottomHalf[i] = Y - (topHalf[i] - Y);
            start++;
        }

        //FOGOFFSET W ZAŁOŻENIU MA OKREŚLAC SZEROKOŚC ŁAGODNEGO PRZEJSCIA,  OFFSET_GRAD-JEGO GRADIENT  - DO DOPRACOWANIA,
        start = X - radius;
        for (int i = 0; i < radius * 2; i++)
        {
            for (int j = bottomHalf[i] + fogOffset; j < topHalf[i] - fogOffset; j++)
            {
                _shadowMap.SetPixel(start, j, new Color(255, 255, 0));

            }

            //PRÓBA STWORZENIA ŁAGODNEGO PRZEJSCIA ROZJAŚNIACZA

            //offset_grad = 30;
            //for (int j = bottomHalf[i] ; j < bottomHalf[i] + fogOffset; j++)
            //{
            //    _shadowMap.SetPixel(start, j, new Color(255-offset_grad, 255-offset_grad, 0));
            //    offset_grad--;
            //}
            //offset_grad = 0;
            //for (int j = topHalf[i] - fogOffset; j < topHalf[i]; j++)
            //{
            //    _shadowMap.SetPixel(start, j, new Color(255-offset_grad, 255-offset_grad, 0));
            //    offset_grad++;
            //}
            start++;
        }

    }

    private void DrawFilledMidpointHexSinglePixelVisit(int centerX, int centerY, int r)
    {
        //DOSTROJENIE X,Y JEDNOSTKI
        int Y, X, half_r, double_r, a, start;
        X = centerX + (int)_mapSize.x / 5;
        Y = centerY + (int)_mapSize.y / 12;
        half_r = r / 2;
        double_r = r * 2;
        int[,] peeks = new int[6, 2];

        int[] topCoordinates = new int[double_r];
        int[] bottomCoordinates = new int[double_r];
        //6 WSPÓŁRZĘDNYCH HEKSAGONU

        peeks[0, 0] = X - r;
        peeks[0, 1] = Y;

        peeks[1, 0] = X - half_r;
        peeks[1, 1] = Y + r;

        peeks[2, 0] = X + half_r;
        peeks[2, 1] = Y + r;

        peeks[3, 0] = X + r;
        peeks[3, 1] = Y;

        peeks[4, 0] = X + half_r;
        peeks[4, 1] = Y - r;

        peeks[5, 0] = X - half_r;
        peeks[5, 1] = Y - r;
        //RÓWNANIE PROSTEJ PRZECHODZACEJ PRZEZ 2 PUNKTY


        for (int i = 0; i < half_r; i++)
        {
            topCoordinates[i] = 2 * i + Y;
            bottomCoordinates[i] = Y - 2 * i;
        }

        for (int i = half_r; i < r + half_r; i++)
        {
            topCoordinates[i] = Y + r;
            bottomCoordinates[i] = Y - r;
        }

        for (int i = r + half_r, j = half_r; i < double_r; i++, j--)
        {
            topCoordinates[i] = topCoordinates[j];
            bottomCoordinates[i] = bottomCoordinates[j];
        }

        start = X - r;
        for (int i = 0; i < double_r; i++)
        {
            for (int j = bottomCoordinates[i]; j < topCoordinates[i]; j++)
            {
                _shadowMap.SetPixel(start, j, new Color(255, 255, 0));
            }
            start++;
        }


    }
    private void Start()
    {
        for (var i = 0; i < _pixels.Length; ++i)
        {
            _pixels[i].r = 0;
            _pixels[i].g = 255;

        }

        terrainTransform = GameObject.Find("ImageTarget").transform;
    }
    private void Update()
    {

		frameCounter++;
		if (frameCounter < 2)
			return;

		distances.Clear ();

        _shadowMap.SetPixels32(_pixels); // malujemy mapę na "szaro"
        UpdateShadowMap(); // odkrywamy część mapy


        foreach (Transform t in plants.transform)
        {
            distances.Add(float.MaxValue);
        }
        int iterator;
        foreach (GameObject g in _revealers)
        {
            iterator = 0;
            foreach (Transform t in plants.transform)
            {

                float distance = Vector3.Distance(g.transform.position, t.transform.position); // liczymy dystans pomiędzy "odkrywaczem" a obiektem

				if (!(distance > g.GetComponent<ShowUnitInfo>().sight + 50)) // jeśli dystans jest większy niż jego wzrok + 50 jednostek 
                {
                    if (distance < distances[iterator]) // jeśli dystans jest mniejszy niż najmniejszy dystans "odkrywacza" a obiektu
                    {
                        distances[iterator] = distance; //ustaw nowy najmniejszy dystans
						float difference = g.GetComponent<ShowUnitInfo>().sight + 50 + darkColor; // różnica, która zapewni, że w największej odległości kolor będzie taki sam jak "darkColor"
                        float colorChange = (difference - distance); // obliczamy nowy kolor w zależności od dystansu
                        t.GetComponentInChildren<Renderer>().material.SetColor("_Color", new Color(colorChange / 255f, colorChange / 255f, colorChange / 255f)); //ustaw nowy kolor
                    }


                }

                iterator++;


            }




        }

        _shadowMap.Apply();
    }




}