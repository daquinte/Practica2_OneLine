using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Clase que funciona como manager de un nivel. 
/// Controla la logica de los tiles del nivel
/// Gestiona el input en la aplicación
/// </summary>
public class BoardManager : MonoBehaviour
{
    //Atributos publicos
    [Tooltip("Prefab of a Tile")]
    public Tile prefabTile;

    [Tooltip("Objeto cursor de tu escena")]
    public Cursor cursor;

    [Space]
    [Tooltip("Si quieres una skin en particular, añadela aquí.")]
    public TileSkin preferedSkin;

    [Tooltip("Array de ScriptableObjects para las skins.")]
    public List<TileSkin> tileSkins;


    public float TARGET_WIDTH = 1080;
    public float TARGET_HEIGHT = 1920;
    public float PIXELS_TO_UNITS = 110;


    //Atributos privados
    private InputManager inputManager;


    /// <summary>
    /// Array con los tiles
    /// </summary>
    private Tile[,] tiles;

    /// <summary>
    /// Camino a la meta
    /// </summary>
    private int[,] pistas;


    /// <summary>
    /// Recorrido efectuado en forma de pila.
    /// </summary>
    private Stack<Tile> caminoTiles;

    private TileSkin currentTileSkin;   //TileSkin del nivel actual


    private int nTotalTiles;            //Número total de tiles 
    private int nFils;                  //numero filas del tablero
    private int nCols;                  //numero columnas del tablero

    private int _anchoTile;
    private int _altoTile;

    bool init = false; //TEMPORAL, QUITAR CUANDO SE CARGE EL NIVEL DESDE EL MENU

    // Use this for initialization
    void Start()
    {
        caminoTiles = new Stack<Tile>();

        GetRandomSkin();
        cursor.SetSprite(currentTileSkin.spriteDedo);

        GameManager.instance.SetBoardManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        //QUITAR ESTE IF CUANDO EL OBJETO SE CARGE DESDE EL MENU
        if (init)
        {
            if (GameManager.instance.GetInputManager().getInputInfo().pulsado)
            {
                coordsDentroMatriz((int)Math.Round(GameManager.instance.GetInputManager().getInputInfo().position.x),
                                    (int)Math.Round(GameManager.instance.GetInputManager().getInputInfo().position.y));
            }
        }
    }

    /// <summary>
    /// Métodos de inicialización del Board Manager.
    /// </summary>
    #region Start Methods

    public void InitMap(InfoNivel infoNivel)
    {
        nFils = infoNivel.layout.Length;
        nCols = infoNivel.layout[0].Length;
        pistas = infoNivel.path;

        tiles = new Tile[nCols, nFils];
        nTotalTiles = 0;

        //Situamos la cámara
        Camera.main.transform.position = new Vector3((nCols / 2.0f) - 0.5f, (nFils / 2.0f) - 0.5f, Camera.main.transform.position.z);
        ResizeCamera(nCols, nFils);

        //Interpretamos la información del layout
        int layout = tiles.GetLength(1) - 1;
        int filaLogica = nFils - 1;
        int columnaLogica = 0;

        for (int filas = 0; filas < tiles.GetLength(1); filas++)
        {
            string infoFila = infoNivel.layout[layout];
            for (int cols = 0; cols < tiles.GetLength(0); cols++)
            {
                char tipoTile = infoFila[cols];
                if (tipoTile != '0')
                {
                    ///Si en el futuro hay que escalar los bloques, si quitas que sean hijos de boardManager se pueden escalar!
                    Tile tile = Instantiate(prefabTile, new Vector3(cols, filas, 0), Quaternion.identity, transform);
                    tile.gameObject.name = "Bloque" + cols + filas;
                    tile.SetPosicionLogica(filaLogica, columnaLogica);
                    tile.SetTileSkin(currentTileSkin);
                    tiles[cols, filas] = tile;
                    nTotalTiles++;
                    if (tipoTile == '2') //Si es inicial...
                    {
                        tile.SetTileInicial();
                        caminoTiles.Push(tiles[cols, filas]);
                    }

                }
                //Si es 0, nos lo saltamos pero aumentamos Y logica
                columnaLogica++;

            }
            filaLogica--;
            columnaLogica = 0;
            layout--;
        }
        init = true;

    }


