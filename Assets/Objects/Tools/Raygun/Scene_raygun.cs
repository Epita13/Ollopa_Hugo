using Godot;
using System;

public class Scene_raygun : Node2D
{
    public override void _Ready()
    {
        
    }

    public float abs(float x) //Retourne la valeur absolue.
    {
        float res = x;
        if (x < 0)
        {
            res = -x;
        }
        return res;
    }

    public void Shooting(Vector2 start,Vector2 end_location)   //Crée le rayon laser avec des particules jusqu'à la position de la souris.
    {
        
        Vector2 start_location = new Vector2((float)Math.Cos((end_location-start).Angle()) * 100,(float)Math.Sin((end_location-start).Angle()) * 100)+start; //Prend le bout du canon comme départ.
        Vector2 distance = end_location - start_location;                                                                                                          //On va réduire la distance pour faire le cas d'arrêt de la boucle while qui va suivre.
        distance.x = abs(distance.x);                                                                                                                              //Simplifie les calculs qu'on va faire.
        distance.y = abs(distance.y);
        float x = (float) (Math.Cos((end_location-start).Angle()) * 25);                                                                                           //Calcule la premiere position de la premiere particule en utilisant l'angle de rotation.
        float y = (float) (Math.Sin((end_location-start).Angle()) * 25);
        float i = 0;
        while (distance.x>0f && distance.y>0f)
        {
            i += 1;
            
            Particles2D trail = (Particles2D)GD.Load<PackedScene>("res://Assets/Objects/Tools/Raygun/Trail.tscn").Instance();
            AddChild(trail);

            Vector2 trail_location = new Vector2(i*x,i*y);                       // Positionne les différentes particules sur une droite en fonction de i.
            trail.Position = start_location + trail_location;                          //Le place au bon endroit en fonction de start_location.
            trail.Emitting = true;
            
            distance.x -= abs(x);
            distance.y -= abs(y);
        }
    }

    public void Show_hit(Vector2 start,Vector2 hit_location)      //Reprend la méthode du shooting en ajoutant un autre comportement si il y a collision avec le raycast.
    {
        
        Particles2D expl = (Particles2D)GD.Load<PackedScene>("res://Assets/Objects/Tools/Raygun/Explosion.tscn").Instance();
        
        AddChild(expl);
        expl.Position = hit_location;
        expl.Emitting = true;
        
        
        
        Vector2 start_location = new Vector2((float)Math.Cos((hit_location-start).Angle()) * 100,(float)Math.Sin((hit_location-start).Angle()) * 100)+start;
        Vector2 distance = hit_location - start_location;
        distance.x = abs(distance.x);
        distance.y = abs(distance.y);
        float i = 0;
        float x = (float) (Math.Cos((hit_location-start).Angle()) * 25);
        float y = (float) (Math.Sin((hit_location-start).Angle()) * 25);
        while (distance.x>0f && distance.y>0f)
        {
            Particles2D trail = (Particles2D)GD.Load<PackedScene>("res://Assets/Objects/Tools/Raygun/Trail.tscn").Instance();
            AddChild(trail);
            i += 1;
            Vector2 trail_location = new Vector2(i*x,i*y);
            
            trail.Position = start_location + trail_location;
            trail.Emitting = true;
            distance.x -= abs(x);
            distance.y -= abs(y);
        }

    }
}



