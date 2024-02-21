using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public enum GRAPH
{
    LINEAR,
    EASESIN,
    EASEQUAD,
    EASECUBIC,
    EASEQUART,
    EASEQUINT,
    EASEEXPO,
    EASECIRC,
    EASEBACK,
    EASEELASTIC,
}

public enum INANDOUT
{
    IN,
    OUT,
    INOUT,
}

public enum LOOP
{
    CLAMP,
    LOOP,
    PINGPONG,
    REVERSE
}

public struct AnimatingCurve
{
    public Vector3 beginValue, endValue;
    public float beginValueF, endValueF;
    public float timeSinceBegin, duration;
    public GRAPH type;
    public LOOP loop;
    public INANDOUT inOut;

    public float reverse;
    public AnimationCurve animCurv;
    public AnimatingCurve(Vector3 _startValue, Vector3 _endValue,float _duration, GRAPH _type, INANDOUT _inOut, LOOP _loop)
    {
        timeSinceBegin = 0f;
        duration = _duration;
        type = _type;
        loop = _loop;
        reverse = 1f;
        inOut = _inOut;

        beginValue = _startValue;
        endValue = _endValue;
        beginValueF = 0f;
        endValueF = 0f;
        animCurv = null;
    }
    public AnimatingCurve(float _startValue, float _endValue, float _duration, GRAPH _type, INANDOUT _inOut, LOOP _loop)
    {
        timeSinceBegin = 0f;
        duration = _duration;
        type = _type;
        loop = _loop;
        reverse = 1f;
        inOut = _inOut;

        beginValue = Vector3.zero;
        endValue = Vector3.zero;
        beginValueF = _startValue;
        endValueF = _endValue;

        animCurv = null;
    }
    public AnimatingCurve(AnimationCurve animCurv, Vector3 _endValue)
    {
        beginValue = Vector3.zero;
        endValue = _endValue;
        beginValueF = 0f;
        endValueF = 0f;
        timeSinceBegin = 0f;
        duration = 0f;
        type = GRAPH.LINEAR;
        inOut = INANDOUT.IN;
        loop = LOOP.CLAMP;
        reverse = 0f;
        this.animCurv = animCurv;
    }
}

public partial class Tools 
{
    private static void DrawCurve(AnimatingCurve curve, ref Vector3 valueToModify)
    {
        float saveModif = Mathf.Clamp01(curve.timeSinceBegin / curve.duration);
        EASELINEAR(curve, ref saveModif);
        EASESIN(curve, ref saveModif);
        EASESCUBIC(curve, ref saveModif);
        EASESQUAD(curve, ref saveModif);
        EASESQUART(curve, ref saveModif);
        EASEQUINT(curve, ref saveModif);
        EASEEXPO(curve, ref saveModif);
        EASECIRC(curve, ref saveModif);
        EASEBACK(curve, ref saveModif);
        EASEELASTIC(curve, ref saveModif);

        valueToModify = (curve.beginValue + (curve.endValue - curve.beginValue) * saveModif);
    }

