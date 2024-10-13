using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public static class ST_Utilidades
{
	private static System.Random r = new System.Random();

	public static float[] ObtenerRGBA(this Color color)
	{
		return new float[4] { color.r, color.g, color.b, color.a };
	}

	public static void AsignarRGBA(Color color, float[] datos)
	{
		color = new Color(datos[0], datos[1], datos[2], datos[3]);
	}

	public static void Resetear(this Transform transform)
	{
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.zero;
	}

	public static void Reproducir(this AudioSource audioSource, AudioClip audioClip, float volumen, bool loop)
	{
		audioSource.clip = audioClip;
		audioSource.volume = volumen;
		audioSource.loop = loop;
		audioSource.Play();
	}

	public static void Vaciar(this bool[] arrayEnteros)
	{
		for (int i = 0; i < arrayEnteros.Length; i++)
		{
			arrayEnteros[i] = false;
		}
	}

	public static float Suma(this float[] valores)
	{
		float num = 0f;
		for (int i = 0; i < valores.Length; i++)
		{
			num += valores[i];
		}
		return num;
	}

	public static AudioClip Random(this AudioClip[] audios)
	{
		return audios[UnityEngine.Random.Range(0, audios.Length)];
	}

	public static T Random<T>(this T[] array)
	{
		return array[UnityEngine.Random.Range(0, array.Length)];
	}

	public static bool Entre(this float f, float minimo, float maximo)
	{
		if (f >= minimo)
		{
			return f <= maximo;
		}
		return false;
	}

	public static Vector3 VectorRandom(this Vector3 vector, float minimo, float maximo, int excluirEje = 0)
	{
		float x = ((excluirEje == 1) ? 0f : UnityEngine.Random.Range(minimo, maximo));
		float y = ((excluirEje == 2) ? 0f : UnityEngine.Random.Range(minimo, maximo));
		float z = ((excluirEje == 3) ? 0f : UnityEngine.Random.Range(minimo, maximo));
		return new Vector3(x, y, z);
	}

	public static float Angulo(this float angulo)
	{
		if (!(angulo > 180f))
		{
			return angulo;
		}
		return angulo - 360f;
	}

	public static void Mezclar<T>(this IList<T> lista)
	{
		int num = lista.Count;
		while (num > 1)
		{
			num--;
			int index = r.Next(num + 1);
			T value = lista[index];
			lista[index] = lista[num];
			lista[num] = value;
		}
	}

	public static void AsignarEscalaGlobal(this Transform transform, Vector3 escalaGlobal)
	{
		transform.localScale = Vector3.one;
		transform.localScale = new Vector3(escalaGlobal.x / transform.lossyScale.x, escalaGlobal.y / transform.lossyScale.y, escalaGlobal.z / transform.lossyScale.z);
	}

	public static void AsignarEstado(this Image imagen, bool estado)
	{
		if (imagen.enabled != estado)
		{
			imagen.raycastTarget = estado;
			imagen.enabled = estado;
		}
	}

	public static Vector3? PosicionDePies(this Vector3 vector, bool aumentarDistanciaVertical)
	{
		RaycastHit hitInfo;
		Physics.Raycast(vector + new Vector3(0f, aumentarDistanciaVertical ? 0.25f : 0f, 0f), -Vector3.up, out hitInfo, 5f, LayerMask.GetMask(ES_Tags.Estatico, ES_Tags.Escalera));
		NavMeshHit hit;
		if (hitInfo.collider != null && NavMesh.SamplePosition(hitInfo.point, out hit, 1.5f, 1))
		{
			return hit.position + new Vector3(0f, 0.1f, 0f);
		}
		return null;
	}

	public static Color AsignarColor(this Color c, float? r, float? g, float? b, float? a)
	{
		Color color = c;
		if (r.HasValue)
		{
			color.r = r.Value;
		}
		if (g.HasValue)
		{
			color.g = g.Value;
		}
		if (b.HasValue)
		{
			color.b = b.Value;
		}
		if (a.HasValue)
		{
			color.a = a.Value;
		}
		return c;
	}

	public static float Adaptar(this float f, float diferenciaAbs = 0.1f, int minimo = -1, int maximo = 1)
	{
		if (Mathf.Abs((float)maximo - f) < diferenciaAbs)
		{
			return maximo;
		}
		if (Mathf.Abs((float)minimo - f) < diferenciaAbs)
		{
			return minimo;
		}
		if (Mathf.Abs(f) < diferenciaAbs)
		{
			return 0f;
		}
		return f;
	}

	public static T GetCopyOf<T>(this Component comp, T other) where T : Component
	{
		Type type = comp.GetType();
		if (type != other.GetType())
		{
			return null;
		}
		BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
		PropertyInfo[] properties = type.GetProperties(bindingAttr);
		foreach (PropertyInfo propertyInfo in properties)
		{
			if (propertyInfo.CanWrite)
			{
				try
				{
					propertyInfo.SetValue(comp, propertyInfo.GetValue(other, null), null);
				}
				catch
				{
				}
			}
		}
		FieldInfo[] fields = type.GetFields(bindingAttr);
		foreach (FieldInfo fieldInfo in fields)
		{
			fieldInfo.SetValue(comp, fieldInfo.GetValue(other));
		}
		return comp as T;
	}

	public static T Añadir<T>(this GameObject go, T toAdd) where T : Component
	{
		return go.AddComponent<T>().GetCopyOf(toAdd);
	}
}
