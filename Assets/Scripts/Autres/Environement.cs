using Godot;
using System;
using System.Collections.Generic;

public class Environement : Node2D
{
    private static Color MODCAN_D = Color.Color8(255, 255, 255);
    private static Color MODCAN_N = Color.Color8(40, 40, 40);
    
    private static float FOG_D = 0.4f;
    private static float FOG_N = 0.5f;
    
    private static float PLAYERLIGHT_D = 0.0f;
    private static float PLAYERLIGHT_N = 1.2f;
    
    
    
    
    private static float length_day = 60*5.0f;  // seconde
    private float time = length_day / 4; // seconde
    private int nb_day = 0;

    private static int hourNight = 19;
    private static int minNight = 00;
    
    private static int hourDay = 7;
    private static int minDay = 0;

    private static float transition = 4.5f; // %

    public const float MAXPOWERSUN = 1.0f; // energy/seconds
    public static float sunPower = 0;
    
    /*Variable de donnees graphiques*/
    public static List<float> sunPowerhistory = new List<float>();
    
    public enum TimeState
    {
        DAY,
        NIGHT
    }
    public static TimeState cycle = TimeState.DAY;

    private int GetHour(float seconds) => Mathf.FloorToInt(seconds * 24 / length_day);
    private int GetMin(float seconds) => Mathf.FloorToInt(((seconds%(length_day/24))*60)/(length_day/24));


    public override void _Ready()
    {
        if (HasNode("Canvas_DayNight"))
        {
            CanvasModulate CM = GetNode<CanvasModulate>("Canvas_DayNight");
            CM.Color = MODCAN_D;
        }
        if (HasNode("fog"))
        {
            Material mat = GetNode<Sprite>("fog").Material;
            mat.Set("shader_param/mult", FOG_D);
        }
        if (GetTree().GetNodesInGroup("Player").Count == 1)
        {
            Light2D light = ((Node) GetTree().GetNodesInGroup("Player")[0]).GetNode<Light2D>("light");
            light.Energy = PLAYERLIGHT_D;
        }
    }

    public override void _Process(float delta)
    {
        time += delta;
        int hour = GetHour(time);
        int minute = GetMin(time);
        if (hour >= 24)
            time = time % length_day;

        if (hour>hourNight || hour==hourNight && minute>=minNight || hour<hourDay || hour==hourDay && minute<minDay)
        {
            if (cycle == TimeState.DAY)
            {
                cycle = TimeState.NIGHT;
                // passe a la nuit
                Night();
            }
        }
        else
        {
            if (cycle == TimeState.NIGHT)
            {
                cycle = TimeState.DAY;
                nb_day++;
                // passe au jour
                Day();
            }
        }

        sunPower = GetSunPower(hour, minute);
    }

    public void _on_Timer_timeout()
    {
        History<float>.Add(sunPowerhistory, sunPower);
    }


    private void Night()
    {
        Tween twe = new Tween();
        AddChild(twe);
        if (HasNode("Canvas_DayNight"))
        {
            CanvasModulate CM = GetNode<CanvasModulate>("Canvas_DayNight");
            twe.InterpolateProperty(CM,"color",
                MODCAN_D,MODCAN_N, transition*length_day/100,Tween.TransitionType.Sine,Tween.EaseType.In);
        }
        if (HasNode("fog"))
        {
            Material mat = GetNode<Sprite>("fog").Material;
            twe.InterpolateProperty(mat,"shader_param/mult",
                FOG_D,FOG_N, transition*length_day/100,Tween.TransitionType.Sine,Tween.EaseType.In);
        }
        if (GetTree().GetNodesInGroup("Player").Count == 1)
        {
            Light2D light = ((Node) GetTree().GetNodesInGroup("Player")[0]).GetNode<Light2D>("light");
            twe.InterpolateProperty(light,"energy",
                PLAYERLIGHT_D,PLAYERLIGHT_N, transition*length_day/100,Tween.TransitionType.Sine,Tween.EaseType.In);
        }
        
        twe.Start();
    }
    
    private void Day()
    {
        Tween twe = new Tween();
        AddChild(twe);
        if (HasNode("Canvas_DayNight"))
        {
            CanvasModulate CM = GetNode<CanvasModulate>("Canvas_DayNight");
            twe.InterpolateProperty(CM,"color", 
                MODCAN_N,MODCAN_D, transition*length_day/100,Tween.TransitionType.Sine,Tween.EaseType.In);
        }
        if (HasNode("fog"))
        {
            Material mat = GetNode<Sprite>("fog").Material;
            twe.InterpolateProperty(mat,"shader_param/mult",
                FOG_N,FOG_D, transition*length_day/100,Tween.TransitionType.Sine,Tween.EaseType.In);
        }
        if (GetTree().GetNodesInGroup("Player").Count == 1)
        {
            Light2D light = ((Node) GetTree().GetNodesInGroup("Player")[0]).GetNode<Light2D>("light");
            twe.InterpolateProperty(light,"energy",
                PLAYERLIGHT_N,PLAYERLIGHT_D, transition*length_day/100,Tween.TransitionType.Sine,Tween.EaseType.In);
        }
        twe.Start();
    }

    private float GetSunPower(int heure, int minute)
    {
        float H = heure + (minute / 60.0f);
        float w = Mathf.Pi * (1 - (H / 12.0f));
        float sunP = 0;
        if (w >= -Mathf.Pi / 2 && w <= Mathf.Pi / 2)
        {
            sunP = Mathf.Cos(w) * MAXPOWERSUN;
        }
        return sunP;
    }
}