    private static void DrawCurve(AnimatingCurve curve, ref float valueToModify)
    {
        float saveModif = Mathf.Clamp01(curve.timeSinceBegin / curve.duration);
        EASELINEAR(curve, ref saveModif);
        EASESIN(curve, ref saveModif);
        EASESCUBIC(curve, ref saveModif);
        EASESQUAD(curve, ref saveModif);
        EASESQUART(curve, ref saveModif);
        EASEQUINT(curve, ref saveModif);
        EASEEXPO(curve, ref saveModif);
        EASECIRC(curve, ref saveModif);
        EASEBACK(curve, ref saveModif);
        EASEELASTIC(curve, ref saveModif);

        valueToModify = (curve.beginValueF + (curve.endValueF - curve.beginValueF) * saveModif);
    }
    private static void LoopGestion(ref AnimatingCurve curve, ref Vector3 valueToModify)
    {
        if (curve.loop == LOOP.LOOP)
            curve.timeSinceBegin = 0f;
        else if (curve.loop == LOOP.PINGPONG)
        {
            curve.timeSinceBegin = 0f;
            (curve.beginValue, curve.endValue) = (curve.endValue, curve.beginValue);
        }
        else if (curve.loop == LOOP.REVERSE)
        {
            curve.reverse = curve.reverse * -1f;
            if (curve.reverse == -1)
                curve.timeSinceBegin = curve.duration;
            else
                curve.timeSinceBegin = 0f;
        }
    }
    private static void LoopGestion(ref AnimatingCurve curve, ref float valueToModify)
    {
        if (curve.loop == LOOP.LOOP)
            curve.timeSinceBegin = 0f;
        else if (curve.loop == LOOP.PINGPONG)
        {
            curve.timeSinceBegin = 0f;
            (curve.beginValue, curve.endValue) = (curve.endValue, curve.beginValue);
        }
        else if (curve.loop == LOOP.REVERSE)
        {
            curve.reverse = curve.reverse * -1f;
            if (curve.reverse == -1)
                curve.timeSinceBegin = curve.duration;
            else
                curve.timeSinceBegin = 0f;
        }
    }
    private static void EASELINEAR (AnimatingCurve curve, ref float valueToModify)
    {
        if (curve.type == GRAPH.LINEAR)
        {
            float t = Mathf.Clamp01(curve.timeSinceBegin / curve.duration);
            valueToModify = t;
        }
    }
    private static void EASESIN (AnimatingCurve curve, ref float valueToModify)
    {
        if (curve.type == GRAPH.EASESIN)
        {
            float t = Mathf.Clamp01(curve.timeSinceBegin / curve.duration);

            if (curve.inOut == INANDOUT.IN)
                valueToModify = 1f - Mathf.Cos((float)(t * Math.PI / 2f));
            else if(curve.inOut == INANDOUT.OUT)
                valueToModify = Mathf.Sin((float)(t * Math.PI / 2f));
            else
                valueToModify = -((Mathf.Cos((float)(Math.PI * t))) - 1) / 2; 
        }
    }
    
