using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public class FOV : Node2D
{


	private int field_of_view = 40;

	[Export]
	public int Field_of_view
	{
		get
		{
			return field_of_view;
		}
		set
		{
			field_of_view = value;
		}
	}


	private int warn_distance = 500;

	[Export]
	public int Warn_distance
	{
		get { return warn_distance; }
		set
		{
			warn_distance = value;
		}
	}


	private int danger_distance = 500;

	[Export]
	public int Danger_distance
	{
		get { return danger_distance; }
		set { danger_distance = value; }
	}


	private bool show_fov = true;
	[Export]
	public bool Show_fov
	{
		get { return show_fov; }
		set { show_fov = value; }
	}

	[Export]
	public bool show_target_line = true;


	private Color fov_color = new Color("#b23d7f0b");

	[Export]
	public Color Fov_color
	{
		get { return fov_color; }
		set
		{
			fov_color = value;
		}
	}

	private Color warn_fov_color = new Color("#b1eedf0b");

	[Export]
	public Color Warn_fov_color
	{
		get { return warn_fov_color; }
		set
		{
			warn_fov_color = value;
		}
	}

	private Color danger_fov_color = new Color("#9dfb320b");

	[Export]
	public Color Danger_fov_color
	{
		get { return danger_fov_color; }
		set
		{
			danger_fov_color = value;
		}
	}


	private int view_detail = 200;



	[Export]
	public int View_detail
	{
		get { return view_detail; }
		set { view_detail = value; }
	}

	private System.Collections.Generic.List<string> target_groups = new System.Collections.Generic.List<string>() {"Player"};

	[Export]
	public System.Collections.Generic.List<string> Target_groups
	{
		get { return target_groups; }
		set { target_groups = value; }
	}


	int collision_mask = 24640;
	

	[Export(PropertyHint.Layers2dPhysics)]
	public int Collision_mask
	{
		get { return collision_mask; }
		set
		{
			collision_mask = value;
		}
	}


	private System.Collections.Generic.List<string> in_danger_area = new System.Collections.Generic.List<string>();
	private System.Collections.Generic.List<string> in_warn_area = new System.Collections.Generic.List<string>();

	private System.Collections.Generic.List<Vector2> points_arc= new System.Collections.Generic.List<Vector2>();

	private float frequency = 0.01f;

	[Export]
	public float Frequency
	{
		get { return frequency; }
		set { frequency = value; }
	}


	Timer timer;
	double start_angle, end_angle, dir_deg;


	public Player player;
	private Node2D fieldOfViewNode;

	Godot.Collections.Array fovWarnArea, fovDangerArea;

	public bool inWarnAreaOnly, inDangerArea = false;

    public override void _Ready()
	{
		GDScript fieldOfViewScript = (GDScript)GD.Load("res://addons/luisboch.field_of_view/field_of_view.gd");
		fieldOfViewNode = (Node2D)fieldOfViewScript.New();


		AddChild(fieldOfViewNode);

        fieldOfViewNode.Set("collision_mask", Collision_mask);
        fieldOfViewNode.Set("target_groups", new List<string>() { "Player" });
        fieldOfViewNode.Set("frequency", Frequency);
		fieldOfViewNode.Set("field_of_view", Field_of_view);
		fieldOfViewNode.Set("warn_distance", Warn_distance);
		fieldOfViewNode.Set("danger_distance", Danger_distance);
		fieldOfViewNode.Set("view_detail", View_detail);
		fieldOfViewNode.Set("show_target_line", false);
		fieldOfViewNode.Call("_enter_tree");

        fieldOfViewNode.Connect("target_enter", this, "targetEntered");
        fieldOfViewNode.Connect("target_exit", this, "targetExited");
		
		fovWarnArea = fieldOfViewNode.Get("in_warn_area") as Godot.Collections.Array;
        fovDangerArea = fieldOfViewNode.Get("in_danger_area") as Godot.Collections.Array;


    }

	public bool can_see_player()
	{
        return player != null;

    }


	public void targetEntered(Player obj)
	{
        player = obj.GetParent() as Player;
        fovDangerArea = fieldOfViewNode.Get("in_danger_area") as Godot.Collections.Array;
		
		inDangerArea = true;
        inWarnAreaOnly = false;

        fieldOfViewNode.Set("chase", inDangerArea);
        fieldOfViewNode.Set("investigating", inWarnAreaOnly);
    }

    public void targetExited(Player obj) {
		player = null;
		inDangerArea = false;
        fieldOfViewNode.Set("chase", inDangerArea);

    }


	//Sets FOV to yellow when investigating
	public void setFOVWarnColor()
	{
		inDangerArea = false;
		inWarnAreaOnly = true;
        fieldOfViewNode.Set("chase", inDangerArea);
        fieldOfViewNode.Set("investigating", inWarnAreaOnly);
    }

	//Sets FOV to green when moving back
	public void setFOVDefault()
	{
        inDangerArea = false;
        inWarnAreaOnly = false;
        fieldOfViewNode.Set("chase", inDangerArea);
        fieldOfViewNode.Set("investigating", inWarnAreaOnly);
    }
	public void setup_timer()
	{
		AddChild(timer);
		timer.Owner = this;
		timer.WaitTime = frequency;
	}


	public void draw_fov()
	{
		Color color = new Color();
		if(in_danger_area.Count > 0)
		{
			color = danger_fov_color;
		}
		else if(in_warn_area.Count > 0)
		{
			color = warn_fov_color;
		}
		else
		{
			color = fov_color;
		}


		foreach (Vector2 aux in points_arc) {

	   }
	}

	public Vector2 deg_to_vector(float deg)
	{
		return new Vector2(Mathf.Cos(deg), Mathf.Sin(Mathf.Deg2Rad(deg)));
	}



}
