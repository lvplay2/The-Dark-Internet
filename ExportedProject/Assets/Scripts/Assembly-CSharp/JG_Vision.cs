using UnityEngine;
using UnityEngine.UI;

public class JG_Vision : MonoBehaviour
{
	public JG_Jugador jugador;

	[Header("Configuración")]
	public float distanciaMaxima;

	public LayerMask layerMask;

	[HideInInspector]
	public IT_Interactivo ElementoEnVista;

	public Text TextoNombre;

	private void Update()
	{
		ActualizarVision(base.transform);
	}

	public void ActualizarVision(Transform t)
	{
		RaycastHit hitInfo;
		Physics.Raycast(t.position, t.forward, out hitInfo, distanciaMaxima, layerMask);
		IT_Interactivo iT_Interactivo = null;
		if (hitInfo.collider != null)
		{
			iT_Interactivo = hitInfo.transform.GetComponent<IT_Interactivo>();
			ElementoEnVista = ((iT_Interactivo != null) ? iT_Interactivo : null);
			TextoNombre.text = ((ElementoEnVista != null) ? ElementoEnVista.Nombre : string.Empty);
		}
		else
		{
			TextoNombre.text = string.Empty;
			ElementoEnVista = null;
		}
	}
}
