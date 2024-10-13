using UnityEngine;

public class EN_ObjetoEnemigo : MonoBehaviour
{
	public bool Utilizado { get; protected set; }

	public bool Disponible { get; protected set; }

	public virtual void Interaccionar()
	{
	}
}