    private void GetRandomSkin()
    {
        if (preferedSkin == null)
        {
            int rnd = UnityEngine.Random.Range(0, tileSkins.Count);
            currentTileSkin = tileSkins[rnd];
        }
        else currentTileSkin = preferedSkin;
    }

    private void ResizeCamera(int cols, int fils)
    {
        float desiredRatio = TARGET_WIDTH / TARGET_HEIGHT;
        float currentRatio = (float)Screen.width / (float)Screen.height;

        if (currentRatio >= desiredRatio)
        {
            Camera.main.orthographicSize = TARGET_WIDTH / 1.5f / PIXELS_TO_UNITS;
        }
        else
        {
            float differenceInSize = desiredRatio / currentRatio;
            Camera.main.orthographicSize = TARGET_WIDTH / 1.5f / PIXELS_TO_UNITS * differenceInSize;
        }
    }
    #endregion

    /// <summary>
    /// Métodos de lógica de juego y comprobaciones del mismo.
    /// </summary>
    #region Logic Methods
    public void SetTilePulsado(int x, int y)
    {
        if (tiles[x, y].GetPulsado())
        {
            DeshacerCamino(tiles[x, y]);
        }
        else
        {
            Vector3 posicion = new Vector3(0, 0, 0);
            Vector3 sentido = new Vector3(0, 0, 0);
            if (esCandidato(caminoTiles.Peek(), tiles[x, y], ref posicion, ref sentido))
            {
                tiles[x, y].Pulsar();
                tiles[x, y].marcarCamino(false, caminoTiles.Peek(), posicion, sentido);
                caminoTiles.Push(tiles[x, y]);

                if (NivelCompletado())
                {
                    Debug.Log("Nivel completado :DD:D");
                }
            }
        }
    }

    public void coordsDentroMatriz(int x, int y)
    {
        if ((x >= 0 && x < tiles.GetLength(0)) && (y >= 0 && y < tiles.GetLength(1)) && (tiles[x, y] != null))
        {
            SetTilePulsado(x, y);
        }
    }

    /// <summary>
    /// Reinicia el camino del nivel al inicio del mismo.
    /// Sólo se respeta el tile inicial
    /// </summary>
    public void ReiniciaCamino()
    {
        while (caminoTiles.Count != 1)
        {
            caminoTiles.Peek().Despulsar();
            caminoTiles.Pop();
            caminoTiles.Peek().DesmarcarCamino();
        }
    }

    public void MostrarPista()
    {
        if (!NivelCompletado())
        {
            int fila = 0;
            bool flag = false;


            Tile ultimoCorrecto = null; //Minimo, el primero del camino siempre es igual a la pista.
            Stack<Tile> reversedStack = new Stack<Tile>();
            while (caminoTiles.Count != 0)
            {
                reversedStack.Push(caminoTiles.Pop());
            } //Revertimos el stack para recorrerlo
            //Recorremos el stack del camino del jugador buscando en que tile deja de seguir las pistas
            foreach (Tile tile in reversedStack)
            {
                int filaLogicaCamino = tile.filaLogica;
                int columnaLogicaCamino = tile.columnaLogica;
                Debug.Log("FILA-L: " + filaLogicaCamino + ", COL-L: " + columnaLogicaCamino);
                Debug.Log("FILA-P: " + pistas[fila, 0] + ", COL-P: " + pistas[fila, 1]);
                if (filaLogicaCamino == pistas[fila, 0] && columnaLogicaCamino == pistas[fila, 1])
                {
                    fila++;
                    ultimoCorrecto = tile;
                    Debug.Log("TILE: " + tile.gameObject.name + "Correcto!");
                }
                else
                {
                    Debug.Log("TILE: " + tile.gameObject.name + "incorrecto. ");
                    flag = true;
                    break;
                }
            }

            if (flag)
            {
                Debug.Log("Deshago el camino hasta: " + ultimoCorrecto.gameObject.name);
                DeshacerCamino(ultimoCorrecto);
            }
            Debug.Log("ultimo correcto: " + ultimoCorrecto.gameObject.name);

            MarcarCaminoPistas(fila, ultimoCorrecto);
        }
    }

