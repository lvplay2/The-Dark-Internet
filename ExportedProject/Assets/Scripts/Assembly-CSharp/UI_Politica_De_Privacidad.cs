using UnityEngine;

public class UI_Politica_De_Privacidad : MonoBehaviour
{
	private string link = "https://sites.google.com/view/thedarkinternet-privacypolicy/p%C3%A1gina-principal";

	public void Abrir_Politica_De_Privacidad()
	{
		Application.OpenURL(link);
	}
}