    private static void EASESCUBIC (AnimatingCurve curve, ref float valueToModify)
    {
        if (curve.type == GRAPH.EASECUBIC)
        {
            float t = Mathf.Clamp01(curve.timeSinceBegin / curve.duration);
            if (curve.inOut == INANDOUT.IN)
                valueToModify = t * t * t;
            else if(curve.inOut == INANDOUT.OUT)
                valueToModify = 1 - Mathf.Pow(1f - t, 3f);
            else
                valueToModify = t < 0.5 ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2;
        }
    }
    private static void EASESQUAD (AnimatingCurve curve, ref float valueToModify)
    {
        if (curve.type == GRAPH.EASEQUAD)
        {
            float t = Mathf.Clamp01(curve.timeSinceBegin / curve.duration);
            if (curve.inOut == INANDOUT.IN)
                valueToModify = t * t;
            else if (curve.inOut == INANDOUT.OUT)
                valueToModify = 1f - (1f - t) * (1 - t);
            else
                valueToModify = t < 0.5f ? 2f * t * t : 1 - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
        }
    }
    private static void EASESQUART (AnimatingCurve curve, ref float valueToModify)
    {
        if (curve.type == GRAPH.EASEQUART)
        {
            float t = Mathf.Clamp01(curve.timeSinceBegin / curve.duration);
            if (curve.inOut == INANDOUT.IN)
                valueToModify = t * t * t * t;
            else if (curve.inOut == INANDOUT.OUT)
                valueToModify = 1f - Mathf.Pow(1f - t, 4f);
            else
                valueToModify = t < 0.5f ? 8f * t * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 4f) / 2f;
        }
    }
    
    private static void EASEQUINT (AnimatingCurve curve, ref float valueToModify)
    {
        if (curve.type == GRAPH.EASEQUINT)
        {
            float t = Mathf.Clamp01(curve.timeSinceBegin / curve.duration);
            if (curve.inOut == INANDOUT.IN)
                valueToModify = Mathf.Pow(t,5f);
            else if (curve.inOut == INANDOUT.OUT)
                valueToModify = 1f - Mathf.Pow(1f - t, 5f);
            else
                valueToModify = t < 0.5f ? 16f * Mathf.Pow(t, 5f) : 1f - Mathf.Pow(-2f * t + 2f, 5f) / 2f;
        }
    }
    private static void EASEEXPO (AnimatingCurve curve, ref float valueToModify)
    {
        if (curve.type == GRAPH.EASEEXPO)
        {
            float t = Mathf.Clamp01(curve.timeSinceBegin / curve.duration);
            if (curve.inOut == INANDOUT.IN)
                valueToModify = t == 0f ? 0f : Mathf.Pow(2f, 10f * t - 10f);
            else if (curve.inOut == INANDOUT.OUT)
                valueToModify = t == 1f ? 1f : 1f - Mathf.Pow(2f, -10f * t);
            else
                valueToModify = t == 0f ? 0f: t == 1f? 1f: t < 0.5f ? Mathf.Pow(2f, 20f * t - 10f) / 2f: (2f - Mathf.Pow(2f, -20f * t + 10f)) / 2f;
        }
    }
    private static void EASECIRC (AnimatingCurve curve, ref float valueToModify)
    {
        if (curve.type == GRAPH.EASECIRC)
        {
            float t = Mathf.Clamp01(curve.timeSinceBegin / curve.duration);
            if (curve.inOut == INANDOUT.IN)
                valueToModify = t >= 1f ? 1f :t <= 0f ? 0f : 1f - Mathf.Sqrt(1f - Mathf.Pow(t, 2f));
            else if (curve.inOut == INANDOUT.OUT)
                valueToModify = t == 1f ? 1f : t <= 0f ? 0f : Mathf.Sqrt(1f - Mathf.Pow(t - 1f, 2f));
            else
                valueToModify = t < 0.5f ? (1f - Mathf.Sqrt(1f - Mathf.Pow(2f * t, 2f))) / 2f : (Mathf.Sqrt(1f - Mathf.Pow(-2f * t + 2f, 2f)) + 1f) / 2f;
        }
    }
    
    private static void EASEBACK (AnimatingCurve curve, ref float valueToModify)
    {
        if (curve.type == GRAPH.EASEBACK)
        {
            float c1 = 1.70158f;
            float c2 = c1 * 1.525f;
            float c3 = c1 + 1f;

            float t = Mathf.Clamp01(curve.timeSinceBegin / curve.duration);
            if (curve.inOut == INANDOUT.IN)
                valueToModify = c3 * t * t * t - c1 * t * t;
            else if (curve.inOut == INANDOUT.OUT)
                valueToModify = 1 + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
            else
                valueToModify = t < 0.5f? (Mathf.Pow(2f * t, 2f) * ((c2 + 1f) * 2f * t - c2)) / 2f: (Mathf.Pow(2f * t - 2f, 2f) * ((c2 + 1f) * (t * 2f - 2f) + c2) + 2f) / 2f;
        }
    }
    private static void EASEELASTIC (AnimatingCurve curve, ref float valueToModify)
    {
        if (curve.type == GRAPH.EASEELASTIC)
        {
            float c4 = (float)(2f * Math.PI) / 3f;
            float c5 = (float)(2f * Math.PI) / 4.5f;

            float t = Mathf.Clamp01(curve.timeSinceBegin / curve.duration);
            if (curve.inOut == INANDOUT.IN)
                valueToModify = t == 0 ? 0: t == 1? 1: -Mathf.Pow(2f, 10f * t - 10f) * Mathf.Sin((t * 10f - 10.75f) * c4);
            else if (curve.inOut == INANDOUT.OUT)
                valueToModify = t == 0f ? 0f: t == 1f? 1f: Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * c4) + 1f;
            else
                valueToModify = t == 0f? 0f: t == 1f? 1f: t < 0.5f? -(Mathf.Pow(2f, 20f * t - 10f) * Mathf.Sin((20f * t - 11.125f) * c5)) / 2f: (Mathf.Pow(2f, -20f * t + 10f) * Mathf.Sin((20f * t - 11.125f) * c5)) / 2f + 1f;
        }
    }
}
