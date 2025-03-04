﻿using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class Table
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	private ArrayList m;
	private ArrayList L1;
	private ArrayList L2;
	private ArrayList L3;
	private ArrayList C1;
	public static bool exists = false;

	public Table ()
	{
		m = new ArrayList (46);
		L1 = new ArrayList (46);
		L2 = new ArrayList (46);
		L3 = new ArrayList (46);
		C1 = new ArrayList (46);

		int i = 0;
		while (i < values.Length) {
			this.C1.Add (values[i++]);
			this.m.Add (values[i++]);
			this.L1.Add (values[i++]);
			this.L2.Add (values[i++]);
			this.L3.Add (values[i++]);
		}
	}

	private float [] values = {3.57027f, 0.10f, 0.71751f, 1.22831f, -0.61414f,
		3.61668f, 0.12f, 0.70273f, 1.23986f, -0.59821f,
		3.65660f, 0.14f, 0.68982f, 1.24997f, -0.58455f,
		3.69136f, 0.16f, 0.67834f, 1.25900f, -0.57260f,
		3.72192f, 0.18f, 0.66799f, 1.26715f, -0.56196f,
		3.74899f, 0.20f, 0.65856f, 1.27460f, -0.55238f,
		3.77310f, 0.22f, 0.64989f, 1.28147f, -0.54368f,
		3.79469f, 0.24f, 0.64187f, 1.28785f, -0.53570f,
		3.81410f, 0.26f, 0.63441f, 1.29380f, -0.52834f,
		3.83161f, 0.28f, 0.62743f, 1.29938f, -0.52152f,
		3.84745f, 0.30f, 0.62087f, 1.30464f, -0.51515f,
		3.86182f, 0.32f, 0.61468f, 1.30961f, -0.50919f,
		3.87487f, 0.34f, 0.60883f, 1.31433f, -0.50358f,
		3.88676f, 0.36f, 0.60327f, 1.31882f, -0.49829f,
		3.89759f, 0.38f, 0.59799f, 1.32311f, -0.49329f,
		3.90748f, 0.40f, 0.59295f, 1.32720f, -0.48855f,
		3.91652f, 0.42f, 0.58813f, 1.33113f, -0.48404f,
		3.92748f, 0.44f, 0.58352f, 1.33489f, -0.47974f,
		3.93233f, 0.46f, 0.57909f, 1.33852f, -0.47563f,
		3.93925f, 0.48f, 0.57484f, 1.34200f, -0.47170f,
		3.94557f, 0.50f, 0.57075f, 1.34537f, -0.46794f,
		3.95136f, 0.52f, 0.56681f, 1.34862f, -0.46432f,
		3.95665f, 0.54f, 0.56301f, 1.35176f, -0.46085f,
		3.96148f, 0.56f, 0.55933f, 1.35481f, -0.45751f,
		3.96590f, 0.58f, 0.55578f, 1.35775f, -0.45428f,
		3.96993f, 0.60f, 0.55234f, 1.36061f, -0.45118f,
		3.97360f, 0.62f, 0.54901f, 1.36339f, -0.44817f,
		3.97693f, 0.64f, 0.54578f, 1.36609f, -0.44527f,
		3.97996f, 0.66f, 0.54264f, 1.36872f, -0.44246f,
		3.98271f, 0.68f, 0.53960f, 1.37127f, -0.43973f,
		3.98518f, 0.70f, 0.53663f, 1.37376f, -0.43710f,
		3.98741f, 0.72f, 0.53375f, 1.37619f, -0.43453f,
		3.98941f, 0.74f, 0.53095f, 1.37855f, -0.43204f,
		3.99119f, 0.76f, 0.52821f, 1.38087f, -0.42963f,
		3.99277f, 0.78f, 0.52555f, 1.38312f, -0.42727f,
		3.99417f, 0.80f, 0.52295f, 1.38532f, -0.42498f,
		3.99538f, 0.82f, 0.52041f, 1.38748f, -0.42276f,
		3.99643f, 0.84f, 0.51794f, 1.38959f, -0.42059f,
		3.99733f, 0.86f, 0.51552f, 1.39165f, -0.41847f,
		3.99808f, 0.88f, 0.51316f, 1.39367f, -0.41640f,
		3.99870f, 0.90f, 0.51084f, 1.39565f, -0.41439f,
		3.99918f, 0.92f, 0.50858f, 1.39759f, -0.41242f,
		3.99955f, 0.94f, 0.50637f, 1.39949f, -0.41050f,
		3.99980f, 0.96f, 0.50420f, 1.40136f, -0.40862f,
		3.99995f, 0.98f, 0.50208f, 1.40319f, -0.40678f,
		4.00000f, 1.00f, 0.50000f, 1.40498f, -0.40498f};

	public ArrayList getC1 ()
	{
		return C1;
	}

	public ArrayList getL1 ()
	{
		return L1;
	}

	public ArrayList getL2 ()
	{
		return L2;
	}

	public ArrayList getL3 ()
	{
		return L3;
	}

	public ArrayList getM ()
	{
		return m;
	}

}