    /// <summary>
    /// Marca el camino de las pistas. Toma, desde el cominezo, las 5 siguientes posiciones
    /// y las señala como camino a seguir.
    /// </summary>
    /// <param name="comienzo">Int de en que posicion debes comenzar a trazar el camino</param>
    private void MarcarCaminoPistas(int comienzo, Tile ultimoCorrecto)
    {
        Vector3 posicion = new Vector3(0, 0, 0);
        Vector3 sentido = new Vector3(0, 0, 0);
        int final = 0;
        Tile anterior;
        if (nTotalTiles - caminoTiles.Count >= 5)
        {
            final = nTotalTiles;//comienzo + 5;
        }
        else
        {
            final = nTotalTiles;// - caminoTiles.Count;
        }
        anterior = ultimoCorrecto; //para evitar el OPERATION NOT VALID DUE TO THE CURRENT STATE DE MIERDA
        for (int fils = comienzo; fils < final; fils++)
        {
            //Tile aux = BuscaSiguientePista(fils); //Precaucion, no descomentar
            //Debug.Log("TILE PISTA: " + aux.gameObject.name);
            Tile aux = caminoTiles.Peek(); //Te va a dar Operation not valid :DD:D
            bool placebo = esCandidato(anterior, aux, ref posicion, ref sentido);
            tiles[pistas[fils, 1], pistas[fils, 0]].marcarCamino(false, anterior, posicion, sentido);
            anterior = tiles[pistas[fils, 1], pistas[fils, 0]];
        }
    }

    //Si llamas a este método se te cuelga Unity
    //No es una cosa sorprendente, es más: se veía venir.
    //Pero así es la vida.
    private Tile BuscaSiguientePista(int fils)
    {
        int f = 0;
        int c = 0;
        bool stop = false;
        Tile aux = null;
        while (!stop && f < tiles.GetLength(1))
        {
            while (!stop && c < tiles.GetLength(0))
            {
                Tile t = tiles[f, c];
                if (t.filaLogica == pistas[fils, 0] && t.filaLogica == pistas[fils, 1])
                {
                    aux = t;
                    stop = true;
                }
            }
        }
        return aux;
    }
    // TODO: Comentar
    private bool esCandidato(Tile peek, Tile tileCandidato, ref Vector3 posicion, ref Vector3 sentido)
    {
        //Tile top = caminoTiles.Peek();
        int diferenciaX = (int)(tileCandidato.gameObject.transform.position.x - peek.gameObject.transform.position.x);
        int diferenciaY = (int)(tileCandidato.gameObject.transform.position.y - peek.gameObject.transform.position.y);
        if (Math.Abs(diferenciaX) == 1 && diferenciaY == 0)
        {
            if (diferenciaX < 0)
            {
                posicion = new Vector3(-0.5f, 0, 0);
            }
            else
            {
                posicion = new Vector3(0.5f, 0, 0);
            }
            sentido = new Vector3(0, 0, 0);
            return true;
        }
        else if (diferenciaX == 0 && Math.Abs(diferenciaY) == 1)
        {
            if (diferenciaY < 0)
            {
                posicion = new Vector3(0, -0.5f, 0);
            }
            else
            {
                posicion = new Vector3(0, 0.5f, 0);
            }
            sentido = new Vector3(0, 0, 90);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Deshace el camino hasta que el top del stack 
    /// es el bloque que queremos
    /// </summary>
    /// <param name="bloquePulsado"></param>
    private void DeshacerCamino(Tile bloquePulsado)
    {
        bool stop = false;
        while (!stop && caminoTiles.Count >= 1)
        {
            if (caminoTiles.Peek() != bloquePulsado)
            {
                caminoTiles.Peek().Despulsar();
                caminoTiles.Pop();
                caminoTiles.Peek().DesmarcarCamino();
            }
            else stop = true;
        }
    }

    /// <summary>
    /// Encargado de determinar si el nivel se ha completado con éxito o no.
    /// </summary>
    /// <returns>-True si se han marcado todas las casillas, false en caso contrario</returns>
    private bool NivelCompletado()
    {
        return (caminoTiles.Count == nTotalTiles);
    }


    #endregion

}
