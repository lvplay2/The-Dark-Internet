using UnityEngine;

public class ES_ExtrasSkin : MonoBehaviour, IN_ISkin
{
	public ES_Datos_Controlador.TipoSkin tipoSkin;

	private MeshRenderer meshRenderer;

	private SkinnedMeshRenderer skinnedMeshRenderer;

	private Material[] materiales;

	private void Awake()
	{
		meshRenderer = GetComponent<MeshRenderer>();
		if (meshRenderer != null)
		{
			materiales = meshRenderer.materials;
			return;
		}
		skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
		materiales = skinnedMeshRenderer.materials;
	}

	private void Start()
	{
		Cargar_Skin();
	}

	public void Cargar_Skin()
	{
		ES_Skin_Contenedor eS_Skin_Contenedor = ES_EstadoJuego.estadoJuego.DatosContenido.ObtenerSkin_Contenedor(ES_EstadoJuego.estadoJuego.DatosControlador.Consultar_Skin_Seleccionada(tipoSkin), tipoSkin);
		for (int i = 0; i < materiales.Length; i++)
		{
			materiales[i].mainTexture = eS_Skin_Contenedor.textura;
		}
	}
}
