 using Godot;
using System;
 using System.Runtime.CompilerServices;

 public class SceneGenerationScript : Node
{

    public int tick = 0;
    public int length_day = 0;
    public int hours = 0;
    public int nb_day = 0;

    public enum Day
    {
        DAY,
        NIGHT
    }
    public Day cycle = Day.DAY;
    
    
    public override void _Ready()
    {
        TileMap ground = (TileMap)GetTree().GetRoot().GetNode("SceneGeneration").GetNode("Ground");
        World.Init(10, ground);
        World.Draw();
        Building.Init(this);

        length_day = 60 * 60 * 10;  //Nombre de ticks par jour (10 minutes)
        tick = length_day / 2;      //On commence à midi.
    }

    public void day_cycle()
    {
        hours = tick / (length_day / 24);
        if (tick > length_day)
        {
            tick = 0;
            nb_day += 1;
        }
        if (hours < 7 || hours > 18)
        {
            cycle_test(Day.NIGHT);
        }
        Console.WriteLine("{0}  -  {1}  |  {3}",hours,tick,cycle);
    }

    public void cycle_test(Day new_cycle)
    {
        if (cycle != new_cycle)
        {
            cycle = new_cycle;
            Tween twe = new Tween();
            if (cycle == Day.NIGHT)
            {
                AddChild(twe);
                CanvasModulate CM = GetNode<CanvasModulate>("Cycle_DayNight");
                twe.InterpolateProperty(CM,"Color",Colors.White,Colors.DarkSlateBlue,10,Tween.TransitionType.Sine,Tween.EaseType.In);
                twe.Start();
                RemoveChild(twe);
            }
            else
            {
                AddChild(twe);
                CanvasModulate CM = GetNode<CanvasModulate>("Cycle_DayNight");
                twe.InterpolateProperty(CM,"Color",Colors.DarkSlateBlue,Colors.White,10,Tween.TransitionType.Sine,Tween.EaseType.In);
                twe.Start();
                RemoveChild(twe);
            }
        }
    }

    public override void _PhysicsProcess(float delta)  //60 TICKS par seconde
    {
        tick += 1;
        day_cycle();
    }
}
