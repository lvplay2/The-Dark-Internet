using UnityEngine;

public class PZ_3_Gancho : MonoBehaviour
{
	private Transform objeto;

	public Animator animator;

	public Transform posicionExpulsion;

	public Vector3 fuerzaExpulsion;

	private bool _objetoEnGancho;

	private bool _objetoDisponible;

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag(ES_Tags.ObjetoDeMaquina))
		{
			objeto = other.transform;
			_objetoDisponible = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag(ES_Tags.ObjetoDeMaquina))
		{
			_objetoDisponible = false;
		}
	}

	public void IntentarRecoger()
	{
		if (_objetoDisponible && !_objetoEnGancho)
		{
			objeto.parent = base.transform;
			_objetoEnGancho = true;
		}
	}

	public void CerrarGancho()
	{
		if (!_objetoEnGancho)
		{
			animator.ResetTrigger("Abrir");
			animator.ResetTrigger("Cerrar");
			animator.SetTrigger("Cerrar");
		}
	}

	public void Soltar()
	{
		if (_objetoEnGancho)
		{
			objeto.parent = null;
			Rigidbody component = objeto.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.collisionDetectionMode = CollisionDetectionMode.Continuous;
				component.isKinematic = false;
				component.useGravity = true;
			}
			Invoke("Liberar", 0.5f);
		}
		animator.ResetTrigger("Abrir");
		animator.ResetTrigger("Cerrar");
		animator.SetTrigger("Abrir");
	}

	private void Liberar()
	{
		objeto.transform.position = posicionExpulsion.position;
		objeto.GetComponent<Rigidbody>().velocity = fuerzaExpulsion;
		objeto.gameObject.layer = LayerMask.NameToLayer(ES_Tags.Interactivo);
		objeto = null;
		_objetoEnGancho = false;
	}
}
